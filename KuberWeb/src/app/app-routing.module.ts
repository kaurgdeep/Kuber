import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './kuber/views/home/home.component';
import { LoginViewComponent } from './kuber/views/login-view/login-view.component';
import { RegisterViewComponent } from './kuber/views/register-view/register-view.component';
import { MapViewComponent } from './kuber/views/map-view/map-view.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginViewComponent },
  { path: 'register', component: RegisterViewComponent },
  { path: 'map', component: MapViewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
