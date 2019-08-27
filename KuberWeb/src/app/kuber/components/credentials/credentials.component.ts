import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { UserType } from '../../Dtos/Enums/UserType';
import { IRegisterUser } from '../../Dtos/Interfaces/IRegisterUser';
import { ILoginUser } from '../../Dtos/Interfaces/ILoginUser';

@Component({
  selector: 'app-credential',
  templateUrl: './credentials.component.html',
  styleUrls: ['./credentials.component.scss']
})
export class CredentialsComponent implements OnInit {

  @Input() emailAddress: string;
  @Input() userType: UserType;
  @Input() isRegister: boolean;
  @Input() busy: boolean;

  @Input() errorStatus: string;
  @Input() status: string;

  @Output() registerClick = new EventEmitter<IRegisterUser>();
  @Output() loginClick = new EventEmitter<ILoginUser>();

  vm: ILoginUser = {};

  constructor() { }

  isDefined(o: any) {
      return (typeof (o) !== 'undefined');
  }

  ngOnInit() {
      this.vm.emailAddress = this.emailAddress;
  }

  register() {
      this.registerClick.emit({ 
      emailAddress: this.vm.emailAddress, 
      password: this.vm.password,
      userType: this.userType
    });
  }

  login() {
      this.loginClick.emit({ 
      emailAddress: this.vm.emailAddress, 
      password: this.vm.password,
      userType: this.userType
    });
  }

}
