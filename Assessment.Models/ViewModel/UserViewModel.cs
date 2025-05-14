using System.ComponentModel.DataAnnotations;

namespace Assessment.Models.ViewModel;

public class UserViewModel
{
    public int Id { get; set;}
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [RegularExpression(@"^(?!@)([A-Za-z0-9]+(?:[._%+-]*[A-Za-z0-9]+)*)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [RegularExpression(@"^\S*$", ErrorMessage = "Password cannot contain spaces.")]
    public string Password { get; set; } = null!;

    public bool IsRememberMe { get; set; }

    public string? RoleName { get; set; }
}
