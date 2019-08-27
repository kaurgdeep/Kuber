import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AgmCoreModule } from '@agm/core';
import { AgmDirectionModule } from 'agm-direction';

import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './kuber/views/home-view/home-view.component';
import { LoginViewComponent } from './kuber/views/login-view/login-view.component';
import { RegisterViewComponent } from './kuber/views/register-view/register-view.component';
import { MapViewComponent } from './kuber/views/map-view/map-view.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BaseUrlHttpInterceptor } from './kuber/services/BaseUrlHttpInterceptor';
import { CredentialsComponent } from './kuber/components/credentials/credentials.component';
import { SelectUserViewComponent } from './kuber/views/select-user-view/select-user-view.component';
import { UserService } from './kuber/services/UserService';
import { AuthenticationStore } from './kuber/services/AuthenticationStore';
import { AuthenticationHttpInterceptor } from './kuber/services/AuthenticationHttpInterceptor';
import { AuthenticationGuardService } from './kuber/services/AuthenticationGuardService';
import { RideViewComponent } from './kuber/views/ride-view/ride-view.component';
import { AddressAutocompleteComponent } from './kuber/components/address-autocomplete/address-autocomplete.component';
import { MapComponent } from './kuber/components/map/map.component';
import { RideComponent } from './kuber/components/ride/ride.component';
import { AcceptRideViewComponent } from './kuber/views/accept-ride-view/accept-ride-view.component';
import { RequestRideViewComponent } from './kuber/views/request-ride-view/request-ride-view.component';
import { RideService } from './kuber/services/RideService';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginViewComponent,
    RegisterViewComponent,
    MapViewComponent,
    CredentialsComponent,
    SelectUserViewComponent,
    RideViewComponent,
    AddressAutocompleteComponent,
    MapComponent,
    RideComponent,
    AcceptRideViewComponent,
    RequestRideViewComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAS-F2urrlEEihFzEHx5S_CLjx-QynksCw',
      libraries: ["places"]
    }),
    AgmDirectionModule,
    AppRoutingModule

  ],
  providers: [
    UserService,
    RideService,
    AuthenticationStore,
    AuthenticationGuardService,
    { provide: HTTP_INTERCEPTORS, useClass: BaseUrlHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationHttpInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
