using System;
using System.Collections.Generic;
using System.Text;

namespace ProcesadorExcel.Domain.Entities
{
    public sealed class Docente
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public double Valor {  get; private set; }

        public static Docente Crear(int id, string nombre, double valor)
        {
            return new Docente
            {
                Id = id,
                Nombre = nombre,
                Valor = valor
            };
        }
    }
}
