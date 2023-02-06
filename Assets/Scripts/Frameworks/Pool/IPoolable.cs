using UnityEngine;

namespace Pool
{
	public interface IPoolable
	{
		object ObjectId { get; }
		void Initialize(object objectId);
		void OnSpawn(Transform parent);
		void ReInitialize();
		void OnDespawn(Transform parent);
	}
}