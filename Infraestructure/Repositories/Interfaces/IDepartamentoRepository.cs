using Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories.Interfaces
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetAllDepartamentosAsync();
        Task<Departamento> GetDepartamentoByIdAsync(int id);
        Task AddDepartamentoAsync(Departamento departamento);
        Task UpdateDepartamentoAsync(Departamento departamento);
        Task DeleteDepartamentoAsync(int id);
    }
}
