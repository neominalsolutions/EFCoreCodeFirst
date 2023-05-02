namespace EFCoreCodeFirst.Entitites
{
  public class Lesson
  {
    public string Id { get; set; }
    public string Name { get; set; }

    public Lesson(string name)
    {
      Name = name;
    }


    // Öğrencinin dersleri
    public List<Student> Students { get; set; }



  }
}
