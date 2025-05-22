import { Component } from '@angular/core';
import { ContentItemService } from '../../services/content-item.service';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-content-items-list',
  standalone: true,
  imports: [NgIf, NgFor,FormsModule, RouterLink],
  templateUrl: './content-items-list.component.html',
  styleUrl: './content-items-list.component.css'
})
export class ContentItemsListComponent {

  contentItems: any[] = []; // Exemplu de listă de articole de conținut

  constructor(
    private contentItemService: ContentItemService
  ) {}

  ngOnInit() {
    this.loadContentItems();
  }

  loadContentItems() {
    this.contentItemService.getContentItems().subscribe(
      (data: any[]) => {
        this.contentItems = data;
      },
      (error) => {
        console.error('Error loading content items:', error);
      }
    );
  }

  getPreview(text: string, length: number = 150): string {
    return text.length > length ? text.slice(0, length) + '...' : text;
  }
}
