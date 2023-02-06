using UnityEngine;
using Zenject;

namespace Pool
{
	public class Pool<TPoolable> : MemoryPool<TPoolable> where TPoolable : IPoolable
	{
		public TPoolable Spawn(Transform parent)
		{
			var poolable = Spawn();
			poolable.OnSpawn(parent);

			return poolable;
		}

		protected override void Reinitialize(TPoolable item)
		{
			item.ReInitialize();
		}

		public void Despawn(TPoolable poolable, Transform parent)
		{
			poolable.OnDespawn(parent);

			base.Despawn(poolable);
		}

		public override string ToString()
		{
			return typeof(TPoolable).Name;
		}
	}
}