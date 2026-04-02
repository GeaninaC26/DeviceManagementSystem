import { Component, OnInit, signal, computed } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDto } from '../../contracts/user.dto';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { UserDeviceService } from '../../services/user-device.service';
import { DeviceDto } from '../../contracts/device.dto';
import { DeviceManagementComponent } from './components/device-management.component';
@Component({
  selector: 'app-home',
  imports: [CommonModule, DeviceManagementComponent],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home implements OnInit {
  readonly users = signal<UserDto[]>([]);
  readonly devices = signal<DeviceDto[]>([]);
  readonly userDevices = signal<any[]>([]);
  readonly selectedDevice = signal<DeviceDto | null>(null);
  readonly assignedUser = computed(() => {
    const selected = this.selectedDevice();
    if (!selected) return null;

    // Find the user-device mapping
    const userDevice = this.userDevices().find(ud => ud.deviceId === selected.id);
    if (!userDevice) return null;

    // Find the user details
    return this.users().find(u => u.id === userDevice.userId) || null;
  });
  readonly deviceSearchText = signal('');
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);
  readonly currentPage = signal(1);
  readonly itemsPerPage = 5;

  readonly paginatedDevices = computed(() => {
    const allDevices = this.devices();
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return allDevices.slice(start, end);
  });

  readonly totalPages = computed(() => {
    return Math.ceil(this.devices().length / this.itemsPerPage);
  });
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
  clearSelection(): void {
    this.selectedDevice.set(null);
  }

  async onDeviceDeleted(device: DeviceDto) {
    try {
      this.isLoading.set(true);
      await this.deviceService.deleteDevice(device.id);
      this.clearSelection();
      await this.loadData();
    } catch (err) {
      console.error('Failed to delete device:', err);
      this.error.set('Failed to delete device.');
      this.isLoading.set(false);
    }
  }

  async onDeviceUnassigned(device: DeviceDto) {
    try {
      console.log('Unassigning device:', device);
      this.isLoading.set(true);
      console.log('Current user-device associations before unassigning:', this.userDevices());
      const ud = this.userDevices().find((u: any) => u.deviceId === device.id);
      console.log('Found user-device association to delete:', ud);
      if (ud) {
        await this.userDeviceService.deleteUserDevice(ud.id);
      }
      await this.loadData();
    } catch (err) {
      console.error('Failed to unassign device:', err);
      this.error.set('Failed to unassign device.');
      this.isLoading.set(false);
    }
  }

  onDeviceCreated(confirmed: any) {
    if (confirmed === true) {
      this.loadData();
    }
    this.loadData();
  }

  onConfirmModalClose(confirmed: any) {
    if (confirmed === true) {
      this.loadData();
    }
  }

  onCreateModalClosed() {
    this.loadData();
  }


  onDeviceAssigned(_: DeviceDto) {
    this.loadData();
  }

  onAssignModalClosed() {
    this.loadData();
  }

  goToPage(page: number) {
    const totalPages = this.totalPages();
    if (page >= 1 && page <= totalPages) {
      this.currentPage.set(page);
    }
  }

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(p => p + 1);
    }
  }

  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update(p => p - 1);
    }
  }
}
