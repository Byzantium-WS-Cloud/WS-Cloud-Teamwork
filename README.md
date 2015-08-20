          Facebook-like
          
          
          1.Users MS (Friends, ICollection<Post>, FriendRequests, Notifications,
          2.Tables :
          - Post (Id, Content, PostDate, PostedBy, Likes, Comments)
          - Comment(Id, Content, CommentDate, PostedBy, Likes, PostId)
          - Groups (id, title, Users, Posts, CreatedOn)
          - Friends ICollection 
          - Likes PostLike and CommentLike
          - Notifications(Id, NotificationType, CreatedOn, Seen?, ByUser)
          
