using Microsoft.EntityFrameworkCore;
using MossadAPI.DAL;
using MossadAPI.Enums;
using MossadAPI.Models;
namespace MossadAPI.Helper
{
    public class CalculateMissionTime
    {
 
        private static DBContextMossadAPI _dBContext;

        public CalculateMissionTime(DBContextMossadAPI dbContext)
        {
            _dBContext = dbContext;
        }
        public static async void CalculateforConnect(Mission mission)
        {
            double TimeLeft = CalculateConnectMission.CalculateDistance(mission.agent.location, mission.Target.location);
            if (TimeLeft == 0)
            {
                mission.agent.counterKilled++;
                mission.status = MissionStatusEnum.complete;
                mission.Target.status = TargetStatusEnum.killed;
                mission.agent.status = AgantStatusEnum.dormantAgent;
            }

            Mission carrent = await _dBContext.missions.FirstOrDefaultAsync(s => s.id == mission.id);
            carrent.timeLeft = TimeLeft / 5;
            await _dBContext.SaveChangesAsync();
        }





        public static async Task CalculateDirection(position agant, position target,Mission mission)
        {
            double timeDistance = CalculateConnectMission.CalculateDistance(agant, target);

            if (agant.x == target.x)
            {
                if (agant.y < target.y)
                {
                   mission.agent.location =  CalculateDiraction.CalculateSteps(DiractionEnum.s, mission.agent.location);
                       return;
                }
                if (agant.y > target.y)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.n, mission.agent.location);
                    return;
                }
             }

            if (agant.y == target.y)
            {
                if (agant.x < target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.e, mission.agent.location);
                    return;
                }
                if (agant.x > target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.w, mission.agent.location);
                    return;
                }
            }


            if (agant.y > target.y)
            {
                if (agant.x < target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.en, mission.agent.location);
                    return;
                }
                if (agant.x > target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.nw, mission.agent.location);
                    return;
                }
            }


            if (agant.y < target.y)
            {
                if (agant.x < target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.es, mission.agent.location);
                    return;
                }
                if (agant.x > target.x)
                {
                    mission.agent.location = CalculateDiraction.CalculateSteps(DiractionEnum.ws, mission.agent.location);
                    return;
                }
            }


        }
    }


    
}
