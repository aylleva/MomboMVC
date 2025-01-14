using MambaMVC.Models;

namespace MambaMVC.Areas.ViewModels.Employee
{
    public class UpdateEmployeeVM
    {
        public IFormFile? Photo { get; set; }
        public string? Name { get; set; }
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
        public List<EmployeeImage> Images { get; set; }
    }
}
