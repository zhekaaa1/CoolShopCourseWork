using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoolShop.Models;
using CoolShop.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrdersPage : ContentPage
    {
        public OrderService OrderService = new OrderService();
        public ClothesInOrderService ClothesInOrdersService = new ClothesInOrderService();
        public ObservableCollection<OneOrder> OrderList = new ObservableCollection<OneOrder>();
        public ObservableCollection<ClothesInOrders> ClothesInOrdersList = new ObservableCollection<ClothesInOrders>();

        public ICommand ItemTappedCommand { get; private set; }


        public MyOrdersPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetOrders();
        }
        protected override bool OnBackButtonPressed()
        {
            GoToProfilePage();
            return false;
        }
        private async void GetOrders()
        {
            await GetOrdersAsync();
        }

        private async Task<bool> GetOrdersAsync()
        {
            IEnumerable<Order> orders = await OrderService.GetOrdersByUserId(LoggedUser.User.Id);
            if (orders.Any())
            {
                foreach (Order order in orders)
                {
                    OneOrder oneOrder = new OneOrder()
                    {
                        Order = order,
                        TotalCost = order.Cost.ToString()

                    };
                    if (order.Status == "P")
                    {
                        oneOrder.Status = "Оплачено";
                        oneOrder.Action = "Детали >";
                    }
                    else if (order.Status == "NP")
                    {
                        oneOrder.Status = "Не оплачено";
                        oneOrder.Action = "Оплатить >";
                    }
                    else if (order.Status == "G")
                    {
                        oneOrder.Status = "Доставлено";
                        oneOrder.Action = "Детали >";
                    }
                    else if (order.Status == "DEL")
                    {
                        oneOrder.Status = "Доставляется";
                        oneOrder.Action = "Детали >";
                    }
                    else if (order.Status == "DEC")
                    {
                        oneOrder.Status = "Отменен";
                        oneOrder.Status = "";
                    }
                    IEnumerable<ClothesInOrders> cio = await ClothesInOrdersService.GetClothesInOrder(order.Id);
                    int count = 0;
                    foreach (ClothesInOrders c in cio)
                    {
                        ClothesInOrdersList.Add(c);
                        count += c.ClothAmount;
                    }
                    oneOrder.TotalQuantity = count.ToString();
                    OrderList.Add(oneOrder);
                }
                orderlist.ItemsSource = OrderList;
                return true;
            }
            orderlist.ItemsSource = OrderList;
            OrdersIsEmpty();
            return false;
        }

        public async void ToMain()
        {
            // Выполнить переход на главную страницу (AboutPage)
            await Shell.Current.GoToAsync($"//AboutPage");
        }
        public async Task Details(Order order)
        {
            await Navigation.PushAsync(new SuccessMadeOrderPage(order, ClothesInOrdersList));
        }
        public async Task ToPaymentPage(Order order)
        {
            await Navigation.PushAsync(new PaymentPage(order));
        }
        public async void GoToProfilePage()
        {
            await Shell.Current.GoToAsync("///ProfilePage");
        }
        private void OrdersIsEmpty()
        {
            base.OnAppearing();
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!! ПУСТАЯ КОРЗИНА");
            /*Удалить дочерние элементы MAINSCROLLVIEW
             И Занести туда лейбл: Корзина пуста, на главную
             */



            // Создание форматированной строки с обычным текстом и ссылкой
            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span { Text = "Заказов нет," });
            var spanLink = new Span { Text = " на главную", TextColor = Color.Blue, TextDecorations = TextDecorations.Underline };
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
            EMPTYSL.Children.Clear();
            EMPTYSL.Children.Add(emptyCartImage);
            EMPTYSL.Children.Add(emptyCartLabel);
            EMPTYSL.IsVisible = true;
            ORDERSSL.IsVisible = false;
            // Добавление лейбла в ScrollView
        }
        
    }
    public class OneOrder
    {
        public Order Order { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string TotalCost { get; set; }
        public string TotalQuantity { get; set; }

        public ICommand OrderActionCommand { get; private set; }

        public OneOrder()
        {
            OrderActionCommand = new Command(ExecuteOrderAction);
        }

        private async void ExecuteOrderAction()
        {
            var myOrdersPage = (MyOrdersPage)Application.Current.MainPage.Navigation.NavigationStack.Last();
            if (Status == "Оплачено" || Status == "Доставлено" || Status == "Доставляется" || Status == "Отменен")
            {
                await myOrdersPage.Details(Order);
            }
            else if (Status == "Не оплачено")
            {
                await myOrdersPage.ToPaymentPage(Order);
            }
            else if (Status == "Отменен")
            {
                return;
            }    
        }
        
    }
}