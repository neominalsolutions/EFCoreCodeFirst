using EFCoreCodeFirst.Entitites.Configurations;
using EFCoreCodeFirst.Entitites.Views;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCodeFirst.Entitites
{
  // veri tabanı bağülantılarını bu sınıftan yönetiyoruz
  public class AppDbContext:DbContext
  {
    // DbContextOptions<AppDbContext> options bu kod ile useMysql, useSqlServer gibi farklı opsiyonları consturctor dan gönderebilme imakanımız oluyor
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {

    }


    public DbSet<Student> Students { get; set; } // tablo isimlerinin sonuna s takısı koyarak entity ile karışmasını engelledik. DbSet EF gelen özel bir property, bu property ile veri tabanı ile tablolar arasında bir geçiş sağlanır. 

    public DbSet<StudentDepartment> StudentDepartments { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    // sanki bir tablo gibi tanımladık
    public DbSet<StudentDepartmentView> StudentDepartmentViews { get; set; }

    /// <summary>
    /// model oluşurken konfigürasyon işlemlerinin model db'de oluşmadan önce dbye tanıtıldığı yer
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // tablo olmadığında program nesnesi olduğundan dolayı programda sadece view olarak kullanacağımızdan dolayı hasNoKey tanımı yaptık.
      modelBuilder.Entity<StudentDepartmentView>().HasNoKey();

      modelBuilder.ApplyConfiguration(new StudentConfiguration());
      modelBuilder.ApplyConfiguration(new StudentDepartmentConfiguration());
      modelBuilder.ApplyConfiguration(new LessonConfiguration());


      base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
      // kayıt edilmeden önce ara işlem yapabilir.

      foreach (var item in this.ChangeTracker.Entries())
      {
        if(item.State == EntityState.Added)
        {
          // add yapmadan önceki kısım.

          if(item.Entity is Student)
          {
            var entity = item.Entity as Student;
            entity.CreatedAt = DateTime.Now; // createdAtleri otomatik ekledik.
          }
          
        }
      }

      return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {

      foreach (var item in this.ChangeTracker.Entries())
      {
        if (item.State == EntityState.Added)
        {
          // add yapmadan önceki kısım.

          if (item.Entity is Student)
          {
            var entity = item.Entity as Student;
            entity.CreatedAt = DateTime.Now; // createdAtleri otomatik ekledik.
          }

        }
      }

      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
  }
}
