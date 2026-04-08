import { Component, OnInit, computed, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../components/modal/modal.component';
import { DeviceDto } from '../../../../contracts/device.dto';
import { UserDto } from '../../../../contracts/user.dto';
import { UserService } from '../../../../services/user.service';
import { UserDeviceService } from '../../../../services/user-device.service';
import { ModalOpts } from '../../../components/modal/modal-opts';
import { DeviceService } from '../../../../services/device.service';
import { form, FormField, required, submit } from '@angular/forms/signals';
import { extractApiErrorMessage } from '../../../../services/api-error.util';
import { GenerateDeviceDescriptionCommand } from '../../../../contracts/commands/description-generation.command';

interface CreateDeviceModel {
  serialNumber: string;
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ram: string;
  description: string;
}

@Component({
  selector: 'app-create-device-modal',
  standalone: true,
  imports: [CommonModule, FormField],
  templateUrl: './create-device-modal.component.html',
  styleUrls: ['../../../components/modal/modal.component.scss'],
})
export class CreateDeviceModalComponent extends ModalComponent implements OnInit {
  device = input<DeviceDto | null>(null);
  error = signal<string | null>(null);
  isGeneratingDescription = signal(false);
  constructor(
    protected deviceService: DeviceService,
    protected modalOpts: ModalOpts,
  ) {
    super(modalOpts);
  }

  createModel = signal<CreateDeviceModel>({
    serialNumber: '',
    name: '',
    manufacturer: '',
    type: '',
    os: '',
    osVersion: '',
    processor: '',
    ram: '',
    description: '',
  });

  createForm = form(this.createModel, (schemaPath) => {
    required(schemaPath.serialNumber, { message: 'Serial number is required' });
    required(schemaPath.name, { message: 'Device name is required' });
    required(schemaPath.manufacturer, { message: 'Manufacturer is required' });
    required(schemaPath.type, { message: 'Device type is required' });
    required(schemaPath.os, { message: 'Operating system is required' });
    required(schemaPath.osVersion, { message: 'OS version is required' });
    required(schemaPath.processor, { message: 'Processor information is required' });
    required(schemaPath.ram, { message: 'RAM information is required' });
    required(schemaPath.description, { message: 'Description is required' });
  });

  override async ngOnInit() {}

  async generateDescription() {
    const model = this.createModel();

    if (
      !model.serialNumber ||
      !model.name ||
      !model.manufacturer ||
      !model.type ||
      !model.os ||
      !model.osVersion ||
      !model.processor ||
      !model.ram
    ) {
      this.error.set('Please complete the device fields first, then generate the description.');
      return;
    }

    this.error.set(null);
    this.isGeneratingDescription.set(true);

    try {
      const generatedDescription = await this.deviceService.generateDeviceDescription(
        new GenerateDeviceDescriptionCommand({
          name: model.name,
          manufacturer: model.manufacturer,
          type: model.type,
          os: model.os,
          osVersion: model.osVersion,
          processor: model.processor,
          ram: model.ram,
        }),
      );

      this.createModel.update((current) => ({
        ...current,
        description: (generatedDescription || 'lala').trim(),
      }));
    } catch (err) {
      this.error.set(
        extractApiErrorMessage(err, 'Failed to generate description. Please try again.'),
      );
    } finally {
      this.isGeneratingDescription.set(false);
    }
  }

  cancelCreate() {
    this.closeResolved(false);
  }

  onSubmit(event: Event) {
    event.preventDefault();
    submit(this.createForm, {
      action: async () => {
        const credentials = this.createModel();
        this.deviceService
          .upsertDevice(credentials)
          .then((createdDevice) => {
            this.closeResolved(true);
          })
          .catch((error) => {
            console.error('Error creating device:', error);
            this.error.set(
              extractApiErrorMessage(error, 'Failed to create device. Please try again.'),
            );
          });
      },
    });
  }
}
