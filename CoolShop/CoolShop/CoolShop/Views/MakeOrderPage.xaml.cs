using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using CoolShop.Models;
using CoolShop.Services;
using CoolShop.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MakeOrderPage : ContentPage
	{
        private OrderService orderService  = new OrderService();
        private ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();
		public MakeOrderPage ()
		{
			InitializeComponent ();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //ВЫВОД ИЗ КОРЗИНЫ
            cartItems.Clear();
            foreach (var item in CartVM.ClothesInCart)
            {
                cartItems.Add(item);
            }
            double itemHeight = 140; // Примерное значение, которое вам нужно подобрать
            cartitemz.ItemsSource = cartItems;
            cartitemz.HeightRequest = itemHeight * CartVM.ClothesInCart.Count;

            //ВЫВОД ГОРОДОВ
            IEnumerable<City> cityList = Loaded.Cities;
            List<string> cityNames = new List<string>();
            foreach (City city in cityList)
            {
                cityNames.Add(city.Name);
            }
            cities.ItemsSource = cityNames;
            cities.SelectedIndex = 0;

            //ВЫВОД СВОДКИ
            double totalquantitythere = 0;
            double totalcostthere = 0;
            foreach (CartItem cartItem in cartItems)
            {
                totalquantitythere += cartItem.Quantity;
                totalcostthere += cartItem.Quantity * cartItem.Cloth.Price;
            }
            totalquantity.Text = totalquantitythere.ToString();
            totalcost.Text = totalcostthere.ToString();
            if (totalcostthere >= 3500)
            {
                delmethod.Text = "Бесплатно";
                
            }
            else
            {
                totalcostthere += 200;
                totalcost.Text = totalcostthere.ToString();
                delmethod.Text = "200 ₽";
            }

            receiverIName.Text = $"{LoggedUser.User.IName}";
            receiverPhone.Text = $"+7{LoggedUser.User.Phone.Substring(1)}";
            
        }
        private void SUBMIT_Clicked(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("https://text-host.ru/bez-zagolovka-10549"));
        }
        public static (Field[], bool) ValidateFields(Field[] fields)
        {
            /*
            НА ВВОД ИДЕТ МАССИВ ПОЛЕЙ. ОДИН ИЗ ЭКЗЕМПЛЯРОВ ПОЛЕЙ:
              *  НАЗВАНИЕ ПОЛЯ
              *  ЗНАЧЕНИЕ
              *  ТИП ЗНАЧЕНИЯ
              *  СТРОКА ОШИБКИ
              *  ИМЯ ПОДСКАЗКИ
              *  ПРАВИЛЬНОСТЬ ЗАПОЛНЕНИЯ
            ВЫВОДИМ МОДИФИЦИРОВАННЫЙ МАССИВ И БУЛЕВОЕ "ВСЕ ВАЛИДНО".
             */

            bool isAllValid = true;
            Regex passregex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$");
            Regex emailregex = new Regex("^[a-zA-Z0-9_-]+(?:\\.[a-zA-Z0-9_-]+)*@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+){0,1}\\.[a-zA-Z]{2,}$");
            Regex nameregex = new Regex("[A-Za-z\\s]+$");
            Regex loginregex = new Regex("^[a-zA-Z0-9_-]{3,16}$");

            foreach (Field f in fields)
            {
                switch (f.TypeOfValue)
                {
                    case "строка":
                        switch (f.Name)
                        {
                            case "логин":
                                if (string.IsNullOrEmpty(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: заполните логин";
                                }
                                else if (!loginregex.IsMatch(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: некорректный логин. Логин должен содержать только буквы верхнего и нижнего регистра, цифры, символы подчеркивания (_) и дефисы (-). Длина логина должна быть от 3 до 16 символов.";
                                }
                                break;
                            case "пароль":
                                if (string.IsNullOrEmpty(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: заполните пароль";
                                }
                                else if (!passregex.IsMatch(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: пароль должен содержать как минимум одну букву в верхнем и нижнем регистрах, одну цифру и один специальный символ, и иметь длину не менее 8 символов.";
                                }
                                break;
                            case "имя":
                                if (string.IsNullOrEmpty(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: заполните имя";
                                }
                                else if (!nameregex.IsMatch(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: используйте только буквы";
                                }
                                break;
                            case "почта":
                                if (string.IsNullOrEmpty(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: заполните почту";
                                }
                                else if (!emailregex.IsMatch(f.Value))
                                {
                                    isAllValid = false;
                                    f.ErrString = "Ошибка: некорректная почта. Пишите в формате example@mail.com, exam_ple1.alo@mail.ru, example@mail.sd.ru";
                                }
                                break;
                            
                        }
                        break;
                    case "число":
                        if (f.Name == "номер")
                        {
                            if (string.IsNullOrEmpty(f.Value))
                            {
                                isAllValid = false;
                                f.ErrString = "Ошибка: заполните номер";
                            }
                        }
                        if (f.Name == "индекс")
                        {
                            if (string.IsNullOrEmpty(f.Value))
                            {
                                isAllValid = false;
                                f.ErrString = "Ошибка: заполните индекс";
                            }
                        }    
                        break;
                    
                        
                }
            }
            return (fields, isAllValid);
        }

        
    }
    public class Field
    {
        public string Name { get; set; }            //НАЗВАНИЕ ПОЛЯ
        public string Value { get; set; }           //ЗНАЧЕНИЕ
        public string TypeOfValue { get; set; }     //ТИП ЗНАЧЕНИЯ
        public string ErrString { get; set; }       //СТРОКА ОШИБКИ
        public string ErrLabelName { get; set; }    //ИМЯ ПОДСКАЗКИ
        public bool IsValid { get; set; }           //ПРАВИЛЬНОСТЬ ЗАПОЛНЕНИЯ
        public Field(string name, string value, string typeofvalue, string errString, string errLabelName, bool isValid)
        {
            Name = name;
            Value = value;
            TypeOfValue = typeofvalue;
            ErrString = errString;
            ErrLabelName = errLabelName;
            IsValid = isValid;
        }
    }
}
