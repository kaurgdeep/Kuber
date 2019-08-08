import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.scss']
})
export class LoginViewComponent implements OnInit {
  private usertype: string;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.usertype =  this.route.snapshot.queryParamMap.get('usertype');
    console.log(this.route.snapshot.queryParamMap);
  }

}
