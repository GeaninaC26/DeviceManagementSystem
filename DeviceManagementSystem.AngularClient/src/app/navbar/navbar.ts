import { Component, computed } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { RoleEnum } from '../../contracts/enums/role.enum';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.scss'],
  standalone: true,
})
export class Navbar {
  appTitle = 'Device Management System';
  readonly currentUser = computed(() => this.authService.currentUser());

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  async logout(): Promise<void> {
    this.authService.logout();
    await this.router.navigate(['/']);
  }

  roleLabel(role: RoleEnum | undefined): string {
    if (role === undefined || role === null) {
      return '';
    }

    return RoleEnum[role] ?? role.toString();
  }
}
