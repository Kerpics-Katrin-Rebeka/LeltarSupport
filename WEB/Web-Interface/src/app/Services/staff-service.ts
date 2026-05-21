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
    var data = this.http.get<UserModel[]>("http://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/users",{headers});    
    return data;
  }

  getRoles(){
    const headers = new HttpHeaders({
      Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    return this.http.get<Role[]>("http://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/roles", {headers});
  }

  Recruit(newGuy:newUser){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.post<UserModel>("http://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/users", newGuy, {headers});    
    return data;
  }

  EditEmployee(employee:UserModel){    
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.put<UserModel[]>(`http://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/users/${employee.id}`, employee, {headers});    
    return data;
  }

  RemoveEmployee(id:number){
    const headers = new HttpHeaders({
    Authorization: sessionStorage.getItem("token")? `Bearer ${sessionStorage.getItem("token")}`:"",
    });
    var data = this.http.delete<UserModel[]>(`https://vizsgaremek-leltar-support.jcloud.jedlik.cloud/api/users/${id}`, {headers});    
    return data;
  }
}
