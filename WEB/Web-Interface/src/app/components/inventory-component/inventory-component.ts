import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';
import IngredientModel from '../../Models/IngredientModel';
import { SalesComponent } from "../sales-component/sales-component";

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent, SalesComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() outOfIngredient = new EventEmitter;
  isOutOfIngredient:boolean=false;
  goal:number=100;
  isInStorage:boolean=false;
  outOf:IngredientModel[]=[];
  underLimit:IngredientModel[]=[];
  ingredients:IngredientModel[] = [
    {id:0,name:'CHEESE',unit:"slice(s)",maxAmount:100,amount:10},
    {id:0,name:'CHEESE(grated)',unit:"g",maxAmount:1000,amount:900},
    {id:0,name:'BUNS',unit:"piece(s)",maxAmount:105,amount:100},
    {id:0,name:'PATTY(made)',unit:"piece(s)",maxAmount:100,amount:0},
    {id:0,name:'SAUCE',unit:"ml",maxAmount:10000,amount:10000},
    ];

  ngOnInit(){
    this.fillTable();
    this.checkForEmpty();
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

  checkForEmpty(){
    this.ingredients.forEach(ing => {
      if (ing.amount == 0) {
        this.outOf.push(ing)
        this.isOutOfIngredient = true;
      }
      else if (ing.amount<= (ing.maxAmount*0.1)) {
        this.underLimit.push(ing)
      }
    });
  }
}
