using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalMat
{
    [Serializable]
    public class Matrix //defini une classe Matrix qui a comme attribut  
    {
        public int Lines          { get; set; }//Lines(nb de ligne)
        public int Columns        { get; set; }//Columns(nb de colomns)
        public double[,] Elements { get; set; }//un tableau a deux dimensions(la matrice)
        public string name        { get; set; }//name(le nom de la Matrice)

        public Matrix(int c, int l) //constructeur de la classe Matrix pour les matrices "de passage"
        {
            this.Elements = new double[c, l];
            this.Lines    = l;
            this.Columns  = c;
        }

        public Matrix(int c, int l, string nom) //constructeur de la classe Matrix pour les matrices "principales"
        {
            this.Elements = new double[c, l];
            this.Lines    = l;
            this.Columns  = c;
            this.name     = nom;
            if (!Calculatrice.listMatrix.ContainsKey(this.name)) //on ajoute la matrice dans le dictionaire si elle n'y est pas
            {
                Calculatrice.listMatrix.Add(this.name, this);
            }
            
        }

        public static Matrix operator +(Matrix a, Matrix b) //operateur '+': defini une methode de calcul avec l'operateur '+' et deux matrices
        {
            if (a.Lines == b.Lines && a.Columns == b.Columns) //verifie si les matrices sont compatible
            {
                Matrix c = new Matrix(a.Columns, a.Lines); //crée une matrice de passage de meme dimension que la matrice a et b
                for (int i = 0; i < a.Lines; i++)
                {
                    for (int j = 0; j < a.Columns; j++) //parcours la matrice c, a et b
                    {
                        c.Elements[j, i] = a.Elements[j, i] + b.Elements[j, i]; //somme des deux termes correspondants
                    }
                }
                return c; //retourne la matrice de passage
            }
            else
            {
                throw new Exception("Opération impossible : dimensions non compatibles. \n"); //sinon retoune une erreur
            }
        }

        public static Matrix operator -(Matrix a, Matrix b) //operateur '-': defini une méthode de calcul avec l'operateur '-' et deux matrices
        {
            if (a.Lines == b.Lines && a.Columns == b.Columns) //verifie si les matrices sont compatible
            {
                Matrix c = new Matrix(a.Columns, a.Lines); //crée une matrice de passage de meme dimension que la matrice a et b
                for (int i = 0; i < a.Lines; i++)
                {
                    for (int j = 0; j < a.Columns; j++) //parcours la matrice c, a et b
                    {
                        c.Elements[j, i] = a.Elements[j, i] - b.Elements[j, i];//difference des deux termes correspondants
                    }
                }
                return c; //retourne la matrice de passage
            }
            else
            {
                throw new Exception("Operation impossible: pas les même dimension.\n"); //sinon retoune une erreur
            }
        }

        public static Matrix operator *(Matrix a, Matrix b) //operateur '*': defini une méthode de calcul avec l'operateur '*' et deux matrices
        {
            if (a.Lines == b.Columns)  //verifie si les matrices sont compatible
            {
                Matrix c = new Matrix(b.Columns, a.Lines); //crée une matrice de passage de dimension juste
                for (int i = 0; i < a.Lines; i++)
                {
                    for (int j = 0; j < b.Columns; j++) //parcours la matrice c
                    {
                        for (int z = 0; z < a.Columns; z++) //parcours la ligne de la matrice a et la colonne de la matrice b correspondante
                        {
                            c.Elements[j, i] += a.Elements[z, i] * b.Elements[j, z]; //somme des produits des termes correspondant
                        }
                    }
                }
                return c; //retourne la matrice de passage
            }
            else
            {
                throw new Exception("Operation impossible: le nombre de ligne de la première matrice n'est pas egale au nombre de collones de la deuxième matrice.\n");
            }    //sinon retoune une erreur
        }

        public static Matrix operator *(string a, Matrix b) //operateur '*': defini une méthode de calcul avec l'operateur '*', un string et une matrice
        {
            double value; //defini une variable value en tant que double
            if (Double.TryParse(a, out value)) //essaye de convertir le string en double
            {
                return value * b; // si réussi, appel l'operateur '*' avec double et une matrice
            }
            throw new Exception("Impossible de convertir '" + a + "' en double dans l'operateur * de matrix.\n"); //sinon retourne une erreur
        }

        public static Matrix operator *(double a, Matrix b) //operateur '*': defini une méthode de calcul avec l'operateur '*', un double et une matrice
        {
            Matrix c = new Matrix(b.Lines, b.Columns); //crée une matrice de passage de dimension de la matrice b
            for (int i = 0; i < b.Lines; i++)
            {
                for (int j = 0; j < b.Columns; j++) //parcours la matrice c et b
                {
                    c.Elements[i, j] = a * b.Elements[i, j]; //produit du double avec les termes correspondant
                }
            }
            return c; //retourne la matrice c
        }

        public void Input() //méthode qui appel la fenetre MatrixInput pour initialiser la matrice
        {
            MatrixInput MatrixInput = new MatrixInput(this, false); //crée une nouvelle fenêtre avec comme paramètre this (la matrice) et un booleen (false qui determine si on modifie ou non la matrice)
            MatrixInput.Show(); //on affiche la fenêtre
        }

        public Matrix Trans() //defini une methode pour la transposer
        {
            Matrix T = new Matrix(this.Lines, this.Columns); //crée une matrice de passage de meme dimension que la matrice this
            for (int i = 0 ; i < this.Lines ; i++)
            {
                for (int j = 0 ; j < this.Columns ; j++) //parcours la matrice T et this
                {
                    T.Elements[i, j] = this.Elements[j, i]; //defini la transposee de this
                }
            }
            return T; //retourne la matrice T
        }

        public override string ToString() // méthode pour mettre la matrice sous forme de string
        {
            String value = ""; //defini une variable value en tant que string et est egale a rien

            for (int j = 0; j < this.Lines; j++) //parcours les lignes de la matrice this
            {
                value += "\t| "; //ajoute une tabulation et |
                for (int i = 0; i < this.Columns; i++) //parcours les colonnes de la matrice this
                {
                    value += Elements[i, j]; //ajoute la valeurs de la case correspondante
                    if (i != this.Columns -1) 
                    {
                        value += "\t"; //si ce n'est pas la l'avant deriere colonne, on ajoute une tabulation
                    }
                }
                value += " |\n"; //ajoute | et un retour a la ligne

            }
            return value; //retourne value
        }
    }
}
