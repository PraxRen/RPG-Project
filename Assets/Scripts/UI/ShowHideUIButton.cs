using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ShowHideUIButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _uiContainer;

        private void OnEnable()
        {
            _button.onClick.AddListener(Toggle);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Toggle);
        }

        private void Toggle()
        {
            _uiContainer.SetActive(!_uiContainer.activeSelf);
        }
    }
}
