import { Component, OnInit } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.model';
import { Observable } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { Router } from '@angular/router';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent implements OnInit{
  model: AddBlogPost;
  categories$?:Observable<Category[]>;
  constructor(private blogPostService: BlogPostService,private router:Router,private categoryService:CategoryService) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
      categories:[]
    }
  }
  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategory();
  }
  onFormSubmit(): void {
    this.blogPostService.createBlogPost(this.model)
    .subscribe({
      next:reponse=>{
        this.router.navigateByUrl('admin/blogposts')
      }
    });
  }
}
