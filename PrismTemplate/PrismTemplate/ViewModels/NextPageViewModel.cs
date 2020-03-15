using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using PrismTemplate.Services;

namespace PrismTemplate.ViewModels
{
    public class NextPageViewModel : ViewModelBase
    {
        public NextPageViewModel(INavigationService navigationService, IDialogService dialogService):base(navigationService, dialogService)
        {
            try
            {
                var properties = new Dictionary<string, string>
                {
                    { "Log_Event", "NextPage"}
                };
                TrackEvent(properties);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Log_Error", "NextPage"}
                };
                TrackError(ex, properties);
            }
        }

        private async void TrackError(Exception ex, Dictionary<string, string> properties)
        {
            if(await Crashes.IsEnabledAsync())
            {
                Crashes.TrackError(ex, properties);
            }
        }

        private async void TrackEvent(Dictionary<string, string> properties)
        {
            if(await Analytics.IsEnabledAsync())
            {
                Analytics.TrackEvent("NextPage_Event", properties);
            }
        }
    }
}
