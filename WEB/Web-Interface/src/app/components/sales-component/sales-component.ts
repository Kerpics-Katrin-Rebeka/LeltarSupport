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

  openSalesLogs(){
    sessionStorage.setItem("isViewingLog","true")
    this.openLog.emit(true);
  }
}
