using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookApi.Data;
using BookApi.Models;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET all
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Books.ToList());
        }

        // GET by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, Book updated)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            book.Title = updated.Title;
            book.Author = updated.Author;
            book.Year = updated.Year;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            return NoContent();
        }
    }
}