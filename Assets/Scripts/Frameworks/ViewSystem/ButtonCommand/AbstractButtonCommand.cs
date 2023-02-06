using UnityEngine;
using UnityEngine.UI;

namespace View.UI.ButtonCommands
{
	[RequireComponent(typeof(Button))]
	public abstract class AbstractButtonCommand : MonoBehaviour
	{
		private Button _button;

		private bool _isLocked;
		protected Button Button => _button ?? (_button = GetComponent<Button>());

		protected virtual bool IgnoreLocking { get; }

		protected virtual void Awake()
		{
			Button.onClick.AddListener(() =>
									   {
										   if (!_isLocked || IgnoreLocking)
											   Activate();
									   });
		}

		public abstract void Activate();

		public void SetLockingState(bool isLocked)
		{
			_isLocked = isLocked;
		}

		public void ChangeInteractability(bool isInteracteble)
		{
			Button.interactable = isInteracteble;
		}
	}
}