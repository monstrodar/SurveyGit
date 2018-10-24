using Survey.ViewModels;
using SurveyWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cl = Survey.Controllers;  ///ATENÇÃO
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SurveyWeb.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [ValidarAcessoFilter]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Gravar(FormCollection form)//ok-alt-conf
        {
            int id = 0;
            try{
                id = Convert.ToInt32(form["txtIdNovo"]);
            } catch (Exception){
                id = 0;
            }
            string email = form["txtEmailNovo"];
            string nome = form["txtNomeNovo"];
            string senha = form["txtSenhaNovo"];
            UsuarioViewModel u = new UsuarioViewModel()
            {
                Id = id,
                Email = email,
                Senha = senha,
                Nome = nome,
                DataCadastro = DateTime.Now,
                DataFim = null
            };
            cl.UsuarioController ctlUsuario = new cl.UsuarioController();
            if (ctlUsuario.Inserir(u) > 0){
                HttpCookie ck = new HttpCookie("token");
                u = ctlUsuario.AutenticarEmail(u.Email);
                ck.Values.Add("idUsuario", u.Id.ToString());
                ck.Values.Add("nomeUsuario", u.Nome.ToString());
                ck.Values.Add("senha", u.Senha.ToString());
                ck.Values.Add("email", u.Email.ToString());
                Response.Cookies.Add(ck);
                return RedirectToAction("Index", "Dashboard");
            }
            else{
                ViewBag.Erro = "ERRO EM ALTERAR.";
                return RedirectToAction("Index", "Usuario");
            }
        }
        public string EnviarEmail( string emailPara)
        {  
           cl.UsuarioController ctlUsuario = new cl.UsuarioController();
            var usuario = ctlUsuario.AutenticarEmail(emailPara);
            if (usuario == null)
                return "Usuário não possui senha neste sistema";

            //Gerando o objeto da mensagem
            MailMessage msg = new MailMessage();
            //Remetente
            msg.From = new MailAddress("reddarprogam@gmail.com", "Survay - Trabalho 2 Bimestre");
            //Destinatários
            msg.To.Add(emailPara);
            //Assunto
            msg.Subject = "Recuperação senha";
            //Texto a ser enviado
            msg.Body = "Sistema Survay - Trabalho 2 Bimestre ---------- Esse email é um email de recuperação de senha" +
                "\nSeu ID = "+usuario.Id+" sua Senha = "+ usuario.Senha;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            //Gerando o objeto para envio da mensagem (Exemplo pelo Gmail)
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("seuemail@gmail.com", "senha");
            try
            {
                client.Send(msg);
                return "Mensagem enviada com sucesso!";
            }
            catch (Exception ex)
            {
                return "Falha: " + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }

        public ActionResult Excluir(int Id)//ok-exl-conf
        {
            cl.UsuarioController ctlUsuario = new cl.UsuarioController();
            UsuarioViewModel usuario = ctlUsuario.Autenticar(Id);
           
            if (ctlUsuario.Excluir(usuario) >  0) 
            {
                return RedirectToAction("Logout", "Home");
            }else{
                ViewBag.Erro = "Não foi POssivel Excluir ";
                return RedirectToAction("Index", "Usuario");
            }
        }
    }
}