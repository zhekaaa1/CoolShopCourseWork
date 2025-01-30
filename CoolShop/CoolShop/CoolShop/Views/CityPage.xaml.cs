using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolShop.Models;
using CoolShop.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CityPage : ContentPage
	{
		public OrderService orderService = new OrderService();
		public ObservableCollection<City> cityList = new ObservableCollection<City>();
		public CityPage ()
		{
			InitializeComponent ();
			BindingContext = this;
			cities.ItemSelected += OnItemSelected;
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            cityList.Clear ();
			foreach (var city in Loaded.Cities) 
            {
                cityList.Add(city);
            }
			cities.ItemsSource = cityList;
        }
        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                // Сбрасываем выбранный элемент
                ((ListView)sender).SelectedItem = null;

                // Передаем выбранный предмет одежды в метод навигации
                Selected.City = (City)e.SelectedItem;
                await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!ВЫБРАН ГОРОД: {Selected.City.Name}");
                await Navigation.PopAsync();
            }
        }
    }
	public static class Selected
	{
		public static City City { get; set; }
	}
}