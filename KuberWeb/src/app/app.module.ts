import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AgmCoreModule } from '@agm/core';

import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './kuber/views/home/home.component';
import { LoginViewComponent } from './kuber/views/login-view/login-view.component';
import { RegisterViewComponent } from './kuber/views/register-view/register-view.component';
import { MapViewComponent } from './kuber/views/map-view/map-view.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginViewComponent,
    RegisterViewComponent,
    MapViewComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAS-F2urrlEEihFzEHx5S_CLjx-QynksCw',
      libraries: ["places"]
    }),
    AppRoutingModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
