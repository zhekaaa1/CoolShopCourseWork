using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CoolShop.Models;
using CoolShop.ViewModels;
using System.Windows.Input;
using System.ComponentModel.Design.Serialization;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : TabbedPage
    {

        public User Model { get; private set; }
        public ApplicationVM ViewModel { get; private set; }
        public AuthPage()
        {
            InitializeComponent();
            ViewModel = new ApplicationVM() { Navigation = this.Navigation };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ICommand command = ViewModel.LoginCommand;
            if (!string.IsNullOrEmpty(mailtext.Text) && !string.IsNullOrEmpty(passtext.Text))
            {
                User user = new User()
                {
                    Mail = mailtext.Text,
                    Password = passtext.Text
                };
                command.Execute(user);
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            ////ViewModel = new ApplicationVM();
            ICommand command = ViewModel.RegistrateCommand;

            if (!string.IsNullOrEmpty(entry1.Text) &&   //почта
                !string.IsNullOrEmpty(entry2.Text) &&   //пароль
                !string.IsNullOrEmpty(entry3.Text) &&   //повтор
                !string.IsNullOrEmpty(entry4.Text) &&   //фамилия
                !string.IsNullOrEmpty(entry5.Text) &&   //имя
                                                        //entry6.Text МОЖЕТ БЫТЬ ПУСТО          //отчество (м.б. null)
                !string.IsNullOrEmpty(entry7.Text) &&   //телефон
                entry2.Text == entry3.Text
                )
            {
                string oName = entry6.Text == string.Empty ? "" : entry6.Text;
                User user = new User()
                {
                    //ID сам впишется в БД
                    Mail = entry1.Text,
                    Password = entry2.Text,
                    //повтор пароля не записываем
                    FName = entry4.Text,
                    IName = entry5.Text,
                    OName = oName,
                    Phone = entry7.Text,
                };
                command.Execute(user);
            }
            //User user = new User()
            //{
            //    FName = "aa",
            //    IName = "bb",
            //    OName = "cc",
            //    Mail = "mail",
            //    Phone = "654321",
            //    Password = "passwordpp"
            //};
            //command.Execute(user);
        }
    }
}