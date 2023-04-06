namespace Acti.Application.Dtos;

public class UserDTO
{
    public UserDTO()
    {
    }

    public UserDTO(int id, string name, string email, string password, string avatar, DateTime createdAt,
        DateTime updatedAt, string resetToken)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Avatar = avatar;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        ResetToken = resetToken;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public string? Password { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}