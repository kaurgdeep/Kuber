import { IAddress } from '../Interfaces/IAddress';

export class Address implements IAddress {
  formattedAddress: string;
  longitude: number;
  latitude: number;
}
