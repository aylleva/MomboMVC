using MambaMVC.Models.Base;

namespace MambaMVC.Models
{
    public class Position:BaseEntity
    {
        public string Name {  get; set; }
        public List<Employee> Employees { get; set; }
    }
}
