import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sales-component',
  imports: [],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() backToMenu = new EventEmitter;
  backBtn:string = "<-";
  weeklyTotal:number=0;
  dailyTotal:number=0;

  goBack(){
    this.backToMenu.emit("menu");
  }

  openSalesLogs(){
    
  }
}
