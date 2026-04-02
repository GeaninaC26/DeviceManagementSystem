import { ComponentRef, Injectable, Type, ViewContainerRef } from "@angular/core";
import { ModalComponent } from "./modal.component";

@Injectable({providedIn: 'root'})
export class ModalService {
  private viewContainer: ViewContainerRef;
  private instances: ComponentRef<ModalComponent>[] = [];

  setViewContainer(viewContainer: ViewContainerRef) {
    this.viewContainer = viewContainer;
  }

  async show<TModal extends ModalComponent>(modal: Type<TModal>, setOptions: (ref: ComponentRef<TModal>) => void = null): Promise<any> {
    if (!this.viewContainer) {
      throw new Error("ModalService: ViewContainerRef is not set. Please call setViewContainer() before showing modals.");
    }

    const ref = this.viewContainer.createComponent(modal);
    this.instances.push(ref);

    if (setOptions) {
      setOptions(ref);
    }

    return new Promise<any>((resolve) => {
      const sub = ref.instance.closeEvent.subscribe((rsp: any) => {
        sub.unsubscribe();
        this.instances = this.instances.filter(instance => instance !== ref);
        ref.destroy();
        resolve(rsp);
      });
    });
  }
}
