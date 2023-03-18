import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot, CanActivate,
  RouterStateSnapshot, UrlTree,
  Router
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService,
    private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    if (!this.auth.currentUser)
      this.router.navigate(['/register-passanger', { requestedUrl: state.url }])

    return true;
  }
  
}
