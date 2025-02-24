import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'events',
    loadChildren: () =>
      import('./modules/events/events.routes').then((m) => m.routes),
  },
  { path: '**', component: NotFoundComponent },
];
