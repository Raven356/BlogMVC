using BlogMVC.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.BlogPostOperations.GetBlogPostAndCategoryNameById
{
    public class GetBlogPostAndCategoryNameByIdResponse
    {
        public BlogPost BlogPost { get; set; }

        public string CategoryName { get; set; }
    }
}
