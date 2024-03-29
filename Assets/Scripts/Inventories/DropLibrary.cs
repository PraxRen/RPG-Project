using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {
        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;

            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                {
                    return 1;
                }

                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(minNumber, level);
                return Random.Range(min, max + 1);
            }
        }

        [SerializeField] DropConfig[] _potentialDrops;
        [SerializeField] private float[] _dropChancePercentage;
        [SerializeField] private int[] _minDrops;
        [SerializeField] private int[] _maxDrops;

        public struct Dropped 
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }

            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        private bool ShouldRandomDrop(int level)
        {
            return Random.Range(0,100) < GetByLevel(_dropChancePercentage, level);
        }

        private int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(_minDrops, level);
            int max = GetByLevel(_maxDrops, level);
            return Random.Range(min, max);
        }

        private Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }

        private DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0;
            
            foreach (var drop in _potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }

            return null;
        }

        private float GetTotalChance(int level)
        {
            float total = 0;

            foreach (var drop in _potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }

            return total;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }

            if (level > values.Length)
            {
                return values[values.Length - 1];
            }

            if (level <= 0)
            {
                return default;
            }

            return values[level - 1];
        }
    }
}
