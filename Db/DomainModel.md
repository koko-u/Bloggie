# Domain Model

```mermaid
erDiagram
  blog_posts }|--o| tags : ""

  blog_posts {
      guid id PK
      string heading
      string page_title
      string content
      string short_description
      string featured_image_url
      string url_handle
      date published_on
      string author
      bool is_visible
  }
  
  tags {
      guid id PK
      string name
      guid blog_post_id FK
 }
```
