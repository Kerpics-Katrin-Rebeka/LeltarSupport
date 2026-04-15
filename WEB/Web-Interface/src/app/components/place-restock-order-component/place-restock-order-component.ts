import { ChangeDetectorRef, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef, } from '@angular/material/dialog';
import { DataService } from '../../Services/data-service';
import { FormsModule } from '@angular/forms';
import IngredientModel from '../../Models/IngredientModel';
import { TitleCasePipe } from '@angular/common';
import { OrderModel, RecommendationItemModel, RestockModel } from '../../Models/SalesModel';
import { min, timer } from 'rxjs';
import { SalesService } from '../../Services/sales-service';



@Component({
  selector: 'app-place-restock-order-component',
  imports: [FormsModule,TitleCasePipe],
  templateUrl: './place-restock-order-component.html',
  styleUrl: './place-restock-order-component.css',
})
export class PlaceRestockOrderComponent {
  ingredients:any[] = [];
  orderedIngredients:RecommendationItemModel[] = [];
  ingredientName:string = "";
  quantity:number = 0;
  numberOfIngredients:number = 0;

  constructor(private dataService: DataService, private salesService: SalesService, private cdr: ChangeDetectorRef, @Inject(MatDialogRef) private dialog: MatDialogRef<PlaceRestockOrderComponent>,@Inject(MAT_DIALOG_DATA) private data:any) {}

  ngOnInit(){  
    this.dataService.getIngredients().subscribe({
      next: (ings)=>{      
        this.ingredients = ings.data;
        console.log(this.ingredients);
        
        this.cdr.detectChanges();
      },
      error: (err)=>{
        console.log(err);
        this.ingredients = [];
      }
    })
    timer(500).subscribe(()=>{
      this.addIngredient(this.data);
      this.cdr.detectChanges();
    })

  }

  placeRestockOrder(){
    this.salesService.placeRestockOrder(this.orderedIngredients, 1).subscribe();
    timer(500).subscribe(()=>{this.dialog.close()});
  }

  addIngredient(ing:RecommendationItemModel|undefined = undefined){
    console.log(this.numberOfIngredients);
    
    if(this.numberOfIngredients < this.ingredients.length){      
      this.numberOfIngredients++;
      this.orderedIngredients.push({
          id: this.numberOfIngredients-1,
          ingredient: {
          id: 0,
          name: "",
          minAmount: 0,
          unit: ""
        },
        quantity: 0
      });
    }
    if (ing != undefined) {
      this.orderedIngredients[0].ingredient = ing.ingredient;
      this.orderedIngredients[0].quantity = ing.quantity;      
    }
  }

  switchIngredient(item:RecommendationItemModel){
    let ingredient = this.ingredients.find(ing => ing.ingredient.name == item.ingredient.name);
    if(ingredient){
      item.ingredient = ingredient.ingredient;
    }
  }

  removeIngredient(item:RecommendationItemModel){
    if (this.numberOfIngredients == 1) {
      this.dialog.close();
    }
    this.numberOfIngredients--;
    this.orderedIngredients.splice(this.orderedIngredients.indexOf(item),1);
  }

  cancel(){
    this.dialog.close();
  }
}
