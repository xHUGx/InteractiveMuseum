using System;
using UnityEngine;
using Features.Ar.Configs;

namespace Features.Ar.Controllers
{
    public interface IFilter<T>
    {
        bool AddAndCheckValueIsNewTarget(T newValue);
        T GetAverage();
    }

    public interface IVector3Filter : IFilter<Vector3>
    {
    }

    // TODO: add direction vector3 to Filter
    public class Vector3AverageFilter : IVector3Filter, IDisposable
    {
        private readonly AverageFilterConfig _averageFilterConfig;

        private Vector3[] _buffer;
        private int _valuesInBuffer;

        public Vector3AverageFilter(AverageFilterConfig averageFilterConfig)
        {
            _averageFilterConfig = averageFilterConfig;
        }

        public bool AddAndCheckValueIsNewTarget(Vector3 newValue)
        {
            CheckBuffer();

            var difference = Vector3.Distance(newValue, GetAverage());
            if (difference > _averageFilterConfig.Data.AddValueThreshold || _valuesInBuffer == 0)
            {
                AddValueToBuffer(newValue);
                return true;
            }

            return false;
        }

        private void AddValueToBuffer(Vector3 newValue)
        {
            if (_valuesInBuffer < _buffer.Length)
            {
                _buffer[_valuesInBuffer] = newValue;
                _valuesInBuffer++;
                return;
            }

            for (int i = _buffer.Length - 1; i > 0; i--)
            {
                _buffer[i] = _buffer[i - 1];
            }

            _buffer[0] = newValue;
        }

        public Vector3 GetAverage()
        {
            if (_valuesInBuffer == 0) return Vector3.zero;

            var average = Vector3.zero;
            for (int i = 0; i < _valuesInBuffer; i++)
            {
                average += _buffer[i];
            }

            return average / (_valuesInBuffer * 1f);
        }

        private void CheckBuffer()
        {
            _buffer ??= new Vector3[_averageFilterConfig.Data.BufferLength];

            if (_buffer.Length == _averageFilterConfig.Data.BufferLength) return;

            _valuesInBuffer = _averageFilterConfig.Data.BufferLength > _buffer.Length
                ? _buffer.Length
                : _averageFilterConfig.Data.BufferLength;
            
            var newBuffer = new Vector3[_averageFilterConfig.Data.BufferLength];
            Array.Copy(_buffer, 0, newBuffer, 0, _valuesInBuffer);
            _buffer = newBuffer;
        }

        public void Dispose()
        {
            _buffer = null;
        }

        public void Clear()
        {
            Dispose();
        }
    }
}