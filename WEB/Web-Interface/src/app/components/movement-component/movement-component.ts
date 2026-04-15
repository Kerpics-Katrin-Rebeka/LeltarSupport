import { Component, Input } from '@angular/core';
import { MovementModel } from '../../Models/SalesModel';

@Component({
  selector: 'app-movement-component',
  imports: [],
  templateUrl: './movement-component.html',
  styleUrl: './movement-component.css',
})
export class MovementComponent {
  @Input() movement:MovementModel|undefined;
}
