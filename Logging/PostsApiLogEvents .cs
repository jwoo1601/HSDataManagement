using HyosungManagement.Controllers.Apis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Logging
{
    public class PostsApiLogEvents
    {
        public static readonly int BaseId = 10;

        public static readonly EventId GetAllPosts
            = CreateEventId(LogEventType.Success, 0, nameof(GetAllPosts));
        public static readonly EventId GetAllPostCategories
            = CreateEventId(LogEventType.Success, 1, nameof(GetAllPostCategories));
        public static readonly EventId GetAllPostComments
            = CreateEventId(LogEventType.Success, 2, nameof(GetAllPostComments));
        public static readonly EventId GetAllCommentReplies
            = CreateEventId(LogEventType.Success, 3, nameof(GetAllCommentReplies));

        public static readonly EventId GetPostByID
            = CreateEventId(LogEventType.Success, 4, nameof(GetPostByID));
        public static readonly EventId GetPostCategoryByID
            = CreateEventId(LogEventType.Success, 5, nameof(GetPostCategoryByID));
        public static readonly EventId GetPostCommentByID
            = CreateEventId(LogEventType.Success, 6, nameof(GetPostCommentByID));
        public static readonly EventId GetCommentReplyByID
            = CreateEventId(LogEventType.Success, 7, nameof(GetCommentReplyByID));

        public static readonly EventId AddPost
            = CreateEventId(LogEventType.Success, 8, nameof(AddPost));
        public static readonly EventId AddPostCategory
            = CreateEventId(LogEventType.Success, 9, nameof(AddPostCategory));
        public static readonly EventId AddPostComment
            = CreateEventId(LogEventType.Success, 10, nameof(AddPostComment));
        public static readonly EventId AddCommentReply
            = CreateEventId(LogEventType.Success, 11, nameof(AddCommentReply));

        public static readonly EventId EditPost
            = CreateEventId(LogEventType.Success, 12, nameof(EditPost));
        public static readonly EventId EditPostCategory
            = CreateEventId(LogEventType.Success, 13, nameof(EditPostCategory));
        public static readonly EventId EditPostComment
            = CreateEventId(LogEventType.Success, 14, nameof(EditPostComment));
        public static readonly EventId EditCommentReply
            = CreateEventId(LogEventType.Success, 15, nameof(EditCommentReply));

        public static readonly EventId DeletePost
            = CreateEventId(LogEventType.Success, 16, nameof(DeletePost));
        public static readonly EventId DeletePostCategory
            = CreateEventId(LogEventType.Success, 17, nameof(DeletePostCategory));
        public static readonly EventId DeletePostComment
            = CreateEventId(LogEventType.Success, 18, nameof(DeletePostComment));
        public static readonly EventId DeleteCommentReply
            = CreateEventId(LogEventType.Success, 19, nameof(DeleteCommentReply));


        public static readonly EventId PostNotFound
            = CreateEventId(LogEventType.ClientError, 0, nameof(PostNotFound));
        public static readonly EventId PostCategoryNotFound
            = CreateEventId(LogEventType.ClientError, 1, nameof(PostCategoryNotFound));
        public static readonly EventId PostCommentNotFound
            = CreateEventId(LogEventType.ClientError, 2, nameof(PostCommentNotFound));
        public static readonly EventId CommentReplyNotFound
            = CreateEventId(LogEventType.ClientError, 3, nameof(CommentReplyNotFound));


        public static readonly EventId AddPostError
            = CreateEventId(LogEventType.ServerError, 0, nameof(AddPostError));
        public static readonly EventId AddPostCategoryError
            = CreateEventId(LogEventType.ServerError, 1, nameof(AddPostCategoryError));
        public static readonly EventId AddPostCommentError
            = CreateEventId(LogEventType.ServerError, 2, nameof(AddPostCommentError));
        public static readonly EventId AddCommentReplyError
            = CreateEventId(LogEventType.ServerError, 3, nameof(AddCommentReplyError));

        public static readonly EventId EditPostError
            = CreateEventId(LogEventType.ServerError, 4, nameof(EditPostError));
        public static readonly EventId EditPostCategoryError
            = CreateEventId(LogEventType.ServerError, 5, nameof(EditPostCategoryError));
        public static readonly EventId EditPostCommentError
            = CreateEventId(LogEventType.ServerError, 6, nameof(EditPostCommentError));
        public static readonly EventId EditCommentReplyError
            = CreateEventId(LogEventType.ServerError, 7, nameof(EditCommentReplyError));

        public static readonly EventId DeletePostError
            = CreateEventId(LogEventType.ServerError, 8, nameof(DeletePostError));
        public static readonly EventId DeletePostCategoryError
            = CreateEventId(LogEventType.ServerError, 9, nameof(DeletePostCategoryError));
        public static readonly EventId DeletePostCommentError
            = CreateEventId(LogEventType.ServerError, 10, nameof(DeletePostCommentError));
        public static readonly EventId DeleteCommentReplyError
            = CreateEventId(LogEventType.ServerError, 11, nameof(DeleteCommentReplyError));


        static EventId CreateEventId(LogEventType eventType, int id, string name)
        {
            return LogEvents.CreateEventId(eventType, BaseId, id, name);
        }
    }
}
