
using System;
using TextFile;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LayerTest")]

namespace ass2
{
    internal class Program
    {

        static void Main(string[] args)
        {

            TextFileReader reader = new TextFileReader("inp.txt");

        

            //populating layers

            reader.ReadLine(out string line);
            int n = int.Parse(line);
            List<Layer> layers = new();
            for(int i =0; i< n; i++)
            {
                char[] separators = new char[] { ' ', '\t' };
                Layer layer = null;

                if (reader.ReadLine(out line))
                {
                    string[] tokens = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    char ch = char.Parse(tokens[0]);
                    double p = double.Parse(tokens[1]);

                    Console.WriteLine(ch);
                    switch (ch)
                    {
                        case 'Z': layer = new Ozone("Ozone", p); break;
                        case 'X': layer = new Oxygen("Oxygen", p); break;
                        case 'C': layer = new CarbonD("CarbonD", p); break;
                    }
                }
                layers.Add(layer);
            }

            ///populating the variables
            ///
            //reader.ReadLine(out line);
            //int m = int.Parse(line);
            List<Variable> variables = new List<Variable>();
          List<Layer> temp = new List<Layer>();
           while(reader.ReadChar(out char c) != false) {
                Console.WriteLine(c);
                switch (c)
                {
                    case 'O': variables.Add(Other.Instance()); break;
                    case 'T': variables.Add(Thund.Instance()); break;
                    case 'S': variables.Add(Sunshine.Instance()); break;
                }
            }

            // competition
            // competition
            for (int k = 0; k < n; k++)
            {
                if (layers[k] != null)
                {
                    Console.WriteLine("Layer: " + layers[k].Name + " " + layers[k].getTHS());
                }
            }


            try
            {
               int i = 0;
                int rounds = 1;
                
               while  (true)
                {
                    i = i % variables.Count;
                    Console.WriteLine("ROUND" + rounds);
                    Console.WriteLine("");
                    variables[i].Simulation(ref layers);  
                    Console.WriteLine("Layers in program cs: " + layers.Count);
                    for (int k = 0; k < layers.Count; k++)
                    {
                        
                       Console.WriteLine("Layers in program cs:  " + layers[k].Name + " " + layers[k].getTHS());
                    }
                    i++;
                    rounds++;
                    if (!didPerish(layers))
                    {
                        Console.WriteLine("THE END");
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.ToString());
            }

          




        }

        private static bool didPerish(List<Layer> layers)
        {
            bool hasOx = false;
            bool hasOz = false;
            bool hasCO = false;

            foreach(Layer layer in layers)
            {
                if(layer.GetType().Name == "Oxygen")
                {
                    hasOx = true;
                }
                if (layer.GetType().Name == "Ozone")
                {
                    hasOz = true;
                }
                if (layer.GetType().Name == "CarbonD")
                {
                    hasCO = true;
                }
            }
            return hasOx && hasOz && hasCO;
        }
    }
}