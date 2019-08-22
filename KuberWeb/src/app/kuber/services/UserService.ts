import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ILoginUser } from '../Dtos/Interfaces/ILoginUser';
import { AuthenticationStore } from './AuthenticationStore';
import { ITokenResponse } from '../Dtos/Interfaces/ITokenResponse';
import { IUserInformation } from '../Dtos/Interfaces/IUserInformation';
import { ApiServiceBase } from './ApiServiceBase';

@Injectable()
export class UserService extends ApiServiceBase {

    constructor(httpClient: HttpClient, private authenticationStore: AuthenticationStore) {
        super(httpClient, '/api/users');
    }

    async login(user: ILoginUser): Promise<boolean> {
        const authenticationToken = await super.httpPost<ILoginUser, ITokenResponse>({
            url: 'log-in',
            payload: user
        });

        // update local storage if we got a token, else clear the local storage (by calling logout)
        if (authenticationToken &&
            authenticationToken.token &&
            authenticationToken.token.length > 0) {

            this.authenticationStore.setAuthentication(authenticationToken.token);
            return true;
        }

        this.logout();
        return false;
    }

    async getMe(): Promise<IUserInformation> {
        return super.httpGet({ url: 'me' });
    }

    logout(): void {
        this.authenticationStore.resetAuthentication();
    }
}
