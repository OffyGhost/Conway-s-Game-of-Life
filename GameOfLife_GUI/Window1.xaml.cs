using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife_GUI
{
    public partial class Window1 : Window
    {
        public int step = default(int);
        public bool[,] states;
        // Count of neighbors, its for drawing next generations
        int[,] nextStates;
        // Emulate GRID in GUI
        Button[,] buttonArr;

        const int sizeOfBoard = 33;
        double speedOfRender = 0.25d;

        DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();

        public Window1()
        {
  
        }


        void DoLoop(object sender, EventArgs e)
        {
            if (checkBox1.IsChecked== true)
            {
                DrawBoard();
                CountNeighbors();
                PrepareNextGeneration();
            }
        }

 
        // Bool its for random or coordinate mode
        public void InitBoard(bool random)
        {
            states = new bool[sizeOfBoard, sizeOfBoard];
            nextStates = new int[sizeOfBoard, sizeOfBoard];
            buttonArr = new Button[sizeOfBoard, sizeOfBoard];


            // Randomize for debug
            if (random)
            {
                Random rand = new Random();
                for (int i = 0; i < sizeOfBoard; i++)
                {
                    for (int y = 0; y < sizeOfBoard; y++)
                    {
                        if (rand.Next(10) > 6)
                        {
                            states[i, y] = true;
                        }
                        nextStates[i, y] = default(byte);
                        buttonArr[i,y] = new Button();
                        buttonArr[i, y].Background = new SolidColorBrush(Colors.Black);
                        buttonArr[i, y].MouseEnter += button1_MouseLeaveArea;
                       buttonArr[i, y].Name = "b" + i + "_" + y;
                        myGrid.Children.Add(buttonArr[i, y]);
                    }
                }
            }
            else
            {
                // OR init by coordinates
                //				states[4, 2] = true;
                //				states[3, 2] = true;
                //				states[5, 2] = true;
                //				
                states[6, 5] = true;
                states[6, 6] = true;
                states[6, 7] = true;
                states[5, 7] = true;
                states[4, 6] = true;

                //				states[5, 5] = true;
                //				states[5, 6] = true;
                //				states[6, 5] = true;
                //				states[6, 6] = true;
            }

        }
        

        public void DrawBoard()
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int y = 0; y < sizeOfBoard; y++)
                {
                    if (states[i, y])
                    {
                        buttonArr[i, y].Background = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        buttonArr[i, y].Background = new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }

        public void PrepareNextGeneration()
        {
            // Determinate states from nextStates
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int y = 0; y < sizeOfBoard; y++)
                {
                    try
                    {
                        if (nextStates[i, y] == 3)
                        {
                            // new born
                            states[i, y] = true;

                        }
                        else if (nextStates[i, y] < 2 || nextStates[i, y] > 3)
                        {
                            // OMAE WA MOU SHINDEIRU
                            states[i, y] = false;
                        }

                    }
                    catch (IndexOutOfRangeException)
                    {
                        // ignore for yet
                    }
                }
            }

        }

        public void CountNeighbors()
        {
            // Drop the counter
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int y = 0; y < sizeOfBoard; y++)
                {
                    nextStates[i, y] = 0;
                }
            }
            // Counting heighbors

            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int y = 0; y < sizeOfBoard; y++)
                {
                    try
                    {

                        if (states[i, y - 1])
                            nextStates[i, y]++;
                        if (states[i, y + 1])
                            nextStates[i, y]++;

                        if (states[i - 1, y])
                            nextStates[i, y]++;
                        if (states[i + 1, y])
                            nextStates[i, y]++;

                        if (states[i + 1, y + 1])
                            nextStates[i, y]++;
                        if (states[i + 1, y - 1])
                            nextStates[i, y]++;

                        if (states[i - 1, y - 1])
                            nextStates[i, y]++;
                        if (states[i - 1, y + 1])
                            nextStates[i, y]++;

                    }
                    catch (IndexOutOfRangeException)
                    {
                        // ignore
                        // Console.WriteLine("outOfArr");
                    }
                }
            }


        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            dispTimer();

            InitBoard(true);

        }

        void dispTimer() {

            tmr.Interval = TimeSpan.FromSeconds(speedOfRender);
            tmr.Tick += new EventHandler(DoLoop);

            tmr.Start();
        }


        private void button1_MouseLeaveArea(object sender, RoutedEventArgs e)
        {
            
            var button = (Button)sender;
            if (Mouse.RightButton == MouseButtonState.Pressed || Mouse.LeftButton == MouseButtonState.Pressed)
            {
                int x = Convert.ToInt32(button.Name.Split('b')[1].Split('_')[0]);
                int y = Convert.ToInt32(button.Name.Split('b')[1].Split('_')[1]);
                states[x, y] = !states[x, y];

                if (states[x, y])
                {
                    buttonArr[x, y].Background = new SolidColorBrush(Colors.White);
                }
                else
                {
                    buttonArr[x, y].Background = new SolidColorBrush(Colors.Black);
                }
            }

        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            if (speedOfRender > 0.25)
            {
                tmr.Stop();
                speedOfRender -= 0.25;
                tmr.Interval = TimeSpan.FromSeconds(speedOfRender);
                tmr.Start();
            }
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            if (speedOfRender <= 5)
            {
                tmr.Stop();
                speedOfRender += 0.25;
                tmr.Interval = TimeSpan.FromSeconds(speedOfRender);
                
                tmr.Start();
            }
        }

}
}