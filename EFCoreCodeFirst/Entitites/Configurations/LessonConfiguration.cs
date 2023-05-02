using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreCodeFirst.Entitites.Configurations
{
  public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
  {
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {

      builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
      builder.HasMany(x => x.Students).WithMany(x => x.Lessons);
    }
  }
}
