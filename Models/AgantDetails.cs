using MossadAPI.Enums;

namespace MossadAPI.Models
{
    public class AgantDetails
    {
        public string nickname { get; set; } = "";

        public position position { get; set; } = new position();

        public AgantStatusEnum status { get; set; } = AgantStatusEnum.dormantAgent;

        public string LinkToMission { get; set; } = "";
        public AgantDetails() { }
        public AgantDetails(string NickName,position Position,AgantStatusEnum agantStatusEnum) 
        {
            nickname = NickName;
            position = Position;
            status = agantStatusEnum;
        }
    }
}
