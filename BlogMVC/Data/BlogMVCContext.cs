using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogMVC.Data
{
    public class BlogMVCContext : IdentityDbContext<User>
    {
        public BlogMVCContext (DbContextOptions<BlogMVCContext> options)
            : base(options)
        {
        }

        public DbSet<BlogMVC.Models.Author> Author { get; set; } = default!;

        public DbSet<BlogMVC.Models.BlogPost>? BlogPost { get; set; }

        public DbSet<BlogMVC.Models.Category>? Category { get; set; }

        public DbSet<BlogMVC.Models.Comment>? Comment { get; set; }

        public DbSet<BlogMVC.Models.User>? User { get; set; }
    }
}
