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
//using System.Xml.Serialization;
using System.IO;
//using System.Xml.Linq;
//using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;


namespace CalMat
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*public struct CustomKeyValuePair<T1, T2>
        {
            public CustomKeyValuePair(T1 key, T2 value)
                : this()
            {
                Key = key;
                Value = value;
            }

            public T1 Key { get; set; }
            public T2 Value { get; set; }

            // here I specify how is the cast
            public static explicit operator CustomKeyValuePair<T1, T2>(KeyValuePair<T1, T2> keyValuePair)
            {
                return new CustomKeyValuePair<T1, T2>(keyValuePair.Key, keyValuePair.Value);
            }
        }*/

        public MainWindow() //constructeur de la fenêtre MainWindow
        {
            InitializeComponent(); // on crée les élément defini dans le fichier XAML associé dans a la classe
            CalMat.Calculatrice.mainWindow = this; // on enregistre le pointeur vers la classe
        }

        public void AddMatrix() //methode pour ajouter les matrices sur la colonne de droite (ListBox_display) 
        {
            foreach (KeyValuePair<String, Matrix> matrice in CalMat.Calculatrice.listMatrix) //pour toutes les matrices enregistrées dans le dictionnaire
            {
                if (!ListBox_display.Items.Contains(matrice)) //si elle ne n'est pas déjà affichée
                {
                    ListBox_display.Items.Add(matrice); //on l'ajoute
                }
            }
        }

        public void CalculProcess () //method pour controler l'affichage des resultats
        {
            string error = null;
            TxtBox_console.AppendText(">> " + this.TxtBox_command.Text + "\n"); // on ajoute ">>" et la commande entrer par l'utilisateur à la console (TxtBox_console)
            string result = UserInput.Parse(TxtBox_command.Text, ref error); //on parse la commande et on passe comme reference (pointer) la variable error
            if (error != null) 
            {
                TxtBox_console.AppendText(error); //si il y a une erreur, on affiche l'erreur
            }
            else
            {
                TxtBox_console.AppendText(result); //si il n'y a pas d'erreur, on affiche le resultat
                TxtBox_command.Clear(); // et on enleve la commande de la barre d'ecriture
            }  
            AddMatrix(); // on regarde si il 'y a pas de matrice à afficher en plus
            TxtBox_console.ScrollToEnd(); // on met la vue sur la dernière commande et resultat
        }

       
        #region "Event" 
        private void File_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) // événement quand on clique sur le bouton "Fichier"
        {
            ContextMenu cm = this.FindResource("file") as ContextMenu; // on charge le menu
            cm.IsOpen = true; //on ouvre le menu
        }

        public void Open(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Calcul"; // nom su fichier par default
            dlg.DefaultExt = ".data"; // extension du fichier par default
            dlg.Filter = "data documents (.data)|*.data"; // filtre par default

            // affiche la fenêtre
            Nullable<bool> result = dlg.ShowDialog();

            // quand on accepte
            if (result == true)
            {

                try
                {
                    string filename = dlg.FileName;
                    BinaryFormatter binary = new BinaryFormatter(); //création d'un nouveau format binaire
                    FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read); // on ouvre le fichier
                    while (file.Position != file.Length) //tant que on est pas à la fin du fichier
                    {
                        var obj = (KeyValuePair<String, Matrix>)binary.Deserialize(file);//on charge la matrice
                        if (!CalMat.Calculatrice.listMatrix.ContainsKey(obj.Value.name)) 
                       {
                            CalMat.Calculatrice.listMatrix.Add(obj.Value.name, obj.Value); //si elle n'existe pas, on l'ajoute au dictionnaire
                       }
                       else
                        {
                            CalMat.Calculatrice.listMatrix.Add(obj.Value.name + "_fichier", obj.Value); // si le nom est pris, on change le nom et on l'ajoute
                        }

                    }
                    #region"xml stackoverflow"
                    /*FileStream fs = new FileStream(filename, FileMode.Open);
                    XmlSerializer Xml = new XmlSerializer(typeof(CustomKeyValuePair<string, Matrix>[]));
                    var mat = (CustomKeyValuePair<string, Matrix>[])Xml.Deserialize(fs);
                    foreach (CustomKeyValuePair<string, Matrix> matrice in mat)
                    {
                        if (!CalMat.Calculatrice.listMatrix.ContainsKey(matrice.Value.name))
                        {
                            CalMat.Calculatrice.listMatrix.Add(matrice.Value.name, matrice.Value);
                        }
                        else
                        {
                            CalMat.Calculatrice.listMatrix.Add(matrice.Value.name + "_fichier", matrice.Value);
                        }
                        
                    }*/
                    #endregion

                    TxtBox_console.AppendText("Fichier bien ouvert \n"); //confimation de l'ouverture
                    AddMatrix(); //on affiche les matrices

                }
                catch (Exception l)
                {
                    TxtBox_console.AppendText("Fichier non ouvert. erreur : " + l.Message + "\n"); // si il y a un problème, on l'affiche
                }
            }
        } // événement quand on clique sur le bouton "Ouvrir"

        public void Save(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog(); //on crée une nouvelle fenêtre de sauvegarde
            dlg.FileName = "Calcul"; // nom su fichier par default
            dlg.DefaultExt = ".data"; // extension du fichier par default
            dlg.Filter = "data documents (.data)|*.data"; // filtre par default

            // affiche la fenêtre
            Nullable<bool> result = dlg.ShowDialog();

            // quand on accepte
            if (result == true)
            {
                try
                {
                    string fileName = dlg.FileName;

                    #region "xml stackoverflow"
                    /*Stream fs = new FileStream(fileName,FileMode.Create);
                    XmlSerializer Xml = new XmlSerializer(typeof(CustomKeyValuePair<string,Matrix>[]));
                    var aux = CalMat.Calculatrice.listMatrix.Select(keyValuePair => (CustomKeyValuePair<string, Matrix>)keyValuePair).ToArray();
                    Xml.Serialize(fs, aux);
                    fs.Close();*/
                    #endregion

                    BinaryFormatter binaryFormat = new BinaryFormatter(); //on crée un nouveau format binaire
                    FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write); //on crée le fichier
                    foreach (KeyValuePair<String, Matrix> matrice in CalMat.Calculatrice.listMatrix)
                    {
                        binaryFormat.Serialize(file, matrice); // pour chaque matrice, on l'enregistre dans le fichier
                    }
                    file.Close(); // on ferme le fichier

                    TxtBox_console.AppendText("Fichier enregistre\n"); // confirmation de la sauvegarde
                }

                catch (Exception l)
                {
                    TxtBox_console.AppendText("Fichier non enregistre. erreur : " + l.Message + "\n"); // si il y a un problème, on l'affiche
                }

            }
        } // événement quand on clique sur le bouton "Enregistrer"

        public void Help(object sender, RoutedEventArgs e) // événement quand on clique sur le bouton "Help"
        {
            HelpWindow helpWindow = new HelpWindow(); //on crée une nouvelle page HelpWindow
            helpWindow.Show(); //on affiche la fenêtre
        }

        private void TxtBox_command_GotFocus(object sender, RoutedEventArgs e) // événement quand la ligne d'écriture est sélectionné
        {
            if (this.TxtBox_command.Text == "Entrez votre calcul")
            {
                this.TxtBox_command.Text = ""; // on retire l'écriture par default
            }
        }

        private void Btn_calcul_Click(object sender, RoutedEventArgs e) // événement pour parser la commande écrite en appuyant sur le bouton "Executer"
        {
            CalculProcess(); //on parse la commande
        }

        private void ListBox_display_MouseDown(object sender, MouseButtonEventArgs e) // événement quand on clique sur rien
        {
           ListBox_display.UnselectAll(); // on désélectionne tout
        }

        private void ListBox_display_MouseRightButtonDown(object sender, MouseButtonEventArgs e) // événement quand on fait un clique droit sur une matrice
        {
            try
            {
                ContextMenu cm = this.FindResource("cmitem") as ContextMenu; //on charge le menu
                cm.IsOpen = true; // on ouvre le menu
            }
            catch(Exception)
            {
                
            }
        }

        private void TxtBox_command_KeyDown(object sender, KeyEventArgs e) // événement pour parser la commande écrite en appuyant sur ENTRER
        {
           if( e.Key == Key.Enter)
           {
               CalculProcess();
           }
        }

        public void Modify(object sender, RoutedEventArgs e) //événement pour modifier la matrice sélctionnée 
        {
            try
            {
                Matrix m = ((KeyValuePair<String, Matrix>)ListBox_display.Items.GetItemAt(ListBox_display.SelectedIndex)).Value; // on récupère la matrice sélectionnée
                MatrixInput MatrixInput = new MatrixInput(m, true); // on crée la page pour modifier en chargeant les valeurs de la matrice.
                MatrixInput.Show(); // on affiche la fenêtre
            }
            catch(Exception)
            {
                TxtBox_console.AppendText("La matrice n'a pas peu etre modifiee\n"); // si il y a un problème, on afffiche une erreur
            }
        }

        public void Delete(object sender, RoutedEventArgs e) //événement pour supprimer la matrice séléctionnée 
        {
            try
            {
            Matrix m = ((KeyValuePair<String,Matrix>) ListBox_display.Items.GetItemAt(ListBox_display.SelectedIndex)).Value; // on récupère la matrice sélectionnée
            CalMat.Calculatrice.listMatrix.Remove(m.name); // on la supprime du dictionnaire
            ListBox_display.Items.RemoveAt(ListBox_display.SelectedIndex); // on la supprime de l'affichage
            }
            catch(Exception)
            {
                TxtBox_console.AppendText("La matrice n'a pas peu etre suprimee\n"); // si il y a un problème, on afffiche une erreur
            }
            
            
        }
        #endregion
    }
}
