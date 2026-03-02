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

  goBack(){
    this.backToMenu.emit("menu");
  }
}
