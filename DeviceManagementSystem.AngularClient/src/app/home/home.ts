import { ChangeDetectorRef, Component, OnInit, signal, WritableSignal } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDto } from '../../contracts/user.dto';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { UserDeviceService } from '../../services/user-device.service';
import { DeviceDto } from '../../contracts/device.dto';
import { UserDeviceDto } from '../../contracts/user-device.dto';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home implements OnInit {
  users: WritableSignal<UserDto[]> = signal([]);
  devices: WritableSignal<DeviceDto[]> = signal([]);
  userDevices: WritableSignal<UserDeviceDto[]> = signal([]);
  selectedDevice: WritableSignal<DeviceDto | null> = signal(null);
  selectedUser: WritableSignal<UserDto | null> = signal(null);
  deviceSearchText: WritableSignal<string> = signal('');
  isLoading: WritableSignal<boolean> = signal(false);
  error: WritableSignal<string | null> = signal(null);
  constructor(private userService: UserService,
              private deviceService: DeviceService,
              private userDeviceService: UserDeviceService) {
              }

  ngOnInit(): void {
    this.loadData();
  }
  private async loadData() {
    try {
      this.isLoading.set(true);
      this.error.set(null);

      this.users.set(await this.userService.getUsers());
      this.devices.set(await this.deviceService.getDevices());
      this.userDevices.set(await this.userDeviceService.getUserDevices());
      console.log('Users:', this.users());
      console.log('Devices:', this.devices());
      console.log('User-Device Associations:', this.userDevices());
    } catch (error) {
      console.error('Error fetching users:', error);
      this.error.set('Failed to fetch users.');
    } finally {
      this.isLoading.set(false);
    }

  }

  onDeviceSearchTextChange(text: string) {
    this.deviceSearchText.set(text);
  }

  selectDevice(device: DeviceDto) : void {
    this.selectedDevice.set(device);
  }
}
