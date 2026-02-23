import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./components/login-component/login-component";
import { SidebarComponent } from "./components/sidebar-component/sidebar-component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoginComponent, SidebarComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Web-Interface');
  
  isLoggedIn:boolean = false;

  loginAttempted(isLoggedIn:boolean){
    this.isLoggedIn = isLoggedIn;
  }
}
