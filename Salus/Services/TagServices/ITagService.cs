namespace Salus.Services.TagServices
{
    public interface ITagService
    {
        public Tag Create(TagCreateRequest request);
        public Tag Update(TagUpdateRequest request);
        public void Delete(int id);
    }
}
