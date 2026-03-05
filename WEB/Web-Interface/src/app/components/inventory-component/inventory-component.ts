import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';
import IngredientModel from '../../Models/IngredientModel';

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() outOfIngredient = new EventEmitter;
  goal:number=100;
  isInStorage:boolean=false;
  @Input() ingredients:IngredientModel[] = [];
  @Input() isOutOfIngredient:boolean=false;

  ngOnInit(){
    this.fillTable();
    
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

  openStorage(){
    this.isInStorage = true;
  }
}
