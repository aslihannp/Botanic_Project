using Botanic_Project.Web.Data;
using Botanic_Project.Web.Models;
using Botanic_Project.Web.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Botanic_Project.Web.Controllers
{
    [Authorize] //kullanici giris yapmamis ise, otomatik olarak, Program.cs‘de belirtmiş olduğumuz, /Login/ sayfamıza yönlenecek
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
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var plants = await dbContext.Plant.ToListAsync();
            return View(plants);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var plant = await dbContext.Plant.FindAsync(id);
            return View(plant);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Plant viewModel)
        {
            var plant = await dbContext.Plant.FindAsync(viewModel.Id);
            if (plant is not null) 
            { 
                plant.Name = viewModel.Name;
                plant.WithFlower = viewModel.WithFlower;
                plant.Description = viewModel.Description;
                plant.Family = viewModel.Family;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Plants");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Plant viewModel)
        {
            var plant = await dbContext.Plant
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (plant is not null) 
            {
                dbContext.Plant.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Plants");
        }
    }
}
