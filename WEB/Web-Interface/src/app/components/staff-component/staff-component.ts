import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import UserModel from '../../Models/UserModel';
import { StaffService } from '../../Services/staff-service';
import { MatDialog } from '@angular/material/dialog';
import { RecruitComponent } from '../recruit-component/recruit-component';

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
}
