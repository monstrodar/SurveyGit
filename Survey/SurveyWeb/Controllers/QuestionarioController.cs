using SurveyWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cl = Survey.Controllers;
using Survey.ViewModels;
using System.IO;
using ExemploPDF.PdfUtils;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace SurveyWeb.Controllers
{
    [ValidarAcessoFilter]
    public class QuestionarioController : Controller
    {
        // GET: Questionario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObterPorUsuario(int id)
        {
            var dados = new cl.QuestionarioController().ObterPorUsuario(id);
            return Json(dados,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Obter(int id, int idUsuario)
        {
            var dados = new cl.QuestionarioController().Obter(id, idUsuario);
            return dados == null ? Json("") : Json(dados);
        }

        [HttpPost]
        public ActionResult ObterPorPalavraChave(string palavra, int idUsuario)
        {
            var dados = new cl.QuestionarioController().ObterPorPalavraChave(palavra, idUsuario);
            return dados == null ? Json("") : Json(dados);
        }

        [HttpPost]
        public ActionResult Gravar(FormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                int id = 0;
                int.TryParse(form["Id"], out id);
                string nome = form["Nome"].Trim();
                DateTime inicio = DateTime.MinValue;
                DateTime.TryParse(form["Inicio"], out inicio);
                DateTime fim = DateTime.MinValue;
                DateTime.TryParse(form["Fim"], out fim);
                string msgFeedback = form["MsgFeedBack"].Trim();
                string guid = form["Guid"].Trim();
                int idUsuario = Util.ObterUsuario;

                QuestionarioViewModel q = new QuestionarioViewModel();
                q.Id = id;
                q.Nome = nome;
                q.Inicio = inicio;
                q.Fim = fim;
                q.MsgFeedback = msgFeedback;
                q.Guid = guid;
                q.UsuarioId = idUsuario;

                if (q.Inicio > q.Fim)
                {
                    return Json("Data fim maior que a data de Inicio!!");
                }

                cl.QuestionarioController ctlQuestionario = new cl.QuestionarioController();
                if (ctlQuestionario.Gravar(q) > 0)
                    return Json("");
                else
                    return Json("Erro ao gravar o questionário: " + q.Nome.ToUpper());
            }
            else
            {
                return Json("O formulário submetido não contem valores válidos.");
            }
        }

        [HttpPost]
        public ActionResult Excluir(int id)
        {
            cl.QuestionarioController ctlQuestionario = new cl.QuestionarioController();
            if (ctlQuestionario.Excluir(id) > 0)
                return Json("");
            else
                return Json("Não foi possível excluir o registro selecionado.");
        }

        //Definir a rota para mapear a URL /PDF/Exportar/_NOME_DO_PARAMETRO_

       
        public ActionResult Exportar2(int Id)
        {
            List<QuestionarioViewModel> quest = new cl.QuestionarioController().ObterPorUsuario(Id);
            UsuarioViewModel usu = new cl.UsuarioController().Autenticar(Id);
            Document doc = new Document(PageSize.A4, 50, 50, 100, 50);
            MemoryStream stream = new MemoryStream();
            PdfWriter pdf = PdfWriter.GetInstance(doc, stream);
            pdf.CloseStream = false;

            TwoColumnHeaderFooter headerFooter = new TwoColumnHeaderFooter();
            pdf.PageEvent = headerFooter;
            headerFooter.Title = "Questionário(s) do(a) "+usu.Nome;
            headerFooter.HeaderLeft = "Survey";
            headerFooter.HeaderRight = "Trabalho 2º Bimestre";
            headerFooter.HeaderFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 14, BaseColor.RED);

            doc.Open();


            PdfPTable tabela = new PdfPTable(5);
            tabela.WidthPercentage = 100;
            PdfPCell titulo = new PdfPCell(new Phrase("Questionarios"));
            titulo.Colspan = 5;
            titulo.MinimumHeight = 30;
            titulo.HorizontalAlignment = 1; // 0 -> esquerda    1 -> centro   2 -> direita
            tabela.AddCell(titulo);
            tabela.AddCell("Id");
            tabela.AddCell("Título");
            tabela.AddCell("Início");
            tabela.AddCell("Fim");
            tabela.AddCell("Guid");
            for (int x = 0; x < quest.Count; x++)
            {
                tabela.AddCell("" + quest[x].Id);
                tabela.AddCell("" + quest[x].Nome);
                tabela.AddCell("" + quest[x].Inicio);
                tabela.AddCell("" + quest[x].Fim);
                tabela.AddCell("" + quest[x].Guid);
            }
            doc.Add(tabela);

            doc.Close();
            stream.Flush();
            stream.Position = 0;
            

            return File(stream, "application/pdf", "Questionario_do_Usuario.pdf");
        }
    }
}