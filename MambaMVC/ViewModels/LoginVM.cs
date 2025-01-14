using System.ComponentModel.DataAnnotations;

namespace MambaMVC.ViewModels
{
    public class LoginVM
    {
        [MaxLength(256)]

        public string UserNameorEmail {  get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
