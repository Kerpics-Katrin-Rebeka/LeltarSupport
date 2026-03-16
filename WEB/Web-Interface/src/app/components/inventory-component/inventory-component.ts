import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';
import IngredientModel from '../../Models/IngredientModel';
import { SalesComponent } from "../sales-component/sales-component";
import { SalesLogComponent } from "../sales-log-component/sales-log-component";
import { DataService } from '../../Services/data-service';

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent, SalesComponent, SalesLogComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() outOfIngredient = new EventEmitter;
  isOutOfIngredient:boolean=false;
  goal:number=100;
  isInStorage:boolean=false;
  isViewingLog: boolean = sessionStorage.getItem("isViewingLog") == "true";

  outOf:IngredientModel[]= DataService.ingredients.filter(ing => ing.amount == 0);
  underLimit:IngredientModel[]= DataService.ingredients.filter(ing => ing.amount <= (ing.maxAmount*0.1) && ing.amount != 0);
  ingredients: IngredientModel[] = DataService.ingredients;


  ngOnInit(){
    sessionStorage.setItem("isViewingLog","false");
    console.log(this.ingredients);
    this.fillTable();
    if (this.outOf.length != 0) {
      this.isOutOfIngredient = true;
    }
  }

  fillTable(){
    const fullers = document.getElementsByClassName("fuller");
    for (let i = 0; i < fullers.length; i++) {
      const curr = this.rng();
      (fullers[i] as HTMLDivElement).style.height = `${(curr/this.goal)*100}%`;
    }
  }

  rng(){
    return Math.round(Math.random()*100)
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
