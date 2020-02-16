using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using DancingGoat.Repositories;
using Microsoft.AspNetCore.Mvc;
using Kentico.AspNetCore.LocalizedRouting.Attributes;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Cafes")]
    [LocalizedRoute("es-ES", "Cafés")]
    public class CafesController : ControllerBase
    {
        public ICafesRepository CafesRepository { get; }

        public CafesController(ICafesRepository cafesRepository) : base()
        {
            CafesRepository = cafesRepository;
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public async Task<ActionResult> Index()
        {
            var cafes = await CafesRepository.GetCafes(Language);

            var viewModel = new CafesViewModel
            {
                CompanyCafes = cafes.Where(c => c.Country == "USA").ToList(),
                PartnerCafes = cafes.Where(c => c.Country != "USA").ToList()
            };

            return View(viewModel);
        }
    }
}