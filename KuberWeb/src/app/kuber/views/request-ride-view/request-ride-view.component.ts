import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { IAddress } from '../../Dtos/Interfaces/IAddress';
import { RideService } from '../../services/RideService';
import { Router } from '@angular/router';
import { UserType } from '../../Dtos/Enums/UserType';
import { IUserInformation } from '../../Dtos/Interfaces/IUserInformation';
import { UserService } from '../../services/UserService';

@Component({
    selector: 'app-request-ride-view',
    templateUrl: './request-ride-view.component.html',
    styleUrls: ['./request-ride-view.component.scss']
})
export class RequestRideViewComponent implements OnInit {

    @ViewChild('searchPickUp', { static: true }) public searchPickUpElement: ElementRef;
    @ViewChild('searchDropOff', { static: true }) public searchDropOffElement: ElementRef;

    userLocation: IAddress = {};
    pickUpAddress: IAddress = {};
    dropOffAddress: IAddress = {};

    UserType = UserType;

    me: IUserInformation;

    origin: { lat: number, lng: number };
    destination: any;

    constructor(public router: Router, private ngZone: NgZone,
        private mapsAPILoader: MapsAPILoader, private userService: UserService,
        private rideService: RideService) { }

    async ngOnInit() {
        this.me = await this.userService.getMe();
        console.log(this.me);
        if (this.me == null) {
            this.router.navigate(['/select-user']);
        }

        // todo: if usertype is not passenger, redirect to home?

        this.mapsAPILoader.load().then(() => {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(this.locationObtained, this.locationFailed);
            }
        });
    }

    locationObtained = (position: Position) => {
        this.userLocation = { latitude: position.coords.latitude, longitude: position.coords.longitude };
    }

    locationFailed = (error) => {
        console.error(error);
    }

    addMarker(lat: number, lng: number) {
        this.userLocation = { latitude: lat, longitude: lng };
    }

    pickUpAddressChanged(address: IAddress) {
        this.ngZone.run(() => {
            this.pickUpAddress = address; // todo: consider using lodash's _.cloneDeep
            this.origin = { lat: address.latitude, lng: address.longitude };
        });
    }

    dropOffAddressChanged(address: IAddress) {
        this.ngZone.run(() => {
            this.dropOffAddress = address;
            this.destination = { lat: address.latitude, lng: address.longitude };
        });
    }

    async requestRide() {
        const response = await this.rideService.create({
            pickupAddress: this.pickUpAddress,
            dropoffAddress: this.dropOffAddress,
        });
        console.log(response);
        if (response) {
            this.router.navigate(['/ride', response.id]);
        }
    }

    onChange(result: google.maps.DirectionsResult) {
        //console.log(event);
        // You can do anything.
    }


    distanceMiles: string;
    timeFormatted: string;

    onResponse(result: google.maps.DirectionsResult) {
        if (result && result.routes && result.routes[0] && result.routes[0].legs && 
            result.routes[0].legs[0] && result.routes[0].legs[0].distance) {
            this.distanceMiles = result.routes[0].legs[0].distance.text;
        }
        if (result && result.routes && result.routes[0] && result.routes[0].legs && 
            result.routes[0].legs[0] && result.routes[0].legs[0].duration) {
            this.timeFormatted = result.routes[0].legs[0].duration.text;
        }
    }

    getStatus(status: any) {
        console.log(status);
    }

}
