using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoThreadTest
{

    class Proceso
    {

        public int TiempoRestante;
        public int IDproces;
        public int prioridad;
        public int TiempoServico;
        public string TipoMoneda;

        public Proceso()
        {

        }

        public Proceso(Proceso OBJ)
        {
            this.TiempoRestante = OBJ.TiempoRestante;
            this.IDproces = OBJ.IDproces;
            this.prioridad = OBJ.prioridad;
            this.TiempoServico = OBJ.TiempoServico;
            this.TipoMoneda = OBJ.TipoMoneda;
        }

    }
}
