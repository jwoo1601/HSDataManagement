using HyosungManagement.Models.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    [JsonConverter(typeof(HSMJsonConverter))]
    public class PostComment
    {
        public int ID { get; set; }
        [Required, MaxLength(1000)]
        public string Text { get; set; }
        [Required]
        public int PostID { get; set; }
        [Required]
        public string WriterID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }


        public virtual Post AssignedPost { get; set; }
        public virtual ICollection<CommentReply> Replies { get; set; }
        public virtual ICollection<Like> Likes { get; set; }


        public class HSMJsonConverter : JsonConverter<PostComment>
        {
            public override PostComment ReadJson(JsonReader reader, Type objectType, PostComment existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var comment = new PostComment();
                serializer.Populate(reader, comment);

                return comment;
            }

            public override void WriteJson(JsonWriter writer, PostComment value, JsonSerializer serializer)
            {
                var mappedComment = new
                {
                    value.ID,
                    value.Text,
                    Post = value.PostID,
                    Writer = value.WriterID,
                    value.Replies,
                    LikeCount = value.Likes.Count,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedComment);
            }
        }
    }
}
