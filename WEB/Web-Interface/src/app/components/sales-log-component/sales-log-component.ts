import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import SalesModel, { OrderModel } from '../../Models/SalesModel';
import { SalesService } from '../../Services/sales-service';
import { timer } from 'rxjs';

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
  isLoading = true;
  loadError = false;

  incomeTotal:number=0;
  salesLog:OrderModel[]=[];

  constructor(private salesService: SalesService,private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.checkDay();
    this.salesService.getSales().subscribe({
      next: (data) => {
        this.salesLog = data as OrderModel[];
        this.IncomeSum();
        timer(1000).subscribe({
          next: () => {
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        });
        console.log(this.salesLog);
        console.log(this.incomeTotal);
        
      },
      error: (error) => {
        this.loadError = true;
        this.isLoading = false;
        console.error('Error fetching sales data:', error);
      }
    });
  }

  IncomeSum(){
    this.salesLog.forEach(order => {
      order.items.forEach(item => {
        this.incomeTotal += item.product.price * item.quantity;
      });
    });
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
