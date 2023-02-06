using Pool;
using UnityEngine;

namespace ViewSystem.Base
{
	public abstract class BaseView : BasePoolable, IView
	{
		public abstract ViewLayer ViewLayer { get; }

		public bool IsShown { get; protected set; }

		public void Initialize(Transform parent)
		{
			transform.SetParent(parent, false);
		}

		public virtual void Show()
		{
			IsShown = true;
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
			IsShown = false;
		}
	}
}