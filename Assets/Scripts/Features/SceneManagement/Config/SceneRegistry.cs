using System;
using Config;
using Features.SceneManagement.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.SceneManagement.Config
{
    [Serializable]
    public class SceneRegistryItem : IRegistryData
    {
        [SerializeField] private SceneType _sceneType;
        [SerializeField] private LoadSceneMode _loadMode;
        
        public string Id => _sceneType.ToString();
        
        public LoadSceneMode LoadMode => _loadMode;
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "SceneRegistry", menuName = "Config/Scene Registry")]
    public class SceneRegistry : RegistryListBase<SceneRegistryItem>
    {

    }
}