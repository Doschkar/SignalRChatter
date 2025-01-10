import { Routes } from '@angular/router';
import { OverviewComponent } from './components/overview/overview.component';

export const routes: Routes = [
  //{ path: '', loadComponent: () => import('./test-pages/test-general/test-general.component').then(x => x.TestGeneralComponent) },
  { path: '', redirectTo: 'overview', pathMatch: 'full'},
  { path: 'overview', component: OverviewComponent }
];
