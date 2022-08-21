using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Despensa_Inteligente.Resources.database;
using Despensa_Inteligente.Resources.model;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Despensa_Inteligente
{
    [Activity(Label = "ListaProdutosActivity", Theme = "@style/AppTheme")]
    public class ListaProdutosActivity : Activity
    {
        TextView titulo;
        Local local;
        ListView lstProdutos;
        List<Produto> produtos = new List<Produto>();
        BancoDadosLocal bancoDadosLocal;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.lista_produtos);
            lstProdutos = FindViewById<ListView>(Resource.Id.lstProdutos);

            bancoDadosLocal = new BancoDadosLocal();
            titulo = FindViewById<TextView>(Resource.Id.txtTitleProdutos);
            local = new Local();

            local.Nome = Intent.GetStringExtra("NomeLocal") ?? String.Empty;
            local.Id_Local = Intent.GetStringExtra("IdLocal") != null ? Convert.ToInt32(Intent.GetStringExtra("IdLocal")) : -1;

            if (local.Nome != null)
            {
                titulo.Text = titulo.Text + " de " + local.Nome;
            }

            this.CarregarProdutos();

            FloatingActionButton addProduto = FindViewById<FloatingActionButton>(Resource.Id.addProduto);
            addProduto.Click += FabOnClick;
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(this, typeof(ProdutoActivity));
            intent.PutExtra("IdLocal", local != null ? local.Id_Local.ToString() : "-1");
            intent.PutExtra("Status", "Incluir");
            StartActivity(intent);
        }

        private void CarregarProdutos()
        {
            Retorno retorno = bancoDadosLocal.ListarProdutos(local);
            if (retorno.Status && retorno.Dados != null && retorno.Dados.Produtos != null && retorno.Dados.Produtos.Count > 0)
            {
                produtos = retorno.Dados.Produtos;
                var adapter = new ListViewProdutos(this, produtos);
                lstProdutos.Adapter = adapter;
                lstProdutos.ItemClick += LstProdutos_ItemClick;
            }
        }

        private void LstProdutos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (produtos[e.Position] != null)
            {
                var intent = new Intent(this, typeof(ProdutoActivity));
                intent.PutExtra("IdLocal", local != null ? local.Id_Local.ToString() : "-1");
                intent.PutExtra("Status", "Cadastro");
                intent.PutExtra("IdProduto", produtos[e.Position].Id.ToString());
                StartActivity(intent);
            }            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}