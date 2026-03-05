import { Component, Input } from '@angular/core';
import IngredientModel from '../../Models/IngredientModel';

@Component({
  selector: 'app-storage-component',
  imports: [],
  templateUrl: './storage-component.html',
  styleUrl: './storage-component.css',
})
export class StorageComponent {
  ingredients:IngredientModel[] = [];

  ngOnInit(){
    this.ingredients = [
    {id:0,name:'CHEESE',unit:"slice",maxAmount:100,amount:100},
    {id:0,name:'CHEESE(grated)',unit:"g",maxAmount:1000,amount:1000},
    {id:0,name:'BUNS',unit:"db",maxAmount:100,amount:100},
    {id:0,name:'PATTY(made)',unit:"db",maxAmount:100,amount:100},
    {id:0,name:'SAUCE',unit:"ml",maxAmount:10000,amount:10000},
    ]
  }
}
