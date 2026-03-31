using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Domain.Entities
{
    public  class UserDevice : Entity<int>
    {
        public int UserId { get; private set; }
        public int DeviceId { get; private set; }

        private UserDevice() { }
        public UserDevice(int userId, int deviceId)
        {
            ValidateInput(userId, deviceId);
            UserId = userId;
            DeviceId = deviceId;
        }

        private void ValidateInput(int userId, int deviceId)
        {
            if (userId <= 0) throw new Exception("User ID is required");
            if (deviceId <= 0) throw new Exception("Device ID is required");
        }

    }
}