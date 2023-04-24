namespace Salus.Services.Interfaces
{
    public interface IOilService
    {
        public Oil Create(OilCreateRequest request);
        public Oil Update(OilUpdateRequest request);
        public void Delete(int id);
        public List<Oil> GetAll();
    }
}
