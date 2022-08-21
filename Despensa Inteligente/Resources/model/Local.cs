using SQLite;
using System.Collections.Generic;
namespace Despensa_Inteligente.Resources.model
{
    [Table("LOCAL")]
    public class Local
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id_Local { get; set; }
        
        [NotNull, MaxLength(30)]
        public string Nome { get; set; }

        [Ignore]
        public List<Produto> Produtos { get; set; }
    }
}