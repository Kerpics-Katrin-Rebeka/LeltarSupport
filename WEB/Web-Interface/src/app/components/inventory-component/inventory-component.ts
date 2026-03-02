import { Component, EventEmitter, Output } from '@angular/core';

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

  ngOnInit(){
    var fullers = document.getElementsByClassName("fuller") as  HTMLCollectionOf<HTMLDivElement>;
    var bars  = document.getElementsByClassName("bar") as  HTMLCollectionOf<HTMLDivElement>;
    for (let i = 0; i < bars.length; i++) {
      var curr = this.rng();
      fullers[i].style.height = `${(curr/this.goal)*100}%`;
      bars[i].appendChild(fullers[i])
    }
  }

  rng(){
    return Math.round(Math.random()*100)
  }

  goBack(){
    this.backToMenu.emit("menu");
  }
}
