using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetAuthorIdByUser
{
    public class GetUserAuthorByUserId : IRequest<Author>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
