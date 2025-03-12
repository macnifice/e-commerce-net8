import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class CookieServ {

  constructor(private cookieService: CookieService) {  }

   setCookie(key: string) {
    this.cookieService.set('token', key);
   }

   getCookie() {
    return this.cookieService.get('token');
   }

   checkCookie() {
    return this.cookieService.check('token');
   }

   removeCookie() {
    this.cookieService.delete('token');
   }

}
