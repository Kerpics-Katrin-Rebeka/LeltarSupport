import { Injectable } from '@angular/core';
import IngredientModel from '../Models/IngredientModel';

@Injectable({
  providedIn: 'root',
})
export class DataService {

    static ingredients:IngredientModel[] = [
    {id:0,name:'CHEESE',unit:"slice(s)",maxAmount:100,amount:10},
    {id:0,name:'CHEESE(grated)',unit:"g",maxAmount:1000,amount:900},
    {id:0,name:'BUNS',unit:"piece(s)",maxAmount:105,amount:100},
    {id:0,name:'PATTY(made)',unit:"piece(s)",maxAmount:100,amount:0},
    {id:0,name:'SAUCE',unit:"ml",maxAmount:10000,amount:10000},
  ];
}
