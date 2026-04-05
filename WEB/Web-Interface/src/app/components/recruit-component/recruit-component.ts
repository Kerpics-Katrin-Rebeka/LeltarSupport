import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { StaffService } from '../../Services/staff-service';
import { Role } from '../../Models/UserModel';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-recruit-component',
  imports: [FormsModule,TitleCasePipe],
  templateUrl: './recruit-component.html',
  styleUrl: './recruit-component.css',
})
export class RecruitComponent {
  email:string = "";
  name:string = ""
  pwd:string = "";
  role:string = "staff";
  roles:Role[] = [];
  ErrorMsg:string = "";

  constructor(private staffService: StaffService,@Inject(MatDialogRef) private dialog:MatDialogRef<RecruitComponent>){}

  ngOnInit(){
    this.staffService.getRoles().subscribe({
      next: (roles)=>{
        this.roles = roles;     
      },
      error: (err)=>{
        console.log(err);
        this.roles = [];
      }
    });
  }

  cancel(){
    this.dialog.close();
  }

  addEmployee(){
    this.staffService.Recruit({
      name: this.name.trim(),
      email: this.email.trim().toLowerCase(),
      password: this.pwd,
      role: this.role.split(';')[1]?.trim().toLowerCase()
    }).subscribe({
      next: (res) => {
        this.dialog.close();
      },
      error: (err) => {
        this.ErrorMsg = err.error.errors.name || err.error.errors.email || err.error.errors.password || "An error occurred while adding the employee.";        
      }
    });
  }
}
