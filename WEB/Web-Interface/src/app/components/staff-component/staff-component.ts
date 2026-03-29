import { Component, EventEmitter, Output } from '@angular/core';
import UserModel from '../../Models/UserModel';

@Component({
  selector: 'app-staff-component',
  imports: [],
  templateUrl: './staff-component.html',
  styleUrl: './staff-component.css',
})
export class StaffComponent {
  employees: UserModel[] = [];
}
