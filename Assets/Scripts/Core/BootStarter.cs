using System;
using CardsSystem;
using Game.UI;
using NetworkSystem;
using VContainer;
using VContainer.Unity;

namespace Core
{
	public class BootStarter : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<AssetLoader>(Lifetime.Scoped);

			builder.Register<ClientController>(Lifetime.Scoped);
			builder.Register<MainMenuController>(Lifetime.Scoped).As<MainMenuController, IStartable, IDisposable>();
			builder.Register<CardsController>(Lifetime.Scoped).As<CardsController, IStartable>();
			builder.Register<PopupController>(Lifetime.Scoped);
			builder.Register<UIController>(Lifetime.Scoped);
		}
	}
}