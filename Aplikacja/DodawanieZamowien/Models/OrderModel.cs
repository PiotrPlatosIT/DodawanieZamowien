using System;
using System.Collections.Generic;

namespace DodawanieZamowien.Models
{
    public class OrderModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirdth { get; set; }

        public List<OrderItemModel> Items = new List<OrderItemModel>();
    }
}
