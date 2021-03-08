using EmpManagment.Bol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.IBll.IAccountRepository
{
    public interface IAccount
    {
        IEnumerable<Country> CounteryList();
        IEnumerable<State> StateList(int counteryId);
        IEnumerable<City> CityList(int stateId);
    }
}
