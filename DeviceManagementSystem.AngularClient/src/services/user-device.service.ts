import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { UserDto } from '../contracts/user.dto';
import { UserDeviceDto } from '../contracts/user-device.dto';

@Injectable({
  providedIn: 'root',
})
export class UserDeviceService {
  private url = '/api/userDevices';

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
    await this.http.delete(`${this.url}/${id}`);
  }
}
