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
    public class targetsController : ControllerBase, IControllers
    {
        static DBContextMossadAPI _dbContext;

        public targetsController(DBContextMossadAPI dbContext)
        {
            _dbContext = dbContext;
        }
        CalculateConnectMission calculateConnectMission = new CalculateConnectMission(_dbContext);






        //create target by simulation
        [HttpPost]
        public async Task<IActionResult> Create(Target target)
        {
            target.location = new position();

            await _dbContext.targets.AddAsync(target);
            await _dbContext.SaveChangesAsync();

            //called to static func that check the missions 

            calculateConnectMission.CheckMission();

            return StatusCode(StatusCodes.Status201Created, new { newTarget = target });
        }

        //get all targets by simulation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Target = await _dbContext.targets.Include(t => t.location).ToArrayAsync();
            if (Target != null)
            {
                return Ok(Target);
            }
            return Ok("there isn't agants");
        }

        //start pozision by simulation
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> SetPozision(int id, position location)
        {
            Target tmp = await _dbContext.targets.Include(t => t.location).FirstOrDefaultAsync(x => x.id == id);

            if (tmp == null)
            {
                return StatusCode(404, new { massage = "target not found" });
            }

            tmp.location = location;

            _dbContext.SaveChanges();

            //called to static func that check the missions 

            calculateConnectMission.CheckMission();


            return StatusCode(StatusCodes.Status202Accepted, new { massage = "location sets" });

        }

        //move pozision by simulation
        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, DiractionEnum diraction)
        {
            Target tmp = await _dbContext.targets.Include(t => t.location).FirstOrDefaultAsync(x => x.id == id);

            if (tmp == null)
            {
                return StatusCode(404, new { massage = "target not found" });
            }

            tmp.location = CalculateDiraction.CalculateSteps(diraction, tmp.location);

            _dbContext.SaveChanges();

            //called to static func that check the missions 

            calculateConnectMission.CheckMission();


            return StatusCode(StatusCodes.Status202Accepted, new { massage = "location update" });
        }

        //get count targets
        [HttpGet("count")]
        public async Task<IActionResult> GetAllTargetsCount()
        {
            var targets = await _dbContext.targets.ToArrayAsync();

            return Ok(targets.Length + 1);
        }


        //get  count of targets killed
        [HttpGet("KilledCount")]
        public async Task<IActionResult> GetKilledTargets()
        {
            int i = 0;
            var targets = await _dbContext.targets.ToArrayAsync();
           foreach (var target in targets) 
            {
            
                if (target.status == TargetStatusEnum.killed)
                    i++;
            }
            return Ok(i);
        }


        //return all targets details
        [HttpGet("allDetails")]
        public async Task<IActionResult> GetAllTargetsDetails()
        {
            List<string> detailsAlltargets = new List<string>();

            string detailOneTarget = "";

            int i = 0;

            var targets = await _dbContext.targets.Include(a => a.position).Include(a => a.location).Include(a => a.status).ToArrayAsync();

            while (targets[i] != null)
            {
                detailOneTarget += "position: " + targets[i].position.ToString() + ", location:" + targets[i].location.ToString() + ", status:" + targets[i].status.ToString() + "/n";
                detailsAlltargets.Add(detailOneTarget);
            }

            return Ok(detailsAlltargets);
        }




        [HttpGet("{id}/allDetails")]
        public async Task<IActionResult> GetTargetDetails(int id)
        {

            string detailOneTarget = "";

            int i = 0;

            Target tmp = await _dbContext.targets.Include(a => a.position).Include(a => a.location).Include(a => a.status).FirstOrDefaultAsync(x => x.id == id);


            if (tmp != null)
            {

                detailOneTarget += "position: " + tmp.position.ToString() + ", location:" + tmp.location.ToString() + ", status:" + tmp.status.ToString();


                return Ok(detailOneTarget);
            }
            else
            {
                return Ok("target not found");
            }

        }

    }
}
