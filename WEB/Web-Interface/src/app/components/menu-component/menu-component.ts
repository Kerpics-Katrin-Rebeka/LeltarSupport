import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { InventoryComponent } from '../inventory-component/inventory-component';
import { StaffComponent } from '../staff-component/staff-component';
import { SalesComponent } from '../sales-component/sales-component';
import { SidebarComponent } from '../sidebar-component/sidebar-component';
import IngredientModel from '../../Models/IngredientModel';

@Component({
  selector: 'app-menu-component',
  imports: [InventoryComponent, SalesComponent, StaffComponent, SidebarComponent],
  templateUrl: './menu-component.html',
  styleUrl: './menu-component.css',
})
export class MenuComponent {
  @Output() isLoggedIn=new EventEmitter;
  currentPage: string = 'menu';
  isOutOfIngredients:boolean = false;
  ingredients:IngredientModel[]=[];

  ngOnInit(){
    sessionStorage.setItem("loggedIn","true")

    this.ingredients = [
    {id:0,name:'CHEESE',unit:"slice",maxAmount:100,amount:110},
    {id:0,name:'CHEESE(grated)',unit:"g",maxAmount:1000,amount:900},
    {id:0,name:'BUNS',unit:"piece(s)",maxAmount:105,amount:100},
    {id:0,name:'PATTY(made)',unit:"piece(s)",maxAmount:100,amount:0},
    {id:0,name:'SAUCE',unit:"ml",maxAmount:10000,amount:10000},
    ]
    this.checkForEmpty();
  }

  navigateTo(chosenPage: string) {
    console.log(chosenPage);
    this.isLoggedIn.emit(true)
    this.currentPage = chosenPage;
  }

  logout(){
    sessionStorage.setItem("loggedIn","false"),
    this.isLoggedIn.emit(false)
  }

    checkForEmpty(){
    this.ingredients.forEach(ing => {
      if (ing.amount == 0) {
        this.isOutOfIngredients = true
        return;
      }
    });
    return;
  }
}
