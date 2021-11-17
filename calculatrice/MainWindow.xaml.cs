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
        public bool negate_number = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        //Mettre le blocage si division par 0
        private void Calculate()
        {
            foreach (string number in this.dataa)
            {
                string[] split_element = number.Split(",");
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
                    //Debug.WriteLine("Modulo" + Int64.Parse(this.dataa[0]) % Int64.Parse(this.dataa[1]));
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

        //Le retour en arriere enleve juste le 1er char et le ce enleve toute la saisie en cours
        //Gerer le 0 devant les chiffres
        //Gerer le +/-
        //Ajouter la possibilité d'executer des calculs si on a la licence ==> inscription sur appli et générer une clef que l'on save sur l'outil de l'utilisateur
        //Ajouter un menu qui apparaît quand on appuie sur alt

        private void Render_Screen(string context = "")
        {
            if (this.reset_after_equal)
            {
                screen_display_historic.Text = "";
                this.reset_after_equal = false;
            }

            if (this.historic.Count() > 2)
            {
                Calculate();
                this.dataa.Clear();
                if (context == "equal")
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

            foreach (string element in this.historic)
            {
                string tmp = screen_display_historic.Text;
                screen_display_historic.Text = tmp + element;
            }
            foreach (string test in this.dataa)
            {
                Debug.WriteLine(test);
            }
            Debug.WriteLine("END");
        }

        /// <summary>
        /// Ajoute les chiffres et +/- saisis par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_next(object sender, RoutedEventArgs e)
        {
            //On ajoute le contenu des chiffres sur lequel on clique si c'est 0 en tout début alors on ne fait rien
            var element_pressed = (Button)sender;
            if(element_pressed.Content.ToString() == "0" && screen_display.Text == "")
            {

            }
            else
            {
                string tmp = screen_display.Text;
                screen_display.Text = tmp + element_pressed.Content;

                //Si l'utilisateur a cliqué sur +/- avant d'écrire son numbre alors on ajoute le moins juste après l'ajout de son 1er chiffre
                //if (this.negate_number && screen_display.Text != "")
                //{
                //    Add_before();
                //}
            }

        }
        private void Plus_Moins(object sender, RoutedEventArgs e)
        {
            Add_before();
        }
        /// <summary>
        /// Zone commentaires
        /// 
        /// </summary>
        private void Add_before()
        {
            if (screen_display.Text != "")
            {
                string tmp = screen_display.Text;
                if (this.negate_number && tmp.Contains("-"))
                {
                    screen_display.Text = tmp.Substring(1, tmp.Length);
                    this.negate_number = false;
                }
                else
                {
                    screen_display.Text = "-" + tmp;
                    this.negate_number = true;
                }
            }
            //else
            //{
            //    //Ici on gère si oui ou non le chiffre est négatif
            //    this.negate_number = !this.negate_number;
            //}
        }

        private void Operators_click(object sender, RoutedEventArgs e)
        {
            var element_pressed = (Button)sender;
            //Verifier s'il y a déjà eu une valeur ajoutée sinon on ne fait rien
            if(screen_display.Text != "")
            {
                //Si l'utilisateur a cliqué sur = alors on reset l'historique pour n'afficher que le résultat
                if (this.reset_after_equal)
                {
                    this.historic.Clear();
                }
                else
                {
                    this.dataa.Add(screen_display.Text);
                }
                this.historic.Add(screen_display.Text);
                this.historic.Add(element_pressed.Content.ToString());
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
        }

        //Gestion du si c'est 0 et qu'on appuie sur le point alors on prend en comtpe

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
            this.operation_to_execute = null;
            this.dataa.Clear();
            this.historic.Clear();
            this.data_int = 0;
            this.data_decimal = 0;
        }

        private void Erase_content_char(object sender, RoutedEventArgs e)
        {
            string tmp = screen_display.Text;
            if(tmp != "")
            {
                int limit = tmp.Length - 1;
                screen_display.Text = tmp.Substring(0, limit);
            }
        }

        private void Erase_content_CE(object sender, RoutedEventArgs e)
        {
            if (this.reset_after_equal == true)
            {
                Clear_All_Screen();
            }
            else
            {
                Clear_Screen();
            }
            
            
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
