using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    [Owned]
    public class Like
    {
        public string LikedBy { get; set; }
        public DateTime LikedAt { get; set; }
        public int OwnerID { get; set; }
    }
}
