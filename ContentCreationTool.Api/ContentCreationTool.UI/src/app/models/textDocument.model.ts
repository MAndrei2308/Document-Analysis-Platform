export interface TextDocument {
    id: number; // ID-ul documentului text
    fileName: string; // Titlul documentului
    extractedText: string; // Conținutul documentului
    contentItemId: number; // ID-ul conținutului asociat
}