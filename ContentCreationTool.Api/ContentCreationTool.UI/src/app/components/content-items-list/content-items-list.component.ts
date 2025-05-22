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

  // Aici poți adăuga logica pentru a obține și a afișa lista de articole de conținut
  // De exemplu, poți apela un serviciu pentru a obține datele necesare
  contentItems: any[] = []; // Exemplu de listă de articole de conținut

  constructor(
    private contentItemService: ContentItemService // Asigură-te că ai importat și injectat serviciul corespunzător
  ) {}

  ngOnInit() {
    this.loadContentItems(); // Apelează metoda pentru a încărca articolele de conținut la inițializare
  }

  loadContentItems() {
    this.contentItemService.getContentItems().subscribe(
      (data: any[]) => {
        this.contentItems = data; // Aici poți prelucra datele primite de la serviciu
      },
      (error) => {
        console.error('Error loading content items:', error); // Gestionează erorile corespunzător
      }
    );
  }

  getPreview(text: string, length: number = 150): string {
    return text.length > length ? text.slice(0, length) + '...' : text;
  }
}
