using System;
using System.Collections.Generic;
using Organizador;

namespace pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            Organizador.Organizador a = new Organizador.Organizador();
            ITarea t1 = new Tarea(new DateTime(2010,12,1),new DateTime(2010,12,2),"pepe",1,new IRecurso[]{new Recurso("coco", TipoRecurso.Local)},new ITarea[0]);
            ITarea t2 = new Tarea(new DateTime(2010, 12, 1), new DateTime(2010, 12, 1), "kiko", 1, new IRecurso[] { new Recurso("k", TipoRecurso.Local) }, new ITarea[0]);
            ITarea t3 = new Tarea(new DateTime(2010, 11, 20), new DateTime(2010, 12, 5), "comer", 10, new IRecurso[] { new Recurso("mesa", TipoRecurso.Local) }, new ITarea[] { t1, t2 });
            ITarea p1 = new Tarea(new DateTime(2010,9,19), new DateTime(2010,9,21),"beber",50, t1.Recursos, new ITarea[0]);
            ITarea p2 = new Tarea(new DateTime(2010,9, 20), new DateTime(2010, 9, 21), "azul", 3, t1.Recursos, new Tarea[0]);
            a.AdicionaTarea(t3);
            a.AdicionaTarea(p1);
            Imprimir(a.Tareas());
            a.EliminaTarea(t2);
            a.EliminaTarea(p1);
            a.EliminaTarea(t3);
            Imprimir(a.Tareas());


            
        }

        static void Imprimir(IEnumerable<ITarea> tareas) 
        {
            foreach (Tarea item in tareas)
            {
                Console.WriteLine(item.Inicio + " " + item.Asunto);
            }
            Console.ReadLine();
        }
    }
}
