using System;
using Xamarin.Forms;

namespace PrismTemplate.Views
{
    public partial class NextPage : ContentPage
    {
        public NextPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<object, string>(this, App.NotifReceivedKey, OnMessageReceived);
            btnSend.Clicked += OnBtnSendClicked;
        }

        void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblMsg.Text = msg;
            });
        }

        void OnBtnSendClicked(object sender, EventArgs e)
        {
            //
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object>(this, App.NotifReceivedKey);
        }
    }
}
