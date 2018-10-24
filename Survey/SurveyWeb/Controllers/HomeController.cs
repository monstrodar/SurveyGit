using Survey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cl = Survey.Controllers;

namespace SurveyWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Validar(int id, string Senha)//aut-conf
        {

            if (id >0 && Senha != "")
            {
                cl.UsuarioController ctlUsuario = new cl.UsuarioController();
                var usuario = ctlUsuario.Autenticar(id, Senha);
                if (usuario != null)
                {
                    HttpCookie ck = new HttpCookie("token");
                    ck.Values.Add("idUsuario", usuario.Id.ToString());
                    if (usuario.Nome.Length > 15)
                        usuario.Nome = usuario.Nome.Substring(0, 15);
                    ck.Values.Add("nomeUsuario", usuario.Nome);
                    ck.Values.Add("senha", usuario.Senha);
                    ck.Values.Add("email", usuario.Email);
                    Response.Cookies.Add(ck);

                    return Json("");
                }
                else
                {
                    return Json("O usuário e/ou a senha informados não conferem.");
                }
            }
            else
            {
                return Json("Por favor, informe um usuário e uma senha para acesso.");
            }
        }

        [HttpPost]
        public ActionResult Gravar(string Email, string Nome, string Senha)//novo-conf
        {
            if (Email != "" && Nome.Length > 2 && Senha.Length > 0)
            {
                cl.UsuarioController ctlUsuario = new cl.UsuarioController();
                UsuarioViewModel usuario = new UsuarioViewModel()
                {
                    Id = 0,
                    Nome = Nome,
                    Senha = Senha,
                    Email = Email,
                    DataCadastro = DateTime.Now,
                    DataFim = null
                };
                int idRecuperado =  ctlUsuario.Gravar(usuario);//não esta funcionando esta retornando 1
                if (idRecuperado > 0)  //sera o numero que retorna aqui é o codigo id que vou colacr em uma view bag
                {
                     HttpCookie ck = new HttpCookie("tokenId");
                     ck.Values.Add("idUsuario", Convert.ToString(idRecuperado));
                    Response.Cookies.Add(ck);
                    return Json("");
                }
                   
                else
                    return Json("Erro ao gravar o novo usuário.");
            }
            else
            {
                return Json("Por favor, informe todos os dados para criar um novo usuário.");
            }
        }

        public ActionResult Logout()
        {
            HttpCookie ck = Request.Cookies["token"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ck);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}