import { Dto } from "./dto";

export class DeviceDto extends Dto {
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ram: string;
  description: string;
}
