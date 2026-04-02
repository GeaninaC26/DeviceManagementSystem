import { Injectable } from "@angular/core";
import { HttpService } from "./http.service";
import { UserDto } from "../contracts/user.dto";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private url = 'https://localhost:7250/api/users';

  constructor(private http: HttpService) {}

  async getUsers(): Promise<UserDto[]> {
    return this.http.get(this.url);
  }

  async getUser(id: number): Promise<UserDto> {
    return this.http.get(`${this.url}/${id}`);
  }

  async upsertUser(user: any): Promise<UserDto> {
    return this.http.post(this.url, user);
  }

  async deleteUser(id: number): Promise<void> {
    await this.http.delete(`${this.url}/${id}`);
  }

}
