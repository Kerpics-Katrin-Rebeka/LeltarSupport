import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sales-component',
  imports: [],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() openLog = new EventEmitter;

  ngOnInit(){
  }

  openSalesLogs(day:string){
    sessionStorage.setItem("isViewingLog","true")
    sessionStorage.setItem("selectedDay",`${day}`)
    this.openLog.emit(true);
  }
}
