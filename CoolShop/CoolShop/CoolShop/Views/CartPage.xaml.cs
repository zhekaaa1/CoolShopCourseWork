using CoolShop.Models;
using CoolShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<CartItem> _cartItems;
        public ObservableCollection<CartItem> CartItems
        {
            get { return _cartItems; }
            set
            {
                if (_cartItems != value)
                {
                    _cartItems = value;
                    OnPropertyChanged(nameof(CartItems));
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!CartItems изменено");
                }
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CartPage()
        {
            BindingContext = this;
            InitializeComponent();
        }

        private async void Submit_Button_Clicked(object sender, EventArgs e)
        {
            if (LoggedUser.User == null)
            {
                await Shell.Current.GoToAsync($"//ProfilePage");
                await Navigation.PushAsync(new AuthPage());
            }
            await Navigation.PushAsync(new MakeOrderPage());
        }
        private async void ToMain()
        {
            // Выполнить переход на главную страницу (AboutPage)
            await Shell.Current.GoToAsync($"//AboutPage");
        }
        protected override void OnAppearing()
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!вызван onAppearing");
            base.OnAppearing();
            CartItems = CartVM.ClothesInCart;
            // Определите максимальное количество элементов, которые вы хотите отобразить без прокрутки


            if (CartItems.Count == 0)
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!cartitems = 0");
                CartIsEmpty();
            }
            else
            {
                CartItems = CartVM.ClothesInCart;
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!cartitems != 0");
                foreach (var item in CartItems)
                {
                    Console.WriteLine($"CartItem: {item.Cloth.Name}, Quantity: {item.Quantity}");
                }
                foreach (var item in CartVM.ClothesInCart)
                {
                    Console.WriteLine($"ClothInCart: {item.Cloth.Name}, Quantity: {item.Quantity}");
                }

                CartIsNotEmpty();
            }
        }

        private void CartIsEmpty()
        {
            base.OnAppearing();
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!! ПУСТАЯ КОРЗИНА");
            /*Удалить дочерние элементы MAINSCROLLVIEW
             И Занести туда лейбл: Корзина пуста, на главную
             */



            // Создание форматированной строки с обычным текстом и ссылкой
            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span { Text = "Корзина пуста, " });
            var spanLink = new Span { Text = "на главную", TextColor = Color.Blue, TextDecorations = TextDecorations.Underline };
            spanLink.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ToMain)
            });
            formattedString.Spans.Add(spanLink);

            double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

            // Создание лейбла с форматированной строкой
            var emptyCartLabel = new Label
            {
                FormattedText = formattedString,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 20, 0, 0)
            };
            var emptyCartImage = new Image
            {
                Margin = new Thickness(0, screenHeight / 10, 0, 0),
                Source = "emptycart.png",
                HeightRequest = 150
            };
            MAINSTACKLAYOUT2.Children.Clear();
            MAINSTACKLAYOUT2.Children.Add(emptyCartImage);
            MAINSTACKLAYOUT2.Children.Add(emptyCartLabel);
            MAINSTACKLAYOUT2.IsVisible = true;
            MAINSTACKLAYOUT.IsVisible = false;
            // Добавление лейбла в ScrollView
        }
        private void CartIsNotEmpty()
        {
            MAINSTACKLAYOUT.IsVisible = true;
            MAINSTACKLAYOUT2.IsVisible = false;

            Console.WriteLine($"!!!!!!!!!!!!!!!{CartItems.Count}");
            foreach (var item in CartItems)
            {
                Console.WriteLine($"Cloth: {item.Cloth.Name}, Quantity: {item.Quantity}");
            }
            Console.WriteLine($"!!!!!!!!!!!!!!!{CartVM.ClothesInCart.Count}");
            foreach (var item in CartVM.ClothesInCart)
            {
                Console.WriteLine($"Cloth: {item.Cloth.Name}, Quantity: {item.Quantity}");
            }
            cartitemslw.ItemsSource = null;
            
            CartItems = CartVM.ClothesInCart;
            cartitemslw.ItemsSource = CartItems;

            Console.WriteLine("!!!!!!!!!!!!!!!! ПОСЛЕ ОБНОВЛЕНИЯ");
            Console.WriteLine($"!!!!!!!!!!!!!!!{CartItems.Count}");
            foreach (var item in CartItems)
            {
                Console.WriteLine($"Cloth: {item.Cloth.Name}, Quantity: {item.Quantity}");
            }
            Console.WriteLine($"!!!!!!!!!!!!!!!{CartVM.ClothesInCart.Count}");
            foreach (var item in CartVM.ClothesInCart)
            {
                Console.WriteLine($"Cloth: {item.Cloth.Name}, Quantity: {item.Quantity}");
            }


            RefactorListView();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Button minusButton = sender as Button;
            StackLayout parentLayout = minusButton.Parent as StackLayout;
            Label quantityLabel = parentLayout.FindByName<Label>("quantityNum");
            CartItem cartItem = (CartItem)minusButton.BindingContext;
            CartVM.Remove(cartItem.Cloth);
            quantityLabel.Text = $"{cartItem.Quantity}";
            Console.WriteLine($"!!!!!!!!!!!!!!!!!!!ФАКТ: {cartItem.Quantity}");
            OnPropertyChanged(nameof(CartItems));
            RefactorListView();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Button addButton = sender as Button;
            StackLayout parentLayout = addButton.Parent as StackLayout;
            Label otherLabel = parentLayout.FindByName<Label>("quantityNum");
            CartItem cartItem = (CartItem)addButton.BindingContext;
            CartVM.Add(cartItem.Cloth, 1);
            otherLabel.Text = $"{cartItem.Quantity}";
            Console.WriteLine($"!!!!!!!!!!!!!!!!!!!ФАКТ: {cartItem.Quantity}");
            OnPropertyChanged(nameof(CartItems));

            RefactorListView();
        }
        private void RefactorListView()
        {
            SUBMIT.IsEnabled = true;
            double totalCost = 0;

            foreach (var cartItem1 in CartItems)
            {
                // Получаем стоимость одной позиции (товара)
                double itemCost = cartItem1.Cloth.Price * cartItem1.Quantity;

                // Добавляем стоимость позиции к общей стоимости заказа
                totalCost += itemCost;
            }

            cost.Text = totalCost.ToString();
            int maxItemsToShowWithoutScroll = 3; // Примерное значение, которое вам нужно подобрать

            // Высота одного элемента списка
            double itemHeight = 200; // Примерное значение, которое вам нужно подобрать
            if (CartVM.ClothesInCart.Count <= maxItemsToShowWithoutScroll)
            {
                // Устанавливаем фиксированную высоту для ListView
                cartitemslw.HeightRequest = itemHeight * CartVM.ClothesInCart.Count;
            }
            else
            {
                // Снимаем ограничение на высоту
                cartitemslw.HeightRequest = -1;
            }
            if (CartVM.ClothesInCart.Count==0)
            {
                SUBMIT.IsEnabled = false;
            }


            totalquantity.Text = CartItems.Count.ToString();
        }
    }
}