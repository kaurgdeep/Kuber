import { UserType } from "../Enums/UserType";

export interface IUserInformation {
    userId?: number;
    emailAddress?: string;
    userType?: UserType;
}
