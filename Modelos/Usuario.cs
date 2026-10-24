using System;

namespace Planazo.Modelos
{
    /// <summary>
    /// Reto Adicional 2 (+0,2): Clase Usuario para identificar a las personas que realizan reservas
    /// y permitir consultar todas las reservas asociadas a un usuario.
    /// </summary>
    public class Usuario
    {
        private string _documento = string.Empty;
        private string _nombre = string.Empty;
        private string _correo = string.Empty;

        public string Documento
        {
            get => _documento;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El documento o identificador del usuario no puede estar vacío.");
                _documento = value.Trim();
            }
        }

        public string Nombre
        {
            get => _nombre;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del usuario no puede estar vacío.");
                _nombre = value.Trim();
            }
        }

        public string Correo
        {
            get => _correo;
            private set => _correo = string.IsNullOrWhiteSpace(value) ? "sin-correo@planazo.co" : value.Trim();
        }

        public Usuario(string documento, string nombre, string correo = "sin-correo@planazo.co")
        {
            Documento = documento;
            Nombre = nombre;
            Correo = correo;
        }

        public override string ToString()
        {
            return $"{Nombre} (ID: {Documento} | Email: {Correo})";
        }
    }
}
