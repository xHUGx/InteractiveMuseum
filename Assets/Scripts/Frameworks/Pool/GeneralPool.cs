using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pool
{
	public class GeneralPool
	{
		private readonly DiContainer _diContainer;
		private readonly Transform _inactiveObjectsContainer;

		private readonly Dictionary<object, IMemoryPool> _pools = new Dictionary<object, IMemoryPool>();

		public GeneralPool(DiContainer diContainer)
		{
			_inactiveObjectsContainer = new GameObject(GetType().Name).transform;
			_diContainer = diContainer;

			Object.DontDestroyOnLoad(_inactiveObjectsContainer.gameObject);
		}

		public TPoolable Spawn<TPoolable>(Transform parent) where TPoolable : IPoolable
		{
			return Spawn<TPoolable>(parent, typeof(TPoolable));
		}

		public TPoolable Spawn<TPoolable>(Transform parent, object id) where TPoolable : IPoolable
		{
			var pool = GetPool<TPoolable>(id);
			var poolable = pool.Spawn(parent);
			poolable.Initialize(id);

			return poolable;
		}

		public void Despawn<TPoolable>(TPoolable poolable) where TPoolable : IPoolable
		{
			if (poolable == null)
				return;

			var pool = GetPool<TPoolable>(poolable.ObjectId);

			pool.Despawn(poolable, _inactiveObjectsContainer);
		}

		public void DespawnAll<TPoolable>(IEnumerable<TPoolable> enumerable) where TPoolable : IPoolable
		{
			foreach (var poolable in enumerable)
				Despawn(poolable);

			if (enumerable is ICollection<TPoolable> collection)
				collection.Clear();
		}

		private Pool<TPoolable> GetPool<TPoolable>(object id) where TPoolable : IPoolable
		{
			if (_pools.ContainsKey(id))
				return _pools[id] as Pool<TPoolable>;

			var pool = _diContainer.ResolveId<Pool<TPoolable>>(id);

			_pools.Add(id, pool);

			return pool;
		}
	}
}