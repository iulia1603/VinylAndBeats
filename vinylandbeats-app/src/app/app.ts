import { Component, signal } from '@angular/core';
import { ReviewPage } from './features/reviews/review-page/review-page';

@Component({
  selector: 'app-root',
  imports: [ReviewPage],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('vinylandbeats-app');
}