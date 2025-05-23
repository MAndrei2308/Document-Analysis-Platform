import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TextDocumentService } from '../../services/text-document.service';
import { ContentItemService } from '../../services/content-item.service';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-text-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './text-upload.component.html',
  styleUrls: ['./text-upload.component.css'],
})
export class TextUploadComponent {
  title: string = ''; // Titlul documentului text
  body: string = ''; // Conținutul documentului text
  selectedLLMModel: string = '0'; // Modelul LLM selectat (default 0)
  llmModels = ['llama3.2', 'llama3.1', 'gemma3', 'deepseek-r1', 'phi-4-mini', 'mistral']; // Lista modelelor LLM disponibile
  selectedFile: File | null = null; // Fișierul selectat pentru încărcare

  llmResponse: string | null = null; // Răspunsul generat de modelul LLM

  isLoading: boolean = false; // Indicator pentru a verifica dacă se încarcă un fișier

  constructor(
    private contentItemService: ContentItemService,
    private textDocumentService: TextDocumentService
  ) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0]; // Obține fișierul selectat
  }

  async submitForm() {
  if (!this.title || !this.body) {
    alert('Please fill in all fields and select a file.'); // Verifică dacă toate câmpurile sunt completate
    return;
  }

  try {
    this.isLoading = true; // Setează indicatorul de încărcare la true

    // Creez FormData pentru a include atât datele text cât și fișierul
    const formData = new FormData();
    formData.append('title', this.title);
    formData.append('body', this.body);
    formData.append('modelTypeUsed', this.parseLLMModel(this.selectedLLMModel).toString());

    // Dacă există un fișier, îl adaug în FormData
    if (this.selectedFile) {
      formData.append('uploadedFile', this.selectedFile, this.selectedFile.name);
    }

    // Creez ContentItem
    const contentItemResponse = await this.contentItemService.createContentItem(formData).toPromise();

    if (!contentItemResponse || !contentItemResponse.contentItem) {
      throw new Error('Content item response is undefined or invalid.');
    }

    const contentItemId = contentItemResponse.contentItem.id; // Obține ID-ul conținutului creat
    console.log("FULL RESPONSE", contentItemResponse); // Afișează răspunsul complet în consolă
    this.llmResponse = contentItemResponse.responseFromOllama; // Obține răspunsul generat de modelul LLM

    console.log('Text document created successfully!'); // Afișează mesaj de succes
    console.log('LLM Response:', this.llmResponse); // Afișează răspunsul generat de modelul LLM

  } catch (error) {
    console.error('Error creating text document:', error); // Afișează eroarea în consolă
    alert('Error creating text document. Please try again.'); // Afișează mesaj de eroare
  } finally {
    this.isLoading = false; // Setează indicatorul de încărcare la false
  }
}


  resetForm() {
    this.title = ''; // Resetează titlul
    this.body = ''; // Resetează conținutul
    this.selectedFile = null; // Resetează fișierul selectat
    this.llmResponse = null; // Resetează răspunsul generat de modelul LLM
    this.selectedLLMModel = '0'; // Resetează modelul LLM selectat
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
