using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    /// <summary>
    /// Created The FundooContext Class For Migration Of DIfferent Tables Into Db
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<UserEntity> UserEntityTable { get; set; }

        public DbSet<NotesEntity> notesEntityTable { get; set; }
        
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
        public DbSet<LabelEntity> LabelTable { get; set; }

    }
}
