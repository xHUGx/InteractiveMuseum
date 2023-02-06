using EffectSystem;
using Pool;
using UnityEngine;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Bootstrap
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[MainInstaller] Installing");

            Container.Install<InitialInstaller>();

            Container.Install<PoolInstaller>();
            
            Container.Install<EffectsInstaller>();
        }
    }
}