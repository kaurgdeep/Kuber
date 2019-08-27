import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserType } from '../../Dtos/Enums/UserType';
import { IRegisterUser } from '../../Dtos/Interfaces/IRegisterUser';
import { UserService } from '../../services/UserService';


@Component({
  selector: 'app-register-view',
  templateUrl: './register-view.component.html',
  styleUrls: ['./register-view.component.scss']
})
export class RegisterViewComponent implements OnInit {

  // private usertype: string;
  // constructor(private route: ActivatedRoute, private httpClient: HttpClient) { }

  // ngOnInit() {
  //   this.usertype =  this.route.snapshot.queryParamMap.get('usertype');
  //   console.log(this.route.snapshot.queryParamMap);
  // }

  // register(){

  //   this.httpClient.post("/api/users/register", {emailAddress: "client@a.com", password: "aaaaaaaa"})
  //     .subscribe((data)=>{console.log(data)},(error)=>{console.error(error)});
  // }


  userType: UserType;
  apiCall: boolean;
  errorStatus: string;

  constructor(private activatedRoute: ActivatedRoute, public router: Router, private userService: UserService ) { }

  ngOnInit() {
    this.userType = UserType[this.activatedRoute.snapshot.queryParamMap.get('userType')];
  }

  async register(user: IRegisterUser) {
    const response = await this.userService.create(user);
    console.log(response);
    if (response) {
      //this.status = 'Login succeeded! Redirecting to home page';
      setTimeout(() => {
        console.log('Redirecting to home', JSON.stringify(localStorage));
        this.router.navigate(['/']);
      }, 100);
    } else {
      this.apiCall = false;
      this.errorStatus = 'Login failed';
    }

    
    
  }
}
