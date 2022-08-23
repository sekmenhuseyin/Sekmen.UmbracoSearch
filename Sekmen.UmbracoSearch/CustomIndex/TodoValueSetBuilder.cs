using Examine;
using Sekmen.UmbracoSearch.Models;
using Umbraco.Cms.Infrastructure.Examine;

namespace Sekmen.UmbracoSearch.CustomIndex
{
    public class TodoValueSetBuilder : IValueSetBuilder<ToDoModel>
    {
        public IEnumerable<ValueSet> GetValueSets(params ToDoModel[] content)
        {
            return from todo
                    in content
                   let indexValues = new Dictionary<string, object>
                   {
                       ["userId"] = todo.UserId,
                       ["id"] = todo.Id,
                       ["title"] = todo.Title,
                       ["completed"] = todo.Completed
                   }
                   select new ValueSet(todo.Id.ToString(), "todo", indexValues);
        }
    }

}
