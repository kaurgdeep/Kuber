import { IRegisterUser } from '../Interfaces/IRegisterUser';
import { UserType } from '../Enums/UserType';


export class RegisterUser implements IRegisterUser {
    emailAddress: string;
    password: string;
    userType: UserType;

}
