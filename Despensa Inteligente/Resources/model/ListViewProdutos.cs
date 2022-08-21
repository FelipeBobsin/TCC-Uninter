using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Despensa_Inteligente.Resources.model
{
    public class ListViewProdutos : BaseAdapter
    {
        private Activity context { get; }
        private List<Produto> produtos { get; }

        public override int Count => this.produtos != null ? this.produtos.Count : 0;

        public ListViewProdutos(Activity context, List<Produto> produtos)
        {
            this.context = context;
            this.produtos = produtos;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return produtos[position] != null ? produtos[position].Id : -1;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.listviewproduto_layout, parent, false);
            var txtItemIdOculto = view.FindViewById<TextView>(Resource.Id.txtItemIdOculto);
            var txtItemNome = view.FindViewById<TextView>(Resource.Id.txtItemNome);
            var txtItemQuantidade = view.FindViewById<TextView>(Resource.Id.txtItemQuantidade);
            var txtItemValidade = view.FindViewById<TextView>(Resource.Id.txtItemValidade);

            if (this.Count > 0 && position <= this.Count)
            {
                txtItemIdOculto.Text = produtos[position].Id.ToString();
                txtItemNome.Text = produtos[position].Nome;
                txtItemQuantidade.Text = produtos[position].Quantidade.ToString();
                txtItemValidade.Text = produtos[position].Validade;
            }

            return view;
        }
    }
}