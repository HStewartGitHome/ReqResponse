using System.Collections.Generic;
using System.Threading.Tasks;

// From Tim Covey's Blazor Course

namespace ReqResponse.DataLayer.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);

        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}