using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;

namespace BlogMVC.BLL.Services.ControllersService
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<User> _userRepository;

        public CommentsService(IRepository<Comment> commentRepository, IRepository<User> userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<BlogPostWithCommentsDTO> AddNewComment(BlogPostWithCommentsDTO request)
        {
            await _commentRepository.Add(request.NewComment);

            request.CommentList = _commentRepository.GetAll()
                .Where(c => c.BlogPostId == request.BlogPostValue.Id).ToList();

            request.CommentList.ToList()
                .ForEach(c => c.User = _userRepository.GetById(c.UserId!).Result);

            return request;
        }
    }
}
