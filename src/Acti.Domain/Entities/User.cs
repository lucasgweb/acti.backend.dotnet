namespace Acti.Domain.Entities;

public class User : Base
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public  string ResetToken { get; set; }
}