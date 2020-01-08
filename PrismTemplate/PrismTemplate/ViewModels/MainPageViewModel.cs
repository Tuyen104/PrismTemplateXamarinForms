using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using PrismTemplate.Services;
using PrismTemplate.Views;
using Reactive.Bindings;
using Xamarin.Forms;

namespace PrismTemplate.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand TestCommand { get; }
        
        public MainPageViewModel(INavigationService navigationService, IDialogService dialogService) : base (navigationService, dialogService)
        {
            TestCommand = new AsyncReactiveCommand();
            TestCommand.Subscribe(async () =>
            {
                await _navigationService.NavigateAsync(nameof(NextPage));
            });
        }
    }
}
