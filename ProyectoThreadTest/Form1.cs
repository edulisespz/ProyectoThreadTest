using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ProyectoThreadTest
{
    public partial class Form1 : Form
    {


        private Object Lock = new Object();
        public int LabelCount = -1; // variable principal del procesador
        int NumOfProces=10;

        bool ResetFlag=false;

        bool SRTbussy = false, PRIbussy = false;

        int count20 = 0;

        int DeadPrograms = 0;


        Thread newThread;

        List<Proceso> List_Proceso;

        Proceso Srtproces = null, PrirityProces = null;
        


        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           this.newThread = new Thread( new ThreadStart (this.AMethod_Label));


           List_Proceso = new List<Proceso>();
           llenarLista();
        }

        

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
            this.newThread.Start();


        }

        private void AMethod_Label()
        {
            
            while (true)
            {
                if (InvokeRequired)
                {
                    try
                    {
                        
                        Thread.Sleep(1000);
                        LabelCount++;
                        lock(Lock)
                        {
                        Invoke(new Action(() => labelClock.Text = "Step: " + LabelCount.ToString() ));
                        }
                        
                    }
                    catch(ThreadInterruptedException ex)
                    {

                    }
                    
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //newThread.Suspend();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
              
                
                newThread.Interrupt();
                newThread.Abort();
            }
            catch(Exception ex)
            {

            }
            
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            procesamiento();
        }


        private void procesamiento() //procesador
        {
            

            int PosShortestTime;

            int PosPriority;

            
            
            //textBox1.Text = LabelCount.ToString();

            count20++; //counter every 20 steps
            if (count20 == 20)
            {
                ResetFlag = true;
            }

            if (List_Proceso.Count == 0)
            {
                ResetFlag = true;
            }

            if(ResetFlag){ // se resetea a los 20
                ResetFlag = false;

                DeadPrograms += List_Proceso.Count;

                List_Proceso = new List<Proceso>();

                llenarLista();

                count20 = 0;
            }

            ///// First Procesor Srtproces  ////////

            if (SRTbussy == false)//nuevo proceso "SRT proceser"
            {
                PosShortestTime = FindShortestTime();


                Srtproces = new Proceso( List_Proceso.ElementAt(PosShortestTime) );

                List_Proceso.RemoveAt(PosShortestTime);

                textBoxSRT_execution.Text = Srtproces.TiempoRestante.ToString();
                SRTbussy = true;

                

            }
            else//the porcess continues
            {
                textBoxSRT_execution.Text = "Tiempo restante " + Srtproces.TiempoRestante.ToString() + Environment.NewLine +
                                            "Tipo_Moneda " + Srtproces.TipoMoneda + Environment.NewLine +
                                            "idMoneda" + Srtproces.IDproces.ToString();
                                            

                Srtproces.TiempoRestante--;

                if (Srtproces.TiempoRestante == 0)
                {
                    SRTbussy = false;
                }
            }


            /// End First Procesor Srtproces /////
            
            /// Second Procesor Priority ///

            if (PRIbussy == false)//nuevo proceso "priority proceser"
            {
                PosPriority = FindPriority();


                PrirityProces = new Proceso(List_Proceso.ElementAt(PosPriority));

                List_Proceso.RemoveAt(PosPriority);

                textBoxPriority_Execution.Text = Srtproces.TiempoRestante.ToString();
                PRIbussy = true;



            }
            else//the porcess continues
            {
                textBoxPriority_Execution.Text = "Tiempo restante " + PrirityProces.TiempoRestante.ToString() + Environment.NewLine +
                                            "Tipo_Moneda " + PrirityProces.TipoMoneda + Environment.NewLine +
                                            "idMoneda" + PrirityProces.IDproces.ToString();


                PrirityProces.TiempoRestante--;

                if (PrirityProces.TiempoRestante == 0)
                {
                    PRIbussy = false;
                }
            }



            ////// end second procesor



            



            ///// Drawing   /////

            if (List_Proceso.Count >= 1) 
                textBox1.Text = List_Proceso.ElementAt(0).TipoMoneda; 
            else textBox1.Text = "Vacia";

            if (List_Proceso.Count >= 2)
                textBox2.Text = List_Proceso.ElementAt(1).TipoMoneda;
            else textBox2.Text = "Vacia";

            if (List_Proceso.Count >= 3) 
                textBox3.Text = List_Proceso.ElementAt(2).TipoMoneda;
            else textBox3.Text = "Vacia";

            if (List_Proceso.Count >= 4)
                textBox4.Text = List_Proceso.ElementAt(3).TipoMoneda;
            else textBox4.Text = "Vacia";

            if (List_Proceso.Count >= 5)
                textBox5.Text = List_Proceso.ElementAt(4).TipoMoneda;
            else textBox5.Text = "Vacia";

            if (List_Proceso.Count >= 6)
                textBox6.Text = List_Proceso.ElementAt(5).TipoMoneda;
            else textBox6.Text = "Vacia";

            if (List_Proceso.Count >= 7)
                textBox7.Text = List_Proceso.ElementAt(6).TipoMoneda;
            else textBox7.Text = "Vacia";

            if (List_Proceso.Count >= 8)
                textBox8.Text = List_Proceso.ElementAt(7).TipoMoneda;
            else textBox8.Text = "Vacia";

            if (List_Proceso.Count >= 9)
                textBox9.Text = List_Proceso.ElementAt(8).TipoMoneda;
            else textBox9.Text = "Vacia";

            if (List_Proceso.Count >= 10)
                textBox10.Text = List_Proceso.ElementAt(9).TipoMoneda;
            else textBox10.Text = "Vacia";

            labelDeadPrograms.Text = "Procesos Muertos "+ DeadPrograms.ToString();

            //// end Drawing   /////

        }


        
        private void llenarLista()
        {
            int i;
            int Tipo;
            Proceso temp;

            Random rand = new Random();

            for (i = 1; i <= NumOfProces; i++)
            {//aqui relleno monedas



                
                Tipo = rand.Next(1, 10);

                temp = new Proceso();

                temp.IDproces = i;
                temp.TiempoServico = rand.Next(5, 10); // This change the Service time
                temp.prioridad = rand.Next(0, 3);
                temp.TiempoRestante = temp.TiempoServico;


                switch (Tipo)
                {
                    case 1:
                        //cola.push_back("peso");
                        //falta ver lo del objeto
                        temp.TipoMoneda = "peso";
                        break;
                    case 2:
                        //cola.push_back("U.S. Dollar");
                        temp.TipoMoneda = "U.S. Dollar";
                        break;
                    case 3:
                        //cola.push_back("Euro");
                        temp.TipoMoneda = "Euro";
                        break;
                    case 4:
                        //cola.push_back("Yen");
                        temp.TipoMoneda = "Yen";
                        break;
                    case 5:
                        //cola.push_back("British Pound");
                        temp.TipoMoneda = "British Pound";
                        break;
                    case 6:
                        //cola.push_back("Swiss Franc");
                        temp.TipoMoneda = "Swiss Franc";
                        break;
                    case 7:
                        //cola.push_back("Canadian Dollar");
                        temp.TipoMoneda = "Canadian Dollar";
                        break;
                    case 8:
                        //cola.push_back("New Zealand Dollar");
                        temp.TipoMoneda = "New Zealand Dollar";
                        break;
                    case 9:
                        //cola.push_back("African Rand ");
                        temp.TipoMoneda = "African Rand";
                        break;
                    case 10:
                        //cola.push_back("Indian rupee");
                        temp.TipoMoneda = "Indian rupee";
                        break;
                }//end switch
                

                //add to the list
                List_Proceso.Add(temp);


            }//end for
        }


        private int FindShortestTime()
        {
            /**return the interger position of the List_Proceso*/
            int i=-1,result;
            int menorTiempo = 0;
            bool foundit=false;

            while (foundit == false)
            {
                
                for (i = 0; i < List_Proceso.Count; i++)
                {
                    if (foundit)
                    {
                        break;
                    }
                    else
                    {
                        if (menorTiempo == List_Proceso.ElementAt(i).TiempoServico)
                        {
                            foundit = true;
                        }
                    }

                }//end For

                menorTiempo++;
            }

                return i-1;
        }


        private int FindPriority()
        {
            /**return the interger position of the List_Proceso*/
            int i = -1, result;
            int prioridad = 0;
            bool foundit = false;

            while (foundit == false)
            {

                for (i = 0; i < List_Proceso.Count; i++)
                {
                    if (foundit)
                    {
                        break;
                    }
                    else
                    {
                        if (prioridad == List_Proceso.ElementAt(i).prioridad)
                        {
                            foundit = true;
                        }
                    }

                }//end For

                prioridad++;
            }

            return i - 1;
        }






        
        
    }
}
