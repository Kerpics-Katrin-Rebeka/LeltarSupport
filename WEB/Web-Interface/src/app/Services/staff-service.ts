import { Injectable } from '@angular/core';
import UserModel from '../Models/UserModel';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class StaffService {
  constructor(private http: HttpClient){}

  getEmployees(){
    var data = this.http.get<UserModel[]>("http://127.0.0.1:8000/api/employees");
    return data;
  }
}
