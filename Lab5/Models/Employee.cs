namespace Lab4.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Position { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
