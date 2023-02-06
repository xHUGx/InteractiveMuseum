using UnityEngine;

namespace Features.DebugSystem.Config
{
    [CreateAssetMenu(menuName = "Config/Debug Settings", fileName = "DebugSettings")]
    public class DebugSettings : ScriptableObject
    {
        [Header("Active")] 
        [SerializeField] private bool _isEnabledInEditor;
        [SerializeField] private bool _isEnabledInBuild;
        
        [Header("Ar")]
        [SerializeField] private bool _imageTrackingDebug_old;
        // [SerializeField] private bool _simpleCameraController;
        [SerializeField] private bool _isShowTrackedImages;
       

        [Header("Editor")]
        [SerializeField] private bool _isShowBackground;
        [SerializeField] private bool _isAddSimpleCameraController;
        [SerializeField] private bool _isShowContentBoundsPositions;
        
        [Header("Scenes")]
        [SerializeField] private bool _isShowButtonsToMockTracking;
        [SerializeField] private bool _isShowDebugGraphics; 
        [SerializeField] private bool _isSimulateTrackedImageMoving;
        
        
        public bool IsDebugSettingsEnabled => Application.isEditor ? _isEnabledInEditor : _isEnabledInBuild;

        public bool ImageTrackingDebug => _imageTrackingDebug_old;
        // public bool SimpleCameraController => _simpleCameraController;
        public bool IsShowTrackedImages => _isShowTrackedImages;
       
        public bool IsShowBackground => _isShowBackground;
        public bool IsAddSimpleCameraController => _isAddSimpleCameraController;
        public bool IsShowContentBoundPositions => _isShowContentBoundsPositions;
        public bool IsShowButtonsToMockTracking => _isShowButtonsToMockTracking;
        public bool IsShowDebugGraphics => _isShowDebugGraphics;
        public bool IsSimulateTrackedImageMoving => _isSimulateTrackedImageMoving;
    }
}