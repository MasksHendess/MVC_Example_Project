using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAssingment1.Data;
using MVCAssingment1.Domain.Entities;

namespace MVCAssingment1.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;

        public MoviesController(ApplicationDbcontext context)
        {
            _context = context;
        }

      
        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies(string title)
        {
            if(title == null)
            { 
            var AllMovies = _context.Movies.Select(x=> new
            {
                x.Id,
                x.Title
            });
                return Ok(AllMovies);
            }
            var Movies = _context.Movies.Where(x => x.Title.Contains(title));
            return Ok(Movies);
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<Comment>> GetComment(int Id)
        {

            var Comments = _context.Comments.Include(x => x.Movie).Where(x => x.MovieId == Id);
            return Ok(Comments);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

     
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            foreach (var item in _context.Movies)
            {
                if(item.Id == comment.MovieId)
                {
                    comment.Movie = item;
                }
            }
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
