import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar-component',
  imports: [],
  templateUrl: './sidebar-component.html',
  styleUrl: './sidebar-component.css',
})
export class SidebarComponent {
    @Output() logOut = new EventEmitter;
    @Output() navigated=new EventEmitter;
    currentPage: string = 'menu'

  logout(){
    sessionStorage.setItem("loggedIn","false"),
    this.logOut.emit(false)
  }

  navigateTo(chosenPage:string){
    console.log(chosenPage);
    this.navigated.emit(chosenPage)
  }
}

