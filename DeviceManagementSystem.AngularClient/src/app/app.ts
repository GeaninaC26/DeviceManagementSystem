import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-root',
  imports: [Navbar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('DeviceManagementSystem.AngularClient');

  constructor(private authService: AuthService) {}

  async ngOnInit(): Promise<void> {
    await this.authService.loadCurrentUser();
  }
}
