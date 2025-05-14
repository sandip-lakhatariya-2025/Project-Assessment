namespace Assessment.Models.ViewModel;

public class UserViewModel
{
    public int Id { get; set;}
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsRememberMe { get; set; }
}
