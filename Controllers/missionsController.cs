using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAPI.DAL;
using MossadAPI.Enums;
using MossadAPI.Helper;
using MossadAPI.Models;

namespace MossadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {
        static DBContextMossadAPI _dbContext;

        public missionsController(DBContextMossadAPI dbService)
        {
            _dbContext = dbService;
        }



        //get all missions by simulation
        [HttpGet]
        public async Task<IActionResult> GetAllMissions()
        {
            var Missions = await _dbContext.missions.ToArrayAsync();
            if (Missions != null)
            {
                return Ok(Missions);
            }
            return Ok("there isn't Missions");
        }


        //update mission status mvc
        [HttpPut("/{id}")]
        public async Task<IActionResult> SetPozision(int Id,MissionStatusEnum missionStatusEnum)
        {
            Mission tmp = await _dbContext.missions.FirstOrDefaultAsync(x => x.id == Id);

            if (tmp == null)
            {
                return StatusCode(404, new { massage = "mission not found" });
            }
            if(!CalculateConnectMission.CalculateDistanceBool(tmp.agent.location,tmp.Target.location))
                {
                _dbContext.missions.Remove(tmp);
                return StatusCode(StatusCodes.Status404NotFound, new { massage = "agant too far from target" });

                }
            tmp.status = missionStatusEnum;
            tmp.timeLeft = CalculateConnectMission.CalculateDistance(tmp.agent.location, tmp.Target.location);

            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status202Accepted, new { massage = "status updated" });

        }

        //update 

        [HttpPut("/update")]
        public async Task<IActionResult> Update()
        {
            var missions = await _dbContext.missions.ToListAsync();

            if (missions == null)
            {
                return StatusCode(404, new { massage = "mission not found" });
            }
            for (int i = 0; i < missions.Count; i++)
            {
                if (missions[i].status != MissionStatusEnum.assigned)
                {
                    continue;
                    CalculateMissionTime.CalculateforConnect(missions[i]);
                }
               
            }

 
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status202Accepted, new { massage = "status updated" });

        }

        //get all missions  
        [HttpGet("/count")]
        public async Task<IActionResult> GetAllMissionsCount()
        {
            var missions = await _dbContext.missions.Include(a => a.status).ToArrayAsync();

            return Ok(missions.Length);
        }


        //get all active missions 
        [HttpGet("/Activecount")]
        public async Task<IActionResult> GetActiveMissionsCount()
        {
            int i = 0;
            var misssions = await _dbContext.missions.Include(a => a.status).ToArrayAsync();
            while (misssions[i] != null)
            {
                if (misssions[i].status == MissionStatusEnum.assigned)
                    i++;
            }
            return Ok(i);
        }


        //get  mission by agant id 
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetailsMissionId(int id)
        {
            string missionDetails = "";

            var mission = await _dbContext.missions.Include(a => a.Target).Include(a => a.status).Include(a => a.agent).Include(a => a.timeLeft).FirstOrDefaultAsync(x => x.agent.id == id);

            if (mission != null)
            {

                missionDetails += "agant: " + mission.agent.nickname + ", target: " + mission.Target.name  + ", time left: " + mission.timeLeft.ToString() + ", status:" + mission.status + "/n";
                

                return Ok(missionDetails);
            }
            else
            {
                return Ok("mission not found");
            }



            
        }
    }
}
