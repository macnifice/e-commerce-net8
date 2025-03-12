import { ChangeDetectorRef, Component, EventEmitter, inject, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormDialogService } from '../../../services/dialog/form-dialog.service';
import { StoreService } from '../../../services/store/store.service';
import { FormStoreComponent } from './form-store/form-store.component';
import { Store } from '../../../interfaces/store/store.interface';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-store',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  templateUrl: './store.component.html',
  styleUrl: './store.component.css'
})
export class StoreComponent implements OnInit {
  @Output() storeSelected = new EventEmitter<Store>();

  dataSource = new MatTableDataSource<Store>([]);
  selectedStore: Store | null = null;

  private readonly matDialogData = inject(MAT_DIALOG_DATA);

  constructor(
    private modalService: FormDialogService,
    private storeService: StoreService,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.refreshStores();
  }

  selectStore(store: Store) {
    this.selectedStore = store;
    this.storeSelected.emit(store);
  }

  refreshStores() {
    this.storeService.getStores().subscribe({
      next: (stores) => {
        this.dataSource.data = stores;
      },
      error: (error) => {
        console.error('Error al obtener las tiendas:', error);
      }
    });
  }

  openNewStoreDialog():void {
    const dialogRef = this.modalService.openModal<FormStoreComponent, Store>(FormStoreComponent);
    dialogRef.afterClosed().subscribe({
      next: (store: Store) => {
        if (store) {
          this.refreshStores();
        }
      }
    });
  }

  editStore(store: Store): void {
    const dialogRef = this.modalService.openModal<FormStoreComponent, Store>(FormStoreComponent, store, true);
    dialogRef.afterClosed().subscribe({
      next: (updatedStore: Store) => {
        if (updatedStore) {
          this.refreshStores();
        }
      }
    });
  }

  deleteStore(store: Store) {
    if (store.id) {
      this.storeService.deleteStore(store.id).subscribe({
        next: () => {
          this.refreshStores();
          this.snackBar.open('Tienda eliminada correctamente', 'Cerrar', {
            duration: 3000
          });
        },
        error: (error) => {
          console.error('Error al eliminar la tienda:', error);
        }
      });
    }
  }
}
