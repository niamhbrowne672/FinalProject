namespace FinalProject.Data.Entities;

// Add User roles relevant to your application
public enum Role { admin, manager, guest }
    
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DogBreed { get; set; }

    // User role within application
    public Role Role { get; set; }

    //Default to "NoProfileImage.jpg" if no image is provided
    public string ProfileImageUrl { get; set; } = "/images/users/NoProfileImageNew.svg";

    //Registration
    public ICollection<Registration> Registrations { get; set; }
}

