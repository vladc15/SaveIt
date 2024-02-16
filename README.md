# SaveIt

## Description ##

Social Bookmarking website that allows users to do CRUD operations on posts. Posts can be organized in Boards. Each post can contain text content, uploaded pictures/embedded videos and multiple tags.
It features a login system, having 3 types of users:
* Unregistered: can see all the posts on the website
* Registered User: can create new posts and can leave comments, as well as edit their own content. They can also create their own boards with selected posts.
* Admin: can edit all content on the website.

Registered users can like other posts, in order to constantly recommend the most liked posts. They also have access to a search bar, in addition to pagination for posts, for an easier navigation through the platform.



## Technologies ## 

* ASP.NET Core, C#
* Razor
* Microsoft SQL Server
* Bootstrap
* jQuery


## Database Diagram ##
```mermaid
erDiagram
    Roles ||--o{ UserRoles : is_attributed
    Roles {
        int Id PK
        string Name
    }
    
    Users ||--|{ UserRoles : own
    Users ||--o{ Boards : have
    Users ||--o{ Likes : give
    Users {
        int Id PK
        string UserName
        string UserEmail
    }
    Likes {
        int Id  PK
        string UserId FK
        int PinId FK
    }
    Users ||--o{ Pins : create
    
    UserRoles {
        int Id
        int UserId PK
        int RoleId PK
    }
    Boards ||--|{ BoardsPins : have
    Boards ||--o{ Comments : have
    Pins ||--o{ Comments : have
    Users ||--o{Comments : commented

    Comments {
        int Id PK
        string Content
        date Date
        int PinId FK
        int BoardId FK
        int UserId FK
    }
    

    Boards {
        int Id PK
        string Name
        int UserId FK
    }
    
    
    Pins ||--o{ PinsTags : own
    Pins ||--o{ BoardsPins : appear_in
    Pins ||--o{ Likes : have
    Pins {
        int Id PK
        string Title
        string Content
        string mediaPath
        string mediaType
        string UserId FK
        date Date
    }
    
    
    PinsTags {
        int Id PK
        int PinId PK, FK
        int TagId PK
    }
    BoardsPins {
        int Id PK
        int BoardId PK
        int PinId PK
    }

    Tags ||--o{ PinsTags : is_attributed
    Tags {
        int Id PK
        string Name
    }
```


