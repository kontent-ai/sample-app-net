using DancingGoat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DancingGoat.Repositories
{
    public interface ICafesRepository
    {
        Task<IEnumerable<Cafe>> GetCafes(string language, string country = null, string order = "system.name");
    }
}