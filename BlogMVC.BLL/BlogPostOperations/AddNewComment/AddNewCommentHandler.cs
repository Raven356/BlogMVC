using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.BlogPostOperations.AddNewComment
{
    public class AddNewCommentHandler : IRequestHandler<AddNewCommentCommand, BlogPostWithComments>
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<User> _userRepository;

        public AddNewCommentHandler(IRepository<Comment> repository, IRepository<User> userRepository)
        {
            _commentRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<BlogPostWithComments> Handle(AddNewCommentCommand request, CancellationToken cancellationToken)
        {
            await _commentRepository.Add(request.BlogPostWithComments.NewComment);
            request.BlogPostWithComments.CommentList = _commentRepository.GetAll()
                .Where(c => c.BlogPostId == request.BlogPostWithComments.BlogPostValue.Id).ToList();
            request.BlogPostWithComments.CommentList.ForEach(async c => c.User = await _userRepository.GetById(c.UserId));
            return request.BlogPostWithComments;
        }
    }
}
