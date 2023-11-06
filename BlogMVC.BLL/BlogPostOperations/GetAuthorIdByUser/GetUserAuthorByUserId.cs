using BlogMVC.DAL.Models;
using MediatR;
using System.Security.Claims;

namespace BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser
{
    public class GetUserAuthorByUserId : IRequest<Author>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
