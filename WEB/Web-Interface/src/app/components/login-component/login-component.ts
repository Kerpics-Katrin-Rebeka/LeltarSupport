import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import UserModel from '../../Models/UserModel';

@Component({
  selector: 'app-login-component',
  imports: [FormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  @Output() loginAttempted = new EventEmitter;
  users:UserModel[]=[];
  pwd:string = '';
  email:string = '';

  loadUsers(){
    this.users = [];
    this.users.push({name: 'Teszt Coordinator',role: 'coordinator',email: 'cord@cord.cord',pwd: 'coordinatorTest'})
    this.users.push({name: 'Teszt Manager',role: 'manager',email: 'man@man.man',pwd: 'managerTest'})
    this.users.push({name: 'Teszt Gen Manager',role: 'genManager',email: 'gman@gman.gman',pwd: 'genManagerTest'})
  }

  loginAttempt(role:string){
    this.loadUsers();
    var logger = this.users.find(u => u.email === this.email && u.pwd === this.pwd && u.role === role);
    console.log(this.users);
    if (logger != undefined) {
          sessionStorage.setItem("loggedIn","true");
          this.loginAttempted.emit()
    }
    else{
      alert(`No user of ${role} role found with these credentials!`);
      this.pwd = '';
      this.email = '';
    }
  }
}
