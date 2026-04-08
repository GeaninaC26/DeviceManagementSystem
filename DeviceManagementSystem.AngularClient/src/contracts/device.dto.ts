import { Dto } from './dto';

export class DeviceDto extends Dto {
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
