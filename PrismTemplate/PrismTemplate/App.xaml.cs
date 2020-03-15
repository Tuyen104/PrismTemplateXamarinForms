using Prism;
using Prism.Ioc;
using Prism.Unity;
using PrismTemplate.Services;
using PrismTemplate.ViewModels;
using PrismTemplate.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PrismTemplate
{
    public partial class App : PrismApplication
    {
        //public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<NextPage, NextPageViewModel>();

            //register services
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<IConfigurationService, ConfigurationService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            AppCenter.Start("ios=d7b45b5d-0498-449d-a415-86ce3eb10780;" +
                  "android=646efe20-b1f5-4369-bd06-3eb9743b19e0",
                  typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));
            await Distribute.SetEnabledAsync(true);

            bool didAppCrashed = await Crashes.HasCrashedInLastSessionAsync();
            if (didAppCrashed)
            {
                var crashReport = await Crashes.GetLastSessionCrashReportAsync();
            }
        }
    }
}
