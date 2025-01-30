using CoolShop.Models;
using CoolShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangeAvatar : ContentPage
	{
        public ApplicationVM vm { get; set; }
		public ChangeAvatar ()
		{
			InitializeComponent ();
            vm = new ApplicationVM () { Navigation = Navigation };
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ICommand command = vm.UpdateUserCommand;
            if (sender is Image tappedImage)
            {
                var imageSource = tappedImage.Source;
                Console.WriteLine($"________________________You tapped on image with source: {imageSource}");
                string imgsrc = imageSource.ToString()
                                           .Substring(6);
                Console.WriteLine($"________________________Если короче то: {imgsrc}");
                LoggedUser.User.ProfileImg = imgsrc;
                command.Execute(LoggedUser.User);
                await Shell.Current.GoToAsync("//ProfilePage");
            }

        }
    }
}