using api.Models.Store;
using api.Models.Store.Request;

namespace api.Business.Interfaces.Store
{
    public interface IStoreService
    {
        public Task<List<StoreDto>> GetAll();
        public Task<StoreDto> GetbyId(int id);
        public Task<int?> Create(CreateOrEditStoreRqDto rq);
        public Task<int?> Update(int id, CreateOrEditStoreRqDto rq);
        public Task<int?> Delete(int id);
    }
}
