using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Domain.Entities
{
    public class User : Entity<int>
    {
        public const int MaxLength = 100;
        public string UserName {get; private set;}
        public RoleEnum Role {get; private set;}
        public string UserLocation {get; private set;}

        private User(){}
        public User(string userName, RoleEnum role, string location)
        {
            ValidateInput(userName, role, location);
            UserName = userName;
            Role = role;
            UserLocation = location;
        }

        // Constructor for loading from database
        public User(int id, string userName, RoleEnum role, string location) : base(id)
        {
            ValidateInput(userName, role, location);
            UserName = userName ;
            Role = role;
            UserLocation = location;
        }

        private void ValidateInput(string userName, RoleEnum role, string location)
        {
            if(string.IsNullOrWhiteSpace(userName)) throw new Exception("User name is required");
            if(userName.Length > MaxLength) throw new Exception($"User name cannot exceed {MaxLength} characters.");
            if(role == default(RoleEnum)) throw new Exception("Role is required");
            if(string.IsNullOrWhiteSpace(location)) throw new Exception("Location is required");
            if(location.Length > MaxLength) throw new Exception($"Location cannot exceed {MaxLength} characters.");
            if(role != RoleEnum.Admin && role != RoleEnum.User) throw new Exception("Role must be either 'Admin' or 'User'");

        }

        public void ChangeName(string newName)
        {
            if(UserName == newName) return;
            if(string.IsNullOrWhiteSpace(newName)) throw new Exception("User name is required");
            if(newName.Length > MaxLength) throw new Exception($"User name cannot exceed {MaxLength} characters.");
            UserName = newName;
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
            if(UserLocation == newLocation) return;
            if(string.IsNullOrWhiteSpace(newLocation)) throw new Exception("User location is required");
            if(newLocation.Length > MaxLength) throw new Exception($"User location cannot exceed {MaxLength} characters.");
            UserLocation = newLocation;
        }
    }
}