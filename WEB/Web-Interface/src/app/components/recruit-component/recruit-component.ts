import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { StaffService } from '../../Services/staff-service';

@Component({
  selector: 'app-recruit-component',
  imports: [FormsModule],
  templateUrl: './recruit-component.html',
  styleUrl: './recruit-component.css',
})
export class RecruitComponent {
  email:string = "";
  name:string = ""
  pwd:string = "";
  role:string = "staff";

  constructor(private staffService: StaffService,@Inject(MatDialogRef) private dialog:MatDialogRef<RecruitComponent>, @Inject(MAT_DIALOG_DATA) public data:{message:string}){}

  cancel(){
    this.dialog.close();
  }

  addEmployee(){
    console.log(this.email, this.name, this.pwd, this.role);
    
      this.staffService.Recruit({
        name: this.name,
        email: this.email,
        password: this.pwd,
        role: this.role
      }).subscribe(res=>{console.log(res);});
      this.dialog.close();
  }
}
