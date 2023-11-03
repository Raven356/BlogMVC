using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdRequest : IRequest<GetBlogPostAndCategoryNameByIdResponse>
    {
        public int? Id { get; set; }
    }
}
