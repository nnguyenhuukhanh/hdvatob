using hdvatob.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Model;

namespace hdvatob.Data.Model
{
    public class hdvatobDbContext: DbContext
    {
        public hdvatobDbContext(DbContextOptions<hdvatobDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Hoadon>().HasKey(table => new
            {
                table.Idhoadon,
                table.chinhanh
            });
            builder.Entity<Huyhdvat>().HasKey(table => new
            {
                table.Idhoadon,
                table.chinhanh
            });
            builder.Entity<Tachve>().HasKey(table => new
            {
                table.Idhoadon,
                table.chinhanh
            });
            builder.Entity<supplier>().HasKey(table => new
            {
                table.code,
                table.chinhanh
            });
            builder.Entity<cthdvat>().Property(a => a.Id)
                    .HasColumnName("Id")
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();
            builder.Entity<Cttachve>().Property(a => a.Id)
                   .HasColumnName("Id")
                   .HasColumnType("decimal(18, 0)")
                   .ValueGeneratedOnAdd();
            builder.Entity<Huycthdvat>().Property(a => a.Id)
                  .HasColumnName("Id")
                  .HasColumnType("decimal(18, 0)")
                  .ValueGeneratedOnAdd();
        }

        public DbSet<LoginModel> LoginModel { get; set; }
        public DbSet<Hoadon> Hoadon { get; set; }
        public DbSet<cthdvat> cthdvat { get; set; }
        public DbSet<dmtk> dmtk { get; set; }
        public DbSet<dmhttc> dmhttc { get; set; }
        public DbSet<supplier> supplier { get; set; }
        public DbSet<dsdangkyhd> dsdangkyhd { get; set; }
        public DbSet<dmChinhanh> dmChinhanh { get; set; }
        public DbSet<Cttachve> Cttachve { get; set; }
        public DbSet<Tachve> Tachve { get; set; }
        public DbSet<Huyhdvat> Huyhdvat { get; set; }
        public DbSet<Huycthdvat> Huycthdvat { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ListDangkyHoadon> listDangkyHoadons { get; set; }
        public DbSet<Nguonhd> Nguonhd { get; set; }


        public DbSet<DoanhthungayhdViewModel> doanhthungayhd { get; set; }
        public DbSet<NgayhuyhdViewModel> Ngayhuyhd { get; set; }
        public DbSet<DataTuVetour> DataTuVetour { get; set; }
        public DbSet<DataFromTourViewModel> DataFromTourViewModels { get; set; }
        public DbSet<ChitietVetourViewModel> ChitietVetourViewModel { get; set; }
        public DbSet<VetourBySerial> VetourBySerials { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<ListChinhanh> GetListChinhanhs{ get; set; }
        public DbSet<TienCoupon> TienCoupons { get; set; }
        public DbSet<ChinhanhHoadondientu> dsChinhanhHoadondientu { get; set; }
        public DbSet<Dsdangkyhoadondientu> dsDangkyhoadondientu { get; set; }
    }
}
