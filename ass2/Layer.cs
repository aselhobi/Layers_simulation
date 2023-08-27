using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ass2
{
    abstract class Layer
    {


        public string Name { get; }
        protected double thickness;
        public double getTHS()
        {return  thickness;}

        //int[] ints = new int[3];

        // public abstract void numOfLayers();
        public abstract Layer Combine(Layer other);
        public double ModifyThicknss(int ths) { return thickness = thickness - (ths*thickness)/100; }
        //public bool PerishOneGas() {
        //    return thickness < 0.5; 
        //    }

        public  Layer(string str, double ths) {
            Name = str;
            thickness = ths; }

  
        public abstract Layer Traverse(Variable var);
       
    }
    class Oxygen : Layer
    {
        public Oxygen(string str, double ths) : base(str, ths)
        { }

        public override Layer Traverse(Variable var)
        {
            return var.ChangeO(this);
        }
        public override Layer Combine(Layer other)
        {
            double combinedThickness = this.getTHS() + other.getTHS();
            return new Oxygen("Oxygen", combinedThickness);
        }
    }
    class Ozone : Layer
    {
        public Ozone(string str, double ths) : base(str, ths) {        }
       public override Layer Traverse(Variable var) {
            return var.ChangeOz(this);
        }
        public override Layer Combine(Layer other)
        {
            double combinedThickness = this.getTHS() + other.getTHS();
            return new Ozone("Ozone", combinedThickness);
        }

    }
    class CarbonD : Layer
    {
        public CarbonD(string str, double ths) : base(str, ths)
        {
        }
        public  override Layer Traverse(Variable var) {
            return var.ChangeCB(this);
        }

        public override Layer Combine(Layer other)
        {
            double combinedThickness = this.getTHS() + other.getTHS();
            return new CarbonD("CarbonD", combinedThickness);
        }
    }
}
