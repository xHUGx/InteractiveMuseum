using DG.Tweening;
using EffectSystem.Base;
using UnityEngine;

namespace EffectSystem.View
{
    public class FruitExplosionEffect : BaseEffectWithInputData<FruitExplosionEffect.Data>
    {
        private const float Duration = 30f;

        public class Data : IEffectData
        {
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }
        }

        public override void Initialize(Data data)
        {
            base.Initialize(data);

            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }

        protected override Sequence CreateSequence()
        {
            var sequence = DOTween.Sequence();

            sequence.AppendInterval(Duration); 

            return sequence;
        }
    }
}