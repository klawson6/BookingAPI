using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Controllers {
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context) {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems() {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null) {
                return NotFound();
            }

            return todoItem;
        }

        // GET: api/TodoItems/setabunch
        [HttpGet("setabunch")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> SetLotsTodoItem() {
            TodoItemDTO td = new TodoItemDTO();
            td.Name = "test1";
            td.IsComplete = false;
            _context.TodoItems.Add(td);
            await _context.SaveChangesAsync();
            td = new TodoItemDTO();
            td.Name = "test2";
            td.IsComplete = true;
            _context.TodoItems.Add(td);
            await _context.SaveChangesAsync();
            td = new TodoItemDTO();
            td.Name = "test3";
            td.IsComplete = false;
            _context.TodoItems.Add(td);
            await _context.SaveChangesAsync();

            return await _context.TodoItems.ToListAsync();
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItem) {
            if (id != todoItem.Id) {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!TodoItemExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItem) {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDTO>> DeleteTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id) {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
