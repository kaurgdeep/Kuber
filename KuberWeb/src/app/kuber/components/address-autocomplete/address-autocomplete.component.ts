import { Component, OnInit, ElementRef, NgZone, ViewChild, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { IAddress } from '../../Dtos/Interfaces/IAddress';
import { MapsAPILoader } from '@agm/core';


@Component({
  selector: 'app-address-autocomplete',
  templateUrl: './address-autocomplete.component.html',
  styleUrls: ['./address-autocomplete.component.scss']
})
export class AddressAutocompleteComponent implements OnInit {

  @ViewChild('searchAddress', { static: false }) public searchAddressElement: ElementRef;

  @Input() label: string;
  @Input() placeholder: string;
  @Input() location: IAddress;
  @Input() showLatLng: boolean;
  @Input() readOnly: boolean = false;
  @Input() lookupAddress: boolean = false;



  @Output() addressChange = new EventEmitter<IAddress>();

  address: IAddress = {};

  constructor(private mapsAPILoader: MapsAPILoader, private ngZone: NgZone) { }


  ngOnInit() {
    this.mapsAPILoader.load().then(() => {
      if (this.readOnly == false) {
        // Google Maps may not be loaded yet - so the namespace 'google' may not be defined yet
        // Let's keep retrying every so often (100ms) using an interval
        // and clear the timer when we find 'google' is defined
        const timerId = setInterval(() => {
          if (typeof (google) !== 'undefined') {
            clearInterval(timerId);
            this.initAutocomplete();
          }
        }, 100);
      }
    });
  }

  private updateAddressTimerId;
  ngOnChanges(changes: SimpleChanges): void {
    const currentLocation = changes['location'] && changes['location'].currentValue as IAddress;
    if (currentLocation) {
      if (this.updateAddressTimerId) {
        clearInterval(this.updateAddressTimerId);
      }
      if (this.lookupAddress) {
        this.updateAddressTimerId = setInterval(() => {
          if (typeof (google) !== 'undefined') {
            clearInterval(this.updateAddressTimerId);
            this.updateAddressTimerId = undefined;

            this.getAddress(currentLocation.latitude, currentLocation.longitude, (address) => {
              this.ngZone.run(() => {

                this.address.latitude = currentLocation.latitude;
                this.address.longitude = currentLocation.longitude;
                this.address.formattedAddress = address;

                this.addressChange.emit(this.address);
              });
            });
          }
        }, 250); // Bumped from 100 to 250 - to temporarily resolve 'UNKNOWN_ERROR' status being returned by geocoder.geocode
      } else {
        this.address = changes['location'].currentValue;
      }
    }
  }


  initAutocomplete = () => {
    const autocomplete = new google.maps.places.Autocomplete(this.searchAddressElement.nativeElement, { types: ['address'] });
    autocomplete.addListener('place_changed', () => {
      this.ngZone.run(() => {
        const place: google.maps.places.PlaceResult = autocomplete.getPlace();
        if (place.geometry === undefined || place.geometry === null) {
          return;
        }
        this.address.latitude = place.geometry.location.lat();
        this.address.longitude = place.geometry.location.lng();

        this.getAddress(this.address.latitude, this.address.longitude, (address) => {
          this.address.formattedAddress = address;

          this.addressChange.emit(this.address);
        });
      });
    });
  }

  getAddress(latitude: number, longitude: number, callback: (address: string) => void) {
    const point = new google.maps.LatLng(latitude, longitude);
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: point }, (results, status) => {
      //console.log(this.label, point.lat(), point.lng(), results, status);
      if (results && results.length > 0) {
        callback(results[0].formatted_address);
      }
    });
  }


}