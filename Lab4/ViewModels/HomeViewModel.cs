using Lab4.Models;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class HomeViewModel
    {
        public List<AdType> AdTypes { get; set; }
        public List<AdditionalService> AdditionalServices { get; set; }
        public List<Client> Clients { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Location> Locations { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderService> OrderServices { get; set; }
    }
}
