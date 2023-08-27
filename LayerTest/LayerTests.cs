using System.Reflection.Emit;
using ass2;

using Newtonsoft.Json.Linq;

using static ass2.Variable;



namespace LayerTest
{

    [TestClass]

    public class LayerTests

    {

        [TestMethod]

        public void LengthBased()
        {
            //0 layers throw exception in case the file has 0 layers
            List<Layer> layers = new List<Layer>();

            Variable v = Thund.Instance();
            try
            {
                v.Simulation(ref layers);
                Assert.Fail();
            }
            catch (Exception c)
            {
                Assert.IsTrue(c is Variable.NoLayerException);
            }


            //one variable changes one layer

            layers.Add(new Oxygen("ox1", 3));

            v.Simulation(ref layers);


            Assert.AreEqual(1.5, layers[0].getTHS());



            //one variable changes more layers.

            List<Layer> layers2 = new List<Layer>();


            layers2.Add(new Ozone("ozone", 100));
            layers2.Add(new Oxygen("ox", 100));
            layers2.Add(new CarbonD("cb", 100));


            Variable v1 = Thund.Instance();
            v1.Simulation(ref layers2);
            Assert.AreEqual(150, layers2[0].getTHS());
            Assert.AreEqual(50, layers2[1].getTHS()); //thunderstorm changes only oxygen and turns it into ozone
            Assert.AreEqual(100, layers2[2].getTHS());


        }

        [TestMethod]
        public void FirstLast()
        {
            //first and last layer of the layers changes properly depending on the types of variables

            //other 

            List<Layer> layers3 = new List<Layer>();
            layers3.Add(new Oxygen("Oxygen", 100));
            layers3.Add(new Ozone("Ozone", 50));
            Variable v1 = Thund.Instance();
            Variable v2 = Sunshine.Instance();
            Variable v3 = Other.Instance();

            v3.Simulation(ref layers3);
     
            //other should change ozone and oxygen
            Assert.AreEqual(92.5, layers3[0].getTHS());//first
            int last = layers3.Count - 1;
            Assert.AreEqual(47.5, layers3[last].getTHS());//last

            ///thund


            //thunderstorm does not change ozone and carbon
            List<Layer> layers4 = new List<Layer>();
            layers4.Add(new CarbonD("Carbon", 9));
            layers4.Add(new Ozone("Ozone", 100));
     

            v1.Simulation (ref  layers4);
            Assert.AreEqual(9, layers4[0].getTHS()); //first
            Assert.AreEqual(100, layers4[1].getTHS()); //last
           
            
            //sunshine
            v2.Simulation(ref layers4);  // v2= sunshine changes oxyegn and carbon
            //Carbon 9, Ozone 100
            //Carbon 8.55,   Ozone 100
            Assert.AreEqual(8.55, layers4[0].getTHS()); //first
            Assert.AreEqual(100, layers4[1].getTHS()); //last


    

        }

        [TestMethod]

        public void SimulationExam()
        {
            //1.initial layer

            //a.merges into the identical layer
            List<Layer> layers = new List<Layer>();
            layers.Add(new CarbonD("Carbon", 1)); // initial carbon layer comes here 
            layers.Add(new Oxygen("Oxi", 1)); //5% turns to ozone, perishes
            layers.Add(new Ozone("Ozone", 10)); // no changes
            layers.Add(new CarbonD("Carbon", 0.5)); // 5% produces oxygen, the rest goes up to carbon


            Variable v = Sunshine.Instance();
            v.Simulation(ref layers);
            for (int i = 0; i < layers.Count; i++)
            {
                Console.WriteLine(layers[i].Name);
                Console.WriteLine(layers[i].getTHS());
            }

            Assert.AreEqual(1.424, layers[0].getTHS(),0.1 ); // it did merge


            //b.becomes a new layer


            List<Layer> layer0 = new List<Layer>();
            layer0.Add(new Oxygen("oxygen", 22));
            Variable v3 = Thund.Instance();
            v3.Simulation(ref layer0);

            Assert.AreEqual(2, layer0.Count);


            //c.perishes
            List<Layer> layers1 = new List<Layer>(); 
            layers1.Add(new Oxygen("Oxi", 1)); //
            layers1.Add(new Ozone("Ozone", 10)); // 
            layers1.Add(new CarbonD("Carbon", 0.5)); // sunshine changes carbon and turns some to oxygen, the carbon perishes
            Variable v1 = Sunshine.Instance();
            v1.Simulation(ref layers1);
       

            Assert.IsFalse(layers1.Contains(layers1.OfType<CarbonD>().FirstOrDefault())); //false cause it doesnt contain anymore


            ///////////////////////////////////////////////////////////////////////////////////////////////
            
            //2.newly generated layer

            //a.merges into the identical layer
            List<Layer> layers2 = new List<Layer>();
            layers2.Add(new Oxygen("Oxygen", 1)); // must be 1-10% + 0.25 
       
            layers2.Add(new Ozone("Ozone", 5)); // 5% produces oxygen -> 0,25 should go to oxygen

            Variable vb= Other.Instance();
            vb.Simulation(ref layers2);
            Assert.AreEqual(1.15, layers2[0].getTHS());



            //b.becomes a new layer

            List<Layer> layers3 = new List<Layer>();
            layers3.Add(new Oxygen("Oxygen", 5));
            Variable v4 = Thund.Instance();
            v4.Simulation(ref layers3);

            
            Assert.AreEqual(2, layers3.Count);

            //c.perishes
            List<Layer> layers5 = new List<Layer>();
            layers5.Add(new CarbonD("Carbon", 4));
            Variable v5 = Sunshine.Instance();
            v5.Simulation(ref layers5);

            Assert.AreEqual(1, layers5.Count);  // didnt set a new layer


        }
    }

}
