using api.Business.Interfaces.Store;
using api.Data.Entities;
using api.Data.EntityFramework;
using api.Models.Store;
using api.Models.Store.Request;
using Microsoft.EntityFrameworkCore;

namespace api.Business.Services.Store
{
    public class StoreService(AppDbContext _context) : IStoreService
    {
        public async Task<int?> Create(CreateOrEditStoreRqDto rq)
        {
            StoreEntity existStore = await _context.Stores.FirstOrDefaultAsync(s => s.Name == rq.Name);
            if (existStore != null)
                return null;
            StoreEntity store = new StoreEntity
            {
                Name = rq.Name,
                Address = rq.Address,
            };
            _context.Add(store);
            await _context.SaveChangesAsync();
            return store.Id;
        }

        public async Task<int?> Delete(int id)
        {
            StoreEntity store = await _context.Stores.FindAsync(id);
            if (store is null)
            {
                return null;
            }
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return store.Id;
        }

        public async Task<List<StoreDto>> GetAll()
        {
            List<StoreDto> storesDto = new List<StoreDto>();
            List<StoreEntity> stores = await _context.Stores.ToListAsync();

            foreach (StoreEntity store in stores)
            {
                StoreDto storeDto = new StoreDto
                {
                    Id = store.Id,
                    Name = store.Name,
                    Address = store.Address

                };
                storesDto.Add(storeDto);
            }
            return storesDto;
        }

        public async Task<StoreDto> GetbyId(int id)
        {
            if (id == 0)
                return null;
            StoreEntity store = await _context.Stores.FindAsync(id);

            if (store is null)
                return null;
            StoreDto storeDto = new StoreDto { Id = store.Id, Name = store.Name, Address = store.Address };

            return storeDto;

        }

        public async Task<int?> Update(int id, CreateOrEditStoreRqDto rq)
        {
            if (id == 0)
                return null;
            StoreEntity store = await _context.Stores.FindAsync(id);

            if (store is null)
                return null;

            store.Name = rq.Name;
            store.Address = rq.Address;

            _context.Update(store);
            await _context.SaveChangesAsync();
            return store.Id;
        }
    }
}
