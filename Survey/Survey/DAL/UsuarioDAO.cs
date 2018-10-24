using Survey.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.DAL
{
    internal class UsuarioDAO : Banco
    {
        private List<Usuario> TableToList(DataTable dt)//ok
        {
            List<Usuario> dados = null;
            if (dt != null && dt.Rows.Count > 0)
                dados = (from DataRow row in dt.Rows
                         select new Usuario()
                         {
                             Id = Convert.ToInt32(row["Id"]),
                             Nome = row["Nome"].ToString(),
                             Email = row["Email"].ToString(),
                             Senha = row["Senha"].ToString(),
                             DataCadastro = Convert.ToDateTime(row["DataCadastro"]),
                             DataFim = row["DataFim"] is DBNull ? (DateTime?)null : Convert.ToDateTime(row["DataFim"]),
                             Questionarios = null
                         }).ToList();
            return dados;
        }

        internal Usuario Autenticar(int id, string senha)//aut-conf
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select * from Usuario
                                        where Id = @id and 
                                            Senha = @senha and
                                            DataFim is null";
            ComandoSQL.Parameters.AddWithValue("@id", id);
            ComandoSQL.Parameters.AddWithValue("@senha", senha);

            DataTable dt = ExecutaSelect();
            var dados = TableToList(dt);
            return dados == null ? null : dados.FirstOrDefault();
        }


    /*    internal int Gravar(Usuario u)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"insert into Usuario (Nome, Email, Senha, DataCadastro) 
                    values (@nome, @email, @senha, @datacadastro)";
            ComandoSQL.Parameters.AddWithValue("@nome", u.Nome);
            ComandoSQL.Parameters.AddWithValue("@email", u.Email);
            ComandoSQL.Parameters.AddWithValue("@senha", u.Senha);
            ComandoSQL.Parameters.AddWithValue("@datacadastro", u.DataCadastro);

            return ExecutaComando();
        }*/

        //////daniel
        internal int Gravar(Usuario u)//ok-alt-conf ///novo-conf
        {
            ComandoSQL.Parameters.Clear();

            if (u.Id == 0)//novo
            {
                ComandoSQL.CommandText = @"insert into Usuario (Nome, Email, Senha, DataCadastro) 
                     values (@nome, @email, @senha, @dataCadastro)";

            }
            else//altera
            {
                ComandoSQL.CommandText = @"update Usuario set Nome = @nome, Email = @email, 
                        Senha = @senha where Id = @id";
                ComandoSQL.Parameters.AddWithValue("@id", u.Id);
            }
            ComandoSQL.Parameters.AddWithValue("@nome", u.Nome);
            ComandoSQL.Parameters.AddWithValue("@email", u.Email);
            ComandoSQL.Parameters.AddWithValue("@senha", u.Senha);
            ComandoSQL.Parameters.AddWithValue("@dataCadastro", u.DataCadastro);
            int p = 0;
            return ExecutaComando(false,out p );
            return ExecutaComando();
        }

        internal Usuario AutenticarEmail(string email)//ok-alt-conf
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select Id, Nome, Email, Senha ,DataCadastro, DataFim 
                    from Usuario 
                    where Email = @Email and 
                          DataFim is null";
            ComandoSQL.Parameters.AddWithValue("@email", email);
            DataTable dt = ExecutaSelect();
            var dados = TableToList(dt);
            return dados == null ? null : dados.FirstOrDefault();
        }

        internal int Excluir(Usuario u)//ok-exc-conf
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"update Usuario set dataFim = @dataFim where Id = @id";
            ComandoSQL.Parameters.AddWithValue("@id", u.Id);
            ComandoSQL.Parameters.AddWithValue("@nome", u.Nome);
            ComandoSQL.Parameters.AddWithValue("@email", u.Email);
            ComandoSQL.Parameters.AddWithValue("@senha", u.Senha);
            ComandoSQL.Parameters.AddWithValue("@dataCadastro", u.DataCadastro);
            ComandoSQL.Parameters.AddWithValue("@dataFim", u.DataFim);
            return ExecutaComando();
        }

        internal Usuario AutenticarGet(int id)//aut-excluir
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select Id, Nome, Email, Senha ,DataCadastro, DataFim 
                    from Usuario 
                    where Id = @id and 
                          DataFim is null";
            ComandoSQL.Parameters.AddWithValue("@id", id);
            DataTable dt = ExecutaSelect();
            var dados = TableToList(dt);
            return dados == null ? null : dados.FirstOrDefault();
        }
    }
}
