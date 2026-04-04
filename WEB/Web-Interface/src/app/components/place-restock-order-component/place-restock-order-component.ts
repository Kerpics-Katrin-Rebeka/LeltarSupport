import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, } from '@angular/material/dialog';
import { DataService } from '../../Services/data-service';
import { FormsModule } from '@angular/forms';
import IngredientModel from '../../Models/IngredientModel';
import { TitleCasePipe } from '@angular/common';



@Component({
  selector: 'app-place-restock-order-component',
  imports: [FormsModule,TitleCasePipe],
  templateUrl: './place-restock-order-component.html',
  styleUrl: './place-restock-order-component.css',
})
export class PlaceRestockOrderComponent {
  ingredients:any[] = [];
  ingredientName:string|string[] = "";
  quantity:number|number[] = 0;
  numberOfIngredients:number = 1;

  constructor(private dataService: DataService, @Inject(MatDialogRef) private dialog: MatDialogRef<PlaceRestockOrderComponent>) {}

  ngOnInit(){
    this.dataService.getIngredients().subscribe({
      next: (ings)=>{
        this.ingredients = ings;
        console.log(this.ingredients[0]);
      },
      error: (err)=>{
        console.log(err);
        this.ingredients = [];
      }
    })
  }

  placeRestockOrder(){}

  cancel(){
    this.dialog.close();
  }
}
