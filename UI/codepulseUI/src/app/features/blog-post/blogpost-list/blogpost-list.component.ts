import { Component, OnInit } from '@angular/core';
import { BlogPostService } from '../services/blog-post.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css']
})
export class BlogpostListComponent implements OnInit {
  constructor(private blogPostService: BlogPostService) {

  }
  blogPost$?: Observable<BlogPost[]>;
  ngOnInit(): void {
    //get all blog post from API
    this.blogPost$ =this.blogPostService.getAllBlogPost();
  }

}
