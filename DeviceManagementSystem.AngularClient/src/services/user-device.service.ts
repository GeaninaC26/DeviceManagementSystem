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

  async assignDeviceToUser(userDevice: any): Promise<UserDeviceDto> {
    return this.http.post(this.url, userDevice);
  }

  async deleteUserDevice(id: number): Promise<void> {
    console.log(`Deleting user-device association with ID: ${id}`);
    await this.http.delete(`${this.url}/${id}`);
    console.log(`Delete request sent for user-device association with ID: ${id}`);
  }
}
