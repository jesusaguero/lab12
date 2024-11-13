using lab12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public StudentsController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Student> GetAll()
        {
            return _context.Students.Where(s => s.Active).ToList(); 
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public ActionResult<Student> Create(Student student)
        {
            if (student == null)
            {
                return BadRequest("Estudiante inválido");
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = student.StudentID }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest("El ID del estudiante no coincide");
            }

            var existingStudent = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);
            if (existingStudent == null)
            {
                return NotFound("Estudiante no encontrado");
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Phone = student.Phone;
            existingStudent.Email = student.Email;
            existingStudent.GradeId = student.GradeId;

            _context.SaveChanges();

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);
            if (student == null)
            {
                return NotFound("Estudiante no encontrado");
            }

            student.Active = false;
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}
