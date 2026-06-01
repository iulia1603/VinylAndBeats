import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../core/services/product';
import { CategoryService } from '../../../core/services/category';
import { TagService } from '../../../core/services/tag';
import { Category, Tag } from '../../../shared/models/product';

@Component({
  selector: 'app-product-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.css'
})
export class ProductForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private tagService = inject(TagService);

  form!: FormGroup;
  categories = signal<Category[]>([]);
  allTags = signal<Tag[]>([]);
  isEditMode = signal(false);
  productId = signal<number | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.minLength(5)]],
      price: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      imageUrl: [''],
      categoryId: [null, Validators.required],
      tagIds: [[] as number[]]
    });

    this.categoryService.getAll().subscribe({ next: c => this.categories.set(c), error: () => {} });
    this.tagService.getAll().subscribe({ next: t => this.allTags.set(t), error: () => {} });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode.set(true);
      this.productId.set(Number(idParam));
      this.productService.getById(Number(idParam)).subscribe({
        next: p => this.form.patchValue({
          name: p.name,
          description: p.description,
          price: p.price,
          stock: p.stock,
          imageUrl: p.imageUrl ?? '',
          categoryId: p.categoryId,
          tagIds: p.tags.map(t => t.id)
        }),
        error: () => this.error.set('Produsul nu a fost găsit')
      });
    }
  }

  onTagToggle(tagId: number, checked: boolean): void {
    const control = this.form.get('tagIds');
    if (!control) return;
    const current: number[] = control.value ?? [];
    if (checked && !current.includes(tagId)) {
      control.setValue([...current, tagId]);
    } else if (!checked) {
      control.setValue(current.filter(id => id !== tagId));
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading.set(true);
    this.error.set(null);

    if (this.isEditMode()) {
      this.productService.update(this.productId()!, this.form.value).subscribe({
        next: () => this.router.navigate(['/products', this.productId()!]),
        error: () => { this.loading.set(false); this.error.set('Eroare la salvare'); }
      });
    } else {
      this.productService.create(this.form.value).subscribe({
        next: p => this.router.navigate(['/products', p.id]),
        error: () => { this.loading.set(false); this.error.set('Eroare la salvare'); }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }
}