using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Context;
using TaskManager.Models;

namespace TaskManager.Controllers
{

    [Route("[controller]")]
    public class TarefaController : Controller
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Criar")]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            if (ModelState.IsValid)
            {
                _context.Add(tarefa);
                _context.SaveChanges();


                var tarefas = _context.Tarefas.ToList();
                return RedirectToAction(nameof(ObterTodos));
            }
            return View(tarefa);
        }



        [HttpGet("{id}")]
        public IActionResult Detalhar(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null) return NotFound();

            return View(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            return View(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                ViewData["ErrorMessage"] = "Por favor, insira um título para realizar a busca.";
                return View(new List<Tarefa>());
            }

            var tarefas = _context.Tarefas
                .Where(x => x.Titulo.Contains(titulo)).ToList();

            if (!tarefas.Any())
            {
                ViewData["ErrorMessage"] = $"Nenhuma tarefa encontrada com o título: '{titulo}'.";
            }

            return View(tarefas);
        }


        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return View(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa? status)
        {
            if (!status.HasValue)
            {
                ViewData["ErrorMessage"] = "Por favor, selecione um status para realizar a busca.";
                return View(new List<Tarefa>()); // Retorna uma lista vazia
            }

            // Buscar as tarefas que correspondem ao status
            var tarefas = _context.Tarefas
                .Where(x => x.Status == status.Value)
                .ToList();

            if (!tarefas.Any())
            {
                ViewData["ErrorMessage"] = $"Nenhuma tarefa encontrada com o status: '{status.Value}'.";
            }

            return View(tarefas);
        }


        [HttpGet("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null) return NotFound();
            return View(tarefa);
        }


        [HttpPost("Editar/{id}")]
        public IActionResult Editar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Update(tarefaBanco);
            _context.SaveChanges();

            return RedirectToAction(nameof(ObterTodos));
        }

        [HttpGet("Deletar/{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null) return NotFound();
            return View(tarefa);
        }

        [HttpPost("Deletar/{id}")]
        public IActionResult Deletar(Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(tarefa.Id);

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Remove(tarefaBanco);
            _context.SaveChanges();
            return RedirectToAction(nameof(ObterTodos));
        }
    }
}