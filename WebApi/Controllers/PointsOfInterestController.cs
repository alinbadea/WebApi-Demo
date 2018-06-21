using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private ICityInfoRepository _cityRepo;
        private IMailService _mailService;

        public PointsOfInterestController(ICityInfoRepository cityRepo, ILogger<PointsOfInterestController> logger,
            IMailService mailService)
        {
            _logger = logger;
            _cityRepo = cityRepo;
            _mailService = mailService;
        }
        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityRepo.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} not found");
                    return NotFound();
                }
                var pointsOfInterest = _cityRepo.GetPointsOfInterest(cityId);
                var result = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points f interest for cityId: {cityId}", ex);
                return StatusCode(500, "Server error");
            }
        }
        [HttpGet("{cityId}/pointsOfInterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityRepo.CityExists(cityId))
            {
                return NotFound();
            }
            var poi = _cityRepo.GetPointOfInterest(cityId, id);
            if (poi == null)
            {
                return NotFound();
            }
            var res = Mapper.Map<PointOfInterestDto>(poi);
            return Ok(res);
        }
        [HttpPost("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody]PointOfInterestForCreationDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.Name == dto.Description)
            {
                ModelState.AddModelError("Description", "Name and description should not match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_cityRepo.CityExists(cityId))
            {
                return NotFound();
            }
            var toAdd = Mapper.Map<Entities.PointOfInterest>(dto);
            _cityRepo.InsertPointOfInterest(cityId, toAdd);
            if (!_cityRepo.Save())
            {
                return StatusCode(500, "Error handling request");
            }
            var res = Mapper.Map<PointOfInterestDto>(toAdd);
            return CreatedAtRoute("GetPointOfInterest",
                new { cityId = cityId, id = res.Id }, res);

        }
        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.Name == dto.Description)
            {
                ModelState.AddModelError("Description", "Name and description should not match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityRepo.CityExists(cityId))
            {
                return NotFound();
            }
            var poi = _cityRepo.GetPointOfInterest(cityId, id);
            if (poi == null)
            {
                return NotFound();
            }
            Mapper.Map(dto, poi);
            if (!_cityRepo.Save())
            {
                return StatusCode(500, "Error processing request");
            }
            return NoContent();
        }
        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePartialPointOfInterest(int cityId, int id,
            [FromBody]JsonPatchDocument<PointOfInterestForUpdateDto> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }

            if (!_cityRepo.CityExists(cityId))
            {
                return NotFound();
            }
            var poi = _cityRepo.GetPointOfInterest(cityId, id);
            if (poi == null)
            {
                return NotFound();
            }
            var toPatch = Mapper.Map<PointOfInterestForUpdateDto>(poi);
            patch.ApplyTo(toPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (toPatch.Name == toPatch.Description)
            {
                ModelState.AddModelError("Description", "Name and description should not match");
            }
            TryValidateModel(toPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(toPatch, poi);
            if(!_cityRepo.Save())
            {
                return StatusCode(500, "Error processing request");
            }
            return NoContent();
        }
        [HttpDelete("{cityId}/pointsOfInterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityRepo.CityExists(cityId))
            {
                return NotFound();
            }
            var poi = _cityRepo.GetPointOfInterest(cityId, id);
            if (poi == null)
            {
                return NotFound();
            }
            _cityRepo.DeletePointOfInterest(poi);
            if(!_cityRepo.Save())
            {
                return StatusCode(500, "Error processing request");
            }
            _mailService.Send("Point of interest deleted",
                $"Point of interest \"{poi.Name}\" with id {poi.Id} deleted");
            return NoContent();
        }
    }
}
