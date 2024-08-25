using Microsoft.AspNetCore.Mvc;
using MossadAPI.Enums;

namespace MossadAPI.Models
{
    public interface IControllers
    {
        Task<IActionResult> Move(int id, DiractionEnum direction);
        Task<IActionResult> SetPozision(int id,position location);

        Task<IActionResult> GetAll();

    }
}
