using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Domain.Entities
{
    public class User : Entity<int>
    {
        public const int MaxLength = 100;
        public string Name {get; private set;}
        public RoleEnum Role {get; private set;}
        public string Location {get; private set;}

        private User(){}
        public User(string name, RoleEnum role, string location)
        {
            ValidateInput(name, role, location);
            Name = name;
            Role = role;
            Location = location;
        }

        private void ValidateInput(string name, RoleEnum role, string location)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required");
            if(name.Length > MaxLength) throw new Exception($"Name cannot exceed {MaxLength} characters.");
            if(role == default(RoleEnum)) throw new Exception("Role is required");
            if(string.IsNullOrWhiteSpace(location)) throw new Exception("Location is required");
            if(location.Length > MaxLength) throw new Exception($"Location cannot exceed {MaxLength} characters.");
            if(role != RoleEnum.Admin && role != RoleEnum.User) throw new Exception("Role must be either 'Admin' or 'User'");

        }

        public void ChangeName(string newName)
        {
            if(Name == newName) return;
            if(string.IsNullOrWhiteSpace(newName)) throw new Exception("Name is required");
            if(newName.Length > MaxLength) throw new Exception($"Name cannot exceed {MaxLength} characters.");
            Name = newName;
        }

        public void ChangeRole(RoleEnum newRole)
        {
            if(Role == newRole) return;
            if(newRole == default(RoleEnum)) throw new Exception("Role is required");
            if(newRole != RoleEnum.Admin && newRole != RoleEnum.User) throw new Exception("Role must be either 'Admin' or 'User'");
            Role = newRole;
        }

        public void ChangeLocation(string newLocation)
        {
            if(Location == newLocation) return;
            if(string.IsNullOrWhiteSpace(newLocation)) throw new Exception("Location is required");
            if(newLocation.Length > MaxLength) throw new Exception($"Location cannot exceed {MaxLength} characters.");
            Location = newLocation;
        }
    }
}