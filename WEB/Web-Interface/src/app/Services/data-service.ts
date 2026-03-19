import { Injectable } from '@angular/core';
import IngredientModel from '../Models/IngredientModel';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http:HttpClient){}
  public static ingredients: IngredientModel[]=[];

  getIngredients(){
    var data = this.http.get<IngredientModel[]>("http://127.0.0.1:8000/api/ingredients");
    return data;
  }

  
}
