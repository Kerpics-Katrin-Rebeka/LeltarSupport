import { Injectable } from '@angular/core';
import UserModel from '../Models/UserModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class StaffService {
  constructor(private http: HttpClient){}

  getEmployees(){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.get<UserModel[]>("http://127.0.0.1:8000/api/users",{headers});    
    return data;
  }

  Recruit(newGuy:UserModel){

  }
}
