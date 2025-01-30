using CoolShop.Models;
using CoolShop.Services;
using System;
using Xamarin.Forms;

namespace CoolShop.Views
{
    public partial class PaymentPage : ContentPage
    {
        public static string TotalCost = "";
        public Order Order = new Order();
        public OrderService orderService = new OrderService();
        public PaymentPage(Order order)
        {
            InitializeComponent();
            Order = order;
            // Добавляем обработчик для кнопки оплаты
            webView.Navigated += WebView_Navigated;
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith("invokecsharpcode://"))
            {
                var data = e.Url.Substring("invokecsharpcode://".Length);
                string datastr = data.ToString();
                var parts = data.Split('/');
                if (parts.Length == 4)
                {
                    var cardNumber = parts[0];
                    var expiryMonth = parts[1];
                    var expiryYear = parts[2];
                    var cvv = parts[3];


                    // Здесь вы можете использовать данные для выполнения действий в C#
                    await Console.Out.WriteLineAsync($"!!!!!!!!!!!!!!!!!!Полученные данные!!!!!!!!!!!!!!!!" +
                        $"\nНомер карты: {cardNumber}" +
                        $"\nСрок действия: {expiryMonth}/{expiryYear}" +
                        $"\nCVV/CVC: {cvv}" +
                        $"\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                    Order.Status = "P";

                    Order pound = await orderService.Update(Order);

                    await Navigation.PushAsync(new SuccessMadeOrderPage(pound));
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось получить корректные данные", "OK");
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TotalCost = Order.Cost.ToString();
            string orderid = Order.Id.ToString("D6");

            var htmlSource = new HtmlWebViewSource
            {
                Html = @"<!DOCTYPE html>
            <html>
            <body>
            <style>
                .t {
                    margin-left: 10px;
                }
            </style>
            <h3 style='text-align: center;'>COOLSHOP Pay</h3>
            <div class='t'>
                <p style='font-weight: bold;'>Сумма к оплате заказа CS-"+orderid+@": "+TotalCost+@" ₽</p>
                <p>Номер карты:</p>
                <input id='cardNumber' maxlength='16' oninput='validateTelInput(this)' type='text'/>
                <p>Срок действия:</p>
                <select id='expiryMonth'>
                    <option value=''>Месяц</option>
                    <option value='01'>01</option>
                    <option value='02'>02</option>
                    <option value='03'>03</option>
                    <option value='04'>04</option>
                    <option value='05'>05</option>
                    <option value='06'>06</option>
                    <option value='07'>07</option>
                    <option value='08'>08</option>
                    <option value='09'>09</option>
                    <option value='10'>10</option>
                    <option value='11'>11</option>
                    <option value='12'>12</option>
                </select>
                <select id='expiryYear'>
                    <option value=''>Год</option>
                    <option value='24'>24</option>
                    <option value='25'>25</option>
                    <option value='26'>26</option>
                    <option value='27'>27</option>
                    <option value='28'>28</option>
                    <option value='29'>29</option>
                    <option value='30'>30</option>
                </select>
                <p>CVV/CVC:</p>
                <input id='cvv' maxlength='3' oninput='validateTelInput(this)' type='text'/>
                <br/>
                <br/>
                <button onclick='getValues()'>Оплатить</button>
            </div>
            <script>
                function luhnCheck(cardNumber) {
                    let sum = 0;
                    let shouldDouble = false;
                    for (let i = cardNumber.length - 1; i >= 0; i--) {
                        let digit = parseInt(cardNumber.charAt(i));
                        if (shouldDouble) {
                            digit *= 2;
                            if (digit > 9) digit -= 9;
                        }
                        sum += digit;
                        shouldDouble = !shouldDouble;
                    }
                    return (sum % 10) === 0;
                }

                function validateCardNumber(cardNumber) {
                    return /^\d{13,19}$/.test(cardNumber) && luhnCheck(cardNumber);
                }

                function validateTelInput(input) {
                    if (!/^\d+$/.test(input.value)) {
                        input.value = input.value.replace(/[^\d]/g, '');
                    }
                }

                function validateExpiryDate(month, year) {
                    if (!month || !year) return false;
                    let now = new Date();
                    let expiry = new Date(`20${year}`, month - 1);
                    return expiry > now;
                }

                function validateCVV(cvv) {
                    return /^\d{3,4}$/.test(cvv);
                }

                function getValues() {
                    var cardNumber = document.getElementById('cardNumber').value;
                    var expiryMonth = document.getElementById('expiryMonth').value;
                    var expiryYear = document.getElementById('expiryYear').value;
                    var cvv = document.getElementById('cvv').value;

                    if (!validateCardNumber(cardNumber)) {
                        alert('Неверный номер карты.');
                        return;
                    }
                    if (!validateExpiryDate(expiryMonth, expiryYear)) {
                        alert('Неверный срок действия карты.');
                        return;
                    }
                    if (!validateCVV(cvv)) {
                        alert('Неверный CVV/CVC код.');
                        return;
                    }

                    window.location.href = 'invokecsharpcode://' + cardNumber + '/' + expiryMonth + '/' + expiryYear + '/' + cvv;
                }
            </script>
            </body>
            </html>"
            };

            webView.Source = htmlSource;
        }
    }

    public class PaymentData
    {
        public string cardNumber { get; set; }
        public string expiryDate { get; set; }
        public string cvv { get; set; }
    }
}
