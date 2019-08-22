import { Component, OnInit, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RideService } from '../../services/RideService';
import { IAddress } from '../../Dtos/Interfaces/IAddress';
import { IRideResponse } from '../../Dtos/Interfaces/IRideResponse';
import { UserService } from '../../services/UserService';
import { IUserInformation } from '../../Dtos/Interfaces/IUserInformation';
import { UserType } from '../../Dtos/Enums/UserType';
import { MapsAPILoader } from '@agm/core';
import { IRideStatus } from '../../Dtos/Interfaces/IRideStatus';
import { RideStatus } from '../../Dtos/Enums/RideStatus';

@Component({
    selector: 'app-ride-view',
    templateUrl: './ride-view.component.html',
    styleUrls: ['./ride-view.component.scss']
})
export class RideViewComponent implements OnInit {

    UserType = UserType;
    RideStatus = RideStatus;

    rideId: number;
    rideResponse: IRideResponse;

    userLocation: IAddress = {};
    pickUpLocation: IAddress;
    currentLocation: IAddress; // we will fake this for the driver.
    dropOffLocation: IAddress;

    me: IUserInformation;

    constructor(public router: Router, private ngZone: NgZone,
        private mapsAPILoader: MapsAPILoader,
        private activatedRoute: ActivatedRoute,
        private userService: UserService,
        private rideService: RideService) { }

    async ngOnInit() {
        this.mapsAPILoader.load().then(() => {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(this.locationObtained, this.locationFailed);
            }
        });

        this.me = await this.userService.getMe();
        if (this.me == null) {
            return;
        }
        console.log(this.me);

        const id = this.activatedRoute.snapshot.paramMap.get('id');
        if (id) {
            this.rideId = parseInt(id, 10);
            if (this.rideId) {
                console.log('RideId: ', this.rideId);

                this.rideResponse = await this.rideService.get(this.rideId);
                console.log(this.rideResponse);

                if (this.rideResponse) {
                    this.ngZone.run(() => {
                        this.pickUpLocation = {
                            latitude: this.rideResponse.fromAddress.latitude, longitude: this.rideResponse.fromAddress.longitude
                        };
                        this.dropOffLocation = {
                            latitude: this.rideResponse.toAddress.latitude, longitude: this.rideResponse.toAddress.longitude
                        };
                        if (this.rideResponse.currentAddress && this.rideResponse.currentAddress.latitude && this.rideResponse.currentAddress.longitude) {
                            this.currentLocation = {
                                formattedAddress: this.rideResponse.currentAddress.formattedAddress,
                                latitude: this.rideResponse.currentAddress.latitude,
                                longitude: this.rideResponse.currentAddress.longitude
                            };
                        } else {
                            this.currentLocation = undefined;
                        }
                    });
                } else {
                    this.router.navigate(['/']);
                }
            }
            if (this.me.userType == UserType.Driver) {
                // todo: this is temporary
                // hitting the API (which in turn will hit the database) so frequently is not at all recommended
                // let's fake the driver driving from their current location to from-address
                // let driver press 'pick up'
                // fake the driver driving from from address to to-address then
                // let driver press 'drop off' anywhere in between - but we wil only handle/store to-address initially
                this.currentLocation = this.userLocation;
                setInterval(() => {
                    this.currentLocation = {
                        latitude: this.currentLocation.latitude + 0.0004,
                        longitude: this.currentLocation.longitude + 0.0004
                    };
                }, 5000);
            } else if (this.me.userType == UserType.Passenger) {
                // todo: this is temporary and for demonstration purposes only.
                // hitting the API (which in turn will hit the database) so frequently is not at all recommended
                setInterval(async () => {
                    this.rideResponse = await this.rideService.get(this.rideId);
                    if (this.rideResponse) {
                        if (this.rideResponse.currentAddress &&
                            this.rideResponse.currentAddress.latitude &&
                            this.rideResponse.currentAddress.longitude) {

                            this.currentLocation = {
                                formattedAddress: this.rideResponse.currentAddress.formattedAddress,
                                latitude: this.rideResponse.currentAddress.latitude,
                                longitude: this.rideResponse.currentAddress.longitude
                            };
                        }
                    }
                }, 5000);
            }
        } else {
            this.router.navigate(['/']);
        }
    }

    locationObtained = (position: Position) => {
        this.userLocation = { latitude: position.coords.latitude, longitude: position.coords.longitude };
        this.ngZone.run(() => {
            this.currentLocation = { latitude: this.userLocation.latitude, longitude: this.userLocation.longitude };
            console.log(this.currentLocation);
        });
    }

    locationFailed = (error) => {
        console.error(error);
    }

    currentLocationAddressObtained = (location: IAddress) => {
        if (this.me.userType == UserType.Driver) {
            const rideStatus: IRideStatus = {
                currentAddress: {
                    formattedAddress: location.formattedAddress,
                    latitude: location.latitude,
                    longitude: location.longitude
                }
            };
            this.rideService.updateLocation(this.rideId, rideStatus);
        }
    }

    async pickUpRide(rideId: number) {
        const response = await this.rideService.pickUp(rideId);
        console.log(response);
    }

    async dropOffRide(rideId: number) {
        const response = await this.rideService.dropOff(rideId);
        console.log(response);
        if (response) {
            this.router.navigate(['/']);
        }
    }

    async rejectRide(rideId: number) {
        const response = await this.rideService.reject(rideId);
        console.log(response);
        if (response) {
            this.router.navigate(['/']);
        }
    }
}
