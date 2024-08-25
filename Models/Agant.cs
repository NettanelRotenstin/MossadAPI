using Microsoft.AspNetCore.Components.Routing;
using MossadAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Agant
    {
        [Key]
        public int id {  get; set; }

        public string? nickname { get; set; }

        public string? photo_url { get; set; }

        //class of x y
        public position? location { get; set; }

        //enum for status
        public AgantStatusEnum? status { get; set; } = AgantStatusEnum.dormantAgent;

        public int? counterKilled { get; set; }
    }
}
