using Salus.Data;
using Salus.Exceptions;

namespace Salus.Services.TagServices
{
    public class TagService:ITagService
    {
        private readonly GenericService<Tag> _genericServices;
        public TagService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _genericServices = new(dataContext, httpContextAccessor);
        }

        public Tag Create(TagCreateRequest request)
        {
            var tag = new Tag()
            {
                name = request.name,
                description = request.description,
                recipeProperty = request.property,
                max = request.maxValue,
                min = request.minValue
            };
            CheckData(tag);
            tag = _genericServices.Create(tag);
            return tag;
        }

        public Tag Update(TagUpdateRequest request)
        {
            var tag = _genericServices.Read(request.id);
            if (tag == null)
                throw new ETagNotFound();

            tag.description = request.description.Length == 0 ? tag.description : request.description;
            tag.name = request.name.Length == 0 ? tag.name : request.name;
            CheckData(tag);
            tag = _genericServices.Update(tag);
            return tag;
        }

        public void Delete(int id)
        {
            var tag = _genericServices.Read(id);
            if (tag == null)
                throw new ETagNotFound();
            _genericServices.Delete(tag);
        }

        protected void CheckData(Tag tag)
        {
            if (tag.name.Length > 50)
                throw new ENameTooLong();
            if (tag.description.Length > 500)
                throw new EDescriptionTooLong();
            if (tag.name.Length < 3)
                throw new ENameTooShort();
            if (tag.description.Length < 10)
                throw new EDescriptionTooShort();
            if (tag.recipeProperty == null && (tag.min != null || tag.max != null))
                throw new EPropertyValue();
            if (tag.max > 100 || tag.max < 0)
                throw new EMaxValueOutOfRange();
            if (tag.min < 0 || tag.min > 100)
                throw new EMinValueOutOfRange();
        }

        public List<Tag> GetAll()
        {
            return _genericServices.ReadAll().ToList();
        }
    }
}
