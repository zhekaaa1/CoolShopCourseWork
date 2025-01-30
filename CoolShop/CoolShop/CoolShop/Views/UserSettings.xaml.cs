using CoolShop.Models;
using CoolShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettings : ContentPage
    {
        public User Model { get; private set; }
        public ApplicationVM ViewModel { get; private set; }

        public UserSettings()
        {
            InitializeComponent();

            ViewModel = new ApplicationVM() { Navigation = this.Navigation };
            fName.Text = LoggedUser.User.FName;
            iName.Text = LoggedUser.User.IName;
            oName.Text = LoggedUser.User.OName;
            phone.Text = LoggedUser.User.Phone;
            mail.Text = LoggedUser.User.Mail;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            ICommand command = ViewModel.UpdateUserCommand;
            User usertoupd = new User()
            {
                Id = LoggedUser.User.Id,
                FName = fName.Text,
                IName = iName.Text,
                OName = oName.Text,
                Phone = phone.Text,
                Mail = mail.Text,
                Password = LoggedUser.User.Password
            };
            Console.WriteLine(usertoupd.Id);
            command.Execute(usertoupd);
        }
        
        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            LoggedUser.User = null;
            ReloadPage();
        }
        private void ReloadPage()
        {
            Navigation.PushAsync(new ProfilePage());
        }
    }
}