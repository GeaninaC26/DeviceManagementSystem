import { Injectable } from "@angular/core";
import { HttpService } from "./http.service";
import { HttpParams } from "@angular/common/http";
import { UserDto } from "../contracts/user.dto";
import { DeviceDto } from "../contracts/device.dto";
import { GenerateDeviceDescriptionCommand } from "../contracts/commands/description-generation.command";

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private url = '/api/devices';

  constructor(private http: HttpService) {}

  async getDevices(searchQuery?: string): Promise<DeviceDto[]> {
    const url = searchQuery ? `${this.url}?searchQuery=${encodeURIComponent(searchQuery)}` : `${this.url}`;
    return this.http.get(url);
  }

  async getDevice(id: number): Promise<DeviceDto> {
    return this.http.get(`${this.url}/${id}`);
  }

  async getUnassignedDevices(searchQuery?: string): Promise<DeviceDto[]> {
    const url = searchQuery ? `${this.url}/unassigned?searchQuery=${encodeURIComponent(searchQuery)}` : `${this.url}/unassigned`;
    return this.http.get(url);
  }

  async getDevicesForUser(userId: number, searchQuery?: string): Promise<DeviceDto[]> {
    const url = searchQuery ? `${this.url}/user/${userId}?searchQuery=${encodeURIComponent(searchQuery)}` : `${this.url}/user/${userId}`;
    return this.http.get(url);
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
