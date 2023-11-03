using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.AuthorsOperations.GetAuthorById
{
    public class GetAuthorByIdRequest : IRequest<Author>
    {
        public int? Id { get; set; }
    }
}
