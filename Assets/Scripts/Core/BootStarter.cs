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

			builder.Register<ClientController>(Lifetime.Scoped).As<ClientController, IStartable>();
			builder.Register<CardsController>(Lifetime.Scoped).As<CardsController, IStartable>();
			builder.Register<ButtonsMenuController>(Lifetime.Scoped).As<ButtonsMenuController, IStartable, IDisposable>();
			builder.Register<PopupController>(Lifetime.Scoped);
			builder.Register<UIController>(Lifetime.Scoped);
		}
	}
}