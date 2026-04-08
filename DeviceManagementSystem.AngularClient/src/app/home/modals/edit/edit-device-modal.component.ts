import { Component, OnInit, computed, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../components/modal/modal.component';
import { DeviceDto } from '../../../../contracts/device.dto';
import { ModalOpts } from '../../../components/modal/modal-opts';
import { DeviceService } from '../../../../services/device.service';
import { form, FormField, required, submit } from '@angular/forms/signals';
import { extractApiErrorMessage } from '../../../../services/api-error.util';
import { GenerateDeviceDescriptionCommand } from '../../../../contracts/commands/description-generation.command';

interface EditDeviceModel {
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
  selector: 'app-edit-device-modal',
  standalone: true,
  imports: [CommonModule, FormField],
  templateUrl: './edit-device-modal.component.html',
  styleUrls: ['../../../components/modal/modal.component.scss'],
})
export class EditDeviceModalComponent extends ModalComponent implements OnInit {
  device = input<DeviceDto | null>(null);
  error = signal<string | null>(null);
  isGeneratingDescription = signal(false);

  constructor(
    protected deviceService: DeviceService,
    protected modalOpts: ModalOpts,
  ) {
    super(modalOpts);
  }

  editModel = signal<EditDeviceModel>({
    name: '',
    manufacturer: '',
    type: '',
    os: '',
    osVersion: '',
    processor: '',
    ram: '',
    description: '',
  });

  editForm = form(this.editModel, (schemaPath) => {
    required(schemaPath.name, { message: 'Device name is required' });
    required(schemaPath.manufacturer, { message: 'Manufacturer is required' });
    required(schemaPath.type, { message: 'Device type is required' });
    required(schemaPath.os, { message: 'Operating system is required' });
    required(schemaPath.osVersion, { message: 'OS version is required' });
    required(schemaPath.processor, { message: 'Processor information is required' });
    required(schemaPath.ram, { message: 'RAM information is required' });
    required(schemaPath.description, { message: 'Description is required' });
  });

  override ngOnInit() {
    const currentDevice = this.device();
    if (currentDevice) {
      this.editModel.set({
        name: currentDevice.name || '',
        manufacturer: currentDevice.manufacturer || '',
        type: currentDevice.type || '',
        os: currentDevice.os || '',
        osVersion: currentDevice.osVersion || '',
        processor: currentDevice.processor || '',
        ram: currentDevice.ram || '',
        description: currentDevice.description || '',
      });
    }
  }

  cancelEdit() {
    this.closeResolved(false);
  }

  async generateDescription() {
    const model = this.editModel();

    if (
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

      this.editModel.update((current) => ({
        ...current,
        description: (generatedDescription || '').trim(),
      }));
    } catch (err) {
      this.error.set(
        extractApiErrorMessage(err, 'Failed to generate description. Please try again.'),
      );
    } finally {
      this.isGeneratingDescription.set(false);
    }
  }

  onSubmit(event: Event) {
    event.preventDefault();
    submit(this.editForm, {
      action: async () => {
        const currentDevice = this.device();
        if (!currentDevice) {
          this.error.set('Device not found.');
          return;
        }

        const updatedDevice = {
          ...currentDevice,
          ...this.editModel(),
        };

        try {
          await this.deviceService.upsertDevice(updatedDevice);
          this.closeResolved(true);
        } catch (error) {
          console.error('Error updating device:', error);
          this.error.set(
            extractApiErrorMessage(error, 'Failed to update device. Please try again.'),
          );
        }
      },
    });
  }
}
