import { Component } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { TextDocumentService } from '../../services/text-document.service';
import { NgFor, NgIf } from '@angular/common';
import { ContentItemService } from '../../services/content-item.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-document-details',
  standalone: true,
  imports: [RouterLink, NgIf, FormsModule, NgFor],
  templateUrl: './text-document-details.component.html',
  styleUrl: './text-document-details.component.css'
})
export class TextDocumentDetailsComponent {
  document: any; // Exemplu de document text
  showAskLlm: boolean = false; // Variabilă pentru a controla vizibilitatea secțiunii de întrebări
  llmQuestion: string = ''; // Variabilă pentru a stoca întrebarea utilizatorului
  llmAnswer: string | null = null; // Variabilă pentru a stoca răspunsul de la LLM
  modelTypeUsed: string = 'llama2'; // Tipul modelului LLM utilizat
  llmModels = ['llama3.2', 'llama3.1', 'gemma3', 'deepseek-r1', 'phi-4-mini', 'mistral']; // Lista modelelor LLM disponibile
  isLoading: boolean = false; // Variabilă pentru a controla starea de încărcare);
  

  constructor(
    private contentItemService: ContentItemService, // Asigură-te că ai importat și injectat serviciul corespunzător
    private textDocumentService: TextDocumentService, // Asigură-te că ai importat și injectat serviciul corespunzător
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadDocument(); // Apelează metoda pentru a încărca documentul text la inițializare
  }

  loadDocument() {
    const documentId = this.getDocumentIdFromRoute(); // Obține ID-ul documentului din ruta curentă
    if (!documentId) {
      console.error('Document ID not found in route'); // Gestionează cazul în care ID-ul nu este găsit
      return;
    }
    this.textDocumentService.getTextDocumentById(documentId).subscribe(
      (data: any) => {
        this.document = data; // Aici poți prelucra datele primite de la serviciu
      },
      (error) => {
        console.error('Error loading text document:', error); // Gestionează erorile corespunzător
      }
    );
  }

  getDocumentIdFromRoute(): string | null {
    return this.route.snapshot.paramMap.get('id');
  }

  deleteDocument() {
    if (this.document && this.document.id) {
      this.textDocumentService.deleteTextDocument(this.document.id).subscribe(
        () => {
          console.log('Document deleted successfully'); // Afișează mesaj de succes
        },
        (error) => {
          console.error('Error deleting document:', error); // Gestionează erorile corespunzător
        }
      );
    } else {
      console.error('Document ID not found for deletion'); // Gestionează cazul în care ID-ul nu este găsit
    }
  }

  toggleAskLlm() {
    this.showAskLlm = !this.showAskLlm;
    this.llmAnswer = null;
    this.llmQuestion = '';
  }

  askLlm() {
    if (this.llmQuestion.trim() === '') {
      console.error('Question cannot be empty'); // Gestionează cazul în care întrebarea este goală
      return;
    }
    this.isLoading = true; // Setează indicatorul de încărcare la true
    this.contentItemService.askLLM(this.document.id, this.llmQuestion, this.parseLLMModel(this.modelTypeUsed)).subscribe(
      (response: any) => {
        this.llmAnswer = response.response; // Aici poți prelucra răspunsul primit de la LLM
        this.isLoading = false; // Setează indicatorul de încărcare la false
      },
      (error) => {
        console.error('Error asking LLM:', error); // Gestionează erorile corespunzător
      }
    );
  }

  parseLLMModel(model: string): number {
    switch (model) {
      case 'llama3.2':
        return 0;
      case 'llama3.1':
        return 1;
      case 'gemma3':
        return 2;
      case 'deepseek-r1':
        return 3;
      case 'phi-4-mini':
        return 4;
      case 'mistral':
        return 5;
      default:
        return 0; // Default value
    }
  }
}
