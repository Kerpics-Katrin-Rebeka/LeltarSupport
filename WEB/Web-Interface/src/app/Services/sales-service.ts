import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import SalesModel, { OrderModel, RecommendationItemModel, RestockModel } from '../Models/SalesModel';

@Injectable({
  providedIn: 'root',
})
export class SalesService {

  constructor(private http: HttpClient){}

  getSales(date:Date){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<OrderModel[]>(`http://127.0.0.1:8000/api/${date.toISOString().split('T')[0]}/orders`,{headers});
    return data;
  }

  getRestocks(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RestockModel[]>("http://127.0.0.1:8000/api/purchase-orders",{headers});
    return data;
  }

  getSuppliers(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RestockModel[]>("http://127.0.0.1:8000/api/suppliers",{headers});
    return data;
  }

  placeRestockOrder(items:RecommendationItemModel[],supplier_id:number){
    const status = "ordered";
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.post<RestockModel[]>("http://127.0.0.1:8000/api/purchase-orders",{items,supplier_id,status},{headers});
    return data;
  }

}
