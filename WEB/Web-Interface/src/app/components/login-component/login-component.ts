import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import UserModel, { response } from '../../Models/UserModel';
import { DataService } from '../../Services/data-service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { PopUpComponent } from '../pop-up-component/pop-up-component';

@Component({
  selector: 'app-login-component',
  imports: [FormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  @Output() loginAttempted = new EventEmitter;
  private logger:response|undefined;
  users:UserModel[]=[];
  pwd:string = '';
  email:string = '';

  constructor(private dataService: DataService, @Inject(MatDialog) private dialog:MatDialog){};

  loginAttempt(){    
    this.dataService.Login(this.email, this.pwd).subscribe({
      next: (resp) =>{
        console.log(resp, resp.token);
        this.logger = resp;
        if (this.logger != undefined) {
          sessionStorage.setItem("loggedIn","true");
          sessionStorage.setItem("token", this.logger.token);
          sessionStorage.setItem("userEmail", this.email);
          this.loginAttempted.emit()
        }    
      },
      error:(err)=>{
        this.dialog.open(PopUpComponent, {
          width: '250px',
          height: '150px',
          data: {message: "Incorrect credentials!"}
        });
      }
    });
    console.log(this.logger);
  }
}
