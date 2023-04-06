using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AnimatorCharacterManager : MonoBehaviour
    {
        public class AnimatorIdsParams
        {
            public readonly int Die = Animator.StringToHash("die");
            public readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
            public readonly int Attack = Animator.StringToHash("attack");
            public readonly int StopAttack = Animator.StringToHash("stopAttack");
        }

        public static AnimatorCharacterManager Instance { get; private set; }
        public AnimatorIdsParams Params { get; private set; }

        private void Awake()
        {
            Params = new AnimatorIdsParams();
            Instance = this;
        }
    }
}