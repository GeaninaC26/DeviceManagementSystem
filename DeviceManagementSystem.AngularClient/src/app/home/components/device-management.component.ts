import { CommonModule } from '@angular/common';
import { Component, OnChanges, input, output } from '@angular/core';
import { DeviceDto } from '../../../contracts/device.dto';
import { UserDto } from '../../../contracts/user.dto';
import { RoleEnum } from '../../../contracts/enums/role.enum';
import { ConfirmModalComponent } from '../modals/confirm-modal.component';
import { AssignDeviceModalComponent } from '../modals/assign/assign-device-modal.component';
import { CreateDeviceModalComponent } from '../modals/create/create-device-modal.component';
import { EditDeviceModalComponent } from '../modals/edit/edit-device-modal.component';
import { UserDeviceService } from '../../../services/user-device.service';
import { extractApiErrorMessage } from '../../../services/api-error.util';

@Component({
  selector: 'app-device-management',
  imports: [
    CommonModule,
    ConfirmModalComponent,
    AssignDeviceModalComponent,
    CreateDeviceModalComponent,
    EditDeviceModalComponent,
  ],
  templateUrl: './device-management.component.html',
  styleUrls: ['./device-management.component.scss'],
  standalone: true,
})
export class DeviceManagementComponent implements OnChanges {
  readonly selectedDevice = input<DeviceDto | null>(null);
  readonly user = input<UserDto | null>(null);
  readonly cancelled = output<void>();
  readonly deleted = output<DeviceDto>();
  readonly unassigned = output<DeviceDto>();
  readonly assigned = output<DeviceDto>();
  readonly created = output<DeviceDto>();
  readonly assignModalClosed = output<void>();
  readonly currentUser = input<UserDto | null>(null);

  showConfirmModal = false;
  showAssignModal = false;
  showCreateModal = false;
  showEditModal = false;
  isAssigning = false;
  assignError: string | null = null;
  assignModalMode: 'admin' | 'user' = 'admin';
  modalTitle = '';
  modalMessage = '';
  private pendingAction: 'delete' | 'unassign' | 'assign' | null = null;

  constructor(private userDeviceService: UserDeviceService) {}

  ngOnChanges(): void {}

  openDeleteModal() {
    const device = this.selectedDevice();
    if (!device) return;

    this.modalTitle = 'Delete Device';
    this.modalMessage = `Are you sure you want to permanently delete the device "${device.name}"? This action cannot be undone.`;
    this.pendingAction = 'delete';
    this.showConfirmModal = true;
  }

  openUnassignModal() {
    const device = this.selectedDevice();
    var user;
    if (!this.isAdmin()) {
      user = this.user();
    } else {
      user = this.currentUser();
    }
    if (!device || !user) return;

    this.modalTitle = 'Unassign Device';
    this.modalMessage = `Are you sure you want to unassign "${device.name}" from user "${user.name}"?`;
    this.pendingAction = 'unassign';
    this.showConfirmModal = true;
  }

  onConfirmModalClose(confirmed: any) {
    this.showConfirmModal = false;

    if (confirmed === true && this.pendingAction) {
      const device = this.selectedDevice();
      if (!device) return;

      if (this.pendingAction === 'delete') {
        this.deleted.emit(device);
      } else if (this.pendingAction === 'unassign') {
        this.unassigned.emit(device);
      }
    }

    this.pendingAction = null;
  }

  async onAssignModalClose(selection: UserDto | DeviceDto | false | null | undefined) {
    this.showAssignModal = false;
    this.assignModalClosed.emit();
    this.assignError = null;

    if (!selection) return;

    if (this.isAdmin()) {
      if (!this.isUserDto(selection)) return;
      const device = this.selectedDevice();
      if (!device) return;
      await this.assignDevice(device, selection);
      return;
    }

    if (!this.isDeviceDto(selection)) return;
    const currentUser = this.currentUser();
    if (!currentUser) return;

    await this.assignDevice(selection, currentUser);
  }

  openAssignModal() {
    if (this.isAdmin()) {
      const device = this.selectedDevice();
      if (!device) return;
      this.assignModalMode = 'admin';
      this.modalTitle = 'Assign Device';
    } else {
      this.assignModalMode = 'user';
      this.modalTitle = 'Assign Device to Me';
    }

    this.showAssignModal = true;
  }

  openCreateModal() {
    this.modalTitle = 'Create Device';
    this.showCreateModal = true;
  }

  onCreateModalClose(confirmed: any) {
    this.showCreateModal = false;
    if (confirmed === true) {
      this.created.emit(null as any);
    }
  }

  openEditModal() {
    const device = this.selectedDevice();
    if (!device) return;
    this.showEditModal = true;
  }

  onEditModalClose(confirmed: any) {
    this.showEditModal = false;
    if (confirmed === true) {
      const device = this.selectedDevice();
      if (!device) return;
      this.created.emit(device);
    }
  }

  async assignDevice(device: DeviceDto, user: UserDto) {
    try {
      this.isAssigning = true;
      await this.userDeviceService.assignDeviceToUser({ userId: user.id, deviceId: device.id });
      this.assigned.emit(device);
    } catch (err) {
      this.assignError = extractApiErrorMessage(err, 'Failed to assign device.');
    } finally {
      this.isAssigning = false;
    }
  }

  isAdmin(): boolean {
    return this.currentUser()?.role === RoleEnum.Admin;
  }

  private isUserDto(value: UserDto | DeviceDto): value is UserDto {
    return 'location' in value;
  }

  private isDeviceDto(value: UserDto | DeviceDto): value is DeviceDto {
    return 'manufacturer' in value;
  }
}
