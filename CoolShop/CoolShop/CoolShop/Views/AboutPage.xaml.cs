using CoolShop.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CoolShop.ViewModels;
using System.Threading.Tasks;
using CoolShop.Services;
using System.Collections.Generic;
using System.Linq;

namespace CoolShop.Views
{
    public partial class AboutPage : TabbedPage
    {
        ApplicationVM vm;
        OrderService orderService = new OrderService();
        public AboutPage()
        {
            InitializeComponent();
            vm = ViewModelLocator.ApplicationVM;
            BindingContext = vm;
            clothesList1.ItemSelected += OnItemSelected;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.UserAppTheme == OSAppTheme.Light)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;
            }
        }
        protected override async void OnAppearing()
        {
            
            base.OnAppearing();

            if (Loaded.Clothes.Count == 0)
            {
                await vm.GetClothes();
                foreach (var item in vm.Clothes)
                {
                    Loaded.Clothes.Add(item);
                }
            }
            if (Loaded.Cities.Count == 0)
            {
                IEnumerable<City> cities = await orderService.GetCities();
                foreach (City city in cities)
                {
                    Loaded.Cities.Add(city);
                }
            }
        }
        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                // Сбрасываем выбранный элемент
                ((ListView)sender).SelectedItem = null;

                // Передаем выбранный предмет одежды в метод навигации
                await NavigateToClothDetailPage((Cloth)e.SelectedItem);
            }
        }

        private async Task NavigateToClothDetailPage(Cloth selectedCloth)
        {
            // Передаем выбранный предмет одежды и текущий экземпляр ApplicationVM в ClothDetailPage
            await Navigation.PushAsync(new ClothDetailPage(selectedCloth, vm));
        }
    }
}