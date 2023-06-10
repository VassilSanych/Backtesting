using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NLog;
using VGn.CryptoSdk;

namespace IndicatorsApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private ILogger _logger;
		public static Bootstrapper _bootstrapper;

		protected override void OnStartup(StartupEventArgs e)
		{
			Syncfusion.Licensing.SyncfusionLicenseProvider
				.RegisterLicense(
					SyncfusionLicence.String);

			SubscribeEvents();
			_logger = LogManager.GetCurrentClassLogger();
			
			base.OnStartup(e);

			Build();

			_logger.Info("App started");
		}


		/// <summary>
		///  Формирование структуры классов приложения
		/// </summary>
		private void Build()
		{
			try
			{
				_bootstrapper = new Bootstrapper();
			}
			catch (Exception e)
			{
				_logger.Error(e);
				Console.WriteLine(e);
				throw;
			}
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var mess =
				$"APPLICATION FAILED! AppDomain.CurrentDomain.UnhandledException event, UNHANDLED EXCEPTION" +
				$"{Environment.NewLine}IS TERMINATING:{e.IsTerminating}" +
				$"{Environment.NewLine}EXCEPTION OBJECT:{Environment.NewLine}{e.ExceptionObject}";

			_logger.Fatal(mess);

			//ReportErrorAndRestart((Exception)e.ExceptionObject);
		}

		private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var mess =
				$"APPLICATION FAILED! Dispatcher.UnhandledException event, UNHANDLED EXCEPTION" +
				$"{Environment.NewLine}HANDLED:{e.Handled}" +
				$"{Environment.NewLine}EXCEPTION OBJECT:{Environment.NewLine}{e.Exception}";

			_logger.Fatal(mess);

			//ReportErrorAndRestart(e.Exception);
		}

		private void TaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
		{
			var mess =
				$"UNOBSERVED TASK EXCEPTION! TaskScheduler.UnobservedTaskException event, UNHANDLED EXCEPTION" +
				$"{Environment.NewLine}OBSERVED:{e.Observed}" +
				$"{Environment.NewLine}EXCEPTION OBJECT:{Environment.NewLine}{e.Exception}";

			_logger.Error(mess);
		}

		private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var mess =
				$"APPLICATION FAILED! Application.Current.DispatcherUnhandledException event, UNHANDLED EXCEPTION" +
				$"{Environment.NewLine}HANDLED:{e.Handled}" +
				$"{Environment.NewLine}EXCEPTION OBJECT:{Environment.NewLine}{e.Exception}" +
				$"{Environment.NewLine}APPLICATION WILL BE EXITED WITH CODE 1";

			_logger.Fatal(() => mess);

			//ReportErrorAndRestart(e.Exception);
		}

		/// <summary>
		/// Подписка на события
		/// </summary>
		private void SubscribeEvents()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
		}

		private static void CurrentDomainOnFirstChanceException(object? sender, FirstChanceExceptionEventArgs args)
		{
			//#if TRACE 
			//_logger.Trace(() => args.Exception.ToString());
			//         #endif
		}

	}
}
