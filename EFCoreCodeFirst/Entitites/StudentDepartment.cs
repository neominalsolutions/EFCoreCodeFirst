namespace EFCoreCodeFirst.Entitites
{
  public class StudentDepartment
  {
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string? Code { get; set; }


    /// <summary>
    /// Name alanı contructordan girilmesi zorunlu bırakıldı.
    /// </summary>
    /// <param name="name"></param>
    public StudentDepartment(string name)
    {
      Id = Guid.NewGuid().ToString();
      Name = name;
    }

    // bölümün öğrencileri
    public List<Student> Students { get; set; }

  }
}
