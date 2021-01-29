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
    public class CommentReply
    {
        public int ID { get; set; }
        [Required, MaxLength(1000)]
        public string Text { get; set; }
        [Required]
        public int CommentID { get; set; }
        [Required]
        public string WriterID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }


        public virtual PostComment AssignedComment { get; set; }
        public virtual ICollection<Like> Likes { get; set; }


        public class HSMJsonConverter : JsonConverter<CommentReply>
        {
            public override CommentReply ReadJson(JsonReader reader, Type objectType, CommentReply existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var reply = new CommentReply();
                serializer.Populate(reader, reply);

                return reply;
            }

            public override void WriteJson(JsonWriter writer, CommentReply value, JsonSerializer serializer)
            {
                var mappedReply = new
                {
                    value.ID,
                    value.Text,
                    Comment = value.CommentID,
                    Writer = value.WriterID,
                    LikeCount = value.Likes.Count,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedReply);
            }
        }
    }
}
