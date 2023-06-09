﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consultorio.Dominio.Entidades
{
    [BsonCollection("Doctores")]
    public class Doctor : Persona 
    {
        public string Cedula { get; set; }
        public string Telefono { get; set; }

        public Doctor(string nombre, string apellido, string cedula, string telefono):base(nombre, apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
            Cedula = cedula;
            Telefono = telefono;
        }
       
        public override string ToString()
        {
            return $"{Nombre}, {Apellido}, {Cedula}, {Telefono}";
        }
    }
}
