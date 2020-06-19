namespace KTGK.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=DATA")
        {
        }

        public virtual DbSet<Categorys> Categorys { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorys>()
                .Property(e => e.Note)
                .IsFixedLength();

            modelBuilder.Entity<Categorys>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Categorys)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}
