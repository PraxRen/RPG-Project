using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        
        private void Update()
        {
            transform.Rotate(_direction * Time.deltaTime, Space.Self);
        }
    }
}