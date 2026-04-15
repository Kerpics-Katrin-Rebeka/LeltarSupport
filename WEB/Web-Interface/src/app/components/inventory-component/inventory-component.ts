import { ChangeDetectorRef, Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import { StorageComponent } from '../storage-component/storage-component';
import IngredientModel, { ResponseModel, UnderLimitResponseModel } from '../../Models/IngredientModel';
import { SalesComponent } from "../sales-component/sales-component";
import { SalesLogComponent } from "../sales-log-component/sales-log-component";
import { DataService } from '../../Services/data-service';
import { MovementModel, OrderModel, RecommendationItemModel, RestockModel } from '../../Models/SalesModel';
import { MovementComponent } from '../movement-component/movement-component';
import { MovementLogComponent } from '../movement-log-component/movement-log-component';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { PlaceRestockOrderComponent } from '../place-restock-order-component/place-restock-order-component';
import { interval, timeout, timer } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PopUpComponent } from '../pop-up-component/pop-up-component';
import { StaffComponent } from '../staff-component/staff-component';

@Component({
  selector: 'app-inventory-component',
  imports: [StorageComponent, SalesComponent, SalesLogComponent, MovementComponent, MovementLogComponent],
  templateUrl: './inventory-component.html',
  styleUrl: './inventory-component.css',
})
export class InventoryComponent {
  @Output() outOfIngredient = new EventEmitter;
  isOutOfIngredient:boolean=false;
  goal:number=100;
  isInStorage:boolean=false;
  isViewingLog: boolean = sessionStorage.getItem("isViewingLog") == "true";
  isViewingMLog:boolean=false;
  ingredients:IngredientModel[] = [];
  recommendations:ResponseModel[] = [];
  recentMovements:MovementModel[] = [];
  roles:string[] = [];

  constructor(private http: HttpClient,private dataService: DataService,private cdr: ChangeDetectorRef, @Inject(MatDialog) private dialog:MatDialog){}

  ngOnInit(){
    this.dataService.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients.data;
    });
    this.getRecommendations();
    this.getMovements();
    
    timer(500).subscribe(()=>{
      this.roles = sessionStorage.getItem("userRoles")?.split(";")||[];
      this.cdr.detectChanges();
    });

    sessionStorage.setItem("isViewingLog","false");

    interval(10000).subscribe(()=>{
      this.dataService.getIngredients().subscribe(ingredients => {
        this.ingredients = ingredients.data;
        
      });
      this.getRecommendations();
      this.getMovements();
      console.log(this.recommendations);
    });
    sessionStorage.removeItem("userRoles");
  }

  getRecommendations(){
    this.dataService.getRecommendations().subscribe({      
      next: (restocks: UnderLimitResponseModel)=>{
        this.recommendations = (restocks?.data ?? [])
        this.cdr.detectChanges();
      },
      error: (err)=>{
        console.log(err);
      }
    });
  }

  getMovements(){
    this.dataService.getStockMovements().subscribe({
      next: (movements)=>{
        this.recentMovements = movements.slice(0,5);
        this.cdr.detectChanges();
      },
      error: (err)=>{
        console.log(err);
      }
    })
  }

  openMovementLog(){
    this.isViewingMLog=true;    
  }

  openOrderPopUp(){
    console.log(this.roles);
    
    if (this.roles.length!=0 && this.roles.includes("manager")){
      this.dialog.open(PlaceRestockOrderComponent,{
      width: '400px',
      height: '300px',
      disableClose: true,
      });
    }
    else{
      this.dialog.open(PopUpComponent, {
        width: '250px',
        height: '150px',
        data: {message: "You don't have permission to place orders!"},
      });
    }
  }

  openStaffManager(){
    this.dialog.open(StaffComponent,{
      width: '90%',
      height: '80%',
    });
  }

  logout(){      
    sessionStorage.setItem("loggedIn","false")
    this.dataService.LogOut().subscribe();
    location.reload();
  }

  outOfLog(){
    sessionStorage.setItem("isViewingLog","false")
    this.isViewingLog=!this.isViewingLog;
  }

  openLog(isIt:boolean){
    sessionStorage.setItem("isViewingLog",`${isIt}`)
    this.isViewingLog = isIt;
  }

  placeOrder(data:ResponseModel){
    console.log(data);
    
    if (this.roles.length!=0 && this.roles.includes("manager")){
      //this.dataService.updatePurchaseOrder(restock.data.id,"ordered").subscribe();
      this.getRecommendations();
    }
    else{
      this.dialog.open(PopUpComponent, {
        width: '250px',
        height: '150px',
        data: {message: "You don't have permission to place orders!"},
      });
    }
  }
}
