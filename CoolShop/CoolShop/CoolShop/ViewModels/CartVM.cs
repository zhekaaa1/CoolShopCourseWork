using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoolShop.Models;

namespace CoolShop.ViewModels
{
    
    public static class CartVM
    {
        public static ObservableCollection<CartItem> ClothesInCart = new ObservableCollection<CartItem>();
        public static bool Add(Cloth cloth, int quantity)
        {
            if (cloth != null && quantity > 0)
            {
                // Проверяем, есть ли уже такой предмет в корзине
                var existingItem = ClothesInCart.FirstOrDefault(item => item.Cloth.Id == cloth.Id);
                if (existingItem != null)
                {
                    Console.WriteLine($"!!!!!!!!!!!!!!!!!ДОБАВЛЕНО {existingItem.Cloth.Name}, +1 шт.");
                    // Если уже есть, увеличиваем количество
                    existingItem.Quantity += quantity;
                    Console.WriteLine($"СЕЙЧАС {existingItem.Quantity} шт.");
                }
                else
                {
                    CartItem newcartitem = new CartItem(cloth, quantity);
                    // Если нет, добавляем новый элемент
                    ClothesInCart.Add(newcartitem);
                    Console.WriteLine($"!!!!!!!!!!!!!!!!!ДОБАВЛЕНО {newcartitem.Cloth.Name} ОДНА ШТУКА");
                }
                return true;
            }
            return false;
        }
        public static bool Remove(Cloth cloth)
        {
            if (cloth != null)
            {
                // Находим элемент корзины
                var itemToRemove = ClothesInCart.FirstOrDefault(item => item.Cloth.Id == cloth.Id);
                if (itemToRemove != null)
                {
                    // Если количество больше 1, уменьшаем количество на 1
                    if (itemToRemove.Quantity > 1)
                    {
                        itemToRemove.Quantity--;
                    }
                    else
                    {
                        // Если количество равно 1, удаляем элемент из корзины
                        ClothesInCart.Remove(itemToRemove);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
