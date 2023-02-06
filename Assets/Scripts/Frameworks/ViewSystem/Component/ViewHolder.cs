using UnityEngine;
using Zenject;

namespace ViewSystem
{
	public class ViewHolder : MonoBehaviour
	{
#pragma warning disable 0649
		[SerializeField] private ViewLayer _viewLayer;

		private SignalBus _signalBus;
		
		[Inject]
		public void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}

		private void Awake()
		{
			_signalBus.Fire(new ViewSignals.AddHolder(transform, _viewLayer));
		}
	}
}