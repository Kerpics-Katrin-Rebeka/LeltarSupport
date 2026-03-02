import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sales-component',
  imports: [],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() backToMenu = new EventEmitter;

  goBack(){
    this.backToMenu.emit("menu");
  }
}
