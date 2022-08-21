using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Despensa_Inteligente.Resources.database;
using Despensa_Inteligente.Resources.model;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using AlertDialog = Android.App.AlertDialog;
using Android.Content;

namespace Despensa_Inteligente
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/despensa_icone")]   
    public class MainActivity : AppCompatActivity
    {
        ListView lstLocais;        
        List<Local> locais = new List<Local>();
        List<string> listaNomesLocais;

        BancoDadosLocal bancoDadosLocal;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            bancoDadosLocal = new BancoDadosLocal();

            lstLocais = FindViewById<ListView>(Resource.Id.lstLocais);
            FloatingActionButton addLocal = FindViewById<FloatingActionButton>(Resource.Id.addLocal);
            addLocal.Click += FabOnClick;

            this.CarregarLocais();
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View popupLocalCadastro = layoutInflater.Inflate(Resource.Layout.local_cadastro_popup, null);
            AlertDialog.Builder alertbuilder = new AlertDialog.Builder(this);
            alertbuilder.SetView(popupLocalCadastro);
            var inputNomeLocal = popupLocalCadastro.FindViewById<EditText>(Resource.Id.txtNomeLocal);
            alertbuilder.SetCancelable(false)
            .SetPositiveButton("Salvar", delegate
            {
                Local local = new Local()
                {
                    Nome = inputNomeLocal.Text.Trim(),
                };
                Retorno retorno = bancoDadosLocal.IncluirLocal(local);
                if (!retorno.Status)
                {
                    Toast.MakeText(this, retorno.Mensagem, ToastLength.Short).Show();
                    return;
                }
                this.CarregarLocais();
            })
            .SetNegativeButton("Cancelar", delegate
            {
                alertbuilder.Dispose();
            });
            AlertDialog dialog = alertbuilder.Create();
            dialog.Show();
        }        

        private void CarregarLocais()
        {
            locais = bancoDadosLocal.ListarLocais();
            if (locais.Count > 0)
            {
                listaNomesLocais = new List<string>();
                foreach (Local item in locais)
                {
                    if (item.Nome != null)
                    {
                        listaNomesLocais.Add(item.Nome);
                    }
                }
                if (listaNomesLocais.Count > 0)
                {
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listaNomesLocais);
                    lstLocais.Adapter = adapter;
                    lstLocais.ItemClick += LstLocais_ItemClick;
                }                
            }            
        }

        private void LstLocais_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Local localFiltro = new Local();
            localFiltro.Nome = listaNomesLocais[e.Position];
            Retorno retorno = bancoDadosLocal.ObterLocal(localFiltro);
            if (retorno.Status && retorno.Dados != null)
            {
                var intent = new Intent(this, typeof(ListaProdutosActivity));
                intent.PutExtra("IdLocal", retorno.Dados.Id_Local.ToString());
                intent.PutExtra("NomeLocal", retorno.Dados.Nome);
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