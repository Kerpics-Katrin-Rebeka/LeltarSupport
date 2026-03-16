import { Component, EventEmitter, Output } from '@angular/core';
import SalesModel from '../../Models/SalesModel';

@Component({
  selector: 'app-sales-log-component',
  imports: [],
  templateUrl: './sales-log-component.html',
  styleUrl: './sales-log-component.css',
})
export class SalesLogComponent {
  @Output() back = new EventEmitter;
  today:string|null=null;
  yesterday:string|null=null;

  costTotal:number=10000;
  incomeTotal:number=10001;
  salesLog:SalesModel[]=[];

  ngOnInit(){
    this.checkDay();
  }

  checkDay(){

    var day = sessionStorage.getItem("selectedDay");
    if (day === "today") {
      this.today = sessionStorage.getItem("selectedDay");
    }
    else if (day=== "yesterday") {
      this.yesterday= sessionStorage.getItem("selectedDay")
    }
  }

  goBack(){
    sessionStorage.setItem("isViewingLog","false")

    this.back.emit(false);
  }
}
