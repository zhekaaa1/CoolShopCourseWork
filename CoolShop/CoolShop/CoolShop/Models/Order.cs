using System;
using System.Collections.Generic;
using System.Text;

namespace CoolShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Cost { get; set; }
        public string Address { get; set; }
        public string ReserveEmail { get; set; }
        public string Status { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public Order(int id, double cost, string address, string reserveEmail, string status, int cityId, int userId)
        {
            Id = id;
            Cost = cost;
            Address = address;
            ReserveEmail = reserveEmail;
            Status = status;
            CityId = cityId;
            UserId = userId;
        }
    }
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
    public class ClothesInOrders
    {
        public int OrderId { get; set; }
        public int ClothId { get; set; }
        public int ClothAmount { get; set; }
    }
}
