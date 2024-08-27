using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Routing;
using MossadAPI.Enums;

namespace MossadAPI.Models
{
    public class Target
    {
        [Key]
         public int id { get; set; }

        public string? name { get; set; } = "";

        public string? position { get; set; } = "";

        public string? photoUrl { get; set; } = "";

        public position location { get; set; } = new position();

        public TargetStatusEnum status { get; set; } = TargetStatusEnum.alive;
    }
}
