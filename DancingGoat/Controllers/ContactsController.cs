using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using DancingGoat.Repositories;

namespace DancingGoat.Controllers
{
    public class ContactsController : ControllerBase
    {
        public ICafesRepository CafesRepository { get; }

        public ContactsController(ICafesRepository cafesRepository) : base()
        {
            CafesRepository = cafesRepository;
        }
        
        public async Task<ActionResult> Index()
        {
            var cafes = await  CafesRepository.GetCafes(Language, "USA", null);

            var viewModel = new ContactsViewModel
            {
                Roastery = cafes.FirstOrDefault(),
                Cafes = cafes
            };

            return View(viewModel);
        }
    }
}