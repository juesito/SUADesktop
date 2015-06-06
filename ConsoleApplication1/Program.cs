﻿using Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String pathSource = "C:\\SUA";

            String[] files = Directory.GetFileSystemEntries(pathSource);
            foreach (string path in files)
            {

                if (File.Exists(path))
                {
                    String ext = Path.GetExtension(path);
                    if (ext.Trim().Contains(".mdb") || ext.Trim().Contains(".MDB"))
                    {
                        // This path is a file
                        Upload cargarSua = new Upload();
                        cargarSua.uploadAcreditado(path);
                        cargarSua.uploadAsegurado(path);
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine("Error grave: " + e.Message.ToString());
                            Console.WriteLine("no se encontro el archivo: " + path);
                        }
                    }
                }
                else if (Directory.Exists(path))
                {
                    String[] subFiles = Directory.GetFiles(path);
                    foreach (string subPath in subFiles)
                    {
                        if (File.Exists(subPath))
                        {
                            String ext = Path.GetExtension(subPath);
                            if (ext.Trim().Contains(".mdb") || ext.Trim().Contains(".MDB"))
                            {
                                // This path is a file
                                Upload cargarSua = new Upload();
                                cargarSua.uploadAcreditado(subPath);
                                cargarSua.uploadAsegurado(subPath);
                                try
                                {
                                    System.IO.File.Delete(path);
                                }
                                catch (System.IO.IOException e)
                                {
                                    Console.WriteLine("Error grave: " + e.Message.ToString());
                                    Console.WriteLine("no se encontro el archivo: " + subPath);
                                }
                            }
                        }

                    }
                }

            }
        }
    }
}
