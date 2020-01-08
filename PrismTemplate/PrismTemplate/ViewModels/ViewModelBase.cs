using System;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using PrismTemplate.Services;
using IDialogService = PrismTemplate.Services.IDialogService;

namespace PrismTemplate.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService _navigationService { get; private set; }
        protected IDialogService _dialogService { get; private set; }

        public ViewModelBase(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public async Task HandleCommonApiCommonError(int statusCode)
        {
            _dialogService.CloseLoadingDialog();

            switch (statusCode)
            {
                case ApiService.NO_NETWORK_ERROR_CODE:
                    await _dialogService.DisplayAlertAsync("ERROR", "No network!!!", "OK");
                    break;
            }
        }

        public async Task HandleApiError(int statusCode, Action<int> onApiError)
        {
            if (ApiService.HasCommonError(statusCode))
            {
                await HandleCommonApiCommonError(statusCode);
                return;
            }
            onApiError?.Invoke(statusCode);
        }

        public void Destroy()
        {
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
