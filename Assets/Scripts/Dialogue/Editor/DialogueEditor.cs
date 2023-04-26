using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue;
        [NonSerialized] private GUIStyle _nodeStyle;
        [NonSerialized] private GUIStyle _playerNodeStyle;
        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private DialogueNode _creatingNode;
        [NonSerialized] private DialogueNode _deletingNode;
        [NonSerialized] private DialogueNode _linkingParentNode;
        [NonSerialized] private Vector2 _draggingOffset;
        [NonSerialized] bool _isDraggingCanvas;
        [NonSerialized] Vector2 _draggingCanvasOffset;
        private Vector2 _scrollPosition;
        private const float CanvasSize = 4000;
        private const float BackgroundSize = 50;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if (dialogue == null)
                return false;

            ShowEditorWindow();
            return true;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle.normal.textColor = Color.white;
            _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);

            _playerNodeStyle = new GUIStyle();
            _playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            _playerNodeStyle.normal.textColor = Color.white;
            _playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;

            if (newDialogue != null)
            {
                _selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                //Два массива, из-за корректной отрисовки узлов 
                foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (_creatingNode != null)
                {
                    _selectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }

                if (_deletingNode != null)
                {
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null)
            {
                Vector2 mousePoint = Event.current.mousePosition + _scrollPosition;
                _draggingNode = GetNodeAtPoint(mousePoint);

                if (_draggingNode != null)
                {
                    _draggingOffset = _draggingNode.Rect.position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode;
                }
                else
                {
                    _isDraggingCanvas = true;
                    _draggingCanvasOffset = mousePoint;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _isDraggingCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _isDraggingCanvas)
            {
                _isDraggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = _nodeStyle;

            if (node.IsPlayerSpeaking)
            {
                style = _playerNodeStyle;
            }

            GUILayout.BeginArea(node.Rect, style);
            //EditorGUI.BeginChangeCheck();
            string newText = EditorGUILayout.TextField(node.Text);
            node.SetText(newText);                
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                _creatingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("-"))
            {
                _deletingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    _linkingParentNode = node;
                }
            }
            else if(_linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.IsChildrenContains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    _linkingParentNode.RemoveChild(node.name);
                    _linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    _linkingParentNode.AddChild(node.name);
                    _linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.Rect.xMax, node.Rect.center.y);

            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null,4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                if (node.Rect.Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }
    }
}
