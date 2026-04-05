using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Domain.Entities
{
    public class User : Entity<int>
    {
        public const int MaxLength = 100;
        public string Name { get; private set; }
        public RoleEnum Role { get; private set; }
        public string Location { get; private set; }

        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { }
        public User(string name, RoleEnum role, string location, string email, string passwordHash)
        {
            ValidateInput(name, role, location, email);
            Name = name;
            Role = role;
            Location = location;
            Email = email;
            PasswordHash = passwordHash;
        }


        // Constructor for loading from database
        public User(int id, string name, RoleEnum role, string location, string email, string passwordHash) : base(id)
        {
            ValidateInput(name, role, location, email);
            Name = name;
            Role = role;
            Location = location;
            Email = email;
            PasswordHash = passwordHash;
        }

        private void ValidateInput(string name, RoleEnum role, string location, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("User name is required");
            if (name.Length > MaxLength) throw new Exception($"User name cannot exceed {MaxLength} characters.");
            if (string.IsNullOrWhiteSpace(location)) throw new Exception("Location is required");
            if (location.Length > MaxLength) throw new Exception($"Location cannot exceed {MaxLength} characters.");
            if (role != RoleEnum.Admin && role != RoleEnum.User) throw new Exception("Role must be either 'Admin' or 'User'");
            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Email is required");
            if (email.Length > MaxLength) throw new Exception($"Email cannot exceed {MaxLength} characters.");


        }

        public void ChangeName(string newName)
        {
            if (Name == newName) return;
            if (string.IsNullOrWhiteSpace(newName)) throw new Exception("User name is required");
            if (newName.Length > MaxLength) throw new Exception($"User name cannot exceed {MaxLength} characters.");
            Name = newName;
        }

        public void ChangeRole(RoleEnum newRole)
        {
            if (Role == newRole) return;
            if (newRole == default(RoleEnum)) throw new Exception("Role is required");
            if (newRole != RoleEnum.Admin && newRole != RoleEnum.User) throw new Exception("Role must be either 'Admin' or 'User'");
            Role = newRole;
        }

        public void ChangeLocation(string newLocation)
        {
            if (Location == newLocation) return;
            if (string.IsNullOrWhiteSpace(newLocation)) throw new Exception("User location is required");
            if (newLocation.Length > MaxLength) throw new Exception($"User location cannot exceed {MaxLength} characters.");
            Location = newLocation;
        }

    }
}