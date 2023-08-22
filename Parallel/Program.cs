using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace pruebaParallel
{
    internal class Program
    {
        static void ProcesarImagen(string ruta)
        {
            // Ejemplo: calcular el MD5 del archivo
            using (var md5 = MD5.Create())
            {
                using (var datos = File.OpenRead(ruta))
                {
                    byte[] hash = md5.ComputeHash(datos);
                    string hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    //Console.WriteLine($"Archivo: {Path.GetFileName(ruta)}, MD5: {hashString}"); Eso fue para pruebas  nomas
                }
            }
        }
        public static void Main(string[] args)
        {
            string rutaCarpeta = @"C:\Users\sumio\OneDrive\Escritorio\Unikino\UK-ArquiFinal\Wallpapers 2020"; // Reemplaza con la ruta de tu carpeta
            string[] archivos = Directory.GetFiles(rutaCarpeta);

            // Procesamiento síncrono
            Stopwatch syncSW = new Stopwatch();
            syncSW.Start();
            foreach (string archivo in archivos)
            {
                ProcesarImagen(archivo);
            }
            syncSW.Stop();

            // Procesamiento en paralelo con Parallel.ForEach
            Stopwatch parallelSW = new Stopwatch();
            parallelSW.Start();

            Parallel.ForEach(archivos, archivo =>
            {
                ProcesarImagen(archivo);
            });
            parallelSW.Stop();

            Console.WriteLine("\nTiempo de procesamiento síncrono: " + syncSW.Elapsed.TotalMilliseconds + " milisegundos");
            Console.WriteLine("Tiempo de procesamiento en paralelo: " + parallelSW.Elapsed.TotalMilliseconds + " milisegundos");

            double porcentajeDeMejora = (syncSW.Elapsed.TotalMilliseconds - parallelSW.Elapsed.TotalMilliseconds) / syncSW.Elapsed.TotalMilliseconds * 100;
            Console.WriteLine("La mejora del metodo paralelo es " + (int)porcentajeDeMejora + "% mas rapido con respecto al metodo sincrono.");


            Console.ReadLine();
        }
    }
}
