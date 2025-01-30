using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoolShop.Models;
using CoolShop.Services;
using CoolShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public User Model { get; private set; }
        public ApplicationVM ViewModel = new ApplicationVM();

        public ProfilePage(User model, ApplicationVM viewModel)
        {
            base.OnAppearing();
            InitializeComponent();
            Model = model;
            ViewModel = viewModel;
            BindingContext = this;

            ToLoginButton.IsVisible = false;

            MyFeedbacks.IsVisible = true;
            MyOrders.IsVisible = true;
            MyProfile.IsVisible = true;
        }
        public ProfilePage()
        {
            InitializeComponent();

            base.OnAppearing();
            if (LoggedUser.User == null)
            {
                iName.Text = "Войдите";
                oName.Text = "";

                ToLoginButton.IsVisible = true;

                MyFeedbacks.IsVisible = false;
                MyOrders.IsVisible = false;
                MyProfile.IsVisible = false;
            }
            else
            {
                iName.Text = LoggedUser.User.IName;
                oName.Text = LoggedUser.User.OName;

                ToLoginButton.IsVisible = false;

                MyFeedbacks.IsVisible = true;
                MyOrders.IsVisible = true;
                MyProfile.IsVisible = true;

            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Selected.City = Loaded.Cities[0];
        }

        

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage());
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserSettings());
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CityPage());
        }
    }
}