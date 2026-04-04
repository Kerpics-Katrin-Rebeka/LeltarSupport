import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';
import IngredientModel from '../../Models/IngredientModel';
import { SalesComponent } from "../sales-component/sales-component";
import { SalesLogComponent } from "../sales-log-component/sales-log-component";
import { DataService } from '../../Services/data-service';
import { MovementModel, RestockModel } from '../../Models/SalesModel';
import { MovementComponent } from '../movement-component/movement-component';

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent, SalesComponent, SalesLogComponent, MovementComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() outOfIngredient = new EventEmitter;
  isOutOfIngredient:boolean=false;
  goal:number=100;
  isInStorage:boolean=false;
  isViewingLog: boolean = sessionStorage.getItem("isViewingLog") == "true";
  outOf:IngredientModel[] = [];
  underLimit:IngredientModel[] = [];
  ingredients:IngredientModel[] = [];
  restocks:RestockModel[] = [];
  movementLog:MovementModel[] = [];
  recentMovements:MovementModel[] = [];

  constructor(private dataService: DataService,private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.dataService.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients;
      this.outOf= this.ingredients.filter((ing: IngredientModel) => ing.amount == 0);
      this.underLimit= this.ingredients.filter((ing: IngredientModel) => ing.amount < ing.maxAmount*0.1 && ing.amount > 0);
    });
    this.getRestocks();
    this.getMovements();
    
    sessionStorage.setItem("isViewingLog","false");
  }

  getRestocks(){
    this.dataService.getRestock().subscribe({
      next: (restocks)=>{
        this.restocks = restocks;
        this.cdr.detectChanges();
        console.log(this.restocks[0].items);
      },
      error: (err)=>{
        console.log(err);
      }
    });
  }

  getMovements(){
    this.dataService.getStockMovements().subscribe({
      next: (movements)=>{
        this.movementLog = movements;
        this.recentMovements = movements.slice(0,5);
        console.log(this.movementLog);
        this.cdr.detectChanges();
      },
      error: (err)=>{
        console.log(err);
      }
    })
  }

  outOfLog(){
    sessionStorage.setItem("isViewingLog","false")
    this.isViewingLog=!this.isViewingLog;
  }

  openLog(isIt:boolean){
    sessionStorage.setItem("isViewingLog",`${isIt}`)
    this.isViewingLog = isIt;
  }


}
