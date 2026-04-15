import { Component, EventEmitter, Input, Output } from '@angular/core';
import IngredientModel from '../../Models/IngredientModel';
import { DataService } from '../../Services/data-service';

@Component({
  selector: 'app-storage-component',
  imports: [],
  templateUrl: './storage-component.html',
  styleUrl: './storage-component.css',
})
export class StorageComponent {
  @Output() back = new EventEmitter;
  @Output() ranOut = new EventEmitter;
  @Input() ingredients:any = [];
  backBtn:string = "<-";

  constructor(private dataService:DataService){}

  ngOnInit(){
    this.dataService.getIngredients().subscribe(data=>{this.ingredients = data;});
  }

  goBack(){
    this.back.emit(false);
  }
}
