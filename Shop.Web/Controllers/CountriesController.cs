﻿namespace Shop.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shop.Web.Data;
    using Shop.Web.Data.Entities;
    using Shop.Web.Models;
    using System.Threading.Tasks;

    [Authorize(Roles ="Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<IActionResult> DeleteCity(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var countryId = await this.countryRepository.DeleteCityAsync(city);
            return this.RedirectToAction($"Details/{countryId}");
        }

        public async Task<IActionResult> EditCity(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {
            if (this.ModelState.IsValid)
            {
                var countryId = await this.countryRepository.UpdateCityAsycn(city);
                if (countryId !=0)
                {
                    return this.RedirectToAction($"Details/{countryId}");
                }
            }

            return this.View(city);
        }

        public async Task<IActionResult> AddCity(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            var model = new CityViewModel { CountryId = country.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.countryRepository.AddCityAsync(model);
                return this.RedirectToAction($"Details/{model.CountryId}");
            }
            return this.View(model);
        }

        public IActionResult Index()
        {
            return View(this.countryRepository.GetContriesWithCities());
        }

        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetCountriesWithCitiesAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                await this.countryRepository.CreateAsync(country);
                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

    }
}