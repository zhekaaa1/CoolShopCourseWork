using System;
using System.Collections.Generic;
using System.Text;

namespace CoolShop.Services
{
    public class Constants
    {
        /*
                ПРИ ПЕРЕНОСЕ ИЛИ СМЕНЕ ПОДКЛЮЧЕНИЯ - МЕНЯЕМ СТРОКУ MainDomain
                http://xxx.xxx.xxx.xxx:5000/api/
                ГДЕ xxx.xxx.xxx.xxx - ip адрес v4
                НАЙТИ МОЖНО В КОМАНДНОЙ СТРОКЕ -> ipconfig

                !!! НЕ УКАЗЫВАТЬ LOCALHOST !!!
         */
        private const string MainDomain = "http://192.168.1.3:5000/api/";
        public const string UsersApiRoot = MainDomain + "user/";
        public const string ClothesApiRoot = MainDomain + "cloth/";
        public const string MaterialsApiRoot = MainDomain + "material/";
        public const string MaterialsInClothesApiRoot = MainDomain + "materialincloth/";
        public const string CitiesApiRoot = MainDomain + "city/";
        public const string OrdersApiRoot = MainDomain + "order/";
        public const string ClothesInOrdersApiRoot = MainDomain + "clothinorder/";

    }
}
