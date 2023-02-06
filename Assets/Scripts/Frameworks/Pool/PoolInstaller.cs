
using Frameworks.ViewSystem.Data;
using UnityEngine;
using Utils;
using Zenject;

namespace Pool
{
	public class PoolInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<GeneralPool>().AsSingle();
			// Container.Bind<EffectsFactory>().AsSingle();
		}


		private void InstallPool<TComponent>(int size = 0) where TComponent : MonoBehaviour, IPoolable
		{
			Container.BindMemoryPool<TComponent, Pool<TComponent>>()
					 .WithId(typeof(TComponent))
					 .WithInitialSize(size)
					 .FromComponentInNewPrefabResource(
						 ViewSystemResources.RESOURCES_FOLDER_VIEWS_PREFABS + typeof(TComponent).Name);
		}

		private void InstallPoolWithId<TInterface, TComponent>(object id, int size = 0)
				where TInterface : IPoolable
				where TComponent : MonoBehaviour, TInterface, IPoolable
		{
			Container.BindMemoryPool<TInterface, Pool<TInterface>>()
					 .WithId(id)
					 .WithInitialSize(size)
					 .FromComponentInNewPrefabResource(
						 ViewSystemResources.RESOURCES_FOLDER_VIEWS_PREFABS + typeof(TComponent).Name);
		}
		
		private void InstallPoolWithId<TComponent>(object id, int size = 0)
			where TComponent : MonoBehaviour, IPoolable
		{
			Container.BindMemoryPool<TComponent, Pool<TComponent>>()
				.WithId(id)
				.WithInitialSize(size)
				.FromComponentInNewPrefabResource(
					ViewSystemResources.RESOURCES_FOLDER_VIEWS_PREFABS + typeof(TComponent).Name);
		}
	}
}