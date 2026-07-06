import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReviewService } from '../../../core/services/review';
import { ProductService } from '../../../core/services/product';
import { Review } from '../../../shared/models/review';

@Component({
  selector: 'app-review-page',
  imports: [CommonModule],
  templateUrl: './review-page.html',
  styleUrl: './review-page.css'
})
export class ReviewPage implements OnInit {
  private reviewService = inject(ReviewService);
  private productService = inject(ProductService);

  reviews = signal<Review[]>([]);
  productName = signal<string>('');
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const params = new URLSearchParams(window.location.search);
    const pid = Number(params.get('productId'));

    if (!pid) {
      this.error.set('Niciun produs selectat.');
      this.loading.set(false);
      return;
    }

    this.productService.getAll().subscribe({
      next: products => {
        const p = products.find(x => x.id === pid);
        this.productName.set(p ? p.name : `Produsul #${pid}`);
      },
      error: () => {}
    });

    this.reviewService.getForProduct(pid).subscribe({
      next: reviews => {
        this.reviews.set(reviews);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Nu s-au putut încărca recenziile');
        this.loading.set(false);
      }
    });
  }
}