import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import { UserService } from '../../services/UserService';
import { IUserInformation } from '../../Dtos/Interfaces/IUserInformation';
import { UserType } from '../../Dtos/Enums/UserType';
import { MapsAPILoader } from '@agm/core';
import { Router } from '@angular/router';
import { RideService } from '../../services/RideService';
import { IAddress } from '../../Dtos/Interfaces/IAddress';



@Component({
  selector: 'app-home-view',
  templateUrl: './home-view.component.html',
  styleUrls: ['./home-view.component.scss']
})
export class HomeComponent implements OnInit {

  @ViewChild('searchPickUp', { static: true }) public searchPickUpElement: ElementRef;
  @ViewChild('searchDropOff', { static: true }) public searchDropOffElement: ElementRef;

  // pickUpaddress: string;
  // dropOffAddress: string;

  // pickUpLatitude = -28.68352;
  // pickUpLongitude = -147.20785;

  // dropOffLatitude = -28;
  // dropOffLongitude = -147;

  userLocation: IAddress = {};
  pickUpAddress: IAddress = {};
  dropOffAddress: IAddress = {};


  UserType = UserType;

  me: IUserInformation;
  constructor(private router: Router, private userService: UserService, private rideService: RideService) { }

  async ngOnInit() {
    this.me = await this.userService.getMe();
    console.log(this.me);

    if (this.me == null) {
      this.router.navigate(['/select-user']);
      return;
    }

    const activeRide = await this.rideService.getAnyActive();
    console.log(activeRide);
    if (activeRide && activeRide.id) {
      this.router.navigate(['/ride', activeRide.id]);
      return;
    }

    if (this.me.userType === UserType.Passenger) {
      this.router.navigate(['/request-ride']);
      return;
    } else if (this.me.userType == UserType.Driver) {
      this.router.navigate(['/accept-ride']);
      return;


      //   this.mapsAPILoader.load().then(() => {
      //     if (navigator.geolocation) {
      //       navigator.geolocation.getCurrentPosition(this.locationObtained, this.locationFailed);
      //     }

      //     // const autocompletePickUp = new google.maps.places.Autocomplete(this.searchPickUpElement.nativeElement, { types: ['address'] });

      //     // autocompletePickUp.addListener('place_changed', () => {
      //     //   this.ngZone.run(() => {
      //     //     const place: google.maps.places.PlaceResult = autocompletePickUp.getPlace();
      //     //     if (place.geometry === undefined || place.geometry === null) {
      //     //       return;
      //     //     }
      //     //     this.pickUpLatitude = place.geometry.location.lat();
      //     //     this.pickUpLongitude = place.geometry.location.lng();
      //     //   });
      //     // });

      //     // const autocompleteDropOff = new google.maps.places.Autocomplete(this.searchDropOffElement.nativeElement, { types: ['address'] });

      //     // autocompleteDropOff.addListener('place_changed', () => {
      //     //   this.ngZone.run(() => {
      //     //     const place: google.maps.places.PlaceResult = autocompleteDropOff.getPlace();
      //     //     if (place.geometry === undefined || place.geometry === null) {
      //     //       return;
      //     //     }
      //     //     this.dropOffLatitude = place.geometry.location.lat();
      //     //     this.dropOffLongitude = place.geometry.location.lng();
      //     //   });
      //     // });


      //   });
      // }

      // locationObtained = (position: Position) => {

      //   this.userLocation = { latitude: position.coords.latitude, longitude: position.coords.longitude };

      //   // this.pickUpLatitude = position.coords.latitude;
      //   // this.pickUpLongitude = position.coords.longitude;

      //   // this.updateAddress();
      // }

      // // updateAddress() {
      // //   const point = new google.maps.LatLng(this.pickUpLatitude, this.pickUpLongitude);
      // //   const geocoder = new google.maps.Geocoder();
      // //   geocoder.geocode({ location: point }, (results, status) => {
      // //     console.log(results[0].formatted_address);
      // //     this.pickUpaddress = results[0].formatted_address;
      // //     console.log(status);
      // //   });
      // // }

      // locationFailed = (error) => {
      //   console.error(error);
      // }

      // addMarker(lat: number, lng: number) {

      //   this.userLocation = { latitude: lat, longitude: lng };
      // }

      // pickUpAddressChanged(address: IAddress) {
      //     this.pickUpAddress = address;
      // }

      // dropOffAddressChanged(address: IAddress) {
      //     this.dropOffAddress = address;
      //   // this.pickUpLatitude = lat;
      //   // this.pickUpLongitude = lng;

      //   // this.updateAddress();
      // }

      // async requestRide() {
      //   const response = await this.rideService.create({

      //     pickupAddress: this.pickUpAddress,
      //     dropoffAddress: this.dropOffAddress,
      //     // pickupAddress: {
      //     //   formattedAddress: this.pickUpaddress,
      //     //   latitude: this.pickUpLatitude,
      //     //   longitude: this.pickUpLongitude
      //     // },
      //     // dropoffAddress: {
      //     //   formattedAddress: this.dropOffAddress,
      //     //   latitude: this.dropOffLatitude,
      //     //   longitude: this.dropOffLongitude
      //     // }
      //   });
      //   console.log(response);
      //   if (response) {
      //     this.router.navigate(['/ride', response.id]);
      //   }
    }
  }

}
