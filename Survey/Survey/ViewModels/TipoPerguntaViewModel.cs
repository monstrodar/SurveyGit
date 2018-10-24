using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.ViewModels
{
    public class TipoPerguntaViewModel
    {
        public short Id { get; set; }
        public string Nome { get; set; }
        public List<PerguntaViewModel> Perguntas { get; set; }
    }
}
