import { Component, EventEmitter, Input, Output } from '@angular/core';
import IngredientModel from '../../Models/IngredientModel';

@Component({
  selector: 'app-storage-component',
  imports: [],
  templateUrl: './storage-component.html',
  styleUrl: './storage-component.css',
})
export class StorageComponent {
  @Output() back = new EventEmitter;
  @Output() ranOut = new EventEmitter;
  @Input() ingredients:IngredientModel[] = [];
  backBtn:string = "<-";

  ngOnInit(){

  }

  differential(current:number, max:number){
    if (max == current) return 0;
    return (max-current) < 0 ? `+${-(max-current)}` : `-${(max-current)}`;
  }

  goBack(){
    this.back.emit(false);
  }
}
