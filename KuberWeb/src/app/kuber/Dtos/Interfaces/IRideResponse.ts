import { IAddress } from './IAddress';
import { RideStatus } from '../Enums/RideStatus';

export interface IRideResponse {
  id?: number;
  passengerId?: number;
  driverId?: number;
  fromAddress?: IAddress;
  toAddress?: IAddress;
  currentAddress?: IAddress;
  rideStatus?: RideStatus;
  requested?: Date | string;
  cancelled?: Date | string;
  accepted?: Date | string;
  rejected?: Date | string;
  pickedUp?: Date | string;
  droppedOff?: Date | string;
  positionUpdated?: Date | string;
}
