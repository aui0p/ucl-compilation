namespace EntityGarbageWPF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Entity;

    public partial class AccessoriesContext : DbContext
    {
        public AccessoriesContext()
            : base("name=AccessoriesContext")
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Logging> Logs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>()
                .Property(e => e.Name)
                .IsUnicode(false);
            

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Accessories)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
            
        }
    }
}
