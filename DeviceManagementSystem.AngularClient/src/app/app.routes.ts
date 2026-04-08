import { Routes } from '@angular/router';
import { Home } from './home/home';
import { LoginComponent } from './login/login';
import { RegisterComponent } from './register/register';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: LoginComponent,
  },
  {
    path: 'device-management',
    pathMatch: 'full',
    canActivate: [authGuard],
    component: Home,
  },
  {
    path: 'register',
    pathMatch: 'full',
    component: RegisterComponent,
  },
];
