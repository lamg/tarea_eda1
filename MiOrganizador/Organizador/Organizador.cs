using System;
using System.Collections.Generic;


namespace Organizador
{
    public class Organizador:IOrganizadorTareas
    {
        public Organizador()
        {
            
        }

        IntervalTree<Tarea,DateTime> arbol;
       

        public bool AdicionaTarea(ITarea tarea)
        {
            Tarea t = tarea as Tarea;
            if (arbol == null)
            {
                arbol = new IntervalTree<Tarea, DateTime>(t, Color.Black);
            }
            foreach (Tarea item in arbol.Overlap(t))
            {
                if (!item.Equals(t) && CompartenRecursos(t,item))
                {
                    return false;
                }
            }
            IntervalTree<Tarea, DateTime>.InsertKey(t, arbol);
            foreach (Tarea item in t.Subtareas)
            {
                IntervalTree<Tarea, DateTime>.InsertKey(item, arbol);
            }
            return true;
        }

        private void CompruebaIntegridad(Tarea t) 
        {
            
        }

        private bool CompartenRecursos(Tarea t1, Tarea t2) 
        {
            List<IRecurso> l = new List<IRecurso>(t2.Recursos);
            foreach (IRecurso item in t1.Recursos)
            {
                if (l.Contains(item))
                {
                    return true;
                }
            }
            return false;

        }

        public void EliminaTarea(ITarea tarea)
        {
            Tarea t = tarea as Tarea;
            
            IntervalTree<Tarea,DateTime>.DeleteKey(t, ref arbol);
            
        }

        public void EliminaTareas(Filtro filtro)
        {
            foreach (Tarea item in this.arbol.Inorder())
            {
                if (filtro.Filtrar(item))
                {
                    IntervalTree<Tarea,DateTime>.DeleteKey(item, ref this.arbol);
                }
            }
        }

        public IEnumerable<ITarea> Tareas()
        {
            return this.arbol.Inorder();
        }

        public IEnumerable<ITarea> Tareas(Filtro filtro)
        {
            throw new NotImplementedException();
        }
    }

    public class Tarea:ITarea,IComparable<ITarea>, IIntervaleable<DateTime>
    {

        public Tarea() {}

        public Tarea(DateTime inicio, DateTime vencimiento, string asunto, int por_ciento, IEnumerable<IRecurso> recursos, IEnumerable<ITarea> tareas) 
        {
            CompruebaAsunto(asunto);
            CompruebaFechas(inicio, vencimiento);
            CompruebaPorCiento(por_ciento);
            CompruebaRecursos(recursos);
            CompruebaSubtareas(tareas);
        }

        DateTime inicio,vencimiento;
        string asunto;
        int prioridad,por_ciento;
        List<IRecurso> recursos;
        List<ITarea> tareas;


        public DateTime Inicio
        {
            get
            {
                return this.inicio;
            }
            set
            {
                CompruebaFechas(value, this.vencimiento);
            }
        }

        public DateTime Vencimiento
        {
            get
            {
                return this.vencimiento;
            }
            set
            {
                CompruebaFechas(this.inicio, value);
            }
        }

        public string Asunto
        {
            get
            {
                return this.asunto;
            }
            set
            {
                CompruebaAsunto(value);
            }
        }

        public int Prioridad
        {
            get
            {
                return this.prioridad;
            }
            set
            {
                this.prioridad = value;
            }
        }

        public int PorcientoCompletado
        {
            get
            {
                return this.por_ciento;
            }
            set
            {
                CompruebaPorCiento(value);
            }
        }

        public IEnumerable<IRecurso> Recursos
        {
            get
            {
                return this.recursos;
            }
            set
            {
                this.recursos = new List<IRecurso>(value as IEnumerable<Recurso>);
            }
        }

        public IEnumerable<ITarea> Subtareas
        {
            get
            {
                return this.tareas;
            }
            set
            {
                this.tareas = new List<ITarea>(value);
            }
        }
          
        public int CompareTo(ITarea other)
        {
            int comp_fecha = this.inicio.CompareTo(other.Inicio);
            if (comp_fecha == 0)
            {
                int comp_prioridad = this.prioridad.CompareTo(other.Prioridad);
                if (comp_prioridad == 0)
                {
                    int comp_asunto = this.asunto.CompareTo(other.Asunto);
                    if (comp_asunto == 0)
                    {
                        return this.por_ciento.CompareTo(other.PorcientoCompletado);
                    }
                    return comp_asunto;
                }
                return comp_prioridad;
            }
            return comp_fecha;
        }
        
        public DateTime Start
        {
            get
            {
                return this.Inicio;
                
            }
            set
            {
                this.Inicio = value;
            }
        }

        public DateTime End
        {
            get
            {
                return this.Vencimiento;
            }
            set
            {
                this.Vencimiento = value;
            }
        }

        private void CompruebaAsunto(string asunto) 
        {
            if (asunto == null || asunto == "")
            {
                throw new ArgumentException("El asunto no puede ser vacio");
            }
            this.asunto = asunto;
        }

        private void CompruebaPorCiento(int por_ciento) 
        {
            if (por_ciento < 0 || por_ciento > 100)
            {
                throw new ArgumentException("Por ciento de cumplimiento no valido");
            }
            this.por_ciento = por_ciento;
        }

        private void CompruebaFechas(DateTime inicio, DateTime vencimiento) 
        {
            if (inicio.CompareTo(vencimiento) > 0)
            {
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la de vencimiento");
            }
            this.inicio = inicio;
            this.vencimiento = vencimiento;
        }

        private void CompruebaRecursos(IEnumerable<IRecurso> recursos) 
        {
            
            this.recursos = new List<IRecurso>();
            foreach (IRecurso item in recursos)
            {
                if (this.recursos.Contains(item as Recurso))
                {
                    throw new ArgumentException("Recurso asignado mas de una vez en la misma tarea");
                }
                this.recursos.Add(item as Recurso);
            }
            if (this.recursos.Count == 0)
            {
                throw new ArgumentException("Debe haber un recurso asignado");
            }
        }

        private void CompruebaSubtareas(IEnumerable<ITarea> tareas) 
        {
            foreach (ITarea item in tareas)
            {
                if (item.Inicio.CompareTo(this.inicio)<0)
                {
                    throw new ArgumentException("La fecha de inicio de las subtareas no puede ser menor que la de la tarea que las contiene");
                }
            }

            this.tareas = new List<ITarea>(tareas);
        }

        public override string ToString()
        {
            return this.inicio.ToString() + this.vencimiento.ToString()+this.asunto + this.por_ciento.ToString()+this.prioridad.ToString() + recursos.ToString() + Subtareas.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }

    public class Recurso:IRecurso
    {
        public Recurso(string nombre, TipoRecurso tipo)
        {
            this.nombre = nombre;
            this.tipo = tipo;
        }

        string nombre;
        TipoRecurso tipo;

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public TipoRecurso TipoRecurso
        {
            get
            {
                return this.tipo;
            }
            set
            {
                this.tipo = value;
            }
        }
    }
}
