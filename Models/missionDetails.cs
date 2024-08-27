using MossadAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class missionDetails
    {
        [Key]
        public int? id { get; set; }
        public Agant? agent { get; set; }

        public Target? Target { get; set; }

        public double? timeLeft { get; set; }

        public MissionStatusEnum? status { get; set; }

        public List<Agant>? _agants { get; set; }

        public missionDetails() { }

        public missionDetails(int? id,Agant? agent, Target? target, double? timeLeft, MissionStatusEnum? status, List<Agant>? agants)
        {
            this.id = id;
            this.agent = agent;
            Target = target;
            this.timeLeft = timeLeft;
            this.status = status;
            _agants = agants;
        }
    }
}
