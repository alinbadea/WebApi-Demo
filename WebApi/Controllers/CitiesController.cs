using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityRepo;

        public CitiesController(ICityInfoRepository cityRepo)
        {
            _cityRepo = cityRepo;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var citiesFromDb = _cityRepo.GetCities();
            var cities = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(citiesFromDb);
            return Ok(cities);
        }
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityRepo.GetCity(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }
            if (includePointsOfInterest)
            {
                var res = Mapper.Map<CityDto>(city);
                return Ok(res);
            }
            var c = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(c);
        }
    }
}