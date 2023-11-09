using BlogMVC.DAL.Models;

namespace BlogMVC.BLL.Services.TagsService
{
    public interface ITagsService
    {
        Task<IEnumerable<Tags>> GetTagsByBlogPostId(int? id);

        Task CreateTags(IEnumerable<string> tags, int blogId);

        Task UpdateTags(IEnumerable<string> tags, int blogId);
    }
}
