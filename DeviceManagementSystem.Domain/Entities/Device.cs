using System.Runtime.CompilerServices;
using DeviceManagementSystem.Domain.Core;

namespace DeviceManagementSystem.Domain.Entities
{
    public class Device : Entity<int>
    {
        public string DeviceName { get; private set; }
        public string Manufacturer { get; private set; }
        public string DeviceType { get; private set; }
        public string OS { get; private set; }
        public string OSVersion { get; private set; }
        public string Processor { get; private set; }
        public string RAM { get; private set; }
        public string Description { get; private set; }

        private Device() { }
        public Device(string deviceName, string manufacturer, string deviceType, string os, string osVersion, string processor, string ram, string description)
        {
            ValidateInput(deviceName, manufacturer, deviceType, os, osVersion, processor, ram, description);
            DeviceName = deviceName;
            Manufacturer = manufacturer;
            DeviceType = deviceType;
            OS = os;
            OSVersion = osVersion;
            Processor = processor;
            RAM = ram;
            Description = description;
        }

        // Constructor for loading from database
        public Device(int id, string deviceName, string manufacturer, string deviceType, string os, string osVersion, string processor, string ram, string description) : base(id)
        {
            ValidateInput(deviceName, manufacturer, deviceType, os, osVersion, processor, ram, description);
            DeviceName = deviceName;
            Manufacturer = manufacturer;
            DeviceType = deviceType;
            OS = os;
            OSVersion = osVersion;
            Processor = processor;
            RAM = ram;
            Description = description;
        }

        private void ValidateInput(string deviceName, string manufacturer, string deviceType, string os, string osVersion, string processor, string ram, string description)
        {
            if (string.IsNullOrWhiteSpace(deviceName)) throw new Exception("Device name is required");
            if (string.IsNullOrWhiteSpace(manufacturer)) throw new Exception("Manufacturer is required");
            if (string.IsNullOrWhiteSpace(deviceType)) throw new Exception("Device type is required");
            if (string.IsNullOrWhiteSpace(os)) throw new Exception("Operating system is required");
            if (string.IsNullOrWhiteSpace(osVersion)) throw new Exception("OS version is required");
            if (string.IsNullOrWhiteSpace(processor)) throw new Exception("Processor is required");
            if (string.IsNullOrWhiteSpace(ram)) throw new Exception("RAM is required");
            if (string.IsNullOrWhiteSpace(description)) throw new Exception("Description is required");

        }

        public void ChangeDeviceName(string newName)
        {
            if (DeviceName == newName) return;
            if (string.IsNullOrWhiteSpace(newName)) throw new Exception("Device name is required");
            DeviceName = newName;
        }

        public void ChangeManufacturer(string newManufacturer)
        {
            if (Manufacturer == newManufacturer) return;
            if (string.IsNullOrWhiteSpace(newManufacturer)) throw new Exception("Manufacturer is required");
            Manufacturer = newManufacturer;
        }

        public void ChangeDeviceType(string newDeviceType)
        {
            if (DeviceType == newDeviceType) return;
            if (string.IsNullOrWhiteSpace(newDeviceType)) throw new Exception("Device type is required");
            DeviceType = newDeviceType;
        }

        public void ChangeOS(string newOS)
        {
            if (OS == newOS) return;
            if (string.IsNullOrWhiteSpace(newOS)) throw new Exception("Operating system is required");
            OS = newOS;
        }

        public void ChangeOSVersion(string newOSVersion)
        {
            if (OSVersion == newOSVersion) return;
            if (string.IsNullOrWhiteSpace(newOSVersion)) throw new Exception("OS version is required");
            OSVersion = newOSVersion;
        }

        public void ChangeProcessor(string newProcessor)
        {
            if (Processor == newProcessor) return;
            if (string.IsNullOrWhiteSpace(newProcessor)) throw new Exception("Processor is required");
            Processor = newProcessor;
        }

        public void ChangeRAM(string newRAM)
        {
            if (RAM == newRAM) return;
            if (string.IsNullOrWhiteSpace(newRAM)) throw new Exception("RAM is required");
            RAM = newRAM;
        }

        public void ChangeDescription(string newDescription)
        {
            if (Description == newDescription) return;
            if (string.IsNullOrWhiteSpace(newDescription)) throw new Exception("Description is required");
            Description = newDescription;
        }
    }
}