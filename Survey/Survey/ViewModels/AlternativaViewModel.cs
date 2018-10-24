using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.ViewModels
{
    public class AlternativaViewModel
    {
        public int Id { get; set; }
        public string Opcao { get; set; }
        public short Ordem { get; set; }
        public int PerguntaId { get; set; }
        public TipoPerguntaViewModel TipoPergunta { get; set; }
        public List<RespostaViewModel> Respostas { get; set; }
        public PerguntaViewModel Pergunta { get; set; }
    }
}
