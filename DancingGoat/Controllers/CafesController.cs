using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using DancingGoat.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class CafesController : ControllerBase
    {
        public ICafesRepository CafesRepository { get; }

        public CafesController(ICafesRepository cafesRepository) : base()
        {
            CafesRepository = cafesRepository;
        }
        
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