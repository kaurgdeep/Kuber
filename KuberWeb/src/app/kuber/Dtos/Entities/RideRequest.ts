import { IRideRequest } from '../Interfaces/IRideRequest';
import { Address } from './Address';

export class RideRequest implements IRideRequest {
  pickupAddress: Address;
  dropoffAddress: Address;
}
