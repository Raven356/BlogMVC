using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.TagsService
{
    public class TagsService : ITagsService
    {
        private readonly IRepository<Tags> _tagsRepository;
        private readonly IRepository<TagToBlogPost> _tagsToBlogPostRepository;

        public TagsService(IRepository<Tags> tagsRepository, IRepository<TagToBlogPost> tagsToBlogPostRepository)
        {
            _tagsRepository = tagsRepository;
            _tagsToBlogPostRepository = tagsToBlogPostRepository;
        }

        public async Task CreateTags(IEnumerable<string> tags, int blogId)
        {
            var existing = _tagsRepository.GetAll();
            foreach (var tag in tags)
            {
                Tags newTag = new Tags();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = await _tagsRepository.Add(new Tags { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                await _tagsToBlogPostRepository.Add(new TagToBlogPost { BlogPostId = blogId, TagId =  newTag.Id });
            }
        }

        public async Task UpdateTags(IEnumerable<string> tags, int blogId)
        {
            if (tags.Count() < 0)
            {
                var deleteId = _tagsToBlogPostRepository.GetAll().Where(t => t.BlogPostId == blogId).Select(t => t.Id);
                await deleteId.ForEachAsync(d => _tagsRepository.Delete(d));
            }
            var existing = _tagsRepository.GetAll();
            var tagToBlog = _tagsToBlogPostRepository.GetAll();
            var existingTags = _tagsToBlogPostRepository.GetAll().Where(t => t.BlogPostId == blogId);
            List<int> addedId = new List<int>();
            foreach (var tag in tags)
            {
                Tags newTag = new Tags();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = await _tagsRepository.Add(new Tags { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                if (tagToBlog.Where(t => t.TagId == newTag.Id && t.BlogPostId == blogId).Count() == 0)
                {
                    await _tagsToBlogPostRepository.Add(new TagToBlogPost { BlogPostId = blogId, TagId = newTag.Id });
                }
                addedId.Add(newTag.Id);

            }
            var deleteTags = existingTags.Where(t => !addedId.Contains(t.TagId)).ToList();
            foreach (var tag in deleteTags)
            {
                await _tagsToBlogPostRepository.Delete(tag.Id);
            }

        }

        public async Task<IEnumerable<Tags>> GetTagsByBlogPostId(int? id)
        {
            var tagToBlogPost = _tagsToBlogPostRepository.GetAll().Where(t => t.BlogPostId == id);
            var tags = _tagsRepository.GetAll().Where(t => tagToBlogPost.Select(tb => tb.TagId).Contains(t.Id));
            return await tags.ToListAsync();
        }
    }
}
