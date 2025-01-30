using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolShop.Models;
using CoolShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoolShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClothDetailPage : ContentPage
    {
        public ApplicationVM Vm { get; private set; }
        public Cloth Model { get; private set; }
        public ClothDetailPage(Cloth cloth, ApplicationVM vm)
        {
            InitializeComponent();
            Vm = vm;
            Model = cloth;
            BindingContext = this;
            if (Model!=null)
            {
                name.Text = Model.Name;
            }
            else
            {
                name.Text = "NULL";
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CartItem cartItem = FindInCart(Model);
            if (cartItem != null)
            {
                while (cartItem.Quantity > 0)
                {
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!Удалено");
                    bool success = CartVM.Remove(Model);
                    if (!success)
                    {
                        break;
                    }
                }
                addtocart.Text = "Добавить в корзину";
            }
            else
            {
                CartVM.Add(Model, 1);
                addtocart.Text = "Убрать из корзины";
            }    
            
            
        }
        private CartItem FindInCart(Cloth model)
        {
            CartItem cartitem = CartVM.ClothesInCart.FirstOrDefault(m => m.Cloth.Id == model.Id);
            if (cartitem != null)
            {
                return cartitem;
            }
            return null;
            
        }
        private void AfterAdd(object sender, EventArgs e)
        {

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //ВЫВОД МАТЕРИАЛОВ
            IEnumerable<Material> materials = await Vm.GetMaterialsByClothId(Model.Id);
            foreach (var material in materials)
            {
                if (material != null)
                {
                    materialList.Text += $"{material.Name} {material.Percentage}% "; 
                }
            }

            //КНОПКА ДОБАВИТЬ/УБРАТЬ
            if (FindInCart(Model) != null)
            {
                addtocart.Text = "Убрать из корзины";
            }
            else
            {
                addtocart.Text = "Добавить в корзину";
            }

            //КАТЕГОРИЯ И ПРОИЗВОДИТЕЛЬ
            switch (Model.Manufacturer)
            {
                case "nk":
                    manufacturer.Text = "Nike";
                    break;
                case "ad":
                    manufacturer.Text = "Adidas";
                    break;
                case "rb":
                    manufacturer.Text = "ReeBok";
                    break;
                case "hm":
                    manufacturer.Text = "H&M";
                    break;
                default:
                    manufacturer.Text = "НЕИЗВЕСТНО";
                    break;
            }
            switch (Model.Category)
            {
                case "fb":
                    category.Text = "Футболки";
                    break;
                case "ob":
                    category.Text = "Обувь";
                    break;
                case "hd":
                    category.Text = "Худи";
                    break;
                    default :
                    category.Text = "Не определено";
                    break;
            }
        }
    }
}