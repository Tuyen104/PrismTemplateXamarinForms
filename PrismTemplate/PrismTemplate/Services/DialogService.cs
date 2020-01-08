using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Prism.Common;
using Prism.Services;

namespace PrismTemplate.Services
{
    public enum DialogType
    {

    }
    public interface IDialogService : IPageDialogService
    {
        void CloseLoadingDialog();
        Task<bool> ShowDialogAsync(int statusCode);
    }
    public class DialogService : PageDialogService, IDialogService
    {
        public DialogService(IApplicationProvider applicationProvider) : base(applicationProvider)
        {

        }

        public void CloseLoadingDialog()
        {
            if (UserDialogs.Instance != null)
            {
                if (UserDialogs.Instance.Loading() != null && UserDialogs.Instance.Loading().IsShowing)
                    UserDialogs.Instance.Loading().Hide();
            }
        }
        public async Task<bool> ShowDialogAsync(int statusCode)
        {
            CloseLoadingDialog();
            switch (statusCode)
            {
                case ApiService.NO_NETWORK_ERROR_CODE:
                    await DisplayAlertAsync("ERROR", "No network!!!", "OK");
                    break;
                default:
                    await DisplayAlertAsync("ERROR", "Check api or something error!!!", "OK");
                    break;
            }

            return true;
        }
    }
}
