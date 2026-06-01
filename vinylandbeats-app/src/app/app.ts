import { Component, signal } from '@angular/core';
import { ProductList } from './features/products/product-list/product-list';

@Component({
  selector: 'app-root',
  imports: [ProductList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('vinylandbeats-app');
}