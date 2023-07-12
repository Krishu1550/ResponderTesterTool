using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutoResponder.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class StudentController : Controller
	{

		List<Student> students= new List<Student>()
		{
			new Student()
			{
				StudentId= 1,
				Name= "Name2"
			},
			new Student()
			{
				StudentId = 2,
				Name= "Name1"
			}
		};
		[HttpGet]
		public IEnumerable<Student> Index()
		{
			return students;
		}
		[HttpGet]
		[Route("Student")]
		public Student Get(int id) { 

			return students[1];
		}

		[HttpPost]
		public string Post(Student student) 
		{
			return "Hello World";
		
		}
	}



	public class Student
	{
       public int StudentId { get; set; }
	   public string Name { get; set; }	
	   public bool IsEnable { get; set; }
	}
}
