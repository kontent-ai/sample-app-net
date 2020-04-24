using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using DancingGoat.Repositories;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Contacts")]
    [LocalizedRoute("es-ES", "Contacto")]
    public class ContactsController : ControllerBase
    {
        public ICafesRepository CafesRepository { get; }

        public ContactsController(ICafesRepository cafesRepository) : base()
        {
            CafesRepository = cafesRepository;
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
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