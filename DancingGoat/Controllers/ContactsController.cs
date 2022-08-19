﻿using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Mvc.Routing.Localization.Attributes;
using DancingGoat.Repositories;
using DancingGoat.Configuration;

namespace DancingGoat.Controllers
{
    [LocalizedRoute(Constants.EnglishCulture, "Contacts")]
    [LocalizedRoute(Constants.SpanishCulture, "Contacto")]
    public class ContactsController : ControllerBase
    {
        public ICafesRepository CafesRepository { get; }

        public ContactsController(ICafesRepository cafesRepository) : base()
        {
            CafesRepository = cafesRepository;
        }

        [LocalizedRoute(Constants.EnglishCulture, "Index")]
        [LocalizedRoute(Constants.SpanishCulture, "Indice")]
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