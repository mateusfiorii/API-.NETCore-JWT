using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// Endpoint => URL
// http://localhost:5000
// https://localhost:5001
namespace Shop.Controllers
{
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get(
            [FromServices] DataContext context
        )
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            var categoria = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post(
            [FromBody] Category model,
            [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { Mensagem = "Não foi possível criar um categoria" });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Category>> Put(
            int id,
            [FromBody] Category model,
            [FromServices] DataContext context
        )
        {
            // Verifica se o ID informado é o mesmo do model.
            if (id != model.Id)
                return NotFound(new { messagem = "Categoria não encontrada" });

            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { mensagem = "Este registro já foi atualizado." });
            }
            catch
            {
                return BadRequest(new { mensagem = "Não foi possível atualizar a categoria." });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { mensagem = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { mensagem = "Categoria removida com Sucesso!" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { mensagem = "Não possível excluir uma categoria" });
            }
        }
    }
}