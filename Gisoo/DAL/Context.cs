using Gisoo.Models;
using Gisoo.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
           
        }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<AllPrice> AllPrices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactUs> ContactUss { get; set; }
        public DbSet<AboutUs> AboutUss { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<FactorItem> FactorItems { get; set; }
        public DbSet<Advertisment> Advertisments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<AndroidVersion> AndroidVersions { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<LineType> LineTypes { get; set; }
        public DbSet<LineImage>  LineImages { get; set; }
        public DbSet<Line>  Lines { get; set; }

        public DbSet<ClassRoomType> ClassRoomTypes { get; set; }
        public DbSet<ClassRoomImage>  ClassRoomImages { get; set; }
        public DbSet<ClassRoom>  ClassRooms { get; set; }
        public DbSet<Reserve> Reserves { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<ProductImage>  ProductImages { get; set; }
        public DbSet<Visit>  Visits { get; set; }
        public DbSet<UserDocumentImage>  UserDocumentImages { get; set; }
        public DbSet<AllSearchDetailViewModel>  AllSearchDetailViewModels { get; set; }
        public DbSet<LineWeekDate>  LineWeekDates { get; set; }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //MapToStoredProcedures();
            //modelBuilder.Entity<Notice>()
            //    .HasQueryFilter(u => u.isDeleted);
            // modelBuilder.Entity<Advertisment>()
            //    .HasQueryFilter(u => u.isDeleted);
            modelBuilder.Entity<Notice>()
            .HasOne(i => i.city)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<Notice>()
            .HasOne(i => i.province)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<Notice>()
            .HasOne(i => i.area)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


             modelBuilder.Entity<Advertisment>()
            .HasOne(i => i.city)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<Advertisment>()
            .HasOne(i => i.province)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
             modelBuilder.Entity<Advertisment>()
            .HasOne(i => i.area)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
