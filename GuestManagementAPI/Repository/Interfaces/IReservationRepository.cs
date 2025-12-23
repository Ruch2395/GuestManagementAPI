
using global::GuestManagementAPI.Models;

namespace GuestManagementAPI.Data.Repositories
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(Guid id);
        Task UpdateAsync(Reservation reservation);
    }
}
