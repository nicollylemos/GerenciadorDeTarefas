using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using GerenciadorTarefasApp.Data;
using GerenciadorTarefasApp.Models;

namespace GerenciadorTarefasApp.Controllers
{
    public class TarefasController : Controller
    {
        private readonly AppDbContext _context;
        public TarefasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tarefas = await _context.Tarefas.ToListAsync(); // Obtém todas as tarefas do banco
            return View(tarefas); // Passa a lista de tarefas para a View
        }

        [HttpGet]
        // Exibe o formulário para criar uma nova tarefa
        public IActionResult Create()
        {
            return View(); // Retorna a view vazia para o formulário
        }

        // Recebe os dados do formulário e cria uma nova tarefa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Titulo, Descricao, DataCriacao, DataConclusao, Conclusao ")] Tarefas taferas)
        {
            if (ModelState.IsValid) // Verifica se o modelo é válido
            {
                _context.Add(taferas); // Adiciona a nova tarefa ao contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                return RedirectToAction(nameof(List)); // Redireciona para a lista de tarefas
            }
            return View(taferas); // Retorna a view com os dados da tarefa se houver erro
        }

        

    }
}