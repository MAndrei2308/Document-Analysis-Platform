import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ContentItemService } from '../../services/content-item.service';

@Component({
  selector: 'app-content-item-details',
  standalone: true,
  imports: [NgIf, RouterLink],
  templateUrl: './content-item-details.component.html',
  styleUrl: './content-item-details.component.css'
})
export class ContentItemDetailsComponent {
  contentItem: any; // Exemplu de conținut

  constructor(
    private contentItemService: ContentItemService, // Asigură-te că ai importat și injectat serviciul corespunzător
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadContentItem(); // Apelează metoda pentru a încărca conținutul la inițializare
  }

  loadContentItem() {
    const contentItemId = this.getContentItemIdFromRoute(); // Obține ID-ul conținutului din ruta curentă
    if (!contentItemId) {
      console.error('Content Item ID not found in route'); // Gestionează cazul în care ID-ul nu este găsit
      return;
    }
    this.contentItemService.getContentItemById(contentItemId).subscribe(
      (data: any) => {
        this.contentItem = data; // Aici poți prelucra datele primite de la serviciu
      },
      (error) => {
        console.error('Error loading content item:', error); // Gestionează erorile corespunzător
      }
    );
  }

  getContentItemIdFromRoute(): string | null {
    return this.route.snapshot.paramMap.get('id'); // Obține ID-ul din parametrii rutei
  }

  deleteContentItem() {
    if (this.contentItem && this.contentItem.id) {
      this.contentItemService.deleteContentItem(this.contentItem.id).subscribe(
        () => {
          console.log('Content Item deleted successfully'); // Afișează mesaj de succes
        },
        (error) => {
          console.error('Error deleting content item:', error); // Gestionează erorile corespunzător
        }
      );
    } else {
      console.error('Content Item ID not found for deletion'); // Gestionează cazul în care ID-ul nu este găsit
    }
  }

  // ...existing code...
getModelTypeName(modelType?: number): string {
  switch (modelType) {
    case 0: return 'llama3.2';
    case 1: return 'llama3.1';
    case 2: return 'gemma3';
    case 3: return 'deepseek-r1';
    case 4: return 'phi-4-mini';
    default: return 'Unknown Model';
  }
}
// ...existing code...

}
