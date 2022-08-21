using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Despensa_Inteligente.Resources.database;
using Despensa_Inteligente.Resources.model;
using System;
using System.Collections.Generic;

namespace Despensa_Inteligente
{
    [Activity(Label = "ProdutoActivity", Theme = "@style/AppTheme", Icon = "@drawable/despensa_icone")]
    public class ProdutoActivity : Activity
    {
        Local local;
        string status = string.Empty;
        EditText txtNome;
        EditText txtQuantidade;
        EditText txtValidade;
        EditText txtIdOculto;
        Button btnIncluir;
        Button btnAlterar;
        Button btnExcluir;
        BancoDadosLocal bancoDadosLocal;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.produto_cadastro);

            txtNome = FindViewById<EditText>(Resource.Id.txtNome);
            txtQuantidade = FindViewById<EditText>(Resource.Id.txtQuantidade);
            txtValidade = FindViewById<EditText>(Resource.Id.txtValidade);
            txtIdOculto = FindViewById<EditText>(Resource.Id.txtIdOculto);
            btnAlterar = FindViewById<Button>(Resource.Id.btnAlterar);
            btnExcluir = FindViewById<Button>(Resource.Id.btnExcluir);
            btnIncluir = FindViewById<Button>(Resource.Id.btnIncluir);

            bancoDadosLocal = new BancoDadosLocal();
            local = new Local();
            local.Nome = Intent.GetStringExtra("NomeLocal") ?? String.Empty;
            local.Id_Local = Intent.GetStringExtra("IdLocal") != null ? Convert.ToInt32(Intent.GetStringExtra("IdLocal")) : -1;
            status = Intent.GetStringExtra("Status") ?? string.Empty;

            txtValidade.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtValidade.Clickable = true;
            txtValidade.Click += TxtValidade_Click;

            this.VerificaStatus();

            btnIncluir.Click += BtnIncluir_Click;
            btnAlterar.Click += BtnAlterar_Click;
            btnExcluir.Click += BtnExcluir_Click;
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            Produto produto = new Produto();
            produto.Id_Local = local.Id_Local;
            produto.Id = Convert.ToInt32(txtIdOculto.Text);
            produto.Validade = txtValidade.Text;
            produto.Quantidade = Convert.ToInt32(txtQuantidade.Text != null ? txtQuantidade.Text : "0");
            produto.Nome = txtNome.Text.Trim();
            Retorno retorno = bancoDadosLocal.ExcluirProduto(produto);
            if (!retorno.Status)
            {
                Toast.MakeText(this, retorno.Mensagem, ToastLength.Short).Show();
                return;
            }
            var intent = new Intent(this, typeof(ListaProdutosActivity));
            intent.PutExtra("IdLocal", local.Id_Local.ToString());
            intent.PutExtra("NomeLocal", local.Nome);
            StartActivity(intent);
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            Produto produto = new Produto();
            produto.Id_Local = local.Id_Local;
            produto.Id = Convert.ToInt32(txtIdOculto.Text);
            produto.Validade = txtValidade.Text;
            produto.Quantidade = Convert.ToInt32(txtQuantidade.Text != null ? txtQuantidade.Text : "0");
            produto.Nome = txtNome.Text.Trim();
            Retorno retorno = bancoDadosLocal.AlterarProduto(produto);
            if (!retorno.Status)
            {
                Toast.MakeText(this, retorno.Mensagem, ToastLength.Short).Show();
                return;
            }
            var intent = new Intent(this, typeof(ListaProdutosActivity));
            intent.PutExtra("IdLocal", local.Id_Local.ToString());
            intent.PutExtra("NomeLocal", local.Nome);
            StartActivity(intent);
        }

        private void BtnIncluir_Click(object sender, EventArgs e)
        {
            local.Produtos = new List<Produto>();
            Produto produto = new Produto();
            produto.Id_Local = local.Id_Local;
            produto.Validade = txtValidade.Text;
            produto.Quantidade = Convert.ToInt32(txtQuantidade.Text != null ? txtQuantidade.Text : "0");
            produto.Nome = txtNome.Text.Trim();
            local.Produtos.Add(produto);
            Retorno retorno = bancoDadosLocal.IncluirProduto(local);
            if (!retorno.Status)
            {
                Toast.MakeText(this, retorno.Mensagem, ToastLength.Short).Show();
                return;
            }
            var intent = new Intent(this, typeof(ListaProdutosActivity));
            intent.PutExtra("IdLocal", local.Id_Local.ToString());
            intent.PutExtra("NomeLocal", local.Nome);
            StartActivity(intent);
        }

        private void TxtValidade_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
            dialog.DatePicker.MinDate = DateTime.Now.Millisecond;          
            dialog.Show();
        }

        private void VerificaStatus()
        {
            switch (status)
            {
                case "Incluir":
                    {
                        btnIncluir.Enabled = true;
                        btnAlterar.Enabled = false;
                        btnExcluir.Enabled = false;
                        btnIncluir.Visibility = ViewStates.Visible;
                        btnAlterar.Visibility = ViewStates.Gone;
                        btnExcluir.Visibility = ViewStates.Gone;
                        break;
                    }
                case "Cadastro":
                    {
                        this.PopularCampos();
                        btnIncluir.Enabled = false;
                        btnAlterar.Enabled = true;
                        btnExcluir.Enabled = true;
                        btnIncluir.Visibility = ViewStates.Gone;
                        btnAlterar.Visibility = ViewStates.Visible;
                        btnExcluir.Visibility = ViewStates.Visible;
                        break;
                    }
                default:
                    btnAlterar.Enabled = true;
                    btnExcluir.Enabled = true;
                    btnIncluir.Enabled = true;
                    btnIncluir.Visibility = ViewStates.Visible;
                    btnAlterar.Visibility = ViewStates.Visible;
                    btnExcluir.Visibility = ViewStates.Visible;
                    break;
            }
        }

        private void PopularCampos()
        {
            Produto produto = new Produto();
            produto.Id = Intent.GetStringExtra("IdProduto") != null ? Convert.ToInt32(Intent.GetStringExtra("IdProduto")) : -1;
            produto = bancoDadosLocal.ObterProduto(produto);
            if (produto != null)
            {
                txtNome.Text = produto.Nome;
                txtQuantidade.Text = produto.Quantidade.ToString();
                txtValidade.Text = produto.Validade != null ? produto.Validade : String.Empty;
                txtIdOculto.Text = produto.Id.ToString();
            }            
        }

        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            if (e.Date.Date < DateTime.Now.Date)
            {
                Toast.MakeText(this, "Validade deve ser maior que a data atual.", ToastLength.Short).Show();
                txtValidade.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }
            else
            {
                txtValidade.Text = e.Date.Date.ToString("dd/MM/yyyy");
            }            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}