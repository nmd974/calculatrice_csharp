using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;

namespace calculatrice
{
    /**
     * A la saisie des chiffres / nombres, la valeur est stockée dans le tableau dataa et historic
     * Quand la taille du tableau historic > 2 alors on sait qu'il faudra calculer
     */

    public partial class MainWindow : Window
    {
        public double result; //Stocke le dernier résultat d'une opération
        public List<string> dataa = new List<string>(); //Permet de stocker les 2 valeurs à calculer
        public List<string> historic = new List<string>(); //Permet de gérer l'historique à afficher en haut
        public bool reset_after_equal = false; //Permet dérer l'affichage s'il y a eu l'appuie sur =
        public bool negate_number = false; //Gère le comportement du +/- 
        public string operation_to_execute = null; //Indique quelle est l'opération à effectuer

        public MainWindow()
        {
            InitializeComponent();
        }

        //TODO keyDown listener
        //private void DispatchActions(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.NumPad0:
        //            Add_next(sender, new RoutedEventArgs());
        //            break;
        //    }
        //}


        /// <summary>
        /// Réalise un calcul suite au déclenchement dans le render screen 
        /// </summary>
        private void Calculate()
        {
            switch (this.operation_to_execute)
            {
                case "/":
                    if(double.Parse(this.dataa[1]) != 0)
                    {
                        this.result = double.Parse(this.dataa[0]) / double.Parse(this.dataa[1]);
                    }
                    break;
                case "*":
                    this.result = double.Parse(this.dataa[0]) * double.Parse(this.dataa[1]);
                    break;
                case "-":
                    this.result = double.Parse(this.dataa[0]) - double.Parse(this.dataa[1]);
                    break;
                default:
                    foreach (string element in this.dataa)
                    {
                        Debug.WriteLine(element);
                    }
                    
                    this.result = double.Parse(this.dataa[0]) + double.Parse(this.dataa[1]);
                    break;
            }
            
            this.operation_to_execute = null;
        }

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
                    this.dataa.Add(this.result.ToString());
                    screen_display_historic.Text = "";
                }
                else
                {
                    //this.historic.Clear();
                    screen_display_historic.Text = "";
                    //this.historic.Add(this.result.ToString());
                    this.dataa.Add(this.result.ToString());
                }
                screen_display.Text = this.result.ToString();
            }

            foreach (string element in this.historic)
            {
                string tmp = screen_display_historic.Text;
                screen_display_historic.Text = tmp + element;
            }
        }

        /// <summary>
        /// Ajoute les chiffres saisis par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_next(object sender, RoutedEventArgs e)
        {
            //Si enchainement de calculs et que l'utilisateur souhaite ajouter un chiffre
            if (this.historic.Count() > 2)
            {
                Clear_Screen();
            }
            //On ajoute le contenu des chiffres sur lequel on clique si c'est 0 en tout début alors on ne fait rien || si nombre deja en decimal et clic sur decimal alors on ajoute pas
            var element_pressed = (Button)sender;
            if((element_pressed.Content.ToString() == "0" && screen_display.Text == "") || (screen_display.Text.Contains(",") && element_pressed.Content.ToString() == ","))
            {
                //Ne rien faire
            }
            else
            {
                string tmp = screen_display.Text;
                screen_display.Text = tmp + element_pressed.Content;
            }


        }
        /// <summary>
        /// Event listener du +/- qui permet d'ajouter le "-" ou pas devant le chiffre saisi
        /// </summary>
        private void Plus_Moins(object sender, RoutedEventArgs e)
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
        }
        /// <summary>
        /// Event listener sur les operateurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Operators_click(object sender, RoutedEventArgs e)
        {
            var element_pressed = (Button)sender;
            //Verifier s'il y a déjà eu une valeur ajoutée sinon on ne fait rien
            if(screen_display.Text != "")
            {
                //Si l'utilisateur a cliqué sur = alors on reset l'historique pour n'afficher que le résultat, sinon on ajoute au tableau dataa le nombre à calculer
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

                //Si l'utilisateur a appuyé sur égal alors le render screen sera différent
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
                    //On ne supprime pas l'affichage en cours de saisi s'il s'agit d'un enchaînement de calculs
                    if(this.historic.Count() < 3)
                    {
                        Clear_Screen();
                    }
                    
                }
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
            this.operation_to_execute = null;
            this.dataa.Clear();
            this.historic.Clear();
            this.result = 0;
            this.reset_after_equal = false;
        }

        private void Erase_content_char(object sender, RoutedEventArgs e)
        {
            string tmp = screen_display.Text;
            if(tmp != "")
            {
                int limit = tmp.Length - 1;
                screen_display.Text = tmp.Substring(0, limit);
            }
            //Permet de supprimer l'ancienne valeur après un égal
            if (this.reset_after_equal)
            {
                this.dataa.Clear();
                this.reset_after_equal = false;
                this.historic.Clear();
            }
        }

        private void Erase_content_CE(object sender, RoutedEventArgs e)
        {
            if (this.reset_after_equal == true)
            {
                Clear_All_Screen();
                this.operation_to_execute = null;
                this.dataa.Clear();
                this.historic.Clear();
                this.result = 0;
                this.reset_after_equal = false;
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
