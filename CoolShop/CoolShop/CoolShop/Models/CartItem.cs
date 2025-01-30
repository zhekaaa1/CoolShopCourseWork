using System;
using System.Collections.Generic;
using System.Text;

namespace CoolShop.Models
{
    public class CartItem
    {
        public Cloth Cloth { get; set; }
        public int Quantity { get; set; }

        public CartItem(Cloth cloth, int quantity)
        {
            Cloth = cloth;
            Quantity = quantity;
        }
    }
}
