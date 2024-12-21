using Botanic_Project.Web.Data;
using Botanic_Project.Web.Models;
using Botanic_Project.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Botanic_Project.Web.Controllers
{
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PlantsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPlantViewModel viewModel)
        {
            var plant = new Plant
            {
                Name = viewModel.Name,
                Family = viewModel.Family,
                Description = viewModel.Description,
                WithFlower = viewModel.WithFlower,
            };
            await dbContext.Plant.AddAsync(plant);
            await dbContext.SaveChangesAsync();
            return View();
        }
    }
}
