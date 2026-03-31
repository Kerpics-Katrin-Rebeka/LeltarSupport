import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-recruit-component',
  imports: [],
  templateUrl: './recruit-component.html',
  styleUrl: './recruit-component.css',
})
export class RecruitComponent {

  constructor(@Inject(MatDialogRef) private dialog:MatDialogRef<RecruitComponent>, @Inject(MAT_DIALOG_DATA) public data:{message:string}){}

  cancel(){
    this.dialog.close();
  }
}
