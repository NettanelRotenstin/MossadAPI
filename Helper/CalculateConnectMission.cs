using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MossadAPI.DAL;
using MossadAPI.Models;
using MossadAPI.Enums;

namespace MossadAPI.Helper
{
    public class CalculateConnectMission
    {
        private   DBContextMossadAPI _dBContext;

        public CalculateConnectMission(DBContextMossadAPI dbContext)
        {
            _dBContext = dbContext;
        }


        public async Task CheckMission()
        {
            List<Agant> agants = await _dBContext.agants.Include(a => a.location).ToListAsync();
            List<Target> targets = await _dBContext.targets.ToListAsync();
            List<Agant> _relevantAgants = new List<Agant>();
            Agant relevantAgant = new Agant();
            Target relevantTarget = new Target();
       
            double minFar = 201;
             
            foreach(Agant agant in agants) 
            {
                if (agant.status == AgantStatusEnum.activeAgant)
                {
                    continue;
                }
                foreach(Target target in targets)
                {
                    if (target.status == TargetStatusEnum.killed)
                    {

                        continue; 
                    }
                    if (await CalculateDistanceBool(agant.location, target.location))
                    {
                        
                            _relevantAgants.Add(agant);
                            double result = await CalculateDistance(agant.location, target.location);
                         
                            relevantTarget = target;
                            relevantAgant = agant;
                            minFar = result;
                            Mission mission = new Mission();
                           
                            mission.agent = relevantAgant;
                            mission.Target = relevantTarget;
                            mission.status = MissionStatusEnum.offer;
                            mission._agants.Add(relevantAgant);
                           
                            mission.timeLeft =  await CalculateDistance(mission.Target.location,mission.agent.location);
                            await _dBContext.missions.AddAsync(mission);
                            await _dBContext.SaveChangesAsync();
                        
                    }
                }
            }
            
             
        }




        public static async Task<bool> CalculateDistanceBool(position location_1, position location_2)
        {
            double result = await CalculateDistance(location_1,location_2);
            if (result <= 200)
            {
                return true;
            }
            return false;
        }

        
   
    
        public static async Task<double> CalculateDistance(position location_1, position location_2)
        {
            double result = Math.Sqrt(Math.Pow(location_1.x - location_2.x, 2)
               + Math.Pow(location_1.y - location_2.y, 2));
            return result/5;
        }
    
    
    }
    }
