import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import UserModel from '../../Models/UserModel';
import { StaffService } from '../../Services/staff-service';
import { MatDialog } from '@angular/material/dialog';
import { RecruitComponent } from '../recruit-component/recruit-component';
import { EditComponent } from '../edit-component/edit-component';
import { PopUpComponent } from '../pop-up-component/pop-up-component';

@Component({
  selector: 'app-staff-component',
  imports: [CommonModule],
  templateUrl: './staff-component.html',
  styleUrl: './staff-component.css',
})
export class StaffComponent implements OnInit{
  constructor(private staffService: StaffService, private cdr: ChangeDetectorRef,@Inject(MatDialog) private dialog:MatDialog){}
  employees: UserModel[] = [];

  ngOnInit(){
    this.getEmployees();
  }

  getEmployees(){
    this.staffService.getEmployees().subscribe({
      next: (employees) => {
        this.employees = employees;
        this.cdr.detectChanges();
      },
      error: () => {
        this.employees = [];
        this.cdr.detectChanges();
      }
    });
  }

  openRecruitForm(){
    this.dialog.open(RecruitComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
    });
    this.dialog.afterAllClosed.subscribe(()=>{this.getEmployees()});
  }

  EditEmployee(employee:UserModel){
    var dataToSend ={
      name: employee.name,
      email: employee.email,
      role: employee.roles[0].name
    }
    
    this.dialog.open(EditComponent,{
      width: '500px',
      height: '400px',
      disableClose: true,
      data: employee
    });
    this.dialog.afterAllClosed.subscribe(()=>{this.getEmployees()});
  }

  RemoveEmployee(employee:UserModel){
    console.log(employee);

    if (employee.email == sessionStorage.getItem("userEmail")) {
      alert("You cannot remove yourself.");
      return;
    }
    
    this.dialog.open(PopUpComponent,{
      width: '300px',
      height: '200px',
      data: {message:`Are you sure you want to remove ${employee.name}?`}
    })
    .afterClosed().subscribe((confirmed: boolean) => {
      if(confirmed){
        this.staffService.RemoveEmployee(employee.id).subscribe({
          next:()=>{
            this.getEmployees();
          },
          error:(err)=>{
            const msg = err?.error?.message ?? "Failed to remove employee";
            alert(msg);
          }
        });
      }
    });
  }
}
