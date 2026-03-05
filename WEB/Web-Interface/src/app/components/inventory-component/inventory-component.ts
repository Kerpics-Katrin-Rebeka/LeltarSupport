import { Component, EventEmitter, Output } from '@angular/core';
import { timer } from 'rxjs';
import { SidebarComponent } from "../sidebar-component/sidebar-component";

@Component({
  selector: 'app-inventory-component',
  imports: [SidebarComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  goal:number=100;
  interval:any;

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
}
