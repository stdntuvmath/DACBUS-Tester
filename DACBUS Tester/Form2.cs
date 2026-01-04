using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using NAudio.Wave;
using System.Threading;

namespace DACBUS_Tester
{
    public partial class Form2 : Form  //Signal Generator
    {

        private const int sampleRate = 44100;
        private const short bitsPerSample = 16;
        //private float frequency = 340f;




        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public static int waveType;
        public static float Frequency;
        public static float Amplitude;
        public WaveOut waveOut = new WaveOut();

        public abstract class WaveProvider32 : NAudio.Wave.IWaveProvider
        {
            private WaveFormat waveFormat;

            public WaveProvider32()
                : this(44100, 1)
            {
            }

            public WaveProvider32(int sampleRate, int channels)
            {
                SetWaveFormat(sampleRate, channels);
            }

            public void SetWaveFormat(int sampleRate, int channels)
            {
                this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                WaveBuffer waveBuffer = new WaveBuffer(buffer);
                int samplesRequired = count / 4;
                int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
                return samplesRead * 10;
            }

            public abstract int Read(float[] buffer, int offset, int sampleCount);

            public WaveFormat WaveFormat
            {
                get { return waveFormat; }
            }
        }

        public class SineWaveProvider32 : WaveProvider32
        {
            
            int sample;

            public SineWaveProvider32()
            {
                Frequency = 400;
                Amplitude = 0.75f; // let's not hurt our ears 
                
            }

            //public float Frequency { get; set; }
            //public float Amplitude { get; set; }

            public override int Read(float[] buffer, int offset, int sampleCount)
            {
                int sampleRate = WaveFormat.SampleRate;

                if (waveType == 1)
                {
                    for (int n = 0; n < sampleCount; n++)
                    {
                        buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
                        sample++;
                        if (sample >= sampleRate) sample = 0;
                    }
                }

                if (waveType == 2)
                {
                    for (int n = 0; n < sampleCount; n++)
                    {
                        buffer[n + offset] = (float)(Amplitude * Math.Cos((2 * Math.PI * sample * Frequency)/ sampleRate));
                        sample++;
                        if (sample >= sampleRate) sample = 0;
                    }
                }

                if (waveType == 3)
                {
                    for (int n = 0; n < sampleCount; n++)
                    {
                        buffer[n + offset] = (float)(Amplitude * Math.Tan((2 * Math.PI * sample * Frequency) / sampleRate));
                        sample++;
                        //if (sample >= sampleRate) sample = 0;
                    }
                }

                if (waveType == 4)
                {
                    //var items = Enumerable.Range(1, 99);
                    for (int n = 0; n < sampleCount; n++)
                    {
                        buffer[n + offset] = (float)(((1+3/10)/(2*n-1)) * Math.Sin(Math.PI*(2*n-1) *2* sample * Frequency / sampleRate));

                        sample++;
                        if (sample >= sampleRate) sample = 0;
                    }
                }

                if (waveType == 5)
                {
                    for (int n = 0; n < sampleCount; n++)
                    {
                        buffer[n + offset] = (float)(Amplitude * Math.Tan((double)((2 * Math.PI  * sample * Frequency) / sampleRate)));

                        sample++;
                        if (sample >= sampleRate) sample = 0;
                    }
                }

                return sampleCount;
            }
        }

        
        private void StartStopSineWave()
        {
            //waveType = 1 for sine wave
            //waveType = 2 for cosine wave
            //waveType = 3 for tangent wave
            //waveType = 4 for square wave
            //waveType = 5 for sawtooth wave
            //waveType = 2 for sinc wave



            if (waveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32();
                sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
                //sineWaveProvider.Frequency = 400;
                //sineWaveProvider.Amplitude = 0.25f;
                if (Frequency == 0 || Amplitude == 0)
                {
                    Frequency = 400;
                    Amplitude = 0.73f;
                }

                waveOut = new WaveOut();
                waveOut.Init(sineWaveProvider);
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "1.0";
                textBox1.SelectAll();
            }
            else
            {
                Frequency = float.Parse(textBox1.Text);
                
                //Amplitude = float.Parse(textBox2.Text);


            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "0.001";
                textBox2.SelectAll();
            }
            else
            {
                Amplitude = float.Parse(textBox2.Text);


            }
        }



        private void button1_Click(object sender, EventArgs e)//SINE WAVE
        {
            waveType = 1;
            StartStopSineWave();   
        }
        private void button2_Click(object sender, EventArgs e)//COSINE WAVE
        {
            waveType = 2;
            StartStopSineWave();
        }

        private void button3_Click(object sender, EventArgs e)//TANGENT WAVE
        {
            waveType = 3;
            StartStopSineWave();
        }

        private void button4_Click(object sender, EventArgs e)//SQUARE WAVE
        {
            waveType = 4;
            StartStopSineWave();
        }

        private void button5_Click(object sender, EventArgs e)//SAWTOOTH WAVE
        {
            waveType = 5;
            StartStopSineWave();
        }

        private void button7_Click(object sender, EventArgs e)//TEST SCALING START
        {

        }

        private void button8_Click(object sender, EventArgs e)//TEST SCALING STOP
        {
            textBox1.Text = "";
            textBox2.Text = "";

            if (waveOut == null)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button7.Enabled = true;
                return;
            }
            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button7.Enabled = true;
        }


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;
        }

        private void starSIPTestSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            waveType = 1;
            StartStopSineWave();
            if (waveOut == null)
            {
                StartStopSineWave();

            }

            //Thread.Sleep(500);
            textBox1.Text = "400.00";
            textBox2.Text = "0.80";



            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button7.Enabled = false;
        }


    }
}
