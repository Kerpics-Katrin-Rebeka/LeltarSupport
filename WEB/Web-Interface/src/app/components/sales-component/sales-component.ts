import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sales-component',
  imports: [FormsModule],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() openLog = new EventEmitter;
  selectedDate:string = new Date().toISOString().split('T')[0];

  ngOnInit(){
  }

  openSalesLogs(day:string){
    sessionStorage.setItem("isViewingLog","true")
    sessionStorage.setItem("selectedDay",`${day}`)
    this.openLog.emit(true);
  }
}
