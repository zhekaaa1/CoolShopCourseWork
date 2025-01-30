using CoolShop.Models;
using CoolShop.Services;
using CoolShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationPage : ContentPage
    {
        private User _user;
        private UserService _userService;
        public ApplicationVM vm = new ApplicationVM();

        public VerificationPage(User user, ApplicationVM vm)
        {
            InitializeComponent();
            _user = user;
            _userService = new UserService();
            this.vm = vm;
        }

        private async void OnVerifyClicked(object sender, EventArgs e)
        {
            string code = CodeEntry.Text;

            if (string.IsNullOrEmpty(code))
            {
                ErrorLabel.Text = "Пожалуйста, введите код подтверждения.";
                ErrorLabel.IsVisible = true;
                return;
            }

            bool isVerified = await _userService.Verify(_user.Mail, code);

            if (isVerified)
            {
                await DisplayAlert("Успех", "Ваш email успешно подтвержден.", "OK");

                // Переход к странице профиля
                LoggedUser.User = _user;
                Preferences.Set("LoggedUserId", $"{LoggedUser.User.Id}");
                ProfilePage profilePage = new ProfilePage(LoggedUser.User, vm);
                NavigationPage.SetHasBackButton(profilePage, false);
                await Navigation.PushAsync(profilePage);
            }
            else
            {
                ErrorLabel.Text = "Неверный код подтверждения.";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}
