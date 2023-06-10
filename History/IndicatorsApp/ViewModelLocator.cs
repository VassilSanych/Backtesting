/*
  In App.xaml:
    <Application.Resources>
        <ResourceDictionary>
        <historyLoadUi:ViewModelLocator x:Key="Locator" /> <!--d:IsDataSource="True"-->
        </ResourceDictionary>
    </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace IndicatorsApp
{


	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{
		static ViewModelLocator()
		{
			if (App._bootstrapper.Container == null)
				return;
			var csl = new AutofacServiceLocator(App._bootstrapper.Container);
			ServiceLocator.SetLocatorProvider(() => csl);
		}


		public MainWindowViewModel MainWindowViewModel => Resolve<MainWindowViewModel>();


		public T Resolve<T>() where T : notnull
		{
			try
			{
				return App._bootstrapper.Container.Resolve<T>();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

		}
	}
}