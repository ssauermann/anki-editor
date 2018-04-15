using System;
using System.Collections.Generic;
using System.Windows;
using AnkiEditor.Query;
using AnkiEditor.ViewModels;
using Caliburn.Micro;

namespace AnkiEditor
{
    public class Bootstrapper : BootstrapperBase
    {
        //private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }
        /*
        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.RegisterPerRequest(typeof(MainViewModel), null, typeof(MainViewModel));
            _container.RegisterSingleton(typeof(IQuery), null, typeof(Nihongodera));
        }*/

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throw e.Exception;
            //MessageBox.Show(e.Exception.ToString());
        }

        /*
        protected override object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }*/
    }
}
