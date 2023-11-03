using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetCategoryById
{
    public class GetCategoryByIdRequest : IRequest<int>
    {
        public BlogPostCreateViewModel BlogPostCreateViewModel { get; set; }
    }
}
