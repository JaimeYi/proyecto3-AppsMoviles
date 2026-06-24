using SQLite;
using System;

namespace proyecto3.Models
{
    [Table("Transacciones")]
    public class Transaccion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Glosa { get; set; } = string.Empty;

        [NotNull]
        public decimal Cantidad { get; set; }

        [NotNull]
        public DateTime Fecha { get; set; }

        [NotNull]
        public bool EsIngreso { get; set; }

        [Ignore]
        public string CantidadFormateada => $"{(EsIngreso ? "+" : "-")} {Cantidad:C}";

        [Ignore]
        public string ColorMonto => EsIngreso ? "#22C55E" : "#B91C1C";

        [Ignore]
        public string FechaFormateada => Fecha.ToString("dd-MM-yyyy");
    }
}
