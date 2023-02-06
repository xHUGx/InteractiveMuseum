using UnityEngine;

namespace ViewSystem
{
	public class ViewSignals
	{
		public class AddHolder
		{
			public AddHolder(Transform transform, ViewLayer viewLayer)
			{
				Transform = transform;
				ViewLayer = viewLayer;
			}

			public Transform Transform { get; }
			public ViewLayer ViewLayer { get; }
		}

		public class BaseActivitySignal
		{
			public BaseActivitySignal(string viewName)
			{
				ViewName = viewName;
			}

			public string ViewName { get; }

			public bool IsEqual<TClass>() where TClass : class
			{
				return typeof(TClass).Name.Equals(ViewName);
			}
		}

		public class Shown : BaseActivitySignal
		{
			public Shown(string viewName) : base(viewName)
			{
			}
		}

		public class Hidden : BaseActivitySignal
		{
			public Hidden(string viewName, bool allWindowsArerClosed) : base(viewName)
			{
				AllWindowsArerClosed = allWindowsArerClosed;
			}

			public bool AllWindowsArerClosed { get; }
		}

		public class AbilityViewSignals
		{
			public class Show
			{
			}

			public class Hide
			{
			}
		}
	}
}