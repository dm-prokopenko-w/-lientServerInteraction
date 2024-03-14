using VContainer;
using VContainer.Unity;

namespace Core
{
	public class BootStarter : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			/*
			builder.Register<ControlModule>(Lifetime.Scoped);
			builder.Register<AssetLoader>(Lifetime.Scoped);
			builder.Register<SaveModule>(Lifetime.Scoped);
			builder.Register<ProgressController>(Lifetime.Scoped).As<ProgressController, IStartable>();
			builder.Register<ControllerVFX>(Lifetime.Scoped).As<ControllerVFX, IStartable, IDisposable>();
			*/
		}
	}
}