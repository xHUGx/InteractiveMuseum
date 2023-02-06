using UnityEngine;

namespace Pool
{
	public class BasePoolable : MonoBehaviour, IPoolable
	{
		public virtual object ObjectId { get; private set; }

		public void Initialize(object objectId)
		{
			ObjectId = objectId;
		}

		public virtual void OnSpawn(Transform parent)
		{
			if (parent != null)
				transform.SetParent(parent, false);

			gameObject.SetActive(true);
		}

		public virtual void ReInitialize()
		{
			transform.localScale = Vector3.one;
		}

		public virtual void OnDespawn(Transform parent)
		{
			if (parent != null)
				transform.SetParent(parent, false);

			gameObject.SetActive(false);
		}
	}
}