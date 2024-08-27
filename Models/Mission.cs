using Humanizer;
using Microsoft.AspNetCore.Components.Routing;
using MossadAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int id { get; set; }
        public Agant? agent {  get; set; } = new Agant();

        public Target? Target { get; set; } = new Target();
        
        public double? timeLeft {  get; set; }

        public MissionStatusEnum? status {  get; set; }

        public List<Agant>? _agants { get; set; } = new List<Agant>();

        public string? token {  get; set; }
 
    }
}
