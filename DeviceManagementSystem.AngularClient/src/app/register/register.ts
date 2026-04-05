import {ChangeDetectionStrategy, Component, signal} from '@angular/core';
import {email, form, FormField, required, submit} from '@angular/forms/signals';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { extractApiErrorMessage } from '../../services/api-error.util';

interface LoginData {
  name: string;
  location: string;
  email: string;
  password: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.html',
  styleUrls: ['./register.scss', '../login/login.scss'],
  imports: [FormField, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);

  constructor(private authService: AuthService, private router: Router) {}

  registrationModel = signal<LoginData>({
    name: '',
    location: '',
    email: '',
    password: '',
  });
  registrationForm = form(this.registrationModel, (schemaPath) => {
    required(schemaPath.email, {message: 'Email is required'});
    email(schemaPath.email, {message: 'Enter a valid email address'});
    required(schemaPath.password, {message: 'Password is required'});
    required(schemaPath.name, {message: 'Name is required'});
    required(schemaPath.location, {message: 'Location is required'});
  });
  onSubmit(event: Event) {
    event.preventDefault();
    submit(this.registrationForm, {
      action: async () => {
        this.errorMessage.set(null);
        this.isSubmitting.set(true);
        try {
          const credentials = this.registrationModel();
          await this.authService.register(credentials);
          await this.authService.login(credentials.email, credentials.password);
          await this.router.navigate(['/device-management']);
        } catch (error) {
          this.errorMessage.set(extractApiErrorMessage(error, 'Registration failed. Please verify your data and try again.'));
        } finally {
          this.isSubmitting.set(false);
        }
      },
    });
  }
}
