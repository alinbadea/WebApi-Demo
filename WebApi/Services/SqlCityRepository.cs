using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Services
{
    public class SqlCityRepository : ICityInfoRepository
    {
        private CityInfoContext _ctx;

        public SqlCityRepository(CityInfoContext ctx)
        {
            _ctx = ctx;
        }
        public bool CityExists(int cityId)
        {
            return _ctx.Cities.Any(x => x.Id == cityId);
        }
        public IEnumerable<City> GetCities()
        {
            return _ctx.Cities.OrderBy(x=>x.Name).ToList();
        }

        public City GetCity(int id, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return _ctx.Cities.Include(x => x.PointsOfInterest)
                    .FirstOrDefault(x => x.Id == id);
            }
            return _ctx.Cities.FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            return _ctx.PointsOfInterest
                .Where(x=>x.CityId==cityId).ToList();
        }
        public PointOfInterest GetPointOfInterest(int cityId, int id)
        {
            return _ctx.PointsOfInterest
                .FirstOrDefault(x=>x.CityId==cityId && x.Id==id);
        }

        public void InsertPointOfInterest(int cityId, PointOfInterest poi)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(poi);
        }
        public void DeletePointOfInterest(PointOfInterest poi)
        {
            _ctx.PointsOfInterest.Remove(poi);
        }
        public bool Save()
        {
            int res = _ctx.SaveChanges();
            return res >= 0;
        }
    }
}
