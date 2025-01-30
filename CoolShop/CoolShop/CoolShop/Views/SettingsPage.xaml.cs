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
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
            string savedTheme = Preferences.Get("AppTheme", string.Empty);

            if (savedTheme != null)
            {
                if (savedTheme == "Dark")
                {
                    theme.IsToggled = true;
                }
                else
                {
                    theme.IsToggled = false;
                }
            }

		}

        private void theme_Toggled(object sender, ToggledEventArgs e)
        {
            // Изменение темы приложения в зависимости от состояния переключателя
            if (e.Value) // Если переключатель включен
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark; // Устанавливаем темную тему
                Preferences.Set("AppTheme", "Dark"); // Сохранение темы в настройках
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Light; // Устанавливаем светлую тему
                Preferences.Set("AppTheme", "Light"); // Сохранение темы в настройках
            }
        }
    }
}