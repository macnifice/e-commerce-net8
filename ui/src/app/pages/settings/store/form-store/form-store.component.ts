import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Store } from '../../../../interfaces/store/store.interface';
import { MatDividerModule } from '@angular/material/divider';
import { StoreService } from '../../../../services/store/store.service';

@Component({
  selector: 'app-form-store',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './form-store.component.html',
  styleUrl: './form-store.component.css'
})
export class FormStoreComponent implements OnInit {
  storeForm!: FormGroup;
  isEditing = false;
  sotoreId: number=0;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<FormStoreComponent>,
    private storeService: StoreService,
    @Inject(MAT_DIALOG_DATA) private data: Store
  ) { }

  ngOnInit(): void {
    this.isEditing = this.data.isEditing || false;
    this.createForm();
  }

  createForm(): void {
    this.storeForm = this.fb.group({
      name: [this.data?.name || '', Validators.required],
      address: [this.data?.address || '', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.storeForm.valid) {
      const storeData: Store = {
        name: this.storeForm.value.name,
        address: this.storeForm.value.address
      };
      if (this.isEditing) {
        this.storeService.updateStore(this.data.id!, storeData).subscribe(() => {
          this.dialogRef.close(storeData);
        });
      } else {
        this.storeService.createStore(storeData).subscribe(() => {
          this.dialogRef.close(storeData);
        });
      }
    }
  }
}
