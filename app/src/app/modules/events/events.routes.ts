import { Routes } from '@angular/router';
import { EventsComponent } from './event-list/event-list.component';
import { EventComponent } from './event/event.component';

export const routes: Routes = [
  { path: '', component: EventsComponent }, // Shows all events
  { path: ':id', component: EventComponent }, // Shows a specific event by ID
];
