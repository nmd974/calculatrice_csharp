using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace calculatrice
{
    /**
     * Récupérer toutes les valeurs saisies 
     * Si clic sur autre que chiffres alors on ajoute dans le tableau et on ajoute l'opérateur
     * 
     * 
     */

    public partial class MainWindow : Window
    {
        public Int64 data_int;
        public decimal data_decimal;
        public List<string> dataa = new List<string>();
        public List<string> historic = new List<string>();
        public string operation_to_execute = null;
        public bool decimal_activated = false;
        public bool reset_after_equal = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calculate()
        {
            
            foreach (string number in this.dataa)
            {
                Debug.WriteLine(number);
                string[] split_element = number.Split(".");
                if(split_element.Count() > 1)
                {
                    this.decimal_activated = true;
                }
            }


            if (this.operation_to_execute == "/")
            {
                if (this.decimal_activated)
                {
                    this.data_decimal = decimal.Parse(this.dataa[0]) / decimal.Parse(this.dataa[1]);
                }
                else
                {
                    Debug.WriteLine("Modulo" + Int64.Parse(this.dataa[0]) % Int64.Parse(this.dataa[1]));
                    if(Int64.Parse(this.dataa[0]) % Int64.Parse(this.dataa[1]) > 0)
                    {
                        this.decimal_activated = true;
                        this.data_decimal = decimal.Parse(this.dataa[0]) / decimal.Parse(this.dataa[1]);
                    }
                    else
                    {
                        this.data_int = Int64.Parse(this.dataa[0]) / Int64.Parse(this.dataa[1]);
                    }
                }
            }
            else if (this.operation_to_execute == "*")
            {
                if (this.decimal_activated)
                {
                    this.data_decimal = decimal.Parse(this.dataa[0]) * decimal.Parse(this.dataa[1]);
                }
                else
                {
                    this.data_int = Int64.Parse(this.dataa[0]) * Int64.Parse(this.dataa[1]);
                }
            }
            else if (this.operation_to_execute == "-")
            {
                if (this.decimal_activated)
                {
                    this.data_decimal = decimal.Parse(this.dataa[0]) - decimal.Parse(this.dataa[1]);
                }
                else
                {
                    this.data_int = Int64.Parse(this.dataa[0]) - Int64.Parse(this.dataa[1]);
                }
            }
            else
            {
                if (this.decimal_activated)
                {
                    this.data_decimal = decimal.Parse(this.dataa[0]) + decimal.Parse(this.dataa[1]);
                }
                else
                {
                    this.data_int = Int64.Parse(this.dataa[0]) + Int64.Parse(this.dataa[1]);
                }
            }

            this.operation_to_execute = null;
        }
        private void Render_Screen(string context = "")
        {
            foreach(string test in this.historic)
            {
                Debug.WriteLine(test);
            }
            
            if (this.historic.Count() > 2)
            {

                Calculate();
                if(context == "equal")
                {
                    this.reset_after_equal = true;
                    if (this.decimal_activated)
                    {
                        screen_display.Text = this.data_decimal.ToString();
                        this.dataa.Add(this.data_decimal.ToString());
                    }
                    else
                    {
                        screen_display.Text = this.data_int.ToString();
                        this.dataa.Add(this.data_int.ToString());
                    }

                    screen_display_historic.Text = "";
                }
                else
                {
                    this.historic.Clear();
                    this.dataa.Clear();

                    if (this.decimal_activated)
                    {
                        screen_display.Text = this.data_decimal.ToString();
                        this.historic.Add(this.data_decimal.ToString());
                        this.dataa.Add(this.data_decimal.ToString());
                    }
                    else
                    {
                        screen_display.Text = this.data_int.ToString();
                        this.historic.Add(this.data_int.ToString());
                        this.dataa.Add(this.data_int.ToString());
                    }
                }
            }
            if (this.reset_after_equal == true)
            {
                screen_display_historic.Text = "";
                this.reset_after_equal = false;
            }
            foreach (string element in this.historic)
            {
                string tmp = screen_display_historic.Text;
                screen_display_historic.Text = tmp + element;
            }
        }
        private void Add_next(object sender, RoutedEventArgs e)
        {
            var element_pressed = (Button)sender;
            string tmp = screen_display.Text;
            screen_display.Text = tmp + element_pressed.Content;

        }
        private void Add_before(object sender, RoutedEventArgs e)
        {
            var element_pressed = (Button)sender;
            string tmp = screen_display.Text;
            screen_display.Text = element_pressed.Content + tmp;
        }

        private void Operators_click(object sender, RoutedEventArgs e)
        {
            var element_pressed = (Button)sender;
            Debug.WriteLine(this.reset_after_equal);
            if (this.reset_after_equal == true)
            {
                this.historic.Clear();
            }
            this.historic.Add(screen_display.Text);
            this.historic.Add(element_pressed.Content.ToString());
            this.dataa.Add(screen_display.Text);
            if (element_pressed.Content.ToString() == "=")
            {
                Render_Screen("equal");
            }
            else
            {
                if (this.operation_to_execute == null)
                {
                    this.operation_to_execute = element_pressed.Content.ToString();
                    Render_Screen();
                }
                else
                {
                    Render_Screen();
                    this.operation_to_execute = element_pressed.Content.ToString();
                }
                Clear_Screen();
            }
                       
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            this.dataa.Add(screen_display.Text);
            this.historic.Add(screen_display.Text);
            Render_Screen();
        }

        private void Erase_content(object sender, RoutedEventArgs e)
        {
            Clear_All_Screen();
            this.decimal_activated = false;
            this.dataa.Clear();
            this.historic.Clear();
            this.data_int = 0;
            this.data_decimal = 0;
        }

        private void Erase_content_char(object sender, RoutedEventArgs e)
        {
            screen_display.Text = screen_display.Text.Substring(0, screen_display.Text.Length);
        }

        private void Clear_Screen()
        {
            screen_display.Text = "";
        }

        private void Clear_All_Screen()
        {
            screen_display.Text = "";
            screen_display_historic.Text = "";
        }

    }
}
