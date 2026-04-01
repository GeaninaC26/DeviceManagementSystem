import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output, OnChanges, signal, WritableSignal } from "@angular/core";
import { DeviceDto } from "../../../contracts/device.dto";
import { UserDto } from "../../../contracts/user.dto";

@Component({
  selector: 'app-device-management',
  imports: [CommonModule],
  templateUrl: './device-management.component.html',
  styleUrls: ['./device-management.component.scss'],
  standalone: true
})
export class DeviceManagementComponent implements OnChanges {
  @Input() selectedDevice: DeviceDto | null = null;
  @Input() user: UserDto | null = null;
  @Output() cancelled = new EventEmitter<void>();

  ngOnChanges(): void {
    console.log('DeviceManagement component changed');
  }
}
