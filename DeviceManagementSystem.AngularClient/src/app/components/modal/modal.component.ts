import { CommonModule } from '@angular/common';
import { Component, Host, HostListener, input, OnDestroy, OnInit, output } from '@angular/core';
import { ModalOpts } from './modal-opts';

@Component({
  selector: 'app-modal',
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
  standalone: true,
})
export class ModalComponent implements OnInit, OnDestroy {
  readonly title = input<string>('Modal Title');
  readonly closeOutsideClick = input<boolean>(true);
  readonly showCloseButton = input<boolean>(true);
  readonly size = input<'small' | 'medium' | 'large' | 'modal-xl'>();
  readonly closeEvent = output<any>();

  scrollYOnOpen = 0;

  constructor(public opts: ModalOpts) {}

  ngOnInit(): void {
    this.disableBodyScroll();
  }

  ngOnDestroy(): void {
    this.enableBodyScroll();
  }

  close(): any {
    this.closeEvent.emit(null);
  }

  closeResolved(val?: any) {
    if (!val) val = true;
    this.closeEvent.emit(val);
  }

  onOutsideClick() {
    if (this.closeOutsideClick()) {
      this.close();
    }
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscapeKeydown(event: Event) {
    event.preventDefault();
    this.close();
  }

  @HostListener('window:popstate', ['$event'])
  onPopState(event: PopStateEvent) {
    event.preventDefault();
    this.closeEvent.emit(null);
  }

  disableBodyScroll() {
    this.scrollYOnOpen = window.scrollY;
    document.body.style.position = 'fixed';
    document.body.style.top = `-${scrollY}px`;
    document.body.style.width = '100%';
  }

  enableBodyScroll() {
    document.body.style.position = '';
    document.body.style.top = '';
    document.body.style.width = '';
    window.scrollTo(0, this.scrollYOnOpen);
  }
}
