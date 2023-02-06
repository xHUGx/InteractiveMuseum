﻿using UnityEngine;

namespace ViewSystem.Base
{
	public interface IViewInput
	{
	}

	public interface IView
	{
		ViewLayer ViewLayer { get; }

		bool IsShown { get; }

		void Initialize(Transform parent);
		void Show();
		void Hide();
	}

	public interface IAnimatedView : IView
	{
	}

	public interface IViewWithInput<in TViewInput> : IView
			where TViewInput : IViewInput
	{
		void Show(TViewInput data);
	}

	public interface IAnimatedViewWithInput<in TViewInput> : IAnimatedView, IViewWithInput<TViewInput>
			where TViewInput : IViewInput
	{
	}
}