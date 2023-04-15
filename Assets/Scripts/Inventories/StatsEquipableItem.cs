using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Stats Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        [SerializeField] Modifier[] addtiveModifiers;
        [SerializeField] Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in addtiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}