using System;

namespace Planazo.Modelos
{
    public enum EstadoReserva
    {
        Activa,
        Cancelada
    }

    /// <summary>
    /// Clase que asocia a una persona/usuario con un evento reservado, controlando cupos, valor total y estado.
    /// </summary>
    public class Reserva
    {
        public int IdReserva { get; private set; }
        public Usuario Usuario { get; private set; }
        public string NombrePersona => Usuario.Nombre;
        public Evento EventoReservado { get; private set; }
        public int CantidadEntradas { get; private set; }
        public decimal ValorTotal { get; private set; }
        public EstadoReserva Estado { get; private set; }
        public DateTime FechaReserva { get; private set; }

        public Reserva(int idReserva, Usuario usuario, Evento eventoReservado, int cantidadEntradas)
        {
            if (idReserva <= 0)
                throw new ArgumentException("El identificador de la reserva debe ser mayor que cero.");

            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario de la reserva no puede ser nulo.");

            if (eventoReservado == null)
                throw new ArgumentNullException(nameof(eventoReservado), "El evento a reservar no puede ser nulo.");

            // Condición 1: No permitir reservas de cantidades negativas o iguales a cero
            if (cantidadEntradas <= 0)
                throw new ArgumentException("La cantidad de entradas a reservar debe ser mayor que cero (no se permiten cantidades negativas ni cero).");

            // Condición 2 y 3: Verificar cupos disponibles y disminuirlos al confirmar la reserva
            eventoReservado.DescontarCupos(cantidadEntradas);

            IdReserva = idReserva;
            Usuario = usuario;
            EventoReservado = eventoReservado;
            CantidadEntradas = cantidadEntradas;
            ValorTotal = eventoReservado.CalcularPrecioFinal() * cantidadEntradas;
            Estado = EstadoReserva.Activa;
            FechaReserva = DateTime.Now;
        }

        /// <summary>
        /// Sobrecarga de conveniencia para crear una reserva directamente con el nombre de la persona.
        /// </summary>
        public Reserva(int idReserva, string nombrePersona, Evento eventoReservado, int cantidadEntradas)
            : this(idReserva, new Usuario($"USR-{idReserva:D3}", nombrePersona), eventoReservado, cantidadEntradas)
        {
        }

        /// <summary>
        /// Cancela la reserva activa y devuelve los cupos correspondientes al evento.
        /// Impedir que una misma reserva sea cancelada dos veces.
        /// </summary>
        public void Cancelar()
        {
            // Condición 5: Impedir que una misma reserva sea cancelada dos veces
            if (Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException($"La reserva #{IdReserva} ya se encuentra cancelada y no puede cancelarse nuevamente.");

            // Condición 4: Devolver los cupos cuando se cancele una reserva
            EventoReservado.DevolverCupos(CantidadEntradas);
            Estado = EstadoReserva.Cancelada;
        }

        public override string ToString()
        {
            return $"Reserva #{IdReserva} | Estado: {Estado.ToString().ToUpper()} | Cliente: {NombrePersona} (Doc: {Usuario.Documento})" +
                   Environment.NewLine +
                   $"    Evento: [{EventoReservado.Id}] {EventoReservado.Nombre} ({EventoReservado.TipoEvento})" +
                   Environment.NewLine +
                   $"    Entradas: {CantidadEntradas} | Valor Unitario: ${EventoReservado.CalcularPrecioFinal():N0} | Valor Total: ${ValorTotal:N0}";
        }
    }
}