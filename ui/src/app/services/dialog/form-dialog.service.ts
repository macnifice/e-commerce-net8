import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class FormDialogService {

  constructor(private dialog:MatDialog) { }

  openModal<T, R>(component: ComponentType<T>, data?: R, isEditing: boolean = false): MatDialogRef<T> {
    return this.dialog.open(component, {
      width: '500px',
      data: {
        ...data,
        isEditing: isEditing
      },
      disableClose: true
    });
  }

  closeModal(): void {
    this.dialog.closeAll();
  }
}
