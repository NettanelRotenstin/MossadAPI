using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.DAL;
using MossadAPI.Models;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MossadAPI.Helper;
using MossadAPI.Enums;

namespace MossadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]



    public class agantsController : ControllerBase/*, IControllers*/
    {
        static DBContextMossadAPI _dbContext;

        public agantsController(DBContextMossadAPI dbContext)
        {
            _dbContext = dbContext;
        }
        CalculateConnectMission calculateConnectMission = new CalculateConnectMission(_dbContext);

        

        //create agant by simulation
        [HttpPost]
        public async Task<IActionResult> CreateAgant(Agant agant)
        {
            
            agant.status = AgantStatusEnum.dormantAgent;

            await _dbContext.agants.AddAsync(agant);
            await _dbContext.SaveChangesAsync();

            //called to static func that check the missions 
                calculateConnectMission.CheckMission();
             return StatusCode(StatusCodes.Status201Created, new { newAgant = agant });
        }

        //get all agants by simulation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Agants = await _dbContext.agants.Include(a => a.location).ToArrayAsync();
            if (Agants != null)
            {
                return Ok(Agants);
            }
            return Ok("there isn't agants");
        }

        //start pozision by simulation
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> SetPozision(int id,[FromBody] position location)
        {
            Agant tmp = await _dbContext.agants.Include(a => a.location).FirstOrDefaultAsync(x => x.id == id);
           
            if (tmp == null)
            {
                return StatusCode(404, new { massage = "agnt not found" });
            }
            tmp.location = location;

            _dbContext.SaveChanges();

            //called to static func that check the missions 

            calculateConnectMission.CheckMission();


            return StatusCode(StatusCodes.Status202Accepted, new { massage = "location sets"});

        }


        //move pozision by simulation
        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, DiractionEnum diraction)
        {
            Agant tmp = await _dbContext.agants.Include(a => a.location).FirstOrDefaultAsync(x => x.id == id);

            if (tmp == null)
            {
                return StatusCode(404, new { massage = "agant not found" });
            }
             
                tmp.location = CalculateDiraction.CalculateSteps(diraction, tmp.location);
            

            _dbContext.SaveChanges();

            //called to static func that check the missions 

              calculateConnectMission.CheckMission();


            return StatusCode(StatusCodes.Status202Accepted, new { massage = "location update" });

        }

        //get all agants count
        [HttpGet("count")]
        public async Task<IActionResult> GetAllAgantCount()
        {
            var Agants = await _dbContext.agants.ToArrayAsync();

               return Ok(Agants.Length + 1);
        }


        //get count active agants
        [HttpGet("Activecount")]
        public async Task<IActionResult> GetActiveAgantCount()
        {
            int i = 0;
            var Agants = await _dbContext.agants.ToArrayAsync();
            foreach (Agant agant in Agants)
            {
                if (agant.status == AgantStatusEnum.activeAgant)
                {
                    i++;
                }
            } 
            return Ok(i);
        }


        //get relative agants targets
        [HttpGet("relativeCount")]
        public async Task<IActionResult> GetRelativeAgantTargets()
        {
            var agants = await _dbContext.agants.ToArrayAsync();

            var targets = await _dbContext.targets.ToArrayAsync();


            return Ok($"{agants.Length + 1} : {targets.Length + 1}");
        }


        //get  relative agant in roles to active missions
        [HttpGet("relativeAgantRoleCount")]
        public async Task<IActionResult> GetRelativeAgantRole()
        {
            int relevantAgants = 0;

            int relevantMissions = 0;


            var Missions = await _dbContext.missions.ToArrayAsync();

            int i = 0;

            foreach(var mission in Missions)
            { 

                if (mission.status == MissionStatusEnum.offer || mission.status == MissionStatusEnum.assigned)
                {
                    relevantMissions++;
                    relevantAgants += mission._agants.Count + 1;
                }
                i++;
            }


            return Ok($"{relevantAgants} : {relevantMissions}");
        }

        //return all agants details
        [HttpGet("allDetails")]
        public async Task<IActionResult> GetAllAgantDetails()
        {
            List<string> detailsAllAgants = new List<string>();

            string detailOneAgants = "";

            int i = 0;

            var Agants = await _dbContext.agants.Include(a => a.nickname).Include(a => a.location).Include(a => a.status).ToArrayAsync();

            while (Agants[i] != null)
            {
                detailOneAgants += "nick name: " + Agants[i].nickname.ToString() + ", location: " + Agants[i].location.ToString() + ", status: " + Agants[i].status.ToString() + "/n";
                detailsAllAgants.Add(detailOneAgants);
            }

            return Ok(detailsAllAgants);
        }

        //return one agant details
        [HttpGet("{id}/allDetails")]
        public async Task<IActionResult> GetAgantDetails(int id)
        {

            string detailOneAgants = "";

            int i = 0;

            Agant tmp = await _dbContext.agants.Include(a => a.nickname).Include(a => a.location).Include(a => a.status).Include(a => a.counterKilled).FirstOrDefaultAsync(x => x.id == id);


            if (tmp != null)
            {

                detailOneAgants += "nick name: " + tmp.nickname.ToString() + ", location: " + tmp.location.ToString() + ", status: " + tmp.status.ToString() + ", killed:" + tmp.counterKilled + "/n";
                if (tmp.status == AgantStatusEnum.activeAgant)
                {
                    Mission mission = await _dbContext.missions.Include(a => a.timeLeft).Include(a => a.agent).Include(a => a.id).FirstOrDefaultAsync(x => x.agent.id == tmp.id);
                    detailOneAgants += $" time left: {mission.timeLeft}    , link to mission: http://localhost:5227/api/missions/{mission.id}/details ";
                }

                return Ok(detailOneAgants);
            }
            else
            {
                return Ok("agant not found");
            }

        }
    }
}
