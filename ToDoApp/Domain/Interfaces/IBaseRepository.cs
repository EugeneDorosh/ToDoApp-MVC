namespace Service.Interfaces
{
    public interface IBaseRepository
    {
        public Task<bool> SaveAsync();
    }
}
