import { Injectable } from "@angular/core";
import { HttpService } from "./http.service";
import { UserDto } from "../contracts/user.dto";
import { DeviceDto } from "../contracts/device.dto";
import { GenerateDeviceDescriptionCommand } from "../contracts/commands/description-generation.command";

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private url = '/api/devices';

  constructor(private http: HttpService) {}

  async getDevices(): Promise<DeviceDto[]> {
    return this.http.get(this.url);
  }

  async getDevice(id: number): Promise<DeviceDto> {
    return this.http.get(`${this.url}/${id}`);
  }

  async getUnassignedDevices(): Promise<DeviceDto[]> {
    return this.http.get(`${this.url}/unassigned`);
  }

  async getDevicesForUser(userId: number): Promise<DeviceDto[]> {
    return this.http.get(`${this.url}/user/${userId}`);
  }

  async upsertDevice(device: any): Promise<DeviceDto> {
    return this.http.post(this.url, device);
  }

  async deleteDevice(id: number): Promise<void> {
    await this.http.delete(`${this.url}/${id}`);
  }

  async generateDeviceDescription(deviceInfo: any): Promise<string> {
    return this.http.postText(`${this.url}/generateDescription`, deviceInfo);
  }

}
