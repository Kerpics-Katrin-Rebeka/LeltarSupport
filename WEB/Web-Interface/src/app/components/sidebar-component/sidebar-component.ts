import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar-component',
  imports: [],
  templateUrl: './sidebar-component.html',
  styleUrl: './sidebar-component.css',
})
export class SidebarComponent {
  @Output() logOut = new EventEmitter;
  @Output() navigated=new EventEmitter;

  constructor(private http:HttpClient){}

  logout(){
    sessionStorage.setItem("loggedIn","false"),
    this.logOut.emit(false)
    this.http.post("http://127.0.0.1:8000/api/logout",{})
  }

  navigateTo(chosenPage:string){
    this.navigated.emit(chosenPage)
  }
}

