using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Management;

namespace DACBUS_Tester
{
    public partial class Form1 : Form  //main
    {

        private static string userName = Environment.UserName;

        //private static Initialize_Card_Communication initializeComms = new Initialize_Card_Communication();

        private static bool cardLoaded = false;

        private static string portName = "";

        private static string cardName = "";

        private static string incomingData = "";

        private static char[] addressDataWord = { };

        private static char[] cPinAddress = { };

        private static char[] dataPinAddress = { };

        private static string homeComputerPathASM = @"C:\Users\stdnt\Desktop\Local Work Folder\Test Fixture Data Manager\C# code\Arduino Serial Monitor\Arduino Serial Monitor\bin\Debug\Arduino Serial Monitor.exe";
        private static string workComputerPathASM = @"Z:\Hardware_Engineering\E4-B\Reference_Library\Devices_Projects\Adaptors\Card Test Fixture Data Transfer Adaptor\Programming\Serial Monitor\Arduino Serial Monitor C# Program\bin\Debug\net7.0-windows\DACBUS Alternative.exe";

        private static string homeComputerPathDBT = @"C:\Users\stdnt\Desktop\Local Work Folder\Test Fixture Data Manager\C# code\DACBUS Tester\DACBUS Tester\bin\Debug\DACBUS Tester.exe";
        private static string workComputerPathDBT = @"Z:\Hardware_Engineering\E4-B\Reference_Library\Devices_Projects\Adaptors\Card Test Fixture Data Transfer Adaptor\Programming\DACBUS_Tester\bin\Debug\net7.0-windows\DACBUS-Tester.exe";

        private static string cardTypePathatWork = @"Z:\Hardware_Engineering\E4-B\Reference_Library\Card Types\";
        private static string cardTypePathatHome = @"C:\Users\stdnt\Desktop\Local Work Folder\Test Fixture Data Manager\C# code\DACBUS Tester\Card Types\";

        private static string cardPicPathatWork = @"Z:\Hardware_Engineering\E4-B\Reference_Library\Card Types\Z_Other Equipment and Details\Card Photos\";
        private static string cardPicPathatHome = @"C:\Users\stdnt\Desktop\Local Work Folder\Test Fixture Data Manager\C# code\DACBUS Tester\Card Photos\";

        private static List<string> cardPhotoList = new List<string>();







        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400,600);
            backgroundWorker1.RunWorkerAsync();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.MaximumSize = new Size(0, 0);
            this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            timer1.Start();
            this.CenterToScreen();
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;

            //populate cardtype combobox depending on which computer is plugged in

            if (userName == "stdnt")
            {
                string cardTypePath = cardTypePathatHome;
                string[] dirs = Directory.GetDirectories(cardTypePath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    string nameOnly = Path.GetFileName(dir);
                    comboBox1.Items.Add(nameOnly);
                }
            }
            else
            {
                string cardTypePath = cardTypePathatWork;
                string[] dirs = Directory.GetDirectories(cardTypePath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    string nameOnly = Path.GetFileName(dir);
                    comboBox1.Items.Add(nameOnly);
                }
            }

            String[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                string portInfo = GetPortInformation();
                if (portInfo.Contains(port))
                {
                    portName = port;
                }
            }

            if (ConnectToPort(portName))
            {

                richTextBox1.Text = "     Arduino Connected";
                richTextBox1.BackColor = Color.Lime;
                richTextBox1.ForeColor = Color.DarkBlue;
                this.richTextBox1.Size = new Size(200, 27);
                this.richTextBox1.Location = new System.Drawing.Point(900, 32);
            }
            else
            {
                richTextBox1.Text = " Arduino Not Connected";
                richTextBox1.BackColor = Color.Red;
                richTextBox1.ForeColor = Color.PaleGoldenrod;
                this.richTextBox1.Size = new Size(200, 27);
                this.richTextBox1.Location = new System.Drawing.Point(900, 32);
                
            }

