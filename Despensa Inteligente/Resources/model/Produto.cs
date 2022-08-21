using SQLite;

namespace Despensa_Inteligente.Resources.model
{
    [Table("PRODUTO")]
    public class Produto
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public int Id_Local { get; set; }

        [NotNull, MaxLength(50)]
        public string Nome { get; set; }

        [NotNull]
        public int Quantidade { get; set; }

        [MaxLength(10)]
        public string Validade { get; set; }
    }
}