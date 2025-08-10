using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefasApp.Models
{
    public class Tarefas
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título não pode ultrapassar de 100 caracteres.")]
        public string Titulo { get; set; }

        [StringLength(600, ErrorMessage = "A descrição não pode ultrapassar de 600 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de criação é obrigatória.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "A data de criação deve ser uma data válida.")]
        public DateTime DataCriacao { get; set; }


        [Required(ErrorMessage = "A data de conclusão é obrigatória.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "A data de conclusão deve ser uma data válida.")]
        public DateTime DataConclusao { get; set; }

        public bool Conclusao { get; set; }
    }
}