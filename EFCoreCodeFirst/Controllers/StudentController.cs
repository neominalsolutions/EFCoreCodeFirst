using EFCoreCodeFirst.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCodeFirst.Controllers
{
  public class StudentController : Controller
  {
    private AppDbContext dbContext;

    public StudentController(AppDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpGet("ogrenci-kayit",Name ="studentCreateRoute")]
    public async Task<IActionResult> Index()
    {
      // hiç kayıt yoksa gir

      var students = await dbContext.Students.ToListAsync();

      if (students.Count ==0 )
      {
        var bolum1 = new StudentDepartment("Bilişim");
        var bolum2 = new StudentDepartment("Elektrik");

        var stList = new List<Student>
      {

        new Student
        {
          Name = "Ali",
          SurName = "Tan",
          Number = "01",
          DepartmentId = bolum1.Id,
          Department = bolum1
        },
         new Student
        {
          Name = "Ahmet",
          SurName = "Kasım",
          Number = "02",
          DepartmentId = bolum1.Id,
          Department = bolum1
        },
          new Student
        {
          Name = "Mustafa",
          SurName = "Günay",
          Number = "03",
          DepartmentId = bolum1.Id,
          Department = bolum1
        },
        new Student
        {
          Name = "Ayşe",
          SurName = "Kanbur",
          Number = "04",
           DepartmentId = bolum2.Id,
           Department = bolum2
        },
         new Student
        {
          Name = "Mahmut",
          SurName = "Şen",
          Number = "05",
          DepartmentId = bolum2.Id,
           Department = bolum2
        }

      };


        // tek seferde bulk ınsert
        await this.dbContext.Students.AddRangeAsync(stList.ToArray());
        var result = await dbContext.SaveChangesAsync();
      }

      //var bl = new StudentDepartment("IT");

      //var sn = new Student();
      //sn.Name = "mert";
      //sn.SurName = "alp";
      //sn.Department = bl;
      //sn.DepartmentId = bl.Id;

      //await dbContext.Students.AddAsync(sn);
      //await dbContext.SaveChangesAsync();




      return View();
    }

    public async Task<IActionResult> Queries()
    {
      // öğrencileri numarasına göre sıralama, orderBy küçükten büyüğe

      var o1 = await dbContext.Students.OrderBy(x => x.Number).ToListAsync();

      // öğrencileri numarasına göre tersten sırlama, büyükten küçüğe sıralama
      var o2 = await dbContext.Students.OrderByDescending(x => x.Number).ToListAsync();

      // iki farklı alana göre sıralama yapmak için thenby
      // adları aynı ise soyada bakacak sıralama
      var o3 = await dbContext.Students.OrderBy(x => x.Name).ThenBy(x => x.SurName).ToListAsync();


      // öğrencileri soyada göre arama (where)
      var o4 = await dbContext.Students.Where(x => x.SurName == "Günay").ToListAsync();
      // soyadı Ka ile başlayan öğrenciler
      var o5 = await dbContext.Students.Where(x => x.SurName.StartsWith("Ka")).ToListAsync();
      // EndsWith
      // adında e geçen öğrenciler
      var o6 = await dbContext.Students.Where(x => x.Name.Contains("e")).ToListAsync();

      // bolum bilişim olan öğrencileri filtreleme
      // not lazy loading özelliği core versiyonunda kalktı.
      var o7 = await dbContext.Students.Include(x => x.Department).Where(x => x.Department.Name == "Bilişim").ToListAsync();

      // createdAt alanı boş geçilen öğrenci kayıtlarını bulma
      var o8 = await dbContext.Students.Where(x => x.CreatedAt == null).ToListAsync();

      // hangi bölümde kaç öğrenci var
      var o9 = await dbContext.Students
        .Include(x=> x.Department)
        .GroupBy(x => x.DepartmentId)
        .Select(a => new
      {
        BolumAdi = a.FirstOrDefault() == null ? "": a.FirstOrDefault().Department.Name,
        KisiSayisi = a.Count()

      }).ToListAsync();

      // LINQ
      var o10 = await (from s in dbContext.Students join sd in dbContext.StudentDepartments on s.DepartmentId equals sd.Id
                 group s by s.DepartmentId into g
                 select new
                 {
                   BolumAdi = g.FirstOrDefault() == null ? "" : g.FirstOrDefault().Department.Name,
                   KisiSayisi = g.Count()

                 }).ToListAsync();

      // RawSql
      var o112 = dbContext.Students.FromSqlRaw("select * from students");
      var o11 = dbContext.StudentDepartmentViews.FromSqlRaw("select Count(*) as StudentCount,sd.Name as DepartmentName from Students s inner join StudentDepartments sd on s.DepartmentId = sd.Id group by s.DepartmentId,sd.Name").AsNoTracking().ToList();



      // Elektronik bölümü var mı
      var o12 = dbContext.StudentDepartments.Any(x => x.Name == "Elektronik");
      // kaç tane öğrencim var ?
      var o13 =  await dbContext.Students.CountAsync();
      // f64f4a3c-19c6-4c65-bffc-a220c21ea9f8 nolu deparmandaki öğrenciler
      // find asNoTracking çalışır.
      var o14 = await dbContext.Students.FindAsync("f64f4a3c-19c6-4c65-bffc-a220c21ea9f8"); // bulmaz ise null bulursa tek kayıt.

      var o15 = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == "f64f4a3c-19c6-4c65-bffc-a220c21ea9f8"); // bulmaz ise null bulursa tek kayıt.

      // toList öncesi ve firstOrDefault() öncesi AsNoTracking() yapalım. AsNoTracking select işlemlerinde sorgunun hızlı dönmesini sağlayan bir sorgu optimizasyon tekniğidir.

      // sayfalama işlemleri için nasıl sorgularız.
      // 2 kayıt atlat 3 kayıt al
      // sayfalama yapmadan önce order işlemini unutmayalım.
      var o16 = await dbContext.Students.Include(x => x.Department).OrderBy(x => x.Name).Skip(2).Take(3).ToListAsync();


      return View();
    }


    [HttpGet("ogrenci-listesi",Name ="ogrenciSayfaRoute")]
    public async Task<IActionResult> SayfaliGetir(int currentPage = 1, int limit = 2)
    {
      ViewBag.ToplamSayfaSayisi = await dbContext.Students.CountAsync() / limit;
      

      var o16 = await dbContext.Students.Include(x => x.Department).OrderBy(x => x.Name).Skip((currentPage - 1) * limit).Take(limit).ToListAsync();

      return View(o16);
    }

  }
}
