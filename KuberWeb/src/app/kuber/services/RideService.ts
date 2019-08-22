import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ICreateResponse } from '../Dtos/Interfaces/ICreateResponse';
import { IRideResponse } from '../Dtos/Interfaces/IRideResponse';
import { IRideRequest } from '../Dtos/Interfaces/IRideRequest';
import { IAddress } from '../Dtos/Interfaces/IAddress';
import { IRideStatus } from '../Dtos/Interfaces/IRideStatus';
import { ApiServiceBase } from './ApiServiceBase';
import { INearbyRequest } from '../Dtos/Interfaces/INearbyRequest';

@Injectable()
export class RideService extends ApiServiceBase {

    constructor(httpClient: HttpClient) {
        super(httpClient, '/api/rides');
    }

    async create(rideRequest: IRideRequest): Promise<ICreateResponse> {
        return await super.httpPost<IRideRequest, ICreateResponse>({ payload: rideRequest });
    }

    async getAnyActive(): Promise<IRideResponse> {
        return await super.httpGet<IRideResponse>();
    }

    async getNearby(location: IAddress, radiusMeters: number): Promise<Array<IRideResponse>> {
        return await super.httpGetQueryParams<INearbyRequest, Array<IRideResponse>>({
            url: 'near-by',
            queryParams: {
                latitude: location.latitude,
                longitude: location.longitude,
                radiusMeters: radiusMeters
            },
            defaultResponse: []
        });
    }

    async get(rideId: number): Promise<IRideResponse> {
        return await super.httpGet<IRideResponse>({ url: `${rideId}` });
    }

    async cancel(rideId: number): Promise<any> {
        return await super.httpPost<{}, {}>({ url: `${rideId}/cancel` });
    }

    async accept(rideId: number): Promise<any> {
        return await super.httpPost<{}, {}>({ url: `${rideId}/accept` });
    }

    async reject(rideId: number): Promise<any> {
        return await super.httpPost<{}, {}>({ url: `${rideId}/reject` });
    }

    async pickUp(rideId: number): Promise<any> {
        return await super.httpPost<{}, {}>({ url: `${rideId}/pick-up` });
    }

    async dropOff(rideId: number): Promise<any> {
        return await super.httpPost<{}, {}>({ url: `${rideId}/drop-off` });
    }

    async updateLocation(rideId: number, rideStatus: IRideStatus): Promise<any> {
        return await super.httpPost<IRideStatus, {}>({
            url: `${rideId}/update-location`,
            payload: rideStatus
        });
    }
}
