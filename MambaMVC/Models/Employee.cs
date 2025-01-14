using MambaMVC.Models.Base;

namespace MambaMVC.Models
{
    public class Employee:BaseEntity
    {
        public string Name {  get; set; }
        public int PositionId {  get; set; }
        public Position Position { get; set; }

        public List<EmployeeImage> EmployeeImages { get; set; }

    }
}
