import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-register-view',
  templateUrl: './register-view.component.html',
  styleUrls: ['./register-view.component.scss']
})
export class RegisterViewComponent implements OnInit {

  private usertype: string;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.usertype =  this.route.snapshot.queryParamMap.get('usertype');
    console.log(this.route.snapshot.queryParamMap);
  }

}
