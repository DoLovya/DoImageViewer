using DoImageViewer.App.ViewModels;
using DoImageViewer.App.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace DoImageViewer.App
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
    }
}
