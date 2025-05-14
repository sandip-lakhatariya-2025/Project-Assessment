namespace Assessment.Models.Models;

public class User
{
    public int Id { get; set;}
    public int RoleId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public virtual Role Role { get; set; } = null!;
}
