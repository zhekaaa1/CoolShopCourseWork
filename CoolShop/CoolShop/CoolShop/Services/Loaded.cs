using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoolShop.Models;

namespace CoolShop.Services
{
    public static class Loaded
    {
        public static ObservableCollection<Cloth> Clothes { get; set; } = new ObservableCollection<Cloth>();
        public static ObservableCollection<City> Cities { get; set; } = new ObservableCollection<City>();
    }
}
