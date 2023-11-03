using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.AuthorsOperations.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<Unit>
    {
        public Author Author { get; set; }
    }
}
