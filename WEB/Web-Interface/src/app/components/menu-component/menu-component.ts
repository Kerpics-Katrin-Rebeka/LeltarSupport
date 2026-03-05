import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { InventoryComponent } from '../inventory-component/inventory-component';
import { StaffComponent } from '../staff-component/staff-component';
import { SalesComponent } from '../sales-component/sales-component';
import { SidebarComponent } from "../sidebar-component/sidebar-component";

@Component({
  selector: 'app-menu-component',
  imports: [InventoryComponent, SalesComponent, StaffComponent, SidebarComponent],
  templateUrl: './menu-component.html',
  styleUrl: './menu-component.css',
})
export class MenuComponent {
  @Output() isLoggedIn=new EventEmitter;
  currentPage: string = 'menu';

  ngOnInit(){
    sessionStorage.setItem("loggedIn","true")
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
}
