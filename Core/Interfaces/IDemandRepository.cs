using Core.Entities;

namespace Core.Interfaces
{
    public interface IDemandRepository
    {
        Task<List<Demand>> CreateDemandsAsync(List<Demand> demands);   

    }
}
