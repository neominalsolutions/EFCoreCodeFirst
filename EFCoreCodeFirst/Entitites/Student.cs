using System.ComponentModel.DataAnnotations;

namespace EFCoreCodeFirst.Entitites
{
  public class Student
  {
    // öğrenci Id PK

    public string Id { get; private set; }
    public string Name { get; set; }

    //[StringLength(20)]
    public string SurName { get; set; }
    public string? Number { get; set; }

    // öğrencinin bölümü fk
    public string DepartmentId { get; set; }

    public DateTime? CreatedAt { get; set; }


    // navigation property Ogrencinin Bolum Bilgisi
    public StudentDepartment Department { get; set; }

    // Öğrenicinin dersleri
    public List<Lesson> Lessons { get; set; }


    public Student()
    {
      Id = Guid.NewGuid().ToString();
    }

  }
}
