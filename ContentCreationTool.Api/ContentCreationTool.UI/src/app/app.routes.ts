import { Routes } from '@angular/router';
import { TextUploadComponent } from './components/text-upload/text-upload.component';
import { ContentItemsListComponent } from './components/content-items-list/content-items-list.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { TextDocumentListComponent } from './components/text-document-list/text-document-list.component';
import { TextDocumentDetailsComponent } from './components/text-document-details/text-document-details.component';
import { ContentItemDetailsComponent } from './components/content-item-details/content-item-details.component';

export const routes: Routes = [
    { path: '', redirectTo: 'homepage', pathMatch: 'full' },
    { path: 'homepage', component: HomepageComponent }, // Placeholder for homepage component
    { path: 'text-upload', component: TextUploadComponent },
    { path: 'content-items', component: ContentItemsListComponent }, // Placeholder for content items list
    { path: 'text-documents', component: TextDocumentListComponent},
    { path: 'text-document/:id', component: TextDocumentDetailsComponent }, // Placeholder for text document details
    { path: 'content-item/:id', component: ContentItemDetailsComponent}, // Placeholder for content item details

];
