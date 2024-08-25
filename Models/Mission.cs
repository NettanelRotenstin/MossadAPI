using Humanizer;
using Microsoft.AspNetCore.Components.Routing;
using MossadAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int? id { get; set; }
        public Agant? agent {  get; set; }

        public Target? Target { get; set; }
        
        public double? timeLeft {  get; set; }

        public MissionStatusEnum? status {  get; set; }

        public List<Agant>? _agants { get; set; }

        public string? token {  get; set; }
 
    }
}
