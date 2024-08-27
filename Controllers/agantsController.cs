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
        CalculateConnectMission _calculateConnectMission;


        public agantsController(DBContextMossadAPI dbContext, CalculateConnectMission calculateConnectMission)
        {
            _dbContext = dbContext;
            _calculateConnectMission = calculateConnectMission;
        }

        

        //create agant by simulation
        [HttpPost]
        public async Task<IActionResult> CreateAgant(Agant agant)
        {
            
            agant.status = AgantStatusEnum.dormantAgent;

            await _dbContext.agants.AddAsync(agant);
            await _dbContext.SaveChangesAsync();

            //called to static func that check the missions 
             await _calculateConnectMission.CheckMission();
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

              _calculateConnectMission.CheckMission();


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

                _calculateConnectMission.CheckMission();


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
            List<AgantDetails> detailsAllAgants = new List<AgantDetails>();

 
 
            var Agants = await _dbContext.agants
                .Include(a => a.location)
                .ToArrayAsync();

            

            foreach(var agant in Agants)
            {
                AgantDetails agantToAdd = new AgantDetails(agant.nickname, agant.location, agant.status);

                Mission mission = await _dbContext.missions.FirstOrDefaultAsync(a => a.agent.id == agant.id);
                
                if (mission != null)
                {
                    agantToAdd.LinkToMission = $"http://localhost:5157/api/missions/{mission.id}/details";
                }

                detailsAllAgants.Add(agantToAdd);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(detailsAllAgants);
        }

        //return one agant details
        [HttpGet("{id}/allDetails")]
        public async Task<IActionResult> GetAgantDetails(int id)
        {

            AgantDetails agantDetails = new AgantDetails();

            

            Agant tmp = await _dbContext.agants.Include(a => a.location).FirstOrDefaultAsync(x => x.id == id);


            if (tmp != null)
            {

                agantDetails.nickname = tmp.nickname;
                agantDetails.position = tmp.location;
                agantDetails.status = tmp.status;
                
                
                if (tmp.status == AgantStatusEnum.activeAgant)
                {
                    Mission mission = await _dbContext.missions.Include(a => a.agent).FirstOrDefaultAsync(x => x.agent.id == tmp.id);
                    agantDetails.LinkToMission = $"http://localhost:5157/api/missions/{tmp.id}/details";
                }

                return Ok(agantDetails);
            }
            else
            {
                return Ok("agant not found");
            }

        }
    }
}
