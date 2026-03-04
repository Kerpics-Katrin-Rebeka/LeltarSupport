import { Component, EventEmitter, Output } from '@angular/core';
import { timer } from 'rxjs';

@Component({
  selector: 'app-inventory-component',
  imports: [],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() backToMenu = new EventEmitter;
  backButton:string = "<-"
  goal:number=100;

  async ngOnInit(){
    var fullers = await document.getElementsByClassName("fuller");
    var bars = await document.getElementsByClassName("bar");
    console.log(fullers);
    for (let i = 0; i < bars.length; i+1) {
      var curr = this.rng();
      console.log(fullers[i]);
      (fullers[i] as HTMLDivElement).style.height = `${(curr/this.goal)*100}%`;
      (fullers[i] as HTMLDivElement).className += "p-3 bg-gradient-from-t from-blue-600 to-blue-200"
    }
  }

  rng(){
    return Math.round(Math.random()*100)
  }

  goBack(){
    this.backToMenu.emit("menu");
  }
}
