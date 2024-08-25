using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MossadAPI.DAL;
using MossadAPI.Models;
using MossadAPI.Enums;

namespace MossadAPI.Helper
{
    public class CalculateConnectMission
    {
        private static DBContextMossadAPI _dBContext;

        public CalculateConnectMission(DBContextMossadAPI dbContext)
        {
            _dBContext = dbContext;
        }


        public  async Task CheckMission()
        {
            List<Agant> agants = await _dBContext.agants.ToListAsync();
            List<Target> targets = await _dBContext.targets.ToListAsync();
            List<Agant> _relevantAgants = new List<Agant>();
            Agant relevantAgant = new Agant();
            Target relevantTarget = new Target();
            bool flag = false;
            double minFar = 201;
            double result = 0;
            for (int i = 0; i < agants.Count; i++)
            {
                if (agants[i].status == AgantStatusEnum.activeAgant)
                {
                    continue;
                }
                for (int j = 0; j < targets.Count; j++)
                {
                    if (targets[j].status == TargetStatusEnum.killed)
                    {

                        continue; 
                    }
                    if (CalculateDistanceBool(agants[i].location, targets[j].location))
                    {
                        flag = true;
                        _relevantAgants.Add(agants[i]);
                        result = CalculateDistance(agants[i].location, targets[j].location);
                        if (result < minFar)
                        {
                            relevantTarget = targets[j];
                            relevantAgant = agants[i];
                            minFar = result;
                        }
                    }
                }
            }
            if (flag)
            {
                Mission mission = new Mission();
                mission.agent = relevantAgant;
                mission.Target = relevantTarget;
                mission.status = MissionStatusEnum.offer;
                mission._agants.Add(relevantAgant);
                mission._agants = _relevantAgants;
            }
             
        }




        public static bool CalculateDistanceBool(position location_1, position location_2)
        {
            double result = CalculateDistance(location_1,location_2);
            if (result <= 200)
            {
                return true;
            }
            return false;
        }

        
   
    
        public static double CalculateDistance(position location_1, position location_2)
        {
            double result = Math.Sqrt(Math.Pow(location_1.x - location_2.x, 2)
               + Math.Pow(location_1.y - location_2.y, 2));
            return result/5;
        }
    
    
    }
    }
