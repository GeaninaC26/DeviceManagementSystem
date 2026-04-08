import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../components/modal/modal.component';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  styleUrls: ['../../components/modal/modal.component.scss'],
  template: `
    <div class="modal-backdrop" (click)="onOutsideClick()">
      <div class="modal-container" [ngClass]="size() || ''">
        <div
          class="modal-dialog"
          style="width: 100%; border: none; box-shadow: none;"
          (click)="$event.stopPropagation()"
        >
          <div class="modal-content">
            <div class="modal-header">
              <div class="modal-title" style="font-weight: bold; font-size: 1.25rem;">
                {{ title() }}
              </div>
              <button
                *ngIf="showCloseButton()"
                type="button"
                class="close-button"
                style="border: none; background: transparent; font-size: 1.5rem; cursor: pointer;"
                (click)="close()"
              >
                &times;
              </button>
            </div>
            <div class="modal-body" style="padding: 1rem;">
              <p style="margin-bottom: 1.5rem;">{{ message() }}</p>
              <div style="display: flex; justify-content: flex-end; gap: 1rem;">
                <button class="btn btn-secondary" (click)="closeResolved(false)">Cancel</button>
                <button class="btn btn-danger" (click)="closeResolved(true)">Confirm</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ConfirmModalComponent extends ModalComponent {
  readonly message = input<string>('Are you sure you want to perform this action?');
}
