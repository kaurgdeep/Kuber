import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserType } from '../../Dtos/Enums/UserType';
import { IRegisterUser } from '../../Dtos/Interfaces/IRegisterUser';
import { UserService } from '../../services/UserService';

@Component({
  selector: 'app-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.scss']
})
export class LoginViewComponent implements OnInit {
  // private usertype: string;
  // constructor(private route: ActivatedRoute) { }

  // ngOnInit() {
  //   this.usertype =  this.route.snapshot.queryParamMap.get('usertype');
  //   console.log(this.route.snapshot.queryParamMap);
  // }
  status: string;
  errorStatus: string;

  userType: UserType;
  apiCall: boolean;

  constructor(private activatedRoute: ActivatedRoute, public router: Router, private userService: UserService) { }

  ngOnInit() {

    this.userService.logout();

    this.userType = UserType[this.activatedRoute.snapshot.queryParamMap.get('userType')];
  }

  async login(user: IRegisterUser) {
    console.log(user);
    this.apiCall = true;
    const response = await this.userService.login(user);
    if (response) {
      this.status = 'Login succeeded! Redirecting to home page';
      setTimeout(() => {
        console.log('Redirecting to home', JSON.stringify(localStorage));
        this.router.navigate(['/']);
      }, 100);
    } else {
      this.apiCall = false;
      this.errorStatus = 'Login failed';
    }
    //console.log(user);
  }

}
