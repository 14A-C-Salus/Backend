using Salus.Data;

namespace Salus.Services.TagServices
{
    public class TagService:ITagService
    {
        private readonly DataContext _dataContext;
        private readonly GenericServices<Tag> _crud;
        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _crud = new CRUD<Tag>(_dataContext);
        }

        public Tag Create(TagCreateRequest request)
        {
            var tag = new Tag()
            {
                name = request.name,
                description = request.description,
                foodProperty = request.property,
                max = request.maxValue,
                min = request.minValue
            };
            CheckData(tag);
            tag = _crud.Create(tag);
            return tag;
        }

        public Tag Update(TagUpdateRequest request)
        {
            var tag = _crud.Read(request.id);
            if (tag == null)
                throw new Exception("This tag doesn't exist.");

            tag.description = request.description.Length == 0 ? tag.description : request.description;
            tag.name = request.name.Length == 0 ? tag.name : request.name;
            CheckData(tag);
            tag = _crud.Update(tag);
            return tag;
        }

        public void Delete(int id)
        {
            var tag = _crud.Read(id);
            if (tag == null)
                throw new Exception("This tag doesn't exist.");
            _crud.Delete(tag);
        }

        protected void CheckData(Tag tag)
        {
            if (tag.name.Length > 50)
                throw new Exception("Name can't be longer then 50 character!");
            if (tag.description.Length > 500)
                throw new Exception("Description can't be longer then 500 character!");
            if (tag.name.Length < 5)
                throw new Exception("Please enter at least 5 character to the name field.");
            if (tag.description.Length < 20)
                throw new Exception("Please enter at least 20 character to the description field.");
            if (tag.foodProperty == null && (tag.min != null || tag.max != null))
                throw new Exception("You can't give value to min or max, if you dont choose a property.");
            if (tag.max > 100 || tag.max < 0)
                throw new Exception("Please enter a number between 0 and 100 to do max field!");
            if (tag.min < 0 || tag.min > 100)
                throw new Exception("Please enter a number between 0 and 100 to do min field!");
        }
    }
}