            button2.Enabled = true;
        }

        //buttons

        private void button2_Click(object sender, EventArgs e)//Unselect Card
        {
            comboBox1.Enabled = true;
            pictureBox1.Image = null;
            cardLoaded = false;
            this.Text = "DACBUS Tester";
            if (userName == "stdnt")
            {
                Process.Start(homeComputerPathDBT);

            }
            else
            {
                Process.Start(workComputerPathDBT);

            }
            this.Dispose();

            label8.Text = "A0 = ";
            label9.Text = "A1 = ";
            label10.Text = "A2 = ";
            label11.Text = "A3 = ";
            label12.Text = "A4 = ";
            label13.Text = "A5 = ";
            label14.Text = "A6 = ";
            label15.Text = "A7 = ";
            label16.Text = "A8 = ";
            label17.Text = "A9 = ";
            label18.Text = "A10 = ";
            label19.Text = "A11 = ";
            label20.Text = "A12 = ";
            label21.Text = "A13 = ";
            label22.Text = "A14 = ";
            label23.Text = "A15 = ";

            label39.Text = "Co0 = ";
            label38.Text = "Cp1 = ";
            label37.Text = "Cp2 = ";
            label36.Text = "Cp3 = ";
            label35.Text = "Cp4 = ";
            label34.Text = "Cp5 = ";
            label33.Text = "Cp6 = ";
            label32.Text = "Cp7 = ";
            label31.Text = "Cp8 = ";
            label30.Text = "Cp9 = ";
            label29.Text = "Cp10 = ";
            label28.Text = "Cp11 = ";
            label27.Text = "Cp12 = ";
            label26.Text = "Cp13 = ";
            label25.Text = "Cp14 = ";
            label24.Text = "Cp15 = ";

        }

        private void button3_Click(object sender, EventArgs e)//Load
        {

            addressDataWord = textBox1.Text.ToCharArray();
            label8.Text = "A0 = " + addressDataWord[0].ToString();
            label9.Text = "A1 = " + addressDataWord[1].ToString();
            label10.Text = "A2 = " + addressDataWord[2].ToString();
            label11.Text = "A3 = " + addressDataWord[3].ToString();
            label12.Text = "A4 = " + addressDataWord[4].ToString();
            label13.Text = "A5 = " + addressDataWord[5].ToString();
            label14.Text = "A6 = " + addressDataWord[6].ToString();
            label15.Text = "A7 = " + addressDataWord[7].ToString();
            label16.Text = "A8 = " + addressDataWord[8].ToString();
            label17.Text = "A9 = " + addressDataWord[9].ToString();
            label18.Text = "A10 = " + addressDataWord[10].ToString();
            label19.Text = "A11 = " + addressDataWord[11].ToString();
            label20.Text = "A12 = " + addressDataWord[12].ToString();
            label21.Text = "A13 = " + addressDataWord[13].ToString();
            label22.Text = "A14 = " + addressDataWord[14].ToString();
            label23.Text = "A15 = " + addressDataWord[15].ToString();

            if (!cardLoaded)
            {
                MessageBox.Show("Please select a card type before loading data.", "You must select a card first.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (addressDataWord.Count() > 16)
            {
                int tooMany = addressDataWord.Length - 16;
                MessageBox.Show("You have " + tooMany.ToString() + " digits too many. The card address can only be 16 digits in length (2 bytes). Please provide 2 bytes of data.", "Too Many Digits in Address word", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (addressDataWord.Count() < 16)
            {
                int tooLittle = 16 - addressDataWord.Count();
                MessageBox.Show("You are missing " + tooLittle.ToString() + " digits. The card address can only be 16 digits in length (2 bytes). Please provide 2 bytes of data.", "Not Enough Digits in Address word", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cardName = this.Text;

            ChannelSelectionMethod(cardName, addressDataWord);

            //load C pins into the address labels
            cPinAddress = textBox2.Text.ToCharArray();
            //load data pins
            dataPinAddress = textBox3.Text.ToCharArray();

            if (cPinAddress.Length > 16)
            {
                int tooMany = cPinAddress.Length - 16;
                MessageBox.Show("You have " + tooMany.ToString() + " digits too many. The C pin address must be 16 digits in length (2 bytes). Please provide 2 bytes of data.", "Too Many Digits in C pin address word", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cPinAddress.Length < 16)
            {
                int tooLittle = 16 - cPinAddress.Length;
                MessageBox.Show("You are missing " + tooLittle.ToString() + " digits. The C pin address must be 16 digits in length (2 bytes). Please provide 2 bytes of data.", "Not Enough Digits in C pin address word", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dataPinAddress.Length < 16)
            {
                int tooLittle = 16 - dataPinAddress.Length;
                MessageBox.Show("You are missing " + tooLittle.ToString() + " digits. The data pin address must be at least 16 digits in length (2 bytes). Please provide 2 bytes of data.", "Not Enough Digits in data pin address word", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //load cp pins
            label39.Text = "Co0 = " + cPinAddress[0];
            label38.Text = "Cp1 = " + cPinAddress[1];
            label37.Text = "Cp2 = " + cPinAddress[2];
            label36.Text = "Cp3 = " + cPinAddress[3];
            label35.Text = "Cp4 = " + cPinAddress[4];
            label34.Text = "Cp5 = " + cPinAddress[5];
            label33.Text = "Cp6 = " + cPinAddress[6];
            label32.Text = "Cp7 = " + cPinAddress[7];
            label31.Text = "Cp8 = " + cPinAddress[8];
            label30.Text = "Cp9 = " + cPinAddress[9];
            label29.Text = "Cp10 = " + cPinAddress[10];
            label28.Text = "Cp11 = " + cPinAddress[11];
            label27.Text = "Cp12 = " + cPinAddress[12];
            label26.Text = "Cp13 = " + cPinAddress[13];
            label25.Text = "Cp14 = " + cPinAddress[14];
            label24.Text = "Cp15 = " + cPinAddress[15];


            

            label43.Text = "D00 = " + dataPinAddress[0];
            label44.Text = "D01 = " + dataPinAddress[1];
            label45.Text = "D02 = " + dataPinAddress[2];
            label46.Text = "D03 = " + dataPinAddress[3];
            label47.Text = "D04 = " + dataPinAddress[4];
            label48.Text = "D05 = " + dataPinAddress[5];
            label49.Text = "D06 = " + dataPinAddress[6];
            label50.Text = "D07 = " + dataPinAddress[7];
            label51.Text = "D08 = " + dataPinAddress[8];
            label52.Text = "D09 = " + dataPinAddress[9];
            label53.Text = "D10 = " + dataPinAddress[10];
            label54.Text = "D11 = " + dataPinAddress[11];
            label55.Text = "D12 = " + dataPinAddress[12];
            label56.Text = "D13 = " + dataPinAddress[13];
            label57.Text = "D14 = " + dataPinAddress[14];
            label58.Text = "D15 = " + dataPinAddress[15];

            if (dataPinAddress.Length > 16)
            {
                label59.Text = "D16 = " + dataPinAddress[16];
                label60.Text = "D17 = " + dataPinAddress[17];
                label61.Text = "D18 = " + dataPinAddress[18];
                label62.Text = "D19 = " + dataPinAddress[19];
                label63.Text = "D20 = " + dataPinAddress[20];
                label64.Text = "D21 = " + dataPinAddress[21];
                label65.Text = "D22 = " + dataPinAddress[22];
                label66.Text = "D23 = " + dataPinAddress[23];
                label67.Text = "D24 = " + dataPinAddress[24];
                label68.Text = "D25 = " + dataPinAddress[25];
                label69.Text = "D26 = " + dataPinAddress[26];
                label70.Text = "D27 = " + dataPinAddress[27];
                label71.Text = "D28 = " + dataPinAddress[28];
                label72.Text = "D29 = " + dataPinAddress[29];
                label73.Text = "D30 = " + dataPinAddress[30];
                label74.Text = "D31 = " + dataPinAddress[31];
            }

            

        }

        private void button4_Click(object sender, EventArgs e)//Send data to arduino for processing
        {
            string address = textBox1.Text;
            //string cpPins = textBox2.Text;
            //string dataPins = textBox3.Text;

            //string entireDataWord = address + cpPins + dataPins;      


            serialPort1.Write(address);
        }




        //Selection box
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)//Select card
        {
            //select comboxbox selection

            string cardType = (string)comboBox1.SelectedItem;
            comboBox1.Enabled = false;
            string[] array1 = { };

            //get card photo list
            if (userName == "stdnt")
            {
                string cardTypePath = cardTypePathatHome;
                string[] dirs = Directory.GetDirectories(cardTypePath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    cardPhotoList = Directory.GetFiles(cardPicPathatHome).Select(f => Path.GetFileName(f)).ToList();

                }
            }
            if (!userName.Contains("stdnt"))
            {
                string cardTypePath = cardTypePathatWork;
                string[] dirs = Directory.GetDirectories(cardTypePath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

                }
            }


            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains(cardType))
                {
                    array1.Append(cardPhoto);
                }
            }
            //check for no picture available and present that in the pictureBox
            if (array1.Length < 1 && !userName.Contains("stdnt"))
            {

                pictureBox1.Load(cardPicPathatWork + "NoPicAvail.jpg");

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                cardLoaded = false;
                comboBox1.Enabled = true;
            }

            if (array1.Length < 1 && userName.Contains("stdnt"))
            {

                pictureBox1.Load(cardPicPathatHome + "NoPicAvail.jpg");

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                cardLoaded = false;
                comboBox1.Enabled = true;
            }

            if (cardType == "TEST")
            {
                this.Text = "DACBUS Tester - TEST Card Loaded";

                pictureBox1.Load(@"C:\Users\stdnt\Desktop\Local Work Folder\Test Fixture Data Manager\C# code\DACBUS Tester\Card Photos\test.jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                cardLoaded = true;
            }

            if (cardType.Contains("86732"))//SOP cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_SOP_Configuration();
            }
            if (cardType.Contains("95920"))//ACCELEROMETER cards (ACC)
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_ACC_Configuration();
            }
            if (cardType.Contains("86712"))//ADC cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.AOP_Card();
                Implement_ADC_Configuration();
            }
            if (cardType.Contains("86716"))//AIP cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_AIP_Configuration();
            }
            if (cardType.Contains("86748"))//AIR
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_AIR_Configuration();
            }
            if (cardType.Contains("86720"))//AOP cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.AOP_Card();
                Implement_AOP_Configuration();
            }
            if (cardType.Contains("93477"))//COMNAV cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_COMNAV_Configuration();
            }
            if (cardType.Contains("96075"))//Control Loading Safety Cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_CLS_Configuration();
            }
            if (cardType.Contains("86724"))//DGN cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.AOP_Card();
                Implement_DGN_Configuration();
            }
            if (cardType.Contains("89172"))//DIO cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_DIO_Configuration();
            }
            if (cardType.Contains("86700"))//DIP cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_DIP_Configuration();
            }
            if (cardType.Contains("86708") || cardType.Contains("86709"))//DOR cards
            {
                cardLoaded = true;
                comboBox1.Enabled = false;
                //initializeComms.SOP_Card();
                Implement_DOR_Configuration();
            }


        }

        //textBoxes

        private void textBox1_KeyDown(object sender, KeyEventArgs e)//Address bus
        {
            if (e.KeyCode == Keys.Enter)
            {
                addressDataWord = textBox1.Text.ToCharArray();
                if (addressDataWord.Length < 16 || addressDataWord.Length > 16)
                {
                    MessageBox.Show("The Address data word must be 16 bits. Please provide a 16-bit word.", "Address Data Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                label8.Text = "A0 = " + addressDataWord[0].ToString();
                label9.Text = "A1 = " + addressDataWord[1].ToString();
                label10.Text = "A2 = " + addressDataWord[2].ToString();
                label11.Text = "A3 = " + addressDataWord[3].ToString();
                label12.Text = "A4 = " + addressDataWord[4].ToString();
                label13.Text = "A5 = " + addressDataWord[5].ToString();
                label14.Text = "A6 = " + addressDataWord[6].ToString();
                label15.Text = "A7 = " + addressDataWord[7].ToString();
                label16.Text = "A8 = " + addressDataWord[8].ToString();
                label17.Text = "A9 = " + addressDataWord[9].ToString();
                label18.Text = "A10 = " + addressDataWord[10].ToString();
                label19.Text = "A11 = " + addressDataWord[11].ToString();
                label20.Text = "A12 = " + addressDataWord[12].ToString();
                label21.Text = "A13 = " + addressDataWord[13].ToString();
                label22.Text = "A14 = " + addressDataWord[14].ToString();
                label23.Text = "A15 = " + addressDataWord[15].ToString();

                cardName = this.Text;

                ChannelSelectionMethod(cardName, addressDataWord);




            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)//CP Address bus
        {
            if (e.KeyCode == Keys.Enter)
            {
                //load C pins into the address labels
                cPinAddress = textBox2.Text.ToCharArray();
                if (cPinAddress.Length < 16 || cPinAddress.Length > 16)
                {
                    MessageBox.Show("The CP data word must be 16 bits. Please provide a 16-bit word.", "CP Data Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                label24.Text = "CP15 = " + cPinAddress[15].ToString();
                label25.Text = "Cp14 = " + cPinAddress[14].ToString();
                label26.Text = "Cp13 = " + cPinAddress[13].ToString();
                label27.Text = "Cp12 = " + cPinAddress[12].ToString();
                label28.Text = "Cp11 = " + cPinAddress[11].ToString();
                label29.Text = "Cp10 = " + cPinAddress[10].ToString();
                label30.Text = "Cp09 = " + cPinAddress[9].ToString();
                label31.Text = "Cp08 = " + cPinAddress[8].ToString();
                label32.Text = "Cp07 = " + cPinAddress[7].ToString();
                label33.Text = "Cp06 = " + cPinAddress[6].ToString();
                label34.Text = "Cp05 = " + cPinAddress[5].ToString();
                label35.Text = "Cp04 = " + cPinAddress[4].ToString();
                label36.Text = "Cp03 = " + cPinAddress[3].ToString();
                label37.Text = "Cp02 = " + cPinAddress[2].ToString();
                label38.Text = "Cp01 = " + cPinAddress[1].ToString();
                label39.Text = "Cp00 = " + cPinAddress[0].ToString();

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)//input data bus
        {
            if (e.KeyCode == Keys.Enter)
            {
                char[] inputDataWord = textBox3.Text.ToCharArray();
                if (inputDataWord.Length < 12 || inputDataWord.Length > 12)
                {
                    MessageBox.Show("The input data word must be 12 bits. Please provide a 12-bit word.", "Input Data Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                label43.Text = "D00 = " + inputDataWord[0].ToString();
                label44.Text = "D01 = " + inputDataWord[1].ToString();
                label45.Text = "D02 = " + inputDataWord[2].ToString();
                label46.Text = "D03 = " + inputDataWord[3].ToString();
                label47.Text = "D04 = " + inputDataWord[4].ToString();
                label48.Text = "D05 = " + inputDataWord[5].ToString();
                label49.Text = "D06 = " + inputDataWord[6].ToString();
                label50.Text = "D07 = " + inputDataWord[7].ToString();
                label51.Text = "D08 = " + inputDataWord[8].ToString();
                label52.Text = "D09 = " + inputDataWord[9].ToString();
                label53.Text = "D10 = " + inputDataWord[10].ToString();
                label54.Text = "D11 = " + inputDataWord[11].ToString();

            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)//DA
        {
            if (e.KeyCode == Keys.Enter)
            {
                char[] DAword = textBox4.Text.ToCharArray();
                //MessageBox.Show("\""+DAword[0].ToString()+"\"");
                //if (Parse(DAword[0].ToString()) != 1 || DAword[0].ToString() != 0)
                //{
                //    MessageBox.Show("The DA bit can only be a one or a zero.", "DA Bit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{

                //}
                label146.Text = "DA = " + DAword[0].ToString();

            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)//DR
        {
            if (e.KeyCode == Keys.Enter)
            {
                char[] DRword = textBox5.Text.ToCharArray();
                //MessageBox.Show("\""+DAword[0].ToString()+"\"");
                //if (Parse(DAword[0].ToString()) != 1 || DAword[0].ToString() != 0)
                //{
                //    MessageBox.Show("The DA bit can only be a one or a zero.", "DA Bit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{

                //}
                label147.Text = "DR = " + DRword[0].ToString();

            }

        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)//CLR
        {
            if (e.KeyCode == Keys.Enter)
            {
                char[] CLRword = textBox6.Text.ToCharArray();
                //MessageBox.Show("\""+DAword[0].ToString()+"\"");
                //if (Parse(DAword[0].ToString()) != 1 || DAword[0].ToString() != 0)
                //{
                //    MessageBox.Show("The DA bit can only be a one or a zero.", "DA Bit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{

                //}
                label150.Text = "CLR = " + CLRword[0].ToString();

            }

        }





        //Serial Port
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(serialPort1_DataReceived));
        }

        public void serialPort1_DataReceived(object sender, EventArgs e)
        {
            incomingData = serialPort1.ReadExisting().ToString();
            label77.Text = "";
            label77.Text = incomingData.ToString();
            
            //where does the data from the arduino need to go?







        }

        public bool ConnectToPort(string portName)
        {
            try
            {
                if (portName is null)
                {
                    return false;
                }
                //MessageBox.Show(portName);
                serialPort1.PortName = portName;
                serialPort1.BaudRate = 115200;
                serialPort1.RtsEnable = true;// This is required to get the Arduino to TX anything
                try
                {
                    serialPort1.Open();
                }
                catch(UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Error:\r"+ex.ToString(), "UnauthorizedAccessException",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show("Error:\r" + ex.ToString(), "ArgumentOutOfRangeException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Error:\r" + ex.ToString(), "ArgumentException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error:\r" + ex.ToString(), "IOException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Error:\r" + ex.ToString(), "InvalidOperationException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                return true;

            }
            catch (Exception ex)
            {
                if (portName.ToString() == "")
                {
                    return false;
                }
                else
                {
                    MessageBox.Show("Could not connect to " + portName + " because:\r" + ex, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    portName = null;
                    serialPort1.Dispose();

                    ConnectToPort(portName);
                }
            }

            button2.Enabled = false;
            comboBox1.Enabled = true;
            return false;

        }

        public string GetPortInformation()
        {
            ManagementClass processClass = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection Ports = processClass.GetInstances();
            int counter = 0;
            foreach (ManagementObject property in Ports)
            {
                //if (counter == 50)
                //{
                //    return string.Empty;
                //}
                var name = property.GetPropertyValue("Name");
                //MessageBox.Show("\""+name.ToString()+"\"");
                if (name is null)
                {
                    return string.Empty;
                }
                //if (name.ToString().Contains("(COM"))
                //{
                //    MessageBox.Show(name.ToString());
                //    //return name.ToString();
                //}
                if (name.ToString().Contains("Arduino"))
                {
                    //MessageBox.Show(name.ToString());
                    return name.ToString();
                }
                counter += 1;
            }
            return string.Empty;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen)
            {
                return;
            }
            if (!serialPort1.IsOpen)
            {
                int counter = 0;
                portName = null;
                String[] ports = SerialPort.GetPortNames();
                //MessageBox.Show("\"" + ports.ToString() + "\"");

                foreach (string port in ports)
                {
                    //if (counter == 50)
                    //{
                    //    return;
                    //}
                    string portInfo = GetPortInformation();
                    if (portInfo.Contains(port))
                    {
                        portName = port;
                    }
                    counter += 1;
                }

                if (ConnectToPort(portName))
                {

                    richTextBox1.Text = "         Arduino Connected";
                    richTextBox1.BackColor = Color.Lime;
                    richTextBox1.ForeColor = Color.DarkBlue;
                    this.richTextBox1.Size = new Size(200, 27);
                    this.richTextBox1.Location = new System.Drawing.Point(900, 32);
                }
                else
                {
                    richTextBox1.Text = "    Arduino Not Connected";
                    richTextBox1.BackColor = Color.Red;
                    //richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.Font.FontFamily,Convert.ToInt16(nudFontSize.Value), FontStyle.Bold ^ this.richTextBox1.SelectionFont.Style);

                    richTextBox1.ForeColor = Color.PaleGoldenrod;
                    this.richTextBox1.Size = new Size(200, 27);
                    this.richTextBox1.Location = new System.Drawing.Point(900, 32);
                }
            }
        }








        //Menu Strip
        private void prepopulateAllInputPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "1111111111111111";
            textBox2.Text = "1111111111111111";
            textBox3.Text = "11111111111111111111111111111111";
        }


        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort1.Dispose();
            timer1.Stop();
            richTextBox1.Text = " Arduino Not Connected";
            richTextBox1.BackColor = Color.Red;
            richTextBox1.ForeColor = Color.White;
            this.richTextBox1.Size = new Size(125, 22);
            this.richTextBox1.Location = new System.Drawing.Point(1000, 30);

            if (userName == "stdnt")
            {
                Process.Start(homeComputerPathASM);

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("Activate_Arduino_Connection_Tester");
                }

            }
            else
            {
                Process.Start(workComputerPathASM);

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("Activate_Arduino_Connection_Tester");
                }


            }
        }

        private void resumeDACBUSTesterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("Deactivate_Arduino_Connection_Tester");
            }
        }

        private void signalGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }















        //Card type Implementations

        private void Implement_SOP_Configuration()//SOP card
        {
            this.Text = "DACBUS Tester - SOP Card Loaded";

            if (userName == "stdnt")
            {
                //get card photo list
                cardPhotoList = Directory.GetFiles(cardPicPathatHome).Select(f => Path.GetFileName(f)).ToList();

                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86732"))
                    {
                        pictureBox1.Load(cardPicPathatHome + cardPhoto);
                    }
                }
            }
            if (!userName.Contains("stdnt"))
            {
                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86732"))
                    {
                        pictureBox1.Load(cardPicPathatWork + cardPhoto);
                    }
                }

            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;




            //set key

            richTextBox2.Text = "Channel Selection Pins";
            richTextBox2.BackColor = Color.Aquamarine;
            label23.BackColor = Color.Aquamarine;
            label22.BackColor = Color.Aquamarine;

            richTextBox3.Text = "Special Inputs";
            richTextBox3.BackColor = Color.LightSteelBlue;
            label146.BackColor = Color.LightSteelBlue;
            label147.BackColor = Color.LightSteelBlue;
            label150.BackColor = Color.LightSteelBlue;

            richTextBox4.Text = "Special Outputs";
            richTextBox4.BackColor = Color.Coral;
            label42.BackColor = Color.Coral;
            label148.BackColor = Color.Coral;

            richTextBox5.Text = "Sinusoidal Output ~~";
            richTextBox5.BackColor = Color.Goldenrod;
            label79.BackColor = Color.Goldenrod;
            label80.BackColor = Color.Goldenrod;
            label83.BackColor = Color.Goldenrod;
            label84.BackColor = Color.Goldenrod;
            label93.BackColor = Color.Goldenrod;
            label94.BackColor = Color.Goldenrod;
            label97.BackColor = Color.Goldenrod;
            label98.BackColor = Color.Goldenrod;

            richTextBox6.Text = "Currently Selected Output";
            richTextBox6.BackColor = Color.Aqua;

            richTextBox7.BackColor = Color.LightGray;
            richTextBox8.BackColor = Color.LightGray;
            richTextBox9.BackColor = Color.LightGray;
            richTextBox10.BackColor = Color.LightGray;
            richTextBox11.BackColor = Color.LightGray;



            //reduce available pins

            //address pins
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            label12.Enabled = false;
            label13.Enabled = false;
            label14.Enabled = false;

            //cp pins
            label24.Enabled = false;
            label25.Enabled = false;
            label26.Enabled = false;
            label27.Enabled = false;
            label28.Enabled = false;
            label29.Enabled = false;
            label39.Enabled = false;

            //input pins
            label55.Enabled = false;
            label56.Enabled = false;
            label57.Enabled = false;
            label58.Enabled = false;
            label59.Enabled = false;
            label60.Enabled = false;
            label61.Enabled = false;
            label62.Enabled = false;
            label63.Enabled = false;
            label64.Enabled = false;
            label65.Enabled = false;
            label66.Enabled = false;
            label67.Enabled = false;
            label68.Enabled = false;
            label69.Enabled = false;
            label70.Enabled = false;
            label71.Enabled = false;
            label72.Enabled = false;
            label73.Enabled = false;
            label74.Enabled = false;

            //output pins
            label77.Enabled = false;
            label78.Enabled = false;
            label79.Text = "Out02 = SOP0X ~~";
            label80.Text = "Out03 = SOP0X ~~";
            label81.Enabled = false;
            label82.Enabled = false;
            label83.Text = "Out06 = SOP0Y ~~";
            label84.Text = "Out07 = SOP0Y ~~";
            label85.Enabled = false;
            label86.Enabled = false;
            label87.Enabled = false;
            label88.Enabled = false;
            label89.Enabled = false;
            label90.Enabled = false;
            label91.Enabled = false;
            label92.Enabled = false;
            label93.Text = "Out16 = SOP1X ~~";
            label94.Text = "Out17 = SOP1X ~~";
            label95.Enabled = false;
            label96.Enabled = false;
            label97.Text = "Out20 = SOP1Y ~~";
            label98.Text = "Out21 = SOP1Y ~~";
            label99.Enabled = false;
            label100.Enabled = false;
            label101.Enabled = false;
            label102.Enabled = false;
            label103.Enabled = false;
            label104.Enabled = false;
            label105.Enabled = false;
            label106.Enabled = false;
            label107.Enabled = false;
            label108.Enabled = false;
            label109.Enabled = false;
            label110.Enabled = false;
            label111.Enabled = false;
            label112.Enabled = false;
            label113.Enabled = false;
            label114.Enabled = false;
            label115.Enabled = false;
            label116.Enabled = false;
            label117.Enabled = false;
            label118.Enabled = false;
            label119.Enabled = false;
            label120.Enabled = false;
            label121.Enabled = false;
            label122.Enabled = false;
            label123.Enabled = false;
            label124.Enabled = false;
            label125.Enabled = false;
            label126.Enabled = false;
            label127.Enabled = false;
            label128.Enabled = false;
            label129.Enabled = false;
            label130.Enabled = false;
            label131.Enabled = false;
            label132.Enabled = false;
            label133.Enabled = false;
            label134.Enabled = false;
            label135.Enabled = false;
            label136.Enabled = false;
            label137.Enabled = false;
            label138.Enabled = false;
            label139.Enabled = false;
            label140.Enabled = false;

        }

        private void Implement_AOP_Configuration()//AOP card
        {
            this.Text = "DACBUS Tester - AOP Card Loaded";

            //get card photo list

            if (userName == "stdnt")
            {
                //get card photo list
                cardPhotoList = Directory.GetFiles(cardPicPathatHome).Select(f => Path.GetFileName(f)).ToList();

                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86720"))
                    {
                        pictureBox1.Load(cardPicPathatHome + cardPhoto);
                    }
                }
            }
            if (!userName.Contains("stdnt"))
            {
                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86720"))
                    {
                        pictureBox1.Load(cardPicPathatWork + cardPhoto);
                    }
                }

            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //set key

            richTextBox2.Text = "Channel Selection Pins";
            richTextBox2.BackColor = Color.Aquamarine;
            label23.BackColor = Color.Aquamarine;
            label22.BackColor = Color.Aquamarine;
            label21.BackColor = Color.Aquamarine;

            richTextBox3.Text = "Special Inputs";
            richTextBox3.BackColor = Color.LightSteelBlue;
            label146.BackColor = Color.LightSteelBlue;
            label147.BackColor = Color.LightSteelBlue;
            label150.BackColor = Color.LightSteelBlue;

            richTextBox4.Text = "Special Outputs";
            richTextBox4.BackColor = Color.Coral;
            label42.BackColor = Color.Coral;
            label148.BackColor = Color.Coral;
            label8.BackColor = Color.Coral;

            richTextBox5.Text = "Sinusoidal Output ~~";
            richTextBox5.BackColor = Color.Goldenrod;
            label81.BackColor = Color.Goldenrod;
            label82.BackColor = Color.Goldenrod;
            label85.BackColor = Color.Goldenrod;
            label86.BackColor = Color.Goldenrod;
            label93.BackColor = Color.Goldenrod;
            label94.BackColor = Color.Goldenrod;
            label89.BackColor = Color.Goldenrod;
            label90.BackColor = Color.Goldenrod;
            label97.BackColor = Color.Goldenrod;
            label98.BackColor = Color.Goldenrod;
            label101.BackColor = Color.Goldenrod;
            label102.BackColor = Color.Goldenrod;
            label105.BackColor = Color.Goldenrod;
            label106.BackColor = Color.Goldenrod;
            label109.BackColor = Color.Goldenrod;
            label110.BackColor = Color.Goldenrod;

            richTextBox6.BackColor = Color.LightGray;
            richTextBox7.BackColor = Color.LightGray;
            richTextBox8.BackColor = Color.LightGray;
            richTextBox9.BackColor = Color.LightGray;
            richTextBox10.BackColor = Color.LightGray;
            richTextBox11.BackColor = Color.LightGray;

            //reduce available pins

            //address pins on P1
            //label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            label12.Enabled = false;
            label13.Enabled = false;
            label14.Enabled = false;

            //cp pins on P1
            label24.Enabled = false;
            label25.Enabled = false;
            label26.Enabled = false;
            label27.Enabled = false;
            label28.Enabled = false;
            label29.Enabled = false;
            label33.Enabled = false;
            label32.Enabled = false;
            label31.Enabled = false;
            label39.Enabled = false;

            //input pins on P1
            label55.Enabled = false;
            label56.Enabled = false;
            label57.Enabled = false;
            label58.Enabled = false;
            label59.Enabled = false;
            label60.Enabled = false;
            label61.Enabled = false;
            label62.Enabled = false;
            label63.Enabled = false;
            label64.Enabled = false;
            label65.Enabled = false;
            label66.Enabled = false;
            label67.Enabled = false;
            label68.Enabled = false;
            label69.Enabled = false;
            label70.Enabled = false;
            label71.Enabled = false;
            label72.Enabled = false;
            label73.Enabled = false;
            label74.Enabled = false;

            //output pins on P2
            label77.Enabled = false;//p9   D00
            label78.Enabled = false;//p10   D01
            label79.Enabled = false;//p11   D02
            label80.Enabled = false;//p12   D03
            label81.Text = "Out04 = ALOP00 ~~";//p13   D04
            label82.Text = "Out05 = RET00";//p14   D05
            label83.Enabled = false;//p15   D06
            label84.Enabled = false;//p16   D07
            label85.Text = "Out08 = ALOP01 ~~";//p17   D08
            label86.Text = "Out09 = RET01";//p18   D09
            label87.Enabled = false;//p19   D10
            label88.Enabled = false;//p20   D11
            label89.Text = "Out12 = ALOP02 ~~";//p21   D12
            label90.Text = "Out13 = RET02";//p22   D13
            label91.Enabled = false;//p23   D14
            label92.Enabled = false;//p24   D15
            label93.Text = "Out16 = ALOP03 ~~";//p25   D16
            label94.Text = "Out17 = RET03";//p26   D17
            label95.Enabled = false;//p27   D18
            label96.Enabled = false;//p28   D19
            label97.Text = "Out20 = ALOP04 ~~";//p29   D20
            label98.Text = "Out21 = RET04";//p30   D21
            label99.Enabled = false;//p31   D22
            label100.Enabled = false;//p32   D23
            label101.Text = "Out24 = ALOP05 ~~";//p33   D24
            label102.Text = "Out25 = RET05";//p34   D25
            label103.Enabled = false;//p35   D26
            label104.Enabled = false;//p36   D27
            label105.Text = "Out28 = ALOP06 ~~";//p37   D28
            label106.Text = "Out29 = RET06";//p38   D29
            label107.Enabled = false;//p39   D30
            label108.Enabled = false;//p40   D31
            label109.Text = "Out32 = ALOP07 ~~";//p41   D32
            label110.Text = "Out33 = RET07";//p42   D33
            label111.Enabled = false;//p43   D34
            label112.Enabled = false;//p44   D35
            label113.Enabled = false;//p45   D36
            label114.Enabled = false;//p46   D37
            label115.Enabled = false;//p47   D38
            label116.Enabled = false;//p48   D39
            label117.Enabled = false;//p49   D40
            label118.Enabled = false;//p50   D41
            label119.Enabled = false;//p51   D42
            label120.Enabled = false;//p52   D43
            label121.Enabled = false;//p53   D44
            label122.Enabled = false;//p54   D45
            label123.Enabled = false;//p55   D46
            label124.Enabled = false;//p56   D47
            label125.Enabled = false;//p57   D48
            label126.Enabled = false;//p58   D49
            label127.Enabled = false;//p59   D50
            label128.Enabled = false;//p60   D51
            label129.Enabled = false;//p61   D52
            label130.Enabled = false;//p62   D53
            label131.Enabled = false;//p63   D54
            label132.Enabled = false;//p64   D55
            label133.Enabled = false;//p65   D56
            label134.Enabled = false;//p66   D57
            label135.Enabled = false;//p67   D58
            label136.Enabled = false;//p68   D59
            label137.Enabled = false;//p69   D60
            label138.Enabled = false;//p70   D61
            label139.Enabled = false;//p71   D62
            label140.Enabled = false;//p72   D63

        }

        private void Implement_ACC_Configuration()//ACC card
        {
            this.Text = "DACBUS Tester - ACC Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("95920"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_ADC_Configuration()//ADC
        {
            this.Text = "DACBUS Tester - ADC Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("86712"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_AIP_Configuration()//AIP card
        {
            this.Text = "DACBUS Tester - AIP Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("86716"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_AIR_Configuration()//AIR card
        {
            this.Text = "DACBUS Tester - AIR Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("86748"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_COMNAV_Configuration()//COMNAV card
        {
            this.Text = "DACBUS Tester - COMNAV Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("93477"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_CLS_Configuration()//CLS card
        {
            this.Text = "DACBUS Tester - CLS Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("96075"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_DGN_Configuration()//DGN cards
        {
            this.Text = "DACBUS Tester - DGN Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("86724"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_DIO_Configuration()//DIO card
        {
            this.Text = "DACBUS Tester - DIO Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("89172"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_DIP_Configuration()//DIP card
        {
            this.Text = "DACBUS Tester - DIP Card Loaded";

            //get card photo list
            cardPhotoList = Directory.GetFiles(cardPicPathatWork).Select(f => Path.GetFileName(f)).ToList();

            foreach (string cardPhoto in cardPhotoList)
            {
                if (cardPhoto.Contains("86700"))
                {
                    pictureBox1.Load(cardPicPathatWork + cardPhoto);
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Implement_DOR_Configuration()//DOR card
        {
            this.Text = "DACBUS Tester - DOR Card Loaded";

            //get card photo list

            if (userName == "stdnt")
            {
                //get card photo list
                cardPhotoList = Directory.GetFiles(cardPicPathatHome).Select(f => Path.GetFileName(f)).ToList();

                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86708") || cardPhoto.Contains("86709"))
                    {
                        pictureBox1.Load(cardPicPathatHome + cardPhoto);
                    }
                }
            }
            if (!userName.Contains("stdnt"))
            {
                foreach (string cardPhoto in cardPhotoList)
                {
                    if (cardPhoto.Contains("86708") || cardPhoto.Contains("86709"))
                    {
                        pictureBox1.Load(cardPicPathatWork + cardPhoto);
                    }
                }

            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //set key

            richTextBox2.Text = "Channel Selection Pins";
            richTextBox2.BackColor = Color.Aquamarine;
            label23.BackColor = Color.Aquamarine;
            label22.BackColor = Color.Aquamarine;
            label21.BackColor = Color.Aquamarine;

            richTextBox3.Text = "Special Inputs";
            richTextBox3.BackColor = Color.LightSteelBlue;
            label146.BackColor = Color.LightSteelBlue;
            label147.BackColor = Color.LightSteelBlue;
            label150.BackColor = Color.LightSteelBlue;

            richTextBox4.Text = "Special Outputs";
            richTextBox4.BackColor = Color.Coral;
            label42.BackColor = Color.Coral;
            label148.BackColor = Color.Coral;
            label8.BackColor = Color.Coral;

            richTextBox5.Text = "Digital Output _-_-";
            richTextBox5.BackColor = Color.Goldenrod;
            label81.BackColor = Color.Goldenrod;
            label82.BackColor = Color.Goldenrod;
            label83.BackColor = Color.Goldenrod;
            label84.BackColor = Color.Goldenrod;

            label87.BackColor = Color.Goldenrod;
            label88.BackColor = Color.Goldenrod;
            label89.BackColor = Color.Goldenrod;
            label90.BackColor = Color.Goldenrod;

            label93.BackColor = Color.Goldenrod;
            label94.BackColor = Color.Goldenrod;
            label95.BackColor = Color.Goldenrod;
            label96.BackColor = Color.Goldenrod;

            label105.BackColor = Color.Goldenrod;
            label106.BackColor = Color.Goldenrod;
            label109.BackColor = Color.Goldenrod;
            label110.BackColor = Color.Goldenrod;

            richTextBox6.BackColor = Color.LightGray;
            richTextBox7.BackColor = Color.LightGray;
            richTextBox8.BackColor = Color.LightGray;
            richTextBox9.BackColor = Color.LightGray;
            richTextBox10.BackColor = Color.LightGray;
            richTextBox11.BackColor = Color.LightGray;

            //reduce available pins

            //address pins on P1
            //label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            label12.Enabled = false;
            label13.Enabled = false;
            label14.Enabled = false;

            //cp pins on P1
            label24.Enabled = false;
            label25.Enabled = false;
            label26.Enabled = false;
            label27.Enabled = false;
            label28.Enabled = false;
            label29.Enabled = false;
            label33.Enabled = false;
            label32.Enabled = false;
            label31.Enabled = false;
            label39.Enabled = false;

            //input pins on P1
            label55.Enabled = false;
            label56.Enabled = false;
            label57.Enabled = false;
            label58.Enabled = false;
            label59.Enabled = false;
            label60.Enabled = false;
            label61.Enabled = false;
            label62.Enabled = false;
            label63.Enabled = false;
            label64.Enabled = false;
            label65.Enabled = false;
            label66.Enabled = false;
            label67.Enabled = false;
            label68.Enabled = false;
            label69.Enabled = false;
            label70.Enabled = false;
            label71.Enabled = false;
            label72.Enabled = false;
            label73.Enabled = false;
            label74.Enabled = false;

            //output pins on P2
            label77.Enabled = false;//p9   D00
            label78.Enabled = false;//p10   D01
            label79.Enabled = false;//p11   D02
            label80.Enabled = false;//p12   D03
            label81.Text = "Out04 = DISOP00 _-_-";//p13   D04
            label82.Text = "Out05 = DISOP00 _-_-";//p14   D05
            label83.Text = "Out06 = DISOP01 _-_-";//p15   D06
            label84.Text = "Out07 = DISOP01 _-_-";//p16   D07
            label85.Enabled = false;//p17   D08
            label86.Enabled = false;//p18   D09
            label87.Text = "Out10 = DISOP02 _-_-";//p19   D10
            label88.Text = "Out11 = DISOP02 _-_-";//p20   D11
            label89.Text = "Out12 = DISOP03 _-_-";//p21   D12
            label90.Text = "Out13 = DISOP03 _-_-";//p22   D13
            label91.Enabled = false;//p23   D14
            label92.Enabled = false;//p24   D15
            label93.Text = "Out16 = DISOP04 _-_-";//p25   D16
            label94.Text = "Out17 = DISOP04 _-_-";//p26   D17
            label95.Text = "Out18 = DISOP05 _-_-";//p27   D18
            label96.Text = "Out19 = DISOP05 _-_-";//p28   D19
            label97.Enabled = false;//p29   D20
            label98.Enabled = false;//p30   D21
            label99.Text = "Out22 = DISOP06 _-_-";//p31   D22
            label100.Text = "Out23 = DISOP06 _-_-";//p32   D23
            label101.Text = "Out24 = DISOP07 _-_-";//p33   D24
            label102.Text = "Out25 = DISOP07 _-_-";//p34   D25
            label103.Enabled = false;//p35   D26
            label104.Enabled = false;//p36   D27
            label105.Text = "Out28 = DISOP08 _-_-";//p37   D28
            label106.Text = "Out29 = DISOP08 _-_-";//p38   D29
            label107.Text = "Out30 = DISOP09 _-_-";//p39   D30
            label108.Text = "Out31 = DISOP09 _-_-";//p40   D31
            label109.Enabled = false;//p41   D32
            label110.Enabled = false;//p42   D33
            label111.Text = "Out34 = DISOP10 _-_-";//p43   D34
            label112.Text = "Out35 = DISOP10 _-_-";//p44   D35
            label113.Text = "Out36 = DISOP11 _-_-";//p45   D36
            label114.Text = "Out37 = DISOP11 _-_-";//p46   D37
            label115.Enabled = false;//p47   D38
            label116.Enabled = false;//p48   D39
            label117.Text = "Out40 = DISOP12 _-_-";//p49   D40
            label118.Text = "Out41 = DISOP12 _-_-";//p50   D41
            label119.Text = "Out42 = DISOP13 _-_-";//p51   D42
            label120.Text = "Out43 = DISOP13 _-_-";//p52   D43
            label121.Enabled = false;//p53   D44
            label122.Enabled = false;//p54   D45
            label123.Text = "Out46 = DISOP14 _-_-";//p55   D46
            label124.Text = "Out47 = DISOP14 _-_-";//p56   D47
            label125.Text = "Out48 = DISOP15 _-_-";//p57   D48
            label126.Text = "Out49 = DISOP15 _-_-";//p58   D49
            label127.Enabled = false;//p59   D50
            label128.Enabled = false;//p60   D51
            label129.Enabled = false;//p61   D52
            label130.Enabled = false;//p62   D53
            label131.Enabled = false;//p63   D54
            label132.Enabled = false;//p64   D55
            label133.Enabled = false;//p65   D56
            label134.Enabled = false;//p66   D57
            label135.Enabled = false;//p67   D58
            label136.Enabled = false;//p68   D59
            label137.Enabled = false;//p69   D60
            label138.Enabled = false;//p70   D61
            label139.Enabled = false;//p71   D62
            label140.Enabled = false;//p72   D63

        }



        //Channel Selection Method
        private void ChannelSelectionMethod(string cardName, char[] dataWord)
        {

            //Select Channels based on channel selecting bits for each card type

            if (cardName.Contains("SOP"))//SOP card
            {
                string channelMSB = dataWord[15].ToString();//p7  MSB
                string channelLSB = dataWord[14].ToString();//p9  LSB

                if (channelLSB == "0" && channelMSB == "0")
                {
                    label97.BackColor = Color.Aqua;
                    label98.BackColor = Color.Aqua;

                    label94.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label84.BackColor = Color.Goldenrod;
                    label83.BackColor = Color.Goldenrod;
                    label80.BackColor = Color.Goldenrod;
                    label79.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "0" && channelMSB == "1")
                {
                    label94.BackColor = Color.Aqua;
                    label93.BackColor = Color.Aqua;

                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label84.BackColor = Color.Goldenrod;
                    label83.BackColor = Color.Goldenrod;
                    label80.BackColor = Color.Goldenrod;
                    label79.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "1" && channelMSB == "0")
                {
                    label84.BackColor = Color.Aqua;
                    label83.BackColor = Color.Aqua;

                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label80.BackColor = Color.Goldenrod;
                    label79.BackColor = Color.Goldenrod;

                }
                if (channelLSB == "1" && channelMSB == "1")
                {
                    label80.BackColor = Color.Aqua;
                    label79.BackColor = Color.Aqua;

                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label84.BackColor = Color.Goldenrod;
                    label83.BackColor = Color.Goldenrod;
                }
            }

            if (cardName.Contains("AOP"))//AOP card
            {
                string channelMSB = dataWord[15].ToString();//p7  MSB
                string Data = dataWord[14].ToString();//p9  Data
                string channelLSB = dataWord[13].ToString();//p9  LSB

                //make sure AO remains an output
                label8.Text = "A0 = ";

                if (channelLSB == "1" && Data == "1" && channelMSB == "1")//ALOP00
                {
                    label81.BackColor = Color.Aqua;
                    label82.BackColor = Color.Aqua;

                    // label81.BackColor = Color.Goldenrod;
                    // label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "1" && Data == "1" && channelMSB == "0")//ALOP01
                {
                    label85.BackColor = Color.Aqua;
                    label86.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    // label85.BackColor = Color.Goldenrod;
                    // label86.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "1" && Data == "0" && channelMSB == "1")//ALOP02
                {
                    label89.BackColor = Color.Aqua;
                    label90.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    // label89.BackColor = Color.Goldenrod;
                    // label90.BackColor = Color.Goldenrod;                        
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;

                }
                if (channelLSB == "1" && Data == "0" && channelMSB == "0")//ALOP03
                {
                    label93.BackColor = Color.Aqua;
                    label94.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    // label93.BackColor = Color.Goldenrod;
                    // label94.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "0" && Data == "1" && channelMSB == "1")//ALOP04
                {
                    label97.BackColor = Color.Aqua;
                    label98.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    // label97.BackColor = Color.Goldenrod;
                    // label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "0" && Data == "1" && channelMSB == "0")//ALOP05
                {
                    label101.BackColor = Color.Aqua;
                    label102.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    // label101.BackColor = Color.Goldenrod;
                    // label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "0" && Data == "0" && channelMSB == "1")//ALOP06
                {
                    label105.BackColor = Color.Aqua;
                    label106.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    // label105.BackColor = Color.Goldenrod;
                    // label106.BackColor = Color.Goldenrod;
                    label109.BackColor = Color.Goldenrod;
                    label110.BackColor = Color.Goldenrod;
                }
                if (channelLSB == "0" && Data == "0" && channelMSB == "0")//ALOP07
                {
                    label109.BackColor = Color.Aqua;
                    label110.BackColor = Color.Aqua;

                    label81.BackColor = Color.Goldenrod;
                    label82.BackColor = Color.Goldenrod;
                    label85.BackColor = Color.Goldenrod;
                    label86.BackColor = Color.Goldenrod;
                    label89.BackColor = Color.Goldenrod;
                    label90.BackColor = Color.Goldenrod;
                    label93.BackColor = Color.Goldenrod;
                    label94.BackColor = Color.Goldenrod;
                    label97.BackColor = Color.Goldenrod;
                    label98.BackColor = Color.Goldenrod;
                    label101.BackColor = Color.Goldenrod;
                    label102.BackColor = Color.Goldenrod;
                    label105.BackColor = Color.Goldenrod;
                    label106.BackColor = Color.Goldenrod;
                    // label109.BackColor = Color.Goldenrod;
                    // label110.BackColor = Color.Goldenrod;

                }



            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Write("read");
        }

        private void createCustomTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                List<string> myList = new List<string>();
                myList.Add("1111111111011000");//clear all
                //chan1
                myList.Add("1111111111010100");
                myList.Add("1111111111011100");
                //chan2
                myList.Add("1111111111010101");
                myList.Add("1111111111011101");
                //chan3
                myList.Add("1111111111010110");
                myList.Add("1111111111011110");
                //chan4
                myList.Add("1111111111010111");
                myList.Add("1111111111011111");

                int tme = 1000;
                int tme2 = 5000;

                textBox1.Text = "1111111111111111";
                textBox2.Text = "1111111111111111";
                textBox3.Text = "11111111111111111111111111111111";
                textBox1.Update();
                textBox2.Update();
                textBox3.Update();


                foreach (string number in myList)
                {
                    Thread.Sleep(1000);
                    textBox1.Text = number;
                    textBox1.Update();
                    button3.PerformClick();
                    label22.Update();
                    label23.Update();
                    button4.PerformClick();
                }

            }


            catch(Exception ex)
            {
                MessageBox.Show("Error:\r" + ex.ToString(), "General Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    if (serialPort1.IsOpen)
        //    {
        //        int dataInput = serialPort1.ReadChar();//reads char and returns int
        //        MessageBox.Show("dataInput");
        //        label77.Text = "Out00 = " + dataInput.ToString();
        //    }

        //}

    }
}
