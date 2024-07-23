import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})

export class EditBlogpostComponent implements OnInit, OnDestroy {

  id: string | null = "null";
  
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[]
  isImageSelectorVisible:boolean = false;


  routeSubcription?: Subscription;
  updateBlogPostSubcription?: Subscription;
  getBlogPostSubcription?:Subscription;
  deleteBlogPostSubcription?:Subscription;

  constructor(private route: ActivatedRoute,
    private blogpostService: BlogPostService,
    private categoryService: CategoryService,
    private router:Router
  ) {

  }



  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategory();
    this.routeSubcription = this.route.paramMap.subscribe({
      next: (param) => {
        this.id = param.get('id');
        //Get BlogPost from API
        if (this.id) {
          this.getBlogPostSubcription = this.blogpostService.getBlogPostById(this.id).subscribe({
            next: (reponse) => {
              this.model = reponse;
              this.selectedCategories = reponse.categories.map(x => x.id);
            }
          });

        }

      }
    })
  }

  onFormSubmit(): void {
    //Convert this model to request object
    if (this.model && this.id) {
      var UpdateBlogPost: UpdateBlogPost = {
        author:this.model.author,
        content:this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl:this.model.featuredImageUrl,
        isVisible:this.model.isVisible,
        publishedDate: this.model.publishedDate,
        title:this.model.title,
        urlHandle:this.model.urlHandle,
        categories:this.selectedCategories??[]
      }
      this.updateBlogPostSubcription = this.blogpostService.updateBlogPost(this.id,UpdateBlogPost).subscribe({
        next:(reponse)=>{
          this.router.navigateByUrl('admin/blogposts')
        }
      });
    }
  }
  onDelete():void{
    if(this.id){
      this.deleteBlogPostSubcription = this.blogpostService.deleteBlogPost(this.id).subscribe({
        next:(reponse)=>{
          this.router.navigateByUrl("/admin/blogposts");
        }
      })
    }
  }

  openImageSelector():void{
    this.isImageSelectorVisible = true;
  }
  closeImageSelector():void{
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.routeSubcription?.unsubscribe();
    this.getBlogPostSubcription?.unsubscribe();
    this.updateBlogPostSubcription?.unsubscribe();
    this.deleteBlogPostSubcription?.unsubscribe();
    
  }

}
