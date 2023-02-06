using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Config
{
    public abstract class RegistryBase<TData> : ScriptableObject, IRegistryClass
        where TData : class
    {
        [SerializeField] protected TData RegistryData;

        public TData Data => RegistryData;
    }

    public abstract class RegistryListBase<TData> : ScriptableObject, IRegistryList where TData : class, IRegistryData
    {
        [SerializeField] protected TData[] RegistryItems;

        public int Length => RegistryItems.Length;

        public IEnumerator GetEnumerator()
        {
            return RegistryItems.GetEnumerator();
        }

        public TData[] GetItems()
        {
            return RegistryItems;
        }

        public Dictionary<string, TData> ToDictionary()
        {
            return RegistryItems.ToDictionary(key => key.Id, value => value);
        }

        public bool TryGetById(string id,out TData result)
        {
            foreach (var item in RegistryItems)
            {
                if (string.CompareOrdinal(item.Id, id) == 0)
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}