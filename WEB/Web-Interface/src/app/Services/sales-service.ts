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
    var data = this.http.get<OrderModel[]>(`https://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/${date.toISOString().split('T')[0]}/orders`,{headers});
    return data;
  }

  getRestocks(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RestockModel[]>("https://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/purchase-orders",{headers});
    return data;
  }

  getSuppliers(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<RestockModel[]>("https://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/suppliers",{headers});
    return data;
  }

  placeRestockOrder(items:RecommendationItemModel[],supplier_id:number){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    const mappedItems = items.map(item => ({
      ingredient_id: item.ingredient.id,
      quantity: item.quantity
    }));
    var data = this.http.post<RestockModel[]>("https://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/purchase-orders",{items: mappedItems,supplier_id},{headers});
    return data;
  }

}
