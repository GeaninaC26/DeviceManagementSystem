import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class ModalOpts {

  constructor(public router: Router) { }

}
