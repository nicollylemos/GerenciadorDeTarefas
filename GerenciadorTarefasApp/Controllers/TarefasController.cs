using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Para usar métodos como ToListAsync(), FindAsync(), FirstOrDefaultAsync()
using GerenciadorTarefasApp.Data;     // Para usar o AppDbContext
using GerenciadorTarefasApp.Models;   // Para usar o modelo Tarefa

namespace GerenciadorTarefasApp.Controllers
{
    public class TarefasController : Controller // Controllers geralmente terminam com "Controller"
    {
        private readonly AppDbContext _context; // Campo para o contexto do banco de dados

        // Construtor: Injeção de Dependência do DbContext
        public TarefasController(AppDbContext context)
        {
            _context = context;
        }


        // 1. Action para listar todas as tarefas (READ - Listar)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefas.ToListAsync(); // Obtém todas as tarefas do banco
            return View(tarefas); // Passa a lista de tarefas para a View
        }

        // 2. Action para exibir o formulário de criação de nova tarefa (CREATE - GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Retorna a view vazia para o formulário
        }

        // 3. Action para processar o envio do formulário de criação (CREATE - POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // Proteção contra ataques CSRF
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,DataConclusao,Conclusao")] Tarefas tarefa) // <--- CORRIGIDO AQUI!
        {
            if (ModelState.IsValid) // Verifica se o modelo (Tarefa) é válido conforme Data Annotations
            {
                _context.Add(tarefa); // Adiciona a nova tarefa ao contexto (em memória)
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de tarefas
            }
            return View(tarefa); // Se o modelo não for válido, retorna a view com os dados para correção
        }

        // 4. Action para exibir os detalhes de uma tarefa (READ - Detalhes)
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna 404 se o ID não for fornecido
            }

            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(m => m.Id == id); // Busca a tarefa por ID
            if (tarefa == null)
            {
                return NotFound(); // Retorna 404 se a tarefa não for encontrada
            }

            return View(tarefa); // Passa a tarefa para a View
        }

        // 5. Action para exibir o formulário de edição (UPDATE - GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas.FindAsync(id); // Busca a tarefa por ID
            if (tarefa == null)
            {
                return NotFound();
            }
            return View(tarefa); // Passa a tarefa para a View para preencher o formulário
        }

        // 6. Action para processar o envio do formulário de edição (UPDATE - POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,DataConclusao,Conclusao")] Tarefas tarefa) // <--- CORRIGIDO AQUI!
        {
            if (id != tarefa.Id) // Verifica se o ID da URL corresponde ao ID do modelo
            {
                return NotFound(); // Se não, algo está errado, retorna 404
            }

            if (ModelState.IsValid) // Valida o modelo
            {
                try
                {
                    _context.Update(tarefa); // Marca a tarefa como modificada no contexto
                    await _context.SaveChangesAsync(); // Salva as alterações
                }
                catch (DbUpdateConcurrencyException) // Lida com conflitos de concorrência (múltiplos usuários editando)
                {
                    if (!_context.Tarefas.Any(e => e.Id == tarefa.Id)) // Verifica se a tarefa ainda existe no banco
                    {
                        return NotFound(); // Se não, ela foi excluída por outro usuário
                    }
                    else
                    {
                        throw; // Se ainda existe, mas houve um conflito, relança a exceção
                    }
                }
                return RedirectToAction(nameof(Index)); // Redireciona para a lista
            }
            return View(tarefa); // Retorna a view com erros de validação
        }

        // 7. Action para exibir o formulário de confirmação de exclusão (DELETE - GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(m => m.Id == id); // Busca a tarefa
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa); // Passa a tarefa para a View de confirmação
        }

        // 8. Action para processar a exclusão (DELETE - POST)
        [HttpPost, ActionName("Delete")] // Mapeia para a URL /Delete mas usa nome diferente para o método
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id); // Busca a tarefa pelo ID
            if (tarefa != null) // Se a tarefa for encontrada
            {
                _context.Tarefas.Remove(tarefa); // Remove a tarefa do contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco
            }
            return RedirectToAction(nameof(Index)); // Redireciona para a lista
        }

        // 9. Action para mudar o status de concluída (Funcionalidade extra - POST)
        [HttpPost]
        public async Task<IActionResult> ToggleConcluida(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id); // Encontra a tarefa
            if (tarefa == null)
            {
                return NotFound();
            }

            tarefa.Conclusao= !tarefa.Conclusao; // Inverte o valor de Concluida (true vira false, false vira true)
            _context.Update(tarefa); // Marca a tarefa como modificada
            await _context.SaveChangesAsync(); // Salva a alteração
            return RedirectToAction(nameof(Index)); // Redireciona para a lista
        }
    }
}