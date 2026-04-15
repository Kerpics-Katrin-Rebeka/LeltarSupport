import { Injectable } from '@angular/core';
import IngredientModel, { UnderLimitResponseModel } from '../Models/IngredientModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import UserModel, { response } from '../Models/UserModel';
import { MovementModel, RecommendationItemModel, RestockModel } from '../Models/SalesModel';

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

  getRecommendations(){
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RecommendationItemModel>("http://127.0.0.1:8000/api/inventory/low-stock",{headers});
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

  createPurchaseOrder(items: RecommendationItemModel[], supplier_id: number) {
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    const mappedItems = items.map(item => ({
      ingredient_id: item.ingredient.id,
      quantity: item.quantity,
    }));
    return this.http.post("http://127.0.0.1:8000/api/purchase-orders", { items: mappedItems, supplier_id }, { headers });
  }

  updatePurchaseOrder(id: number, status: string) {
    const supplier_id = 1;
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    return this.http.post(`http://127.0.0.1:8000/api/purchase-orders/${id}`, { status, supplier_id }, { headers });
  }

  Login(email:string, pwd:string){
    var data = this.http.post<response>("http://127.0.0.1:8000/api/auth/login", {'email':email, 'password':pwd},);
    return data;
  }

  LogOut(){
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.post("http://127.0.0.1:8000/api/logout", {}, {headers});
    return data;
  }

  getLoggedInUser(id:string){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<UserModel>(`http://127.0.0.1:8000/api/users/${id}`,{headers});
    return data;
  }
}
