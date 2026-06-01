import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../core/services/product';
import { Product } from '../../../shared/models/product';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-product-detail',
  imports: [CommonModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css'
})
export class ProductDetail implements OnInit {
  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private currentUserId: string | null = null;
  private isAdmin = false;

  product = signal<Product | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(u => {
      this.currentUserId = u?.id ?? null;
      this.isAdmin = u?.roles.includes('Admin') ?? false;
    });
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getById(id).subscribe({
      next: product => {
        this.product.set(product);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Produsul nu a fost găsit');
        this.loading.set(false);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/products']);
  }

  canModify(): boolean {
    const p = this.product();
    if (!p) return false;
    return this.isAdmin || p.sellerId === this.currentUserId;
  }

  edit(): void {
    this.router.navigate(['/products', this.product()!.id, 'edit']);
  }

  remove(): void {
    if (!confirm('Sigur ștergi produsul?')) return;
    this.productService.delete(this.product()!.id).subscribe({
      next: () => this.router.navigate(['/products']),
      error: () => this.error.set('Eroare la ștergere')
    });
  }
}