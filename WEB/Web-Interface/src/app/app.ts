import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./components/login-component/login-component";
import { SidebarComponent } from "./components/sidebar-component/sidebar-component";
import { InventoryComponent } from "./components/inventory-component/inventory-component";
import { StaffComponent } from './components/staff-component/staff-component';

@Component({
  selector: 'app-root',
  imports: [LoginComponent, InventoryComponent, SidebarComponent, StaffComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Web-Interface');
  currentPage="inventory";

  ngOnInit(){
    sessionStorage.setItem("loggedIn","false");
  }
  
  isLoggedIn?:boolean = sessionStorage.getItem("loggedIn")==="true";


  loginAttempted(){
    this.isLoggedIn = sessionStorage.getItem("loggedIn")==="true";
    console.log(sessionStorage.getItem("loggedIn"))
  }
}
