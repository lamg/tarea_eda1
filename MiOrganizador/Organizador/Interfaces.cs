using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Organizador
{

    public enum TipoRecurso
    {
         // Persona en particular
         Persona,
         // Proyector
         Proyector,
         // Local
         Local
    }

    // Interfaz que debe de cumplir un Recurso
    public interface IRecurso
    {
         // Nombre o identificador del Recurso en particular
         string Nombre { get; set; }
         // Tipo de Recurso
         TipoRecurso TipoRecurso { get; set; }
    }

    // Interfaz que debe de tener una Tarea
    public interface ITarea
    {
        // Fecha de Inicio de la Tarea
        DateTime Inicio { get; set; }
        // Fecha de Vencimiento de la Tarea
        DateTime Vencimiento { get; set; }
        // Asunto o Tema de la Tarea
        string Asunto { get; set; }
        // Prioridad de la Tarea
        int Prioridad { get; set; }
        // Porciento de completado de la Tarea
        int PorcientoCompletado { get; set; }
        // Recursos que tiene asignados la Tarea
        IEnumerable<IRecurso> Recursos { get; set; }
        // Sub Tareas de la Tarea actual.
        IEnumerable<ITarea> Subtareas { get; set; }
    }

    // Interfaz que debe de cumplir un organizador de Tareas
    public interface IOrganizadorTareas
    {
        /// <summary>
        /// Inserta una nueva Tarea. De no ser posible por
        /// las restricciones retorna
        /// false. Si se insertó sin problemas retorna true.
        /// O(min(n, k*log(n))).
        /// </summary>
        /// <param name="tarea">Nueva Tarea a insertar</param>
        /// <returns>True si se insertó la nueva Tarea,
        /// retorna False en cualquier otro caso</returns>
        bool AdicionaTarea(ITarea tarea);
        /// <summary>
        /// Elimina una Tarea. De no existir la Tarea
        /// dispara una Excepción. O(lg(n))
        /// </summary>
        /// <param name="tarea">Tarea a eliminar</param>
        void EliminaTarea(ITarea tarea);
        /// <summary>
        /// Elimina todas las Tareas que cumplen con el
        /// Filtro. Menor orden posible.
        /// </summary>
        /// <param name="filtro">Filtro de las Tareas</param>
        void EliminaTareas(Filtro filtro);
        /// <summary>
        /// Método que retorna un IEnumerable de todas las
        /// Tareas que hay.
        /// </summary>
        /// <returns> IEnumerable de todas las Tareas.</returns>
        IEnumerable<ITarea> Tareas();
        /// <summary>
        /// Método que retorna un IEnumerable de todas
        /// las Tareas que cumplen con el Filtro. Menor orden posible.
        /// </summary>
        /// <param name="filtro">Filtro de las Tareas</param>
        /// <returns>IEnumerable de todas las Tareas que
        /// cumplen con el Filtro</returns>
        IEnumerable<ITarea> Tareas(Filtro filtro);
    }

    

    public class Filtro
    {
        public bool Filtrar(Tarea t) 
        {
            return true;
        }
    }

}
