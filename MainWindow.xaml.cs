using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;


namespace CalMat
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CalMat.Calculatrice.mainWindow = this;
        }

        public void Refresh()
        {
            foreach (KeyValuePair<String, Matrix> matrice in CalMat.Calculatrice.listMatrix)
            {
                if (!ListBox_display.Items.Contains(matrice))
                {
                    ListBox_display.Items.Add(matrice);
                }
            }
        }

        public void Open(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Calcul"; // Default file name
            dlg.DefaultExt = ".dat"; // Default file extension
            dlg.Filter = "data documents (.dat)|*.dat"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {

                try
                {
                    string filename = dlg.FileName;
                    BinaryFormatter binary = new BinaryFormatter();
                    FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    while (file.Position != file.Length)
                    {
                        var obj = (KeyValuePair<String,Matrix>) binary.Deserialize(file);
                        CalMat.Calculatrice.listMatrix.Add(obj.Value.name, obj.Value);
                    }

                    TxtBox_console.AppendText("fichier bien ouvert");
                    Refresh();
                    
                }
                catch (Exception l)
                {
                    TxtBox_console.AppendText("Fichier non ouvert. erreur : " + l.Message + "\n");
                }
            }
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Calcul"; // Default file name
            dlg.DefaultExt = ".dat"; // Default file extension
            dlg.Filter = "data documents (.dat)|*.dat"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    string fileName = dlg.FileName;

                    #region "xml stackoverflow"
                    Stream fs = new FileStream(fileName,FileMode.Create);
                    XmlDictionaryWriter xml = XmlDictionaryWriter.CreateTextWriter(fs);
                    using (XmlDictionaryWriter writer =
                    XmlDictionaryWriter.CreateTextWriter(fs))

                    foreach (KeyValuePair<String, Matrix> matrice in CalMat.Calculatrice.listMatrix)
                    {
                        XmlSerializer x = new XmlSerializer(matrice.GetType());
                        x.Serialize(writer, matrice);
                        //xml.WriteEndElement();
                    }
                    fs.Close();
                    #endregion

                    #region "binary" //marche  


                    /*BinaryFormatter binaryFormat = new BinaryFormatter();
                    FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    foreach (KeyValuePair<String, Matrix> matrice in CalMat.Calculatrice.listMatrix)
                    {
                        binaryFormat.Serialize(file, matrice);
                    }
                    file.Close();*/
                    #endregion
                    TxtBox_console.AppendText("Fichier enregistre\n");
                }

                catch(Exception l)
                {
                    TxtBox_console.AppendText("Fichier non enregistre. erreur : " + l.Message + "\n" );
                }
               
            }
           /* using (FileStream fileStream = new FileStream("test.binary", FileMode.Create))
            {
                IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fileStream, CalMat.Calculatrice.listMatrix);
            }*/
        }

        public void Help(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        public void CalculProcess ()
        {
            //TxtBox_console.Text += ">>> " + this.TxtBox_command.Text + "\n";
            TxtBox_console.AppendText(">> " + this.TxtBox_command.Text + "\n");
            string error = null;
            String result = UserInput.parse(TxtBox_command.Text, ref error);
            if (error != null)
            {
                //TxtBox_console.Text += result ;
                TxtBox_console.AppendText(result);
            }
            else
            {
                //TxtBox_console.Text += result ;
                TxtBox_console.AppendText(result);
                TxtBox_command.Clear();
            }  
            Refresh();
            TxtBox_console.ScrollToEnd();
        }

       
        #region "Event" 

        private void TxtBox_command_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.TxtBox_command.Text == "Entrez votre calcul")
            {
                this.TxtBox_command.Text = "";
            }
        }

        private void Btn_calculus_Click(object sender, RoutedEventArgs e)
        {
            CalculProcess();
        }

        private void ListBox_display_MouseDown(object sender, MouseButtonEventArgs e)
        {
           ListBox_display.UnselectAll();
        }

        private void ListBox_display_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ContextMenu cm = this.FindResource("cmitem") as ContextMenu;
                cm.IsOpen = true;
            }
            catch(Exception)
            {
                
            }
        } 
        private void file_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = this.FindResource("file") as ContextMenu;
            cm.IsOpen = true;
        }
           
        private void TxtBox_command_KeyDown(object sender, KeyEventArgs e)
        {
           if( e.Key == Key.Enter)
           {
               CalculProcess();
           }
        }

        public void Modifier(object sender, RoutedEventArgs e)
        {
            try
            {
                Matrix m = ((KeyValuePair<String, Matrix>)ListBox_display.Items.GetItemAt(ListBox_display.SelectedIndex)).Value;
                MatrixInput MatrixInput = new MatrixInput(m, true);
                MatrixInput.Show();
            }
            catch(Exception)
            {

            }

        }

        public void Supprim(object sender, RoutedEventArgs e)
        {
            try
            {
            Matrix m = ((KeyValuePair<String,Matrix>) ListBox_display.Items.GetItemAt(ListBox_display.SelectedIndex)).Value;
            CalMat.Calculatrice.listMatrix.Remove(m.name);
            ListBox_display.Items.RemoveAt(ListBox_display.SelectedIndex);
            }
            catch(Exception)
            {

            }
            
            
        }
        #endregion
    }
}
