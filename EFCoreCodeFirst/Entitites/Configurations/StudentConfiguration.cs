
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreCodeFirst.Entitites.Configurations
{
  // yönteme ise Fluent API yöntemi diyoruz.
  // bu dosyada Student Program Nesnesinin POCO Class, veri tabanında aşağıdaki özelleştirmeler ile tutulması için ayarlamalar yaptığımız dosya StudentConfiguration dosyasıdır Plain Old CLR Object. 
  public class StudentConfiguration : IEntityTypeConfiguration<Student>
  {
    public void Configure(EntityTypeBuilder<Student> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Name).HasMaxLength(20).IsRequired(); // boş geçilemez ve 20 char olsun
      builder.Property(x => x.SurName).HasMaxLength(50).IsRequired();
      builder.Property(x => x.Number).HasMaxLength(12);

      
      //builder.Property(x => x.Name).HasColumnType("nvarchar(200)");
      builder.HasIndex(x => x.Number).IsUnique(); // Öğrenci numarası eşsiz olmalı aynı numara başka öğrenciye verilemez. Unique Index.

      // açık bir şekilde iki nesne arasında ilişki verme yöntemi.
      builder.HasOne(st => st.Department).WithMany(dp => dp.Students).HasForeignKey(st => st.DepartmentId);


    }
  }
}
