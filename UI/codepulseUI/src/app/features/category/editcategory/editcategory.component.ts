import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-editcategory',
  templateUrl: './editcategory.component.html',
  styleUrls: ['./editcategory.component.css']
})
export class EditcategoryComponent implements OnInit, OnDestroy {
  id: string | null = null;
  paramsSubcription?: Subscription;
  editCategorySubcription?:Subscription;
  deleteCategorySubcription?:Subscription;
  category?: Category;
  constructor(private route: ActivatedRoute, private categoryService: CategoryService,private router:Router) {

  }


  ngOnInit(): void {
    this.paramsSubcription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        if (this.id) {
          //get the data from the API for this category ID
          this.categoryService.getCategoryById(this.id)
            .subscribe({
              next: (response) => {
                this.category = response;
              }
            });
        }
      }
    })
  }
  onDelete():void{
    if(this.id){
      this.deleteCategorySubcription = this.categoryService.deleteCategory(this.id)
      .subscribe({
        next:reponse=>{
          this.router.navigateByUrl("/admin/categories");
        }
      });

    }
  }

  onFormSubmit() :void{
    const updateCategoryRequest : UpdateCategoryRequest = {
      name:this.category?.name ?? '',
      urlHandle: this.category?.urlHandle?? ''
    }
    //pass this object to service
    if(this.id){
      this.editCategorySubcription = this.categoryService.UpdateCategory(this.id,updateCategoryRequest)
      .subscribe({
        next:reponse=>{
          this.router.navigateByUrl('/admin/categories')
        }
      })

    }
  } 
  ngOnDestroy(): void {
    this.paramsSubcription?.unsubscribe();
    this.editCategorySubcription?.unsubscribe();
    this.deleteCategorySubcription?.unsubscribe();

  }
}
