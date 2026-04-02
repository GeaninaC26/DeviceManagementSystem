import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor (private http: HttpClient) {}

  getAsObservable<T>(path: string) {
    return this.http.get<T>(path);
  }

  get<T>(path: string) {
    return firstValueFrom(this.http.get<T>(path));
  }

  post<T>(path: string, body: any) {
    return firstValueFrom(this.http.post<T>(path, body));
  }

  put<T>(path: string, body: any) {
    return firstValueFrom(this.http.put<T>(path, body));
  }

  delete<T>(path: string) {
    return firstValueFrom(this.http.delete<T>(path));
  }
}
