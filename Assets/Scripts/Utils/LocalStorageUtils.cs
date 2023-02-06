using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
    public static class LocalStorageUtils
    {
        public static void Save<T>(string key, T serializableObject)
        {
            var jsonString = JsonConvert.SerializeObject(serializableObject);
            // Debug.Log($"[ConnectionConfig] Saved to Local Storage: {jsonString}");
            PlayerPrefs.SetString(key, jsonString);
            PlayerPrefs.Save();
        }

        public static T Load<T>(string key, T defaultValue)
        {
            if (!PlayerPrefs.HasKey(key)) return defaultValue;
            var jsonString = PlayerPrefs.GetString(key, "");
            try
            {
                // Debug.Log($"[ConnectionConfig] Loaded from Local Storage: {jsonString}");
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}