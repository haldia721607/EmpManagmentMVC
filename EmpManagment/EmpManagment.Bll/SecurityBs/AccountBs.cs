using EmpManagment.Bol.Entities;
using EmpManagment.Dal.DbContextClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bll.SecurityBs
{
    public class AccountBs 
    {
        private EmployeeDbContext DbContext;
        public AccountBs()
        {
            this.DbContext = new EmployeeDbContext();
        }

        //public AccountBs(EmployeeDbContext employeeDbContext)
        //{
        //    this.DbContext = employeeDbContext;
        //}
        public IEnumerable<Country> CounteryList()
        {
            var allCountries = DbContext.Countries.Where(x => x.Status == true).ToList();
            if (allCountries.Count > 0)
            {
                return allCountries;
            }
            return null;
        }
        public IEnumerable<State> StateList(int countryId)
        {
            var getStateByCountry = DbContext.States.Where(x => x.CountryId == countryId && x.Status == true).ToList();
            if (getStateByCountry.Count > 0)
            {
                return getStateByCountry;
            }
            return null;
        }
        public IEnumerable<City> CityList(int stateId)
        {
            var getCityByState = DbContext.Cities.Where(x => x.StateId == stateId && x.Status == true).ToList();
            if (getCityByState.Count > 0)
            {
                return getCityByState;
            }
            return null;
        }
    }
}
