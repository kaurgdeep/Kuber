import { Component, OnInit } from '@angular/core';
import { IAddress } from '../../Dtos/Interfaces/IAddress';
import { UserType } from '../../Dtos/Enums/UserType';
import { IUserInformation } from '../../Dtos/Interfaces/IUserInformation';
import { Router } from '@angular/router';
import { MapsAPILoader } from '@agm/core';
import { UserService } from '../../services/UserService';
import { RideService } from '../../services/RideService';
import { IRideResponse } from '../../Dtos/Interfaces/IRideResponse';


const MilesToMeters = 1609.344;

@Component({
  selector: 'app-accept-ride-view',
  templateUrl: './accept-ride-view.component.html',
  styleUrls: ['./accept-ride-view.component.scss']
})
export class AcceptRideViewComponent implements OnInit {

  radiusMiles = 0.5;

  userLocation: IAddress = {};

  UserType = UserType;

  me: IUserInformation;

  rideRequests: Array<IRideResponse>;
  selectedRequest: IRideResponse;


  constructor(public router: Router, private mapsAPILoader: MapsAPILoader, private userService: UserService, private rideService: RideService) { }

  async ngOnInit() {
      this.me = await this.userService.getMe();
      console.log(this.me);
      if (this.me == null) {
          this.router.navigate(['/select-user']);
      }

      // todo: if usertype is not driver, redirect to home?
      this.mapsAPILoader.load().then(() => {
          if (navigator.geolocation) {
              navigator.geolocation.getCurrentPosition(this.locationObtained, this.locationFailed);
          }
      });
  }

  locationObtained = async (position: Position) => {
      this.userLocation = { latitude: position.coords.latitude, longitude: position.coords.longitude };

      this.rideRequests = await this.rideService.getNearby(this.userLocation, this.radiusMiles*MilesToMeters);
      console.log(this.rideRequests);
  }

  locationFailed = (error) => {
      console.error(error);
  }

  selectRide(e, ride: IRideResponse) {
    this.selectedRequest = (this.selectedRequest == ride) ? undefined: ride;
}

async acceptRide(rideId: number) {
    const response = await this.rideService.accept(rideId);
    console.log(response);
    if (response) {
        this.router.navigate(['/ride', rideId]);
    }
}


}
