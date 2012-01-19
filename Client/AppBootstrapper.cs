using System.Reflection;
using System.Windows;
using Autofac;
using Client.Context;
using Client.Features.Communicator;
using Client.Features.Login;
using Client.Services;
using Client.Utils;
using Common.Hash;

namespace Client
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition.Primitives;
	using System.Linq;
	using Caliburn.Micro;

	public class AppBootstrapper : Bootstrapper<IShell>
	{
		private IContainer _container;

		protected override void Configure()
		{
			_container = CreateContainer();
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			return _container.Resolve(serviceType);
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.Resolve(serviceType.MakeArrayType()) as IEnumerable<object>;
		}

        protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBoxService.ShowError(e.Exception.InnerException);
            e.Handled = true;
        }

		private IContainer CreateContainer()
		{
			var containerBuilder = new ContainerBuilder();

			containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(type => type != typeof(ContactViewModel))
				.AsImplementedInterfaces().AsSelf().PropertiesAutowired(
					PropertyWiringFlags.PreserveSetValues);
			containerBuilder.RegisterInstance(new ServerConnection()).AsImplementedInterfaces();
		    containerBuilder.RegisterInstance(new CurrentContext()).AsImplementedInterfaces();
           
			containerBuilder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
			containerBuilder.RegisterType<WindowManager>().As<IWindowManager>();
			containerBuilder.RegisterType<BCryptHashService>().AsImplementedInterfaces();
			containerBuilder.Register(cc => _container).ExternallyOwned();
			return containerBuilder.Build();
		}
	}
}
