export class GenerateDeviceDescriptionCommand {
  name: string;
  type: string;
  manufacturer: string;
  os: string;
  osVersion: string;
  processor: string;
  ram: string;

  constructor(init: Partial<GenerateDeviceDescriptionCommand>) {
    Object.assign(this, init);
  }
}
