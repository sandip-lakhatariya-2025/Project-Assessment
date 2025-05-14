namespace Assessment.Models.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    public virtual ICollection<User> Users { get; } = new List<User>();
}
