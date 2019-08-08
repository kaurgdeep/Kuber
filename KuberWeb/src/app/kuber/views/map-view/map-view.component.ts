/// <reference types="@types/googlemaps" />
import { Component, OnInit } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { ViewChild, ElementRef, NgZone } from '@angular/core';

@Component({
  selector: 'app-map-view',
  templateUrl: './map-view.component.html',
  styleUrls: ['./map-view.component.scss']
})
export class MapViewComponent implements OnInit {
  @ViewChild('search', {static: false}) public searchElement: ElementRef;
  lat: number;
  lng: number;
  currentAddress: string;

  constructor(private mapsAPILoader: MapsAPILoader, private ngZone: NgZone) { }

  ngOnInit() {
    

    this.mapsAPILoader.load().then(
      () => {
        if (navigator.geolocation) {
          navigator.geolocation.getCurrentPosition(this.displayLocationInfo);
        }
        let autocomplete = new google.maps.places.Autocomplete(this.searchElement.nativeElement, { types: ["address"] });

        autocomplete.addListener("place_changed", () => {
          this.ngZone.run(() => {
            let place: google.maps.places.PlaceResult = autocomplete.getPlace();
            if (place.geometry === undefined || place.geometry === null) {
              return;
            }
            this.lat = place.geometry.location.lat();
            this.lng = place.geometry.location.lng();

          });
        });
      }
    );
  }
  displayLocationInfo = (position) => {
    this.lng = position.coords.longitude; // marker
    this.lat = position.coords.latitude;
    this.updateAddress();
    console.log(`longitude: ${this.lng} | latitude: ${this.lat}`);
  }

  placeMarker($event) {
    this.lng = $event.coords.lng;
    this.lat = $event.coords.lat
    console.log($event.coords.lat);
    console.log($event.coords.lng);
    this.updateAddress();
  }

  updateAddress(){
    const point = new google.maps.LatLng(this.lat, this.lng);
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode({location: point},(result, status)=> {
      console.log(result,status);
      this.currentAddress = result[0].formatted_address;
    })
  }
}
