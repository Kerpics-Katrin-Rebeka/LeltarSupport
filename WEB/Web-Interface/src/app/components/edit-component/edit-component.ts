import { Component, Inject } from '@angular/core';
import { StaffService } from '../../Services/staff-service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { Role } from '../../Models/UserModel';
import { TitleCasePipe } from '@angular/common';



@Component({
  selector: 'app-edit-component',
  imports: [FormsModule,TitleCasePipe],
  templateUrl: './edit-component.html',
  styleUrl: './edit-component.css',
})
export class EditComponent {

  email:string = "";
  name:string = ""
  pwd:string = "";
  role:string = "";
  roles:Role[] = [];

  constructor(private staffService: StaffService,@Inject(MatDialogRef) private dialog:MatDialogRef<EditComponent>, @Inject(MAT_DIALOG_DATA) public data:any){}

  ngOnInit(){
    this.staffService.getRoles().subscribe({
      next: (roles)=>{
        this.roles = roles;
        console.log(roles);
        
      },
      error: (err)=>{
        console.log(err);
        this.roles = [];
      }
    });

    this.email = this.data.email;
    this.name = this.data.name;
    this.role = this.data.role;
    
  }

  cancel(){
    this.dialog.close();
  }

  EditEmployee(){
    console.log(this.email, this.name, this.pwd, this.role);
    var role = this.role.split(';'); 
    this.staffService.EditEmployee({id: this.data.id, token: this.data.token, email: this.email, name: this.name, pwd: this.pwd, roles: [{name:role[1], id:Number(role[0])}] }).subscribe();
    this.dialog.close();
  }
}

