using System;
using UnityEngine;

namespace Features.Ar.Configs
{
    [CreateAssetMenu(fileName = "AverageFilterConfig", menuName = "Config/Average Filter Config")]
    public class AverageFilterConfig : ScriptableObject
    {
        [SerializeField] private AverageFilterConfigData _data;

        public AverageFilterConfigData Data => _data;

        public void Fill(AverageFilterConfigData newData)
        {
            _data.Fill(newData);
        }
    }

    [Serializable]
    public class AverageFilterConfigData
    {
        [Range(1, 50)] [SerializeField] private int _bufferLength;
        [Range(0f, 0.5f)] [SerializeField] private float _addValueThreshold;

        public int BufferLength => _bufferLength;
        public float AddValueThreshold => _addValueThreshold;

        public AverageFilterConfigData(int bufferLength, float addValueThreshold)
        {
            _bufferLength = bufferLength;
            _addValueThreshold = addValueThreshold;
        }

        public void Fill(AverageFilterConfigData newData)
        {
            _bufferLength = newData.BufferLength;
            _addValueThreshold = newData.AddValueThreshold;
        }
    }

    public enum AverageFilterConfigType
    {
        Position,
        Direction
    }
}