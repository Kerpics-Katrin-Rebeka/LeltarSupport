import { Component, EventEmitter, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  goal:number=100;
  isInStorage:boolean=false;

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
