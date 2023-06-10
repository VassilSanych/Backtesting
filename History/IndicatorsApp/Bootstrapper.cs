using System;
using Autofac;
using NLog;
using VGn.CryptoSdk.Dummy;
using VGn.CryptoSdk.HistoryStorage;
using VGn.CryptoSdk.Markets;
using VGn.LogModule;

namespace IndicatorsApp
{
	public class Bootstrapper : IDisposable
	{
		private IContainer? _container;

		public IContainer? Container
		{
			get
			{
				if (_container == null)
					BuildReferences();
				return _container;
			}
		}

		public void BuildReferences()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<NLogModule>();
			builder.RegisterType<MainWindowViewModel>().SingleInstance();

			builder.RegisterType<MainWindow>().SingleInstance();

			builder.RegisterType<DummyApiKeyProvider<KrakenMarket>>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<KrakenMarket>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<CandleHistoryStorage<KrakenMarket>>().AsImplementedInterfaces().SingleInstance();

			_container = builder.Build();
		}


		#region IDisposable

		private readonly object _locker = new();

		bool _disposed;

		// ReSharper disable once UnusedParameter.Local
		private void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			lock (_locker)
			{
				if (_disposed)
					return;
				ReleaseUnmanagedResources();
				_disposed = true;
			}
		}


		private void ReleaseUnmanagedResources()
		{
			try
			{
				_container?.Dispose();
				_container = null;
			}
			catch (Exception ex)
			{
				LogManager.GetCurrentClassLogger()?.Error(ex);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Bootstrapper()
		{
			Dispose(false);
		}

		#endregion
	}
}
