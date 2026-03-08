import { Component } from '@angular/core';
import SalesModel from '../../Models/SalesModel';

@Component({
  selector: 'app-sales-log-component',
  imports: [],
  templateUrl: './sales-log-component.html',
  styleUrl: './sales-log-component.css',
})
export class SalesLogComponent {
  costTotal:number=10000;
  incomeTotal:number=10001;
  salesLog:SalesModel[]=[];

  goBack(){
    sessionStorage.setItem("isViewingLog","false")
  }
}
