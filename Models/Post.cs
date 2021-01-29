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
    public class Post
    {
        public int ID { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        public string Content { get; set; }
        public int? CategoryID { get; set; }
        [Required]
        public string WriterID { get; set; }
        public long ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }


        public virtual PostCategory Category { get; set; }
        public virtual ICollection<PostComment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }


        public class HSMJsonConverter : JsonConverter<Post>
        {
            public override Post ReadJson(JsonReader reader, Type objectType, Post existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var post = new Post();
                serializer.Populate(reader, post);

                return post;
            }

            public override void WriteJson(JsonWriter writer, Post value, JsonSerializer serializer)
            {
                var mappedPost = new
                {
                    value.ID,
                    value.Title,
                    value.Content,
                    value.Category,
                    Writer = value.WriterID,
                    value.ViewCount,
                    LikeCount = value.Likes.Count,
                    value.Comments,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedPost);
            }
        }
    }
}
