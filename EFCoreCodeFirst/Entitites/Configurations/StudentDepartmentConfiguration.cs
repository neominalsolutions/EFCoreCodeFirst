using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreCodeFirst.Entitites.Configurations
{
  public class StudentDepartmentConfiguration : IEntityTypeConfiguration<StudentDepartment>
  {
    public void Configure(EntityTypeBuilder<StudentDepartment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasIndex(x => x.Name).IsUnique();
      builder.Property(x => x.Code).HasMaxLength(15);

      // burada ilişki tanıma gerek yok.
      builder.HasMany(x => x.Students).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId);
    }
  }
}
