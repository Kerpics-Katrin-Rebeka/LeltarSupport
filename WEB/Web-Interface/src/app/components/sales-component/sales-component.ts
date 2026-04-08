import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SalesService } from '../../Services/sales-service';
import { ItemModel } from '../../Models/SalesModel';

@Component({
  selector: 'app-sales-component',
  imports: [FormsModule],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() openLog = new EventEmitter;
  selectedDate:string = new Date().toISOString().split('T')[0];
  topSellers:ItemModel[] = [];

  constructor(private salesService: SalesService){}

  ngOnInit(){
    this.getTopSellers();
  }

  openSalesLogs(day:string){
    sessionStorage.setItem("isViewingLog","true")
    sessionStorage.setItem("selectedDay",`${day}`)
    this.openLog.emit(true);
  }

  getTopSellers(){
    var date = new Date();
    date.setDate(date.getDate()-1);
    this.salesService.getSales(new Date("2026-03-31")).subscribe({
      next: (data) => {
        console.log(data);
        data.forEach(order => {
          order.items.forEach(item => {this.topSellers.push(item)});
        });
        this.topSellers.sort((a,b) => b.quantity - a.quantity);
        this.topSellers = this.topSellers.slice(0,3);
        console.log(this.topSellers);
        
      },
      error: (error) => {
        console.error('Error fetching sales data:', error);
      }
    });
  }
}
