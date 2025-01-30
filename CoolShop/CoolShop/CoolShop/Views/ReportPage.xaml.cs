using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
	{
		public ReportPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var description = DescriptionEditor.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(description))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите действительный адрес электронной почты.", "OK");
                return;
            }

            try
            {
                var message = new EmailMessage
                {
                    Subject = "Репорт",
                    Body = description + $"\n\nReply to: {email}" +
                    $"\n\nName: {DeviceInfo.Name}" +
                    $"\nManufacturer: {DeviceInfo.Manufacturer}" +
                    $"\nPlatform: {DeviceInfo.Platform}" +
                    $"\nVersion: {DeviceInfo.Version}" +
                    $"\nDevice Type: {DeviceInfo.DeviceType}" +
                    $"\nIdiom: {DeviceInfo.Idiom}",
                    To = new List<string> { "zhenya100505@gmail.com" },
                };
                await Email.ComposeAsync(message);
            }

            catch (FeatureNotSupportedException)
            {
                // Email is not supported on this device
                await DisplayAlert("Ошибка", "Отправка электронной почты не поддерживается на этом устройстве.", "OK");
            }

            catch (Exception ex)
            {
                // Some other exception occurred
                await DisplayAlert("Ошибка", $"Произошла ошибка при отправке электронной почты.\n\n{ex.Message}", "OK");
            }
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}