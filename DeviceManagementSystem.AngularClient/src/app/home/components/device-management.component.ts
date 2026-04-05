import { CommonModule } from '@angular/common';
import { Component, OnChanges, input, output } from '@angular/core';
import { DeviceDto } from '../../../contracts/device.dto';
import { UserDto } from '../../../contracts/user.dto';
import { ConfirmModalComponent } from '../modals/confirm-modal.component';
import { AssignDeviceModalComponent } from '../modals/assign/assign-device-modal.component';
import { CreateDeviceModalComponent } from '../modals/create/create-device-modal.component';
import { EditDeviceModalComponent } from '../modals/edit/edit-device-modal.component';

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
  modalTitle = '';
  modalMessage = '';
  private pendingAction: 'delete' | 'unassign' | 'assign' | null = null;

  ngOnChanges(): void {
    console.log('DeviceManagement component changed');
  }

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
    const user = this.user();
    if (!device || !user) return;

    this.modalTitle = 'Unassign Device';
    this.modalMessage = `Are you sure you want to unassign "${device.name}" from user "${user.name}"?`;
    this.pendingAction = 'unassign';
    this.showConfirmModal = true;
  }

  onConfirmModalClose(confirmed: any) {
    this.showConfirmModal = false;

    // Only proceed if exactly true
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

  onAssignModalClose(confirmed: any) {
    this.showAssignModal = false;
    this.assignModalClosed.emit();

    // Only proceed if exactly true
    if (confirmed === true) {
      const device = this.selectedDevice();
      if (!device) return;
      this.assigned.emit(device);
    }
  }

  openAssignModal() {
    const device = this.selectedDevice();
    if (!device) return;
    this.modalTitle = 'Assign Device';
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
}
