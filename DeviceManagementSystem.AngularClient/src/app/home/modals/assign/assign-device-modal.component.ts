import { Component, OnInit, computed, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceDto } from '../../../../contracts/device.dto';
import { UserDto } from '../../../../contracts/user.dto';
import { UserService } from '../../../../services/user.service';
import { DeviceService } from '../../../../services/device.service';
import { extractApiErrorMessage } from '../../../../services/api-error.util';
import { ModalComponent } from '../../../components/modal/modal.component';
import { ModalOpts } from '../../../components/modal/modal-opts';

@Component({
  selector: 'app-assign-device-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './assign-device-modal.component.html',
  styleUrls: ['../../../components/modal/modal.component.scss'],
})
export class AssignDeviceModalComponent extends ModalComponent implements OnInit {
  mode = input<'admin' | 'user'>('admin');
  device = input<DeviceDto | null>(null);
  users = signal<UserDto[]>([]);
  devices = signal<DeviceDto[]>([]);

  selectedUserId = signal<number | null>(null);
  selectedDeviceId = signal<number | null>(null);

  readonly selectedUser = computed(() => {
    const userId = this.selectedUserId();
    if (userId === null) return null;
    return this.users().find((user) => user.id === userId) ?? null;
  });

  readonly selectedDevice = computed(() => {
    const deviceId = this.selectedDeviceId();
    if (deviceId === null) return null;
    return this.devices().find((device) => device.id === deviceId) ?? null;
  });

  readonly isAdminMode = computed(() => this.mode() === 'admin');
  isLoading = signal(false);
  error = signal<string | null>(null);

  constructor(
    protected userService: UserService,
    protected deviceService: DeviceService,
    protected modalOpts: ModalOpts
  ) {
    super(modalOpts);
  }

  override async ngOnInit() {
    await this.loadModalData();
  }

  async loadModalData() {
    if (this.isAdminMode()) {
      await this.loadUsers();
      return;
    }

    await this.loadUnassignedDevices();
  }

  async loadUsers() {
    try {
      this.isLoading.set(true);
      this.error.set(null);
      this.users.set(await this.userService.getUsers());
    } catch (error) {
      console.error('Error fetching users:', error);
      this.error.set(extractApiErrorMessage(error, 'Failed to fetch users.'));
    } finally {
      this.isLoading.set(false);
    }
  }

  async loadUnassignedDevices() {
    try {
      this.isLoading.set(true);
      this.error.set(null);
      this.devices.set(await this.deviceService.getUnassignedDevices());
    } catch (error) {
      console.error('Error fetching unassigned devices:', error);
      this.error.set(extractApiErrorMessage(error, 'Failed to fetch unassigned devices.'));
    } finally {
      this.isLoading.set(false);
    }
  }

  selectUser(userId: number) {
    this.selectedUserId.set(userId);
  }

  selectDevice(deviceId: number) {
    this.selectedDeviceId.set(deviceId);
  }

  async confirmAssign() {
    if (this.isAdminMode()) {
      const selectedUser = this.selectedUser();
      if (!selectedUser) {
        this.error.set('Please select a user.');
        return;
      }
      this.closeResolved(selectedUser);
      return;
    }

    const selectedDevice = this.selectedDevice();
    if (!selectedDevice) {
      this.error.set('Please select a device.');
      return;
    }

    this.closeResolved(selectedDevice);
  }

  cancelAssign() {
    this.closeResolved(false);
  }
}
