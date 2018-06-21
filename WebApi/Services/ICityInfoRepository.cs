using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Services
{
    public interface ICityInfoRepository
    {
        bool CityExists(int cityId);
        IEnumerable<City> GetCities();
        City GetCity(int id, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);
        PointOfInterest GetPointOfInterest(int cityId, int id);
        void InsertPointOfInterest(int cityId, PointOfInterest poi);
        void DeletePointOfInterest(PointOfInterest poi);
        bool Save();
    }
}
