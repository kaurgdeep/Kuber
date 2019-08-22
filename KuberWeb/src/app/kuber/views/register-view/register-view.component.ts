import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserType } from '../../Dtos/Enums/UserType';
import { IRegisterUser } from '../../Dtos/Interfaces/IRegisterUser';


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

  constructor(private activatedRoute: ActivatedRoute, private httpClient: HttpClient) { }

  ngOnInit() {
    this.userType = UserType[this.activatedRoute.snapshot.queryParamMap.get('userType')];
  }

  async register(user: IRegisterUser) {
    const promise = new Promise((resolve, reject) => {
      this.httpClient.post('/api/users/register', user)
        .subscribe((data) => resolve(data),
          (reason) => reject(reason));
    });

    try {
      this.apiCall = true;
      const response = await promise;
      console.log(response);
    } catch (e) {
      this.errorStatus = 'Error registering the user';
    } finally {
      this.apiCall = false;
    }
  }
}
