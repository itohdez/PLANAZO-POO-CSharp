using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Reto Adicional 1 (+0,2): Nuevo tipo de evento derivado de Evento.
    /// Festival: aplica un recargo del 20% por logística e incluye un cargo fijo de manilla Cashless de $5.000.
    /// Demuestra el principio Abierto/Cerrado (Open/Closed Principle) sin modificar las clases existentes.
    /// </summary>
    public class Festival : Evento
    {
        private const decimal PorcentajeRecargo = 0.20m;
        private const decimal CargoManillaCashless = 5000m;

        public int CantidadTarimas { get; private set; }

        public override string TipoEvento => "Festival (+20% + $5.000)";

        public Festival(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima, int cantidadTarimas = 2)
            : base(id, nombre, fecha, precioBase, capacidadMaxima)
        {
            if (cantidadTarimas <= 0)
                throw new ArgumentException("El festival debe contar con al menos una tarima.");
            CantidadTarimas = cantidadTarimas;
        }

        public override decimal CalcularPrecioFinal()
        {
            return (PrecioBase * (1 + PorcentajeRecargo)) + CargoManillaCashless;
        }

        public override string ObtenerDetalle()
        {
            return $"{base.ObtenerDetalle()} | Tarimas: {CantidadTarimas}";
        }
    }
}
