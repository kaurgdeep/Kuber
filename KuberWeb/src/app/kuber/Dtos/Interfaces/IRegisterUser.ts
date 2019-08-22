import { UserType } from "../Enums/UserType";

export interface IRegisterUser {
    emailAddress?: string;
    password?: string;
    userType?: UserType;

}
