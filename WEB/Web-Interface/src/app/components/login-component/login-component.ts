import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import UserModel, { response } from '../../Models/UserModel';
import { DataService } from '../../Services/data-service';

@Component({
  selector: 'app-login-component',
  imports: [FormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  @Output() loginAttempted = new EventEmitter;
  logger:response | undefined;
  users:UserModel[]=[];
  pwd:string = '';
  email:string = '';

  constructor(private dataService: DataService){};

  loginAttempt(role:string){
        console.log(this.email, this.pwd);
    this.dataService.Login(this.email, this.pwd).subscribe(resp=>{
      this.logger = resp;
    });
    if (this.logger != undefined) {
          sessionStorage.setItem("loggedIn","true");
          sessionStorage.setItem("userRole", this.logger.user.role);
          sessionStorage.setItem("token", this.logger.token);
          console.log(this.logger);
          this.loginAttempted.emit()
    }
    else{
      alert(`No user of ${role} role found with these credentials!`);
      this.pwd = '';
      this.email = '';
    }
  }
}
