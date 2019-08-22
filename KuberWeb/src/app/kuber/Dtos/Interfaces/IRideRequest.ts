import { IAddress } from "./IAddress";

export interface IRideRequest {
  pickupAddress?: IAddress;
  dropoffAddress?: IAddress;
}
