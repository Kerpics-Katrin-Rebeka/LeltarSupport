import { Injectable } from '@angular/core';
import IngredientModel from '../Models/IngredientModel';
import { HttpClient } from '@angular/common/http';
import { response } from '../Models/UserModel';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http:HttpClient){}

  getIngredients(){
    var data = this.http.get<IngredientModel[]>("http://127.0.0.1:8000/api/ingredients");
    return data;
  }

  Login(email:string, pwd:string){
    var data = this.http.post<response>("http://127.0.0.1:8000/api/login", {'email':email, 'password':pwd});
    console.log(data);
    return data;
  }
}
