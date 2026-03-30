import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-pop-up-component',
  imports: [],
  templateUrl: './pop-up-component.html',
  styleUrl: './pop-up-component.css',
})
export class PopUpComponent {
  
  constructor(@Inject(MatDialogRef) private dialog:MatDialogRef<PopUpComponent>, @Inject(MAT_DIALOG_DATA) public data:{message:string}){}

  close(){
    this.dialog.close();
  }
}
