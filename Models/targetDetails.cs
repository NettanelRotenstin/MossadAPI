using MossadAPI.Enums;

namespace MossadAPI.Models
{
    public class targetDetails
    {
        public position position { get; set; } = new position();

        public TargetStatusEnum status { get; set; } = TargetStatusEnum.alive;

        public string name { get; set; } = "";

        public string photo_url { get; set; } = "";
    }
}
