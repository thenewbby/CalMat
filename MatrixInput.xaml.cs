using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalMat
{
    /// <summary>
    /// Logique d'interaction pour MatrixInput.xaml
    /// </summary>
    public partial class MatrixInput : Window
    {

        Matrix matrix; // on crée une matrice générale pour que toutes les méthodes de la classe puisse y accéder
        List<TextBox> inputs = new List<TextBox>(); // liste pour les textBox
       
        public MatrixInput(Matrix m, bool load) // création de la fenêtre
        {
           
            InitializeComponent();

            // ne pas insérer dans les boucles imbriquées, ne gère pas les hauteurs de rows

            //on crée le nombre de colonnes qu'il faut
            for (int c = 0; c < m.Columns ; c++ )
            {
                ColumnDefinition ColDefc = new ColumnDefinition();
                ColDefc.Name = "ColDef" + c;
                Grid_Tabeau.ColumnDefinitions.Add(ColDefc);
            }

            //on crée le nombre de lignes qu'il faut
            for (int l = 0; l < m.Lines; l++ )
            {
                RowDefinition RowDef = new RowDefinition();
                RowDef.Name = "RowDef" + l;
                Grid_Tabeau.RowDefinitions.Add(RowDef);
            }

            // on definie les paramettres de la fenêtre
            Grid_Tabeau.Margin = new Thickness(20);
            this.Width  = 30 * m.Columns + 40;
            this.Height = 30 * m.Lines + this.Btn_matrix.Height + 70;
            matrix = m;


            if(load) // si on doit modifier les valeurs de la matrice
            {
                //on crée les textBox avec les valeurs correspondantes
                for (int c = 0; c < m.Columns; c++)
                {
                    for (int l = 0; l < m.Lines; l++)
                    {
                        // on definie les paramètres de chaque textBox
                        TextBox txt = new TextBox();
                        txt.Background = Brushes.Transparent;
                        txt.Foreground = Brushes.White;
                        txt.TextAlignment = TextAlignment.Center;
                        txt.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                        txt.SetValue(Grid.ColumnProperty, c);
                        txt.SetValue(Grid.RowProperty, l);
                        txt.Text = m.Elements[c, l].ToString(); 
                        txt.PreviewTextInput += Txt_PreviewTextInput;
                        Grid_Tabeau.Children.Add(txt);
                        inputs.Add(txt);
                    }
                }
            }
            else // sinon on crée les textBox avec rien à l'interieur
            {
                for (int c = 0; c < m.Columns; c++)
                {
                    for (int l = 0; l < m.Lines; l++)
                    {

                        TextBox txt = new TextBox();
                        txt.Background = Brushes.Transparent;
                        txt.Foreground = Brushes.White;
                        txt.TextAlignment = TextAlignment.Center;
                        txt.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        txt.SetValue(Grid.ColumnProperty, c);
                        txt.SetValue(Grid.RowProperty, l);
                        txt.PreviewTextInput += Txt_PreviewTextInput;
                        Grid_Tabeau.Children.Add(txt);
                        inputs.Add(txt);
                    }
                }
            }
            
        }

        #region"event"
        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e) //événement pour obliger l'utilisateur à écrire des chiffres
        {
            Regex reg = new Regex("[^0-9,\\.]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Btn_matrix_Click(object sender, RoutedEventArgs e) //évenement quand on clique sur le bouton "OK"
        {
            int j = 0;
            for (int c = 0 ; c < matrix.Columns; c++ )
            {
                for (int l = 0 ; l < matrix.Lines; l++)
                {

                    double.TryParse(inputs [j].Text, out matrix.Elements[c, l]); //pour chaque textBox, on les convertis en double et on les place dans la matrice
                    j++;
                }
            }
            CalMat.Calculatrice.mainWindow.TxtBox_console.AppendText(matrix.name + "=\n");
            CalMat.Calculatrice.mainWindow.TxtBox_console.AppendText(matrix.ToString()); // on affiche la matrice sur la console en guise de confirmation
            CalMat.Calculatrice.mainWindow.ListBox_display.Items.Refresh(); // on rafraichie l'affichage des matrices
            this.Close(); // on ferme la fenêtre
        }

        private void Creation_Matrice_SizeChanged(object sender, SizeChangedEventArgs e) // évenement pour déterminer la taille des chiffres
        {
            foreach (TextBox txt in inputs)
            {
                txt.FontSize = this.Height/15 ;
            }
        }
        #endregion
    }
}
