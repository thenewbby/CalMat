using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CalMat
{
    class Calculatrice
    {
        public static Dictionary<String, Matrix> listMatrix = new Dictionary<String, Matrix>(); //je crée un dictionaire (un tableau de hashage) 
        public static MainWindow mainWindow;  //ceci est le pointeur vers la classe MainWindow
    }
}
