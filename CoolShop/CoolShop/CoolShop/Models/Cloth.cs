using System;
using System.Collections.Generic;
using System.Text;

namespace CoolShop.Models
{
    public class Cloth
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
    }
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
    }
    public class MaterialInCloth
    {
        public int ClothId { get; set; }
        public int MaterialId { get; set; }
    }
}
