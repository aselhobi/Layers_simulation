using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using TextFile;


namespace ass2
{
    abstract class Variable
    {
        public abstract Layer ChangeOz(Ozone o);
        public abstract Layer ChangeO(Oxygen o);
        public abstract Layer ChangeCB(CarbonD o);
        #region Exceptions
        public class IncorrectInputException : Exception { };
        public class NoLayerException : Exception { };
        #endregion

        protected Variable() { }

        public void Simulation(ref List<Layer> layers)
        //!layers[j].PerishOneGas()
        {
            if (layers.Count == 0) { throw new Variable.NoLayerException(); }
            List<Layer> temp = new List<Layer>();
            int index = 0;

            while (index < layers.Count)
            {
                Console.WriteLine("Current " + layers[index].GetType().Name + " and " + this.GetType().Name);
                Layer prevLayer = layers[index];
                Layer updatedLayer = layers[index].Traverse(this);

                //if there is no newly transfromed layer
                if (updatedLayer == null)
                {
                    temp.Add(prevLayer);
                }
                else
                {
                    if (prevLayer.getTHS() > 0.5)
                    {
                        temp.Add(prevLayer);

                    }
                    else
                    {
                        bool foundMatch = false;
                        for (int i = temp.Count - 1; i >= 0; i--)
                        {
                            if (temp[i].GetType() == prevLayer.GetType())
                            {
                                Layer combinedLayer = temp[i].Combine(prevLayer);
                                temp[i] = combinedLayer;
                                foundMatch = true;
                                break;
                            }
                        }

                        if (!foundMatch)
                        {
                            if (prevLayer.getTHS() > 0.5)
                            {
                                temp.Add(prevLayer);
                            }
                        }
                    }
                    bool found = false;
                    for (int i = temp.Count - 1; i >= 0; i--)
                    {
                        if (temp[i].GetType() == updatedLayer.GetType())
                        {
                            Layer combinedLayer = temp[i].Combine(updatedLayer);
                            temp[i] = combinedLayer;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        if (updatedLayer.getTHS() > 0.5)
                        {
                            temp.Add(updatedLayer);
                        }
                    }
                }
                index++;
            }

            layers = new List<Layer>(temp);

        


    }


}

    // class of Thunderstorm
    class Thund : Variable
    {
        public override Layer ChangeCB(CarbonD o)
        {
            return null;

        }

        public override Layer ChangeO(Oxygen o)
        {
            double thickPrev = o.getTHS();

            double thickOfNew = thickPrev - o.ModifyThicknss(50);


            return new Ozone("Ozone", thickOfNew);
        }
        public override Layer ChangeOz(Ozone o)
        {
            return null;
        }


        private Thund() : base() { }
        private static Thund instance = null;

        public static Thund Instance()
        {
            if (instance == null)
            {
                instance = new Thund();
            }
            return instance;
        }
    }


    // class of Sunshine
    class Sunshine : Variable
    {
        public override Layer ChangeCB(CarbonD o)
        {
            double thickPrev = o.getTHS();

            double thickOfNew = thickPrev - o.ModifyThicknss(5);

            return new Oxygen("Oxygen", thickOfNew);
        }

        public override Layer ChangeO(Oxygen o)
        {
            double thickPrev = o.getTHS();

            double thickOfNew = thickPrev - o.ModifyThicknss(5);

            return new Ozone("Ozone", thickOfNew);
        }
        public override Layer ChangeOz(Ozone o)
        {
            //o.ModifyThicknss(null);
            return null;
        }


        private Sunshine() { }
        private static Sunshine instance = null;
        public static Sunshine Instance()
        {
            if (instance == null)
            {
                instance = new Sunshine();
            }
            return instance;
        }
    }

    // class of Other
    class Other : Variable
    {
        public override Layer ChangeCB(CarbonD o)
        {
            return null;
        }

        public override Layer ChangeO(Oxygen o)
        {
            double thickPrev = o.getTHS();

            double thickOfNew = thickPrev - o.ModifyThicknss(10);

            return new CarbonD("CarbonD", thickOfNew);
        }
        public override Layer ChangeOz(Ozone o)
        {
            double thickPrev = o.getTHS();

            double thickOfNew = thickPrev - o.ModifyThicknss(5);

            return new Oxygen("Oxygen", thickOfNew);
        }


        private Other() { }
        private static Other instance = null;
        public static Other Instance()
        {
            if (instance == null)
            {
                instance = new Other();
            }
            return instance;
        }
    }


}