using Survey.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Models
{
    internal class Usuario
    {
        private int _id;
        private string _nome;
        private string _email;
        private string _senha;
        private DateTime _dataCadastro;
        private DateTime? _dataFim;
        private List<Questionario> _questionarios;

        internal int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        internal string Nome
        {
            get
            {
                return _nome;
            }

            set
            {
                _nome = value;
            }
        }

        internal string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }

        internal string Senha
        {
            get
            {
                return _senha;
            }

            set
            {
                _senha = value;
            }
        }

        internal DateTime DataCadastro
        {
            get
            {
                return _dataCadastro;
            }

            set
            {
                _dataCadastro = value;
            }
        }

        internal DateTime? DataFim
        {
            get
            {
                return _dataFim;
            }

            set
            {
                _dataFim = value;
            }
        }

        internal List<Questionario> Questionarios
        {
            get
            {
                return _questionarios;
            }

            set
            {
                _questionarios = value;
            }
        }

        internal Usuario Autenticar(int id, string senha)//ok-aut
        {
            if (id >0 && senha.Length > 0)
                return new UsuarioDAO().Autenticar(id, senha);
            else
                return null;
        }

        internal int Gravar()//ok-alt-conf///novo-conf
        {       
            if (_nome.Length >= 2 && _senha.Length >= 3 &&
                _email.Contains("@"))
               return new UsuarioDAO().Gravar(this);
            else
                return -10;
        }

        //////////////////daniel
        internal Usuario ObterEmail(string email)//ok-alt-conf
        {
            if (email != "")
                return new UsuarioDAO().AutenticarEmail(email);
            else
                return null;
        }
        internal int Excluir()//ok-ec-conf
        {
            return new UsuarioDAO().Excluir(this);
        }


        internal Usuario Obter(int id)//aut-excluir
        {
            if (id > 0)
                return new UsuarioDAO().AutenticarGet(id);
            else
                return null;
        }

    }
}
