import { Component, OnInit, computed, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceDto } from '../../../../contracts/device.dto';
import { UserDto } from '../../../../contracts/user.dto';
import { UserService } from '../../../../services/user.service';
import { extractApiErrorMessage } from '../../../../services/api-error.util';
import { ModalComponent } from '../../../components/modal/modal.component';
import { ModalOpts } from '../../../components/modal/modal-opts';

@Component({
  selector: 'app-assign-device-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './assign-device-modal.component.html',
  styleUrls: ['../../../../components/modal/modal.component.scss'],
})
export class AssignDeviceModalComponent extends ModalComponent implements OnInit {
  device = input<DeviceDto | null>(null);
  users = signal<UserDto[]>([]);

  selectedUserId = signal<number | null>(null);
  readonly selectedUser = computed(() => {
    const userId = this.selectedUserId();
    if (userId === null) return null;
    return this.users().find((user) => user.id === userId) ?? null;
  });
  isLoading = signal(false);
  error = signal<string | null>(null);

  constructor(
    protected userService: UserService,
    protected modalOpts: ModalOpts
  ) {
    super(modalOpts);
  }

  override async ngOnInit() {
    this.loadUsers();
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

  selectUser(userId: number) {
    this.selectedUserId.set(userId);
  }

  async confirmAssign() {
    const selectedUser = this.selectedUser();

    if (!selectedUser) {
      this.error.set('Please select a user');
      return;
    }
    this.closeResolved(selectedUser);
  }

  cancelAssign() {
    this.closeResolved(false);
  }
}
