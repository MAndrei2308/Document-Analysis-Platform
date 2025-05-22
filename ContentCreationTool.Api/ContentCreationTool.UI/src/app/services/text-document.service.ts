import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TextDocument } from '../models/textDocument.model';

@Injectable({
  providedIn: 'root',
})
export class TextDocumentService {
  private apiUrl = 'http://localhost:5268/api/TextDocument'; // URL-ul endpoint-ului pentru TextDocument

  constructor(private http: HttpClient) {}

  // Metodă pentru a crea un TextDocument
  createTextDocument(data: TextDocument): Observable<any> {
    console.log('Creating TextDocument with data:', data); // Log pentru a verifica datele trimise
    return this.http.post<any>(this.apiUrl, data);
  }

  // Metoda de upload a fișierului
  uploadTextDocument(file: File, contentItemId: number): Observable<any> {
    const formData = new FormData();
    formData.append('file', file); // Adaugă fișierul în FormData
    formData.append('contentItemId', contentItemId.toString()); // Adaugă ID-ul conținutului în FormData

    return this.http.post<any>(`${this.apiUrl}/upload`, formData); // Trimite cererea POST cu FormData
  }

  // Metodă pentru a obține toate TextDocument-urile
  getTextDocuments(): Observable<TextDocument[]> {
    return this.http.get<TextDocument[]>(this.apiUrl);
  }

  // Metodă pentru a obține un TextDocument după ID
  getTextDocumentById(id: string): Observable<TextDocument> {
    return this.http.get<TextDocument>(`${this.apiUrl}/${id}`);
  }

  // Metodă pentru a șterge un TextDocument
  deleteTextDocument(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}