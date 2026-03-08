import { Component, EventEmitter, Output } from '@angular/core';
import { SalesLogComponent } from '../sales-log-component/sales-log-component';

@Component({
  selector: 'app-sales-component',
  imports: [SalesLogComponent],
  templateUrl: './sales-component.html',
  styleUrl: './sales-component.css',
})
export class SalesComponent {
  @Output() backToMenu = new EventEmitter;
  isViewingLog:boolean=false;

  ngOnInit(){
    if (sessionStorage.getItem("isViewingLog")!= undefined) {
      sessionStorage.getItem("isViewingLog")=="true"?this.isViewingLog=true:this.isViewingLog=false
    }
  }

  goBack(){
    this.backToMenu.emit("menu");
  }

  openSalesLogs(){
    sessionStorage.setItem("isViewingLog","true")
    this.isViewingLog=true;
  }
}
