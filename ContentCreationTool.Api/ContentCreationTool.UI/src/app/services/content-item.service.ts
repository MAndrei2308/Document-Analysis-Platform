import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContentItemWithLLMResponse } from '../models/contentItem.model';

@Injectable({
  providedIn: 'root',
})
export class ContentItemService {
  private apiUrl = 'http://localhost:5268/api/ContentItem'; // URL-ul endpoint-ului pentru ContentItem

  constructor(private http: HttpClient) {}

  // Metodă pentru a crea un ContentItem
  createContentItem(data: FormData): Observable<ContentItemWithLLMResponse> {
    console.log('Creating ContentItem with data:', data); // Log pentru a verifica datele trimise
    return this.http.post<ContentItemWithLLMResponse>(this.apiUrl, data);
  }

  // Metodă pentru a obține toate ContentItems
  getContentItems(): Observable<any[]> {
    console.log('Fetching all ContentItems'); // Log pentru a verifica cererea de obținere
    return this.http.get<any[]>(this.apiUrl);
  }

  // Metodă pentru a obține un ContentItem după ID
  getContentItemById(id: string): Observable<ContentItemWithLLMResponse> {
    console.log(`Fetching ContentItem with ID: ${id}`); // Log pentru a verifica cererea de obținere
    return this.http.get<ContentItemWithLLMResponse>(`${this.apiUrl}/${id}`);
  }

  // Metodă pentru a sterge un ContentItem
  deleteContentItem(id: string): Observable<any> {
    console.log(`Deleting ContentItem with ID: ${id}`); // Log pentru a verifica cererea de ștergere
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  askLLM(textDocumentId: string, question: string, modelType: number): Observable<any> {
    console.log(`Asking LLM for TextDocument ID: ${textDocumentId} with question: ${question}; modelType: ${modelType}`); // Log pentru a verifica cererea de întrebare
    return this.http.post<any>(`${this.apiUrl}/ask`, { "TextDocumentId": textDocumentId, "Prompt": question, "ModelType": modelType });
  }
}