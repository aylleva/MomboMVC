using System.ComponentModel.DataAnnotations;

namespace MambaMVC.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Name {  get; set; }
        [MinLength(6)]
        [MaxLength(50)]
        public string Surname {  get; set; }

        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(6)]
        [MaxLength(100)]
        public string UserName {  get; set; }

        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))] 
        public string ConfirmPassword {  get; set; }

    }
}
