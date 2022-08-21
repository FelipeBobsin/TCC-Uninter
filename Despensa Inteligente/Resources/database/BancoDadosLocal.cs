using Android.Util;
using Despensa_Inteligente.Resources.model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Despensa_Inteligente.Resources.database
{
    public class BancoDadosLocal
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public BancoDadosLocal()
        {
            this.CriarBancoDeDadosLocal();
        }

        public void CriarBancoDeDadosLocal()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.CreateTable<Local>();
                    conexao.CreateTable<Produto>();
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
            }
        }

        #region Métodos Tabela LOCAL
        public Retorno IncluirLocal(Local local)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            try
            {
                if (local == null)
                {
                    retorno.Mensagem = "Local não foi informado.";
                    return retorno;
                }
                if (local.Nome == null || local.Nome == string.Empty)
                {
                    retorno.Mensagem = "Nome do local não foi informado.";
                    return retorno;
                }
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.Insert(local);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação IncluirLocal Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public List<Local> ListarLocais ()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    return conexao.Table<Local>().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                return null;
            }
        }

        public Retorno ExcluirLocal(Local local)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            try
            {
                if (local == null)
                {
                    retorno.Mensagem = "Local não foi informado.";
                    return retorno;
                }
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.Delete(local);
                    conexao.Query<Produto>("DELETE FROM PRODUTO WHERE Id_Local = ?", local.Id_Local);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação ExcluirLocal Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public Retorno ObterLocal(Local local)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;            
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.Query<Local>("SELECT * FROM LOCAL");
                    retorno.Status = true;
                    if (conexao.Table<Local>().ToList().Count > 0)
                    {
                        retorno.Dados = new Local();
                        retorno.Dados = conexao.Table<Local>().ToList().First(x => x.Nome == local.Nome);
                    }
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }
        #endregion

        #region Métodos Tabela PRODUTO
        public Retorno IncluirProduto(Local local)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            try
            {    
                if (local == null || local.Produtos == null || local.Produtos.Count == 0)
                {
                    retorno.Mensagem = "Produto não foi informado.";
                    return retorno;
                }
                Produto produto = new Produto();
                produto = local.Produtos[0];
                produto.Id_Local = local.Id_Local;
                if (produto.Nome == null || produto.Nome == string.Empty)
                {
                    retorno.Mensagem = "Nome não foi informado.";
                    return retorno;
                }
                if (produto.Quantidade <= 0)
                {
                    retorno.Mensagem = "Quantidade não foi informada.";
                    return retorno;
                }
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.Insert(produto);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação IncluirProduto Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public Retorno ListarProdutos(Local local)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            retorno.Dados = local;
            retorno.Dados.Produtos = new List<Produto>();
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {                    
                    retorno.Dados.Produtos = conexao.Query<Produto>("SELECT * FROM PRODUTO Where Id_Local=?", local.Id_Local);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação ListarProdutos Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public Retorno AlterarProduto(Produto produto)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {                    
                    conexao.Update(produto);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação AlterarProduto Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public Retorno ExcluirProduto(Produto produto)
        {
            Retorno retorno = new Retorno();
            retorno.Status = false;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {
                    conexao.Delete(produto);
                    retorno.Status = true;
                    retorno.Mensagem = "Operação ExcluirProduto Realizada com Sucesso!";
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                retorno = new Retorno();
                retorno.Status = false;
                retorno.Mensagem = ex.Message;
                return retorno;
            }
        }

        public Produto ObterProduto(Produto produto)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "BancoDados.db3")))
                {                                        
                    return conexao.Get<Produto>(produto.Id);
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exceção: ", ex.Message);
                return null;
            }
        }
        #endregion

    }
}