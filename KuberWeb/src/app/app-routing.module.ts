import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './kuber/views/home-view/home-view.component';
import { LoginViewComponent } from './kuber/views/login-view/login-view.component';
import { RegisterViewComponent } from './kuber/views/register-view/register-view.component';
import { MapViewComponent } from './kuber/views/map-view/map-view.component';
import { SelectUserViewComponent } from './kuber/views/select-user-view/select-user-view.component';
import { AuthenticationGuardService } from './kuber/services/AuthenticationGuardService';
import { RideViewComponent } from './kuber/views/ride-view/ride-view.component';
import { RequestRideViewComponent } from './kuber/views/request-ride-view/request-ride-view.component';
import { AcceptRideViewComponent } from './kuber/views/accept-ride-view/accept-ride-view.component';


const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthenticationGuardService] },
  { path: 'select-user', component: SelectUserViewComponent },

  { path: 'login', component: LoginViewComponent },
  { path: 'register', component: RegisterViewComponent },
  { path: 'map', component: MapViewComponent },
  { path: 'ride/:id', component: RideViewComponent, canActivate: [AuthenticationGuardService]},
  { path: 'request-ride', component: RequestRideViewComponent, canActivate: [AuthenticationGuardService]},
  { path: 'accept-ride', component: AcceptRideViewComponent, canActivate: [AuthenticationGuardService]},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
