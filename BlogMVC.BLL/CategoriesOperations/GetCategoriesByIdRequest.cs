using BlogMVC.BLL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMVC.BLL.CategoriesOperations
{
    public class GetCategoriesByIdRequest : IRequest<Category>
    {
        public int? Id { get; set; }
    }
}
