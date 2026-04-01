import { Component, OnInit } from "@angular/core";

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrls: ['navbar.css'],
  standalone: true
})
export class Navbar implements OnInit {
  appTitle = 'Device Management System';

  ngOnInit(): void {
  }
}
