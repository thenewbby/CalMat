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

        Matrix Matrice;
        List<TextBox> inputs = new List<TextBox>(); // event enter a faire passer a droite
       
        public MatrixInput(Matrix m, bool load)
        {
           
            InitializeComponent();

            // Ne pas insérer dans les boucles imbriquées, ne gère pas les hauteurs de rows
            for (int c = 0; c < m.Columns ; c++ )
            {
                ColumnDefinition ColDefc = new ColumnDefinition();
                ColDefc.Name = "ColDef" + c;
                Grid_Tabeau.ColumnDefinitions.Add(ColDefc);
            }
    
            for (int l = 0; l < m.Lines; l++ )
            {
                RowDefinition RowDef = new RowDefinition();
                RowDef.Name = "RowDef" + l;
                Grid_Tabeau.RowDefinitions.Add(RowDef);
            }

            
            Grid_Tabeau.Margin = new Thickness(20);
            this.Width  = 30 * m.Columns + 40;
            this.Height = 30 * m.Lines + this.Btn_matrix.Height + 70;
            Matrice = m;


            if(load) //tu charges les valeurs
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
                        txt.Text = m.Elements[c, l].ToString(); 
                        txt.PreviewTextInput += txt_PreviewTextInput;
                        Grid_Tabeau.Children.Add(txt);
                        inputs.Add(txt);
                    }
                }
            }
            else
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
                        txt.PreviewTextInput += txt_PreviewTextInput;
                        Grid_Tabeau.Children.Add(txt);
                        inputs.Add(txt);
                    }
                }
            }
            
        }

        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9,\\.]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Btn_matrix_Click(object sender, RoutedEventArgs e) //quand click sur le bouton "OK"
        {
            int j = 0;
            for (int c = 0 ; c < Matrice.Columns; c++ )
            {
                for (int l = 0 ; l < Matrice.Lines; l++)
                {

                    double.TryParse(inputs [j].Text, out Matrice.Elements[c, l]);
                    j++;
                }
            }

            CalMat.Calculatrice.mainWindow.TxtBox_console.AppendText(Matrice.ToString());
            CalMat.Calculatrice.mainWindow.ListBox_display.Items.Refresh();
            this.Close();
        }

        private void Creation_Matrice_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (TextBox txt in inputs)
            {
                txt.FontSize = this.Height / 15;
            }
        }

    }
}
