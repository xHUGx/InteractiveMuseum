using System;
using Features.Ar.Data;
using UnityEngine;

namespace Features.Ar.Configs
{
    [CreateAssetMenu(fileName = "ImageTrackingConfigData", menuName = "Config/Image Tracking Config")]
    public class ArImageTrackingConfig : ScriptableObject
    {
        [SerializeField] private ArImageTrackingConfigData _data;

        public ArImageTrackingConfigData Data => _data;

        public void Fill(ArImageTrackingConfigData newData)
        {
            _data.Fill(newData);
        }
    }

    [Serializable]
    public class ArImageTrackingConfigData
    {
        [Range(1, 50)] [SerializeField] private int _skipFirstTrackingFrames;
        [SerializeField] private ImageTrackingModeType _imageTrackingModeType;
        public int SkipFirstTrackingFrames => _skipFirstTrackingFrames;
        public ImageTrackingModeType ImageTrackingModeType => _imageTrackingModeType;

        public ArImageTrackingConfigData(int skipFirstTrackingFrames, ImageTrackingModeType imageTrackingModeType)
        {
            _skipFirstTrackingFrames = skipFirstTrackingFrames;
            _imageTrackingModeType = imageTrackingModeType;
        }

        public void Fill(ArImageTrackingConfigData newData)
        {
            _skipFirstTrackingFrames = newData.SkipFirstTrackingFrames;
            _imageTrackingModeType = newData.ImageTrackingModeType;
        }
    }
}