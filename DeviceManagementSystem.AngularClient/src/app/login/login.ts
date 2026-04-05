import {ChangeDetectionStrategy, Component, computed, signal} from '@angular/core';
import {email, form, FormField, required, submit} from '@angular/forms/signals';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { extractApiErrorMessage } from '../../services/api-error.util';

interface LoginData {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss',
  imports: [FormField, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly currentUser = computed(() => this.authService.currentUser());

  constructor(private authService: AuthService, private router: Router) {}

  loginModel = signal<LoginData>({
    email: '',
    password: '',
  });
  loginForm = form(this.loginModel, (schemaPath) => {
    required(schemaPath.email, {message: 'Email is required'});
    email(schemaPath.email, {message: 'Enter a valid email address'});
    required(schemaPath.password, {message: 'Password is required'});
  });
  onSubmit(event: Event) {
    event.preventDefault();
    submit(this.loginForm, {
      action: async () => {
        this.errorMessage.set(null);
        this.isSubmitting.set(true);
        try {
          const credentials = this.loginModel();
          await this.authService.login(credentials.email, credentials.password);
          await this.router.navigate(['/device-management']);
        } catch (error) {
          this.errorMessage.set(extractApiErrorMessage(error, 'Invalid email or password.'));
        } finally {
          this.isSubmitting.set(false);
        }
      },
    });
  }
}
