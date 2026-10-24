using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Clase abstracta que representa las características y comportamientos comunes de cualquier evento en PLANAZO.
    /// Aplica Abstracción y Encapsulamiento.
    /// </summary>
    public abstract class Evento
    {
        private int _id;
        private string _nombre = string.Empty;
        private DateTime _fecha;
        private decimal _precioBase;
        private int _capacidadMaxima;
        private int _cuposDisponibles;

        public int Id
        {
            get => _id;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("El identificador del evento debe ser mayor que cero.");
                _id = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del evento no puede estar vacío.");
                _nombre = value.Trim();
            }
        }

        public DateTime Fecha
        {
            get => _fecha;
            private set => _fecha = value;
        }

        public decimal PrecioBase
        {
            get => _precioBase;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("El precio base no puede ser negativo.");
                _precioBase = value;
            }
        }

        public int CapacidadMaxima
        {
            get => _capacidadMaxima;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("La capacidad máxima debe ser mayor que cero.");
                _capacidadMaxima = value;
            }
        }

        public int CuposDisponibles
        {
            get => _cuposDisponibles;
            private set
            {
                if (value < 0 || value > CapacidadMaxima)
                    throw new InvalidOperationException("Los cupos disponibles deben estar entre 0 y la capacidad máxima.");
                _cuposDisponibles = value;
            }
        }

        public abstract string TipoEvento { get; }

        protected Evento(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima)
        {
            Id = id;
            Nombre = nombre;
            Fecha = fecha;
            PrecioBase = precioBase;
            CapacidadMaxima = capacidadMaxima;
            CuposDisponibles = capacidadMaxima;
        }

        /// <summary>
        /// Método abstracto polimórfico que cada subclase implementa según su regla de negocio.
        /// </summary>
        public abstract decimal CalcularPrecioFinal();

        /// <summary>
        /// Disminuye los cupos disponibles al confirmar una reserva.
        /// </summary>
        public void DescontarCupos(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de entradas a descontar debe ser mayor que cero.");

            if (cantidad > CuposDisponibles)
                throw new InvalidOperationException($"No hay suficientes cupos disponibles. Cupos actuales: {CuposDisponibles}, solicitados: {cantidad}.");

            CuposDisponibles -= cantidad;
        }

        /// <summary>
        /// Devuelve los cupos al evento cuando se cancela una reserva.
        /// </summary>
        public void DevolverCupos(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de entradas a devolver debe ser mayor que cero.");

            if (CuposDisponibles + cantidad > CapacidadMaxima)
                throw new InvalidOperationException("No se pueden devolver más cupos que la capacidad máxima del evento.");

            CuposDisponibles += cantidad;
        }

        public virtual string ObtenerDetalle()
        {
            return $"[{Id}] ({TipoEvento}) {Nombre} | Fecha: {Fecha:dd/MM/yyyy HH:mm}" +
                   Environment.NewLine +
                   $"    Precio Base: ${PrecioBase:N0} | Precio Final: ${CalcularPrecioFinal():N0} | Cupos: {CuposDisponibles}/{CapacidadMaxima}";
        }

        public override string ToString() => ObtenerDetalle();
    }
}