﻿using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class position
    {
        [Key]
        public int Id { get; set; }

        [Range(0,1000)]
        public int x {  get; set; }
        
        [Range(0,1000)]
        public int y { get; set; }
    }
}
