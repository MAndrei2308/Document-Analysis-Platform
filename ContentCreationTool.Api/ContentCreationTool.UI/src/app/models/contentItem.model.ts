import { TextDocument } from "./textDocument.model";

export interface ContentItem {
    id: number; // ID-ul conținutului
    title: string; // Titlul conținutului
    body: string; // Descrierea conținutului
    modelTypeUsed: number; // Tipul modelului LLM asociat
    createdAt: Date; // Data creării

    textDocument?: TextDocument; // Documentul text asociat (opțional)
    }

export interface ContentItemWithLLMResponse {
  contentItem: {
    id: string;
    title: string;
    body: string;
    modelTypeUsed: number;
    createdAt: string;
    textDocument: any;
    imageDocument: any;
  };
  responseFromOllama: string;
}