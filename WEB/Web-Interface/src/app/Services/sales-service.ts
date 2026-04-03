import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import SalesModel, { OrderModel } from '../Models/SalesModel';

@Injectable({
  providedIn: 'root',
})
export class SalesService {

  constructor(private http: HttpClient){}

  getSales(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<SalesModel[]>("http://127.0.0.1:8000/api/orders",{headers});
    return data;
  }

  getRestocks(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<OrderModel[]>("http://127.0.0.1:8000/api/purchase-orders",{headers});
    return data;
  }

  getSalesByDay(day:string){}

}
