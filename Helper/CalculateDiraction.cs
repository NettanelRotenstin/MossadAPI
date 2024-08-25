using MossadAPI.Enums;
using MossadAPI.Models;

namespace MossadAPI.Helper
{
    public static class CalculateDiraction
    {
        public static position CalculateSteps(DiractionEnum diraction, position location)
        {
           
                if (diraction == DiractionEnum.e)
                {
                    location.x += 1;
                }
                if (diraction == DiractionEnum.n)
                {
                    location.y += 1;
                }
                if (diraction == DiractionEnum.w)
                {
                    location.x -= 1;
                }
                if (diraction == DiractionEnum.s)
                {
                    location.y -= 1;
                }
                if (diraction == DiractionEnum.ne || diraction == DiractionEnum.en)
                {
                    location.x += 1;
                    location.y += 1;
                }
                if (diraction == DiractionEnum.nw || diraction == DiractionEnum.wn)
                {
                    location.x -= 1;
                    location.y += 1;
                }
                if (diraction == DiractionEnum.sw || diraction == DiractionEnum.ws)
                {
                    location.x -= 1;
                    location.y -= 1;
                }
                if (diraction == DiractionEnum.se || diraction == DiractionEnum.es)
                {
                    location.x += 1;
                    location.y -= 1;
                }
                return location;
            }
         }
    
}
