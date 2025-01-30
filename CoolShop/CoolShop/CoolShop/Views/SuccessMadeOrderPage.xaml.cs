using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolShop.Models;
using CoolShop.Services;
using CoolShop.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessMadeOrderPage : ContentPage
    {
        public static Order Order { get; set; }
        public ApplicationVM vm = new ApplicationVM();
        private OrderService orderService = new OrderService();
        private ClothesInOrderService clothesInOrderService = new ClothesInOrderService();
        private ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();
        public SuccessMadeOrderPage(Order order)
        {
            InitializeComponent();
            Order = order;
            orderid1.Text = $"CS-{order.Id:D6}";
            receivedate1.Text = $"{order.ReceiveDate}";
            MAINSL2.IsVisible = false;
            MAINSL1.IsVisible = true;
        }
        public SuccessMadeOrderPage(Order order, ObservableCollection<ClothesInOrders> clothesInOrders)
        {
            InitializeComponent();
            foreach (ClothesInOrders cio in clothesInOrders)
            {
                Console.WriteLine($"**************** на вход пришло: ClId: {cio.ClothId}, OrId {cio.OrderId}, Amount: {cio.ClothAmount}");
            }
            Console.WriteLine($"Заказ #{order.Id:D6}");
            MAINSL1.IsVisible = false;
            MAINSL2.IsVisible = true;
            Order = order;
            orderid2.Text = $"CS-{order.Id:D6}";
            receivedate2.Text = $"{order.ReceiveDate}";

            if (order.Status == "P")
            {
                success2.Text = "Ваш заказ принят!";
                
            }
            else if (order.Status == "G")
            {
                success2.Text = "Закак доставлен!";
                deltext.IsVisible = false;
            }
            else if (order.Status == "DEL")
            {
                success2.Text = "Заказ доставляется!";
                deltext.IsVisible = true;
            }

            //ВЫВОД ИЗ КОРЗИНЫ
            cartItems.Clear();

            ObservableCollection<ClothesInOrders> clothesInOrders1 = new ObservableCollection<ClothesInOrders>();
            foreach (ClothesInOrders item in clothesInOrders)
            {
                if (order.Id == item.OrderId)
                {
                    clothesInOrders1.Add(item);
                }
            }
            if (clothesInOrders1.Count !=0)
            {
                foreach (var item in clothesInOrders1)
                {
                    Cloth c = Loaded.AllClothes.FirstOrDefault(l => l.Id == item.ClothId);
                    cartItems.Add(new CartItem(c, item.ClothAmount));
                }
                double itemHeight = 140; // Примерное значение, которое вам нужно подобрать
                cartitemz.ItemsSource = cartItems;
                cartitemz.HeightRequest = itemHeight * cartItems.Count;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            if (CartVM.ClothesInCart.Count == 0)
            {
                OnOrdersButtonClicked();
                return false;
            }
            CartVM.ClothesInCart = new System.Collections.ObjectModel.ObservableCollection<CartItem>();
            GoBack();
            return true;
        }
        private async void OnOrdersButtonClicked()
        {
            await Shell.Current.GoToAsync("MyOrdersPage");
        }
        protected override void OnDisappearing()
        {
            CartVM.ClothesInCart = new System.Collections.ObjectModel.ObservableCollection<CartItem>();
            base.OnDisappearing();
        }
        private async void GoBack()
        {
            await Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            GoBack();
        }
    }
}