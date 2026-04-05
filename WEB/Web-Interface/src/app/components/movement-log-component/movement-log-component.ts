import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { MovementModel } from '../../Models/SalesModel';
import { DataService } from '../../Services/data-service';
import { MovementComponent } from '../movement-component/movement-component';
import { timer } from 'rxjs';

@Component({
  selector: 'app-movement-log-component',
  imports: [MovementComponent],
  templateUrl: './movement-log-component.html',
  styleUrl: './movement-log-component.css',
})
export class MovementLogComponent {
    @Output() back = new EventEmitter;
    movements:MovementModel[] = [];

    constructor(private dataService: DataService, private cdr: ChangeDetectorRef){}

    ngOnInit(){
      this.dataService.getStockMovements().subscribe({
        next: (movements)=>{
          this.movements = movements;
          this.cdr.detectChanges();
          timer(1000).subscribe(() => {});
        },
        error: (err)=>{
          console.log(err);
        }
      });
    }


    goBack(){
      this.back.emit(false);
    } 
}
