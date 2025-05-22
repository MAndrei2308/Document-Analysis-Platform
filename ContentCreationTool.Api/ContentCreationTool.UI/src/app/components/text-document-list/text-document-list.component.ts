import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TextDocumentService } from '../../services/text-document.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-text-document-list',
  standalone: true,
  imports: [NgIf, NgFor, FormsModule, RouterLink],
  templateUrl: './text-document-list.component.html',
  styleUrl: './text-document-list.component.css'
})
export class TextDocumentListComponent {
  textDocuments: any[] = []; // Exemplu de listă de documente text

  /**
   *
   */
  constructor(
    private textDocumentService: TextDocumentService // Asigură-te că ai importat și injectat serviciul corespunzător 
  ) {}

  ngOnInit() {
    this.loadTextDocuments(); // Apelează metoda pentru a încărca documentele text la inițializare
  }

  loadTextDocuments() {
    this.textDocumentService.getTextDocuments().subscribe(
      (data: any[]) => {
        this.textDocuments = data; // Aici poți prelucra datele primite de la serviciu
      },
      (error) => {
        console.error('Error loading text documents:', error); // Gestionează erorile corespunzător
      }
    );
  }

  getPreview(text: string, length: number = 150): string {
    return text.length > length ? text.slice(0, length) + '...' : text;
  }
}
