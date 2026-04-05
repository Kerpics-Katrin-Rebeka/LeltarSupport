import { Injectable } from '@angular/core';
import IngredientModel from '../Models/IngredientModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { response } from '../Models/UserModel';
import { MovementModel, RestockModel } from '../Models/SalesModel';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http:HttpClient){}

  getIngredients(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<IngredientModel[]>("http://127.0.0.1:8000/api/inventory",{headers});
    return data;
  }

  getRestock(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RestockModel[]>("http://127.0.0.1:8000/api/purchase-orders",{headers});
    return data;
  }

  getStockMovements(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<MovementModel[]>("http://127.0.0.1:8000/api/stock-movements",{headers});
    return data;
  }

  updatePurchaseOrder(id: number, status: string) {
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    return this.http.put(`http://127.0.0.1:8000/api/purchase-orders/${id}`, { status }, { headers });
  }

  Login(email:string, pwd:string){
    var data = this.http.post<response>("http://127.0.0.1:8000/api/login", {'email':email, 'password':pwd},);
    return data;
  }
}
