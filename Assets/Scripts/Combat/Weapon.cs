using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        public event Action OnHited;

        public void OnHit()
        {
            OnHited?.Invoke();
        }
    }
}
