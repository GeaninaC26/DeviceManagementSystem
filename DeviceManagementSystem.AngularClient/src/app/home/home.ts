import { Component, OnInit, signal, computed } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDto } from '../../contracts/user.dto';
import { CommonModule } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { UserDeviceService } from '../../services/user-device.service';
import { DeviceDto } from '../../contracts/device.dto';
import { DeviceManagementComponent } from './components/device-management.component';
import { AuthService } from '../../services/auth.service';
import { extractApiErrorMessage } from '../../services/api-error.util';
import { RoleEnum } from '../../contracts/enums/role.enum';
@Component({
  selector: 'app-home',
  imports: [CommonModule, DeviceManagementComponent],
  templateUrl: './home.html',
  styleUrls: ['./home.scss'],
})
export class Home implements OnInit {
  readonly currentUser = computed(() => this.authService.currentUser());
  readonly users = signal<UserDto[]>([]);
  readonly devices = signal<DeviceDto[]>([]);
  readonly userDevices = signal<any[]>([]);
  readonly selectedDevice = signal<DeviceDto | null>(null);
  readonly assignedUser = computed(() => {
    const selected = this.selectedDevice();
    if (!selected) return null;

    // Find the user-device mapping
    const userDevice = this.userDevices().find((ud) => ud.deviceId === selected.id);
    if (!userDevice) return null;

    // Find the user details
    return this.users().find((u) => u.id === userDevice.userId) || null;
  });
  readonly deviceSearchText = signal<string | null>(null);
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

  readonly safeTotalPages = computed(() => {
    return Math.max(this.totalPages(), 1);
  });

  readonly hasDevices = computed(() => this.devices().length > 0);

  readonly hasVisibleDevices = computed(() => this.paginatedDevices().length > 0);
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private deviceService: DeviceService,
    private userDeviceService: UserDeviceService,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }
  private async loadData() {
    try {
      const currentUser = this.currentUser();
      if (!currentUser) {
        this.error.set('No authenticated user found.');
        return;
      }
      if (this.currentUser().role === RoleEnum.Admin) {
        this.isLoading.set(true);
        this.error.set(null);
        this.users.set(await this.userService.getUsers());
        this.devices.set(await this.deviceService.getDevices(this.deviceSearchText()?.toString()));
        this.userDevices.set(await this.userDeviceService.getUserDevices());
      } else {
        this.isLoading.set(true);
        this.error.set(null);
        const userId = currentUser.id;
        this.devices.set(
          await this.deviceService.getDevicesForUser(userId, this.deviceSearchText()?.toString()),
        );
        const userDevices = await this.userDeviceService.getUserDevices();
        this.userDevices.set(userDevices.filter((ud: any) => ud.userId === userId));
      }

      const selected = this.selectedDevice();
      if (selected) {
        const refreshedSelected = this.devices().find((d) => d.id === selected.id) ?? null;
        this.selectedDevice.set(refreshedSelected);
      }

      const totalPages = this.totalPages();
      if (totalPages === 0) {
        this.currentPage.set(1);
      } else if (this.currentPage() > totalPages) {
        this.currentPage.set(totalPages);
      }
    } catch (error) {
      console.error('Error fetching users:', error);
      this.error.set(extractApiErrorMessage(error, 'Failed to fetch users.'));
    } finally {
      this.isLoading.set(false);
    }
  }

  onDeviceSearchTextChange(text: string) {
    this.deviceSearchText.set(text);
    this.currentPage.set(1);
    this.loadData();
  }

  selectDevice(device: DeviceDto): void {
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
      this.error.set(extractApiErrorMessage(err, 'Failed to delete device.'));
      this.isLoading.set(false);
    }
  }

  async onDeviceUnassigned(device: DeviceDto) {
    try {
      this.isLoading.set(true);
      const currentUser = this.currentUser();
      const ud = this.userDevices().find((u: any) => {
        if (u.deviceId !== device.id) return false;
        if (!currentUser || currentUser.role === RoleEnum.Admin) return true;
        return u.userId === currentUser.id;
      });

      if (ud) {
        await this.userDeviceService.deleteUserDevice(ud.id);
      } else {
        this.error.set('Could not find assignment for the selected device.');
        return;
      }
      await this.loadData();
    } catch (err) {
      console.error('Failed to unassign device:', err);
      this.error.set(extractApiErrorMessage(err, 'Failed to unassign device.'));
    } finally {
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
      this.currentPage.update((p) => p + 1);
    }
  }

  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((p) => p - 1);
    }
  }
}
