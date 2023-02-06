/*using Interfaces;
using Services.GameRules;
using Signals;*/
using Zenject;

namespace Utils
{
	public static class ZenjectExtensions
	{
		public static void BindService<TService>(this DiContainer container)
		{
			container
				   .BindInterfacesAndSelfTo<TService>()
				   .AsSingle()
				   .NonLazy();
		}

		public static void BindServiceToInterface<TInterface, TService>(this DiContainer container)
				where TService : TInterface
		{
			container
				   .Bind<TInterface>()
				   .To<TService>()
				   .AsSingle();
		}

        /// <summary>
        ///  Bind interfaces and self to model as Single
        /// </summary>
        /// <param name="container"></param>
        /// <typeparam name="TModel"></typeparam>
        public static void InstallModel<TModel>(this DiContainer container)
		{
			container
				   .BindInterfacesAndSelfTo<TModel>()
				   .AsSingle();
		}

		public static void BindRule<TRule>(this DiContainer container) 
		{
			container
				   .BindInterfacesTo<TRule>()
				   .AsSingle()
				   .NonLazy();
		}
/*
		public static void InstallGameRule<TGameRule>(this DiContainer container) where TGameRule : IGameRule
		{
			container
				   .BindInterfacesAndSelfTo<TGameRule>()
				   .AsSingle()
				   .NonLazy();
		}

		public static void BindToGameTick<TGameRule>(this DiContainer container) where TGameRule : IGameTickRule
		{
			container.BindSignal<GameSignals.TimeTick>()
					 .ToMethod<TGameRule>((rule, tickData) => rule.GameTick(tickData.DeltaTime))
					 .From(x => x.FromResolve().AsCached());
		}

		public static void BindToOneSecondTick<TGameRule>(this DiContainer container)
				where TGameRule : IOneSecondTickRule
		{
			container.BindSignal<GameSignals.SecondPassed>()
					 .ToMethod<TGameRule>((rule, tickData) => rule.SecondPassed())
					 .From(x => x.FromResolve().AsCached());
		}*/

		public static void InstallRegistry<TRegistry>(this DiContainer container, TRegistry registry)
		{
			container
				   .BindInterfacesAndSelfTo<TRegistry>()
				   .FromInstance(registry)
				   .AsSingle();
		}
		
		public static void UnBindService<T>(this DiContainer container)
		{
			container
				.Unbind<T>();
			container
				.UnbindInterfacesTo<T>();
		}
		
		public static void BindSingleFromInstance<T>(this DiContainer container, T item)
		{
			container
				.Bind<T>()
				.FromInstance(item)
				.AsSingle();
		}
	}
}