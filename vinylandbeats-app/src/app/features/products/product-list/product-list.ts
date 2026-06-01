import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../core/services/product';
import { Product } from '../../../shared/models/product';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList implements OnInit {
  private productService = inject(ProductService);

  products = signal<Product[]>([]);
  query = signal('');
  loading = signal(true);
  error = signal<string | null>(null);

  // Lista filtrată — se recalculează automat când se schimbă products() sau query()
  filtered = computed(() => {
    const q = this.query().toLowerCase().trim();
    if (!q) return this.products();
    return this.products().filter(p =>
      p.name.toLowerCase().includes(q) ||
      p.categoryName.toLowerCase().includes(q) ||
      p.tags.some(t => t.name.toLowerCase().includes(q))
    );
  });

  ngOnInit(): void {
    this.productService.getAll().subscribe({
      next: products => { this.products.set(products); this.loading.set(false); },
      error: () => { this.error.set('Nu s-au putut încărca produsele'); this.loading.set(false); }
    });
  }
}