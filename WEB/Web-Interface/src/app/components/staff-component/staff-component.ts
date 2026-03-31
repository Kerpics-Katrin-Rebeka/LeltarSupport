import { Component, EventEmitter, Output } from '@angular/core';
import UserModel from '../../Models/UserModel';
import { StaffService } from '../../Services/staff-service';

@Component({
  selector: 'app-staff-component',
  imports: [],
  templateUrl: './staff-component.html',
  styleUrl: './staff-component.css',
})
export class StaffComponent {
  constructor(private staffService: StaffService){}
  employees: UserModel[] = [];

  ngOnInit(){
    this.staffService.getEmployees().subscribe({
      next:(data)=> this.employees = data,
    })
    console.log(this.employees);
    
  }
}
