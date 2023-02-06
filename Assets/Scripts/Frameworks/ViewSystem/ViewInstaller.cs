using Frameworks.ViewSystem.Controller;
using ViewSystem.Handler;
using Zenject;

namespace ViewSystem
{
	public class ViewInstaller : Installer
	{
		public override void InstallBindings()
		{
			InstallSignals();
			InstallFactory();
			InstallController();
			InstallHandlers();
		}

		private void InstallHandlers()
		{
			InstallHandler<HintHandler>();
			InstallHandler<WindowHandler>();
			InstallHandler<TutorialHandler>();
		}

		private void InstallFactory()
		{
			Container.Bind<ViewFactory>().AsSingle();
		}

		private void InstallController()
		{
			Container.Bind<IViewController>().To<ViewController>().AsSingle();
		}

		private void InstallSignals()
		{
			Container.DeclareSignal<ViewSignals.AddHolder>().OptionalSubscriber();
			Container.DeclareSignal<ViewSignals.Shown>().OptionalSubscriber();
			Container.DeclareSignal<ViewSignals.Hidden>().OptionalSubscriber();

			Container.DeclareSignal<ViewSignals.AbilityViewSignals.Show>();
			Container.DeclareSignal<ViewSignals.AbilityViewSignals.Hide>();
		}

		private void InstallHandler<THandler>() where THandler : IViewHandler
		{
			Container
				   .BindInterfacesAndSelfTo<THandler>()
				   .AsSingle();
		}
	}
}