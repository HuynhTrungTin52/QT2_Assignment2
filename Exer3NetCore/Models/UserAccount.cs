namespace Exer3NetCore.Models;

public class UserAccount
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
}
