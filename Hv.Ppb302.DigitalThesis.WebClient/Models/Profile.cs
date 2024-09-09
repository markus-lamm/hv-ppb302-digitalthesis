namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class Profile
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? NewPassword { get; set; }
    public string? OldPassword { get; set; }
}
