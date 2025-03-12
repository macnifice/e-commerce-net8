import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { NavComponent } from '../../components/nav/nav.component';
import { StoreComponent } from './store/store.component';
import { ArticleComponent } from './article/article.component';
import { ArticleCatalogComponent } from './article-catalog/article-catalog.component';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    MatTabsModule,
    NavComponent,
    StoreComponent,
    ArticleComponent,
    ArticleCatalogComponent
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {
  selectedStore: any = null;
  activeTabIndex: number = 0;

  onStoreSelected(store: any) {
    this.selectedStore = store;
  }

  onTabChange(event: any) {
    this.activeTabIndex = event.index;
    if (this.activeTabIndex === 1) {
      this.selectedStore = null;
    }
  }
}
