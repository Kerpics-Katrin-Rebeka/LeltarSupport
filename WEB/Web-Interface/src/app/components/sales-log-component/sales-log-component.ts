import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { OrderModel } from '../../Models/SalesModel';
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
  errorString:string="";
  requestDate:Date=new Date();

  incomeTotal:number=0;
  salesLog:OrderModel[]=[];

  constructor(private salesService: SalesService,private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.getSales();    
  }

  getSales(){
    if (sessionStorage.getItem("selectedDay") == "yesterday") {
      this.requestDate.setDate(this.requestDate.getDate() - 1);
    }
    else{
      this.requestDate = new Date(sessionStorage.getItem("selectedDay")??"");
    }
    this.salesService.getSales(this.requestDate).subscribe({
      next: (data) => {
        this.salesLog = data as OrderModel[];
        this.IncomeSum();
        timer(1000).subscribe({
          next: () => {
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        });
      },
      error: (error) => {
        if (error.status == 404) {
          this.errorString = "No sales data found for the selected date.";
        } else {
          this.errorString = "An error occurred while fetching sales data.";
        }

        this.loadError = true;
        this.isLoading = false;
        this.cdr.detectChanges();
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

  goBack(){
    sessionStorage.setItem("isViewingLog","false")

    this.back.emit(false);
  }
}
