import { Injectable, signal } from '@angular/core';
import { UserDto } from '../contracts/user.dto';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenStorageKey = 'dms_auth_token';
  private readonly userStorageKey = 'dms_auth_user';

  readonly currentUser = signal<UserDto | null>(this.readStoredUser());

  constructor(private userService: UserService) {}

  async login(email: string, password: string): Promise<UserDto> {
    const response = await this.userService.login(email, password);
    localStorage.setItem(this.tokenStorageKey, response.token);
    localStorage.setItem(this.userStorageKey, JSON.stringify(response.user));
    this.currentUser.set(response.user);
    return response.user;
  }

  async register(payload: { name: string; location: string; email: string; password: string }): Promise<void> {
    await this.userService.upsertUser({
      ...payload,
      role: 'User',
      id: 0
    });
  }

  async loadCurrentUser(): Promise<UserDto | null> {
    const token = this.getToken();
    if (!token) {
      this.currentUser.set(null);
      return null;
    }

    try {
      const user = await this.userService.getCurrentUser();
      localStorage.setItem(this.userStorageKey, JSON.stringify(user));
      this.currentUser.set(user);
      return user;
    } catch {
      this.clearSession();
      return null;
    }
  }

  logout(): void {
    this.clearSession();
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenStorageKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private clearSession(): void {
    localStorage.removeItem(this.tokenStorageKey);
    localStorage.removeItem(this.userStorageKey);
    this.currentUser.set(null);
  }

  private readStoredUser(): UserDto | null {
    const raw = localStorage.getItem(this.userStorageKey);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as UserDto;
    } catch {
      return null;
    }
  }
}
