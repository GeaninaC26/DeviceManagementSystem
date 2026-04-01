import { Injectable } from "@angular/core";
import { HttpService } from "./http.service";
import { UserDto } from "../contracts/user.dto";
import { UserDeviceDto } from "../contracts/user-device.dto";

@Injectable({
  providedIn: 'root'
})
export class UserDeviceService {
  private url = 'https://localhost:7250/api/userDevices';

  constructor(private http: HttpService) {}

  async getUserDevices(): Promise<UserDeviceDto[]> {
    return this.http.get(this.url);
  }

  async getUserDevice(id: number): Promise<UserDeviceDto> {
    return this.http.get(`${this.url}/${id}`);
  }

  async upsertUserDevice(userDevice: any): Promise<UserDeviceDto> {
    return this.http.post(this.url, userDevice);
  }

  async deleteUserDevice(id: number): Promise<void> {
    this.http.delete(`${this.url}/${id}`);
  }
}
