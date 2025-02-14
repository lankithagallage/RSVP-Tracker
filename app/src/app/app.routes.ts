import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'events',
    loadChildren: () =>
      import('./features/events/events.routes').then((m) => m.routes),
  },
  { path: '**', component: NotFoundComponent },
];
