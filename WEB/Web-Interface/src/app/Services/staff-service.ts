import { Injectable } from '@angular/core';
import UserModel, { newUser, Role } from '../Models/UserModel';
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

  getRoles(){
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    return this.http.get<Role[]>("http://127.0.0.1:8000/api/roles", {headers});
  }

  Recruit(newGuy:newUser){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.post<UserModel>("http://127.0.0.1:8000/api/users", newGuy, {headers});    
    return data;
  }

  EditEmployee(employee:UserModel){    
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.put<UserModel[]>(`http://127.0.0.1:8000/api/users/${employee.id}`, employee, {headers});    
    return data;
  }

  RemoveEmployee(id:number){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.delete<UserModel[]>(`http://127.0.0.1:8000/api/users/${id}`, {headers});    
    return data;
  }
}
