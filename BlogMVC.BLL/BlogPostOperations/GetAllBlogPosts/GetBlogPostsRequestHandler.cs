using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.GetAllBlogPosts
{
    public class GetBlogPostsRequestHandler : IRequestHandler<GetBlogPostsRequest, List<BlogPost>>
    {
        private readonly IRepository<BlogPost> _blogPostsRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Category> _categoryRepository;

        public GetBlogPostsRequestHandler(IRepository<BlogPost> blogPostsRepository,
            IRepository<Author> authorRepository, IRepository<Category> categoryRepository)
        {
            _blogPostsRepository = blogPostsRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<BlogPost>> Handle(GetBlogPostsRequest request, CancellationToken cancellationToken)
        {
            var blogs = _blogPostsRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTitle))
            {
                blogs = blogs.Where(b => b.Title.Contains(request.SearchTitle));
            }

            if (!string.IsNullOrEmpty(request.SearchCategory))
            {
                var category = _categoryRepository.GetAll().AsQueryable();
                blogs = blogs.Where(b => category.Where(c => c.Id == b.CategoryId).First()
                    .Name.Contains(request.SearchCategory));
            }

            if (!string.IsNullOrEmpty(request.SearchAuthor))
            {
                var author = _authorRepository.GetAll().AsQueryable();
                blogs = blogs.Where(b =>author.Where(a => a.Id == b.AuthorId).First()
                    .NickName.Contains(request.SearchAuthor));
            }

            await blogs.ForEachAsync(b => b.Author = _authorRepository.GetById(b.AuthorId).Result);
            await blogs.ForEachAsync(b => b.Category = _categoryRepository.GetById(b.CategoryId).Result);

            return await blogs.OrderBy(b => b.Title).ToListAsync();
        }
    }
}
