import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-login-component',
  imports: [],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  @Output() loginStateChanged = new EventEmitter();

  loginAttempt(){
    if(this.LoginRND() === 0) this.loginStateChanged.emit(true)
    else this.loginStateChanged.emit(false)
  }

  LoginRND(){
    return Math.round(Math.random())
  }
}
