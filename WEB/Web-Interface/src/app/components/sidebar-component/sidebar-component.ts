import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar-component',
  imports: [],
  templateUrl: './sidebar-component.html',
  styleUrl: './sidebar-component.css',
})
export class SidebarComponent {
    @Output() backToMenu = new EventEmitter;
    @Output() navigated=new EventEmitter;
    currentPage: string = 'menu'
    backButton:string = "<-"

  goBack(){
    this.backToMenu.emit("menu");
  }

  navigateTo(chosenPage:string){
    console.log(chosenPage);
    this.navigated.emit(chosenPage)
  }
}

