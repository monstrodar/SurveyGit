using Survey.Models;
using Survey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Controllers
{
    public class UsuarioController
    {
        public UsuarioViewModel Autenticar(int id, string senha)//aut-conf
        {
            Usuario u = new Usuario().Autenticar(id, senha);
            if (u != null)
                return new UsuarioViewModel()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    DataCadastro = u.DataCadastro,
                    DataFim = u.DataFim,
                    Questionarios = null//no meu nao tem
                    //Senha = u.Senha tem no meu
                };
            else
                return null;
        }

        public int Gravar(UsuarioViewModel u)//novo-conf
        {
            Usuario usuario = new Usuario();
            usuario.Id = u.Id;
            usuario.Nome = u.Nome;
            usuario.Email = u.Email;
            usuario.Senha = u.Senha;
            usuario.DataCadastro = u.DataCadastro;
            usuario.DataFim = u.DataFim;
            usuario.Questionarios = null;
            return usuario.Gravar();
        }
        /// <summary>
        /// ///////////////////////daniel
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int Inserir(UsuarioViewModel usuario)//ok-alt-conf
        {
            if (usuario != null)
            {
                Usuario u = new Usuario()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    DataCadastro = usuario.DataCadastro,
                    DataFim = usuario.DataFim
                };
                return u.Gravar();
            }
            else
                return -20;
        }

        public UsuarioViewModel AutenticarEmail(string email)///usa para alterar ususario
        {
            Usuario u = new Usuario().ObterEmail(email);
            if (u != null)
            {
                return new UsuarioViewModel()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Senha = u.Senha,
                    DataCadastro = u.DataCadastro,
                    DataFim = u.DataFim
                };
            }
            else
                return null;
        }

        public int Excluir(UsuarioViewModel usuario)//ok-alt-conf
        {
            if (usuario != null)
            {
                Usuario u = new Usuario()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    DataCadastro = usuario.DataCadastro,
                    DataFim = DateTime.Now

                };
                return u.Excluir();
            }
            else
                return -60;
        }


        public UsuarioViewModel Autenticar(int id)//aut-excluir
        {
            Usuario u = new Usuario().Obter(id);
            if (u != null)
            {
                return new UsuarioViewModel()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Senha = u.Senha,
                    DataCadastro = u.DataCadastro,
                    DataFim = u.DataFim
                };
            }
            else
                return null;
        }
        public int Alterar(UsuarioViewModel usuario)///duvida
        {
            return Inserir(usuario);
        }

    }
}
