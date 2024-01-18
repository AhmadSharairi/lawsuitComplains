using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DemandRepository : IDemandRepository
    {
        private readonly ComplaintContext _complaintContext;

        public DemandRepository(ComplaintContext complaintContext)
        {
            _complaintContext = complaintContext;

        }
        public async Task<List<Demand>> CreateDemandsAsync(List<Demand> demands)
        {
            foreach (var demand in demands)
            {
                _complaintContext.Demands.Add(demand);
            }
            await _complaintContext.SaveChangesAsync();
            
            return demands;
        }


    }


}
