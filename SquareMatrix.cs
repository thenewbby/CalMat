using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalMat
{
    [Serializable]
    class SquareMatrix : Matrix //defini une classe SquareMatrix qui hérite de la classe Matrix
    {
        public SquareMatrix(int c) : base(c, c) //constructeur de la classe SquareMatrix pour les matrices carrée "de passage" 
        {
            this.Elements = new double[c, c];
            this.Lines    = c;
            this.Columns  = c;
        }

        public SquareMatrix(int c, string  nom) : base(c,c,nom) //constructeur de la classe SquareMatrix pour les matrices carrée "de passage"
        {
            this.Elements = new double[c, c];
            this.Lines    = c;
            this.Columns  = c;
            this.name     = nom; //pas besoin d'ajouter dans le dico car appele la base
        }

        public new SquareMatrix Trans() //méthode pour faire la tranposée d'une matrice carrée
        {
            SquareMatrix T = new SquareMatrix(this.Lines); //crée une matrice carrée de même dimention que la matrice this
            for (int i = 0; i < this.Lines; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    T.Elements[i, j] = this.Elements[j, i]; //on tranpose la matrice this
                }
            }
            return T; //retourne la matrice T de passage
        }

        public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b) //operateur '+': defini une methode de calcul avec l'operateur '+' et deux matrices carrées
        {
            SquareMatrix c = a + b; //crée une matrice carrée avec les valeurs a + b (appel la méthode  "public static Matrix operator +(Matrix a, Matrix b)" dans la classe Matrix
            return c; //retoune c
        }

        public static SquareMatrix operator -(SquareMatrix a, SquareMatrix b) //operateur '-': defini une methode de calcul avec l'operateur '-' et deux matrices carrées
        {
            SquareMatrix c = a - b; //crée une matrice carrée avec les valeurs a - b (appel la méthode  "public static Matrix operator -(Matrix a, Matrix b)" dans la classe Matrix
            return c;
        }


        public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b) //operateur '*': defini une methode de calcul avec l'operateur '*' et deux matrices carrées
        {
            SquareMatrix c = a * b;
            return c; //retoune c
        }


        public static SquareMatrix operator *(double a, SquareMatrix b) //operateur '*': defini une methode de calcul avec l'operateur '*', un double et une matrice carrée
        {
            SquareMatrix c = a * b;
            return c; //retoune c
        }

        /*protected int SubMatrix(SquareMatrix a, int x)
        {
            if (a.Columns == 2)
            {
                return a.Elements[0, 0] * a.Elements[1, 1] - a.Elements[0, 1] * a.Elements[1, 0];
            }
            else
            {
                SquareMatrix b = new SquareMatrix(a.Lines - 1);
                int f = 0, g = 0, interDet = 0;
                for (int i = 1; i < a.Lines; i++)
                {
                    g = 0;
                    for(int j = 0; j < a.Columns; j++)
                    {
                        if (j == x)
                        {
                            continue;
                        }
                        else
                        {
                            b.Elements[f, g] = a.Elements[i, j];
                            g++;
                        }
                    }
                    f ++;
                }
                Console.WriteLine("\n");
                for(int i = 0;i < a.Lines; i++)
                {
                   interDet += b.SubMatrix(b,i) * (int) Math.Pow(-1,i)* a.Elements[0,i];
                   Console.WriteLine("det:" + interDet);
                   //Console.WriteLine("\n");
                }
                

                return interDet;
               
            }
        }


        public int Det()
        {
                if (this.Lines == 1)
                {
                    return this.Elements[0, 0];
                }
                else
                {
                    int det = 0;
                    for ( int j = 0; j < this.Columns; j++)
                    {
                        det += (int) Math.Pow(-1, j) * this.Elements[0, j] * this.SubMatrix(this, j);
                        

                    }
                    Console.WriteLine(det);
                    return det;
                }
         }
        */


        /*public int Det ()
        {
            if (this.Lines == 1)
            {
                return this.Elements[0, 0];
            }
            else if (this.Columns == 2)
            {
                return this.Elements[0, 0] * this.Elements[1, 1] - this.Elements[0, 1] * this.Elements[1, 0];
            }
            else
            {
                SquareMatrix subMatrix = new SquareMatrix(this.Lines - 1);
                int f, g, det = 0;
                for(int x = 0 ; x < this .Columns; x++)
                {
                    f = 0;
                    for (int i = 1; i < this.Lines; i++)
                    {
                        g = 0;
                        for(int j = 0; j < this.Columns; j++)
                        {
                            if (j != x)
                            {
                                subMatrix.Elements[f, g] = this.Elements[i, j];
                                g++;
                            }
                        }
                        f ++;
                    }
                    Console.WriteLine("\n");
                    Console.Write(subMatrix);
                    for(int i = 0;i < this.Lines; i++)
                    {
                        det += subMatrix.Det() * (int) Math.Pow(-1,i)* this.Elements[0,i];
                        
                    }
                }
                Console.WriteLine("det");
                return det;
            }
        }*/

        public Object Det()
        {
            int Max;
            double k, det = 1;
            SquareMatrix c = this;
            double[,] Array = new double[this.Columns, 1];

            for (int j = 0; j < this.Columns; j++)
            {
                Max = j;
                for (int i = j; i < this.Lines - 1; i++)
                {
                    if (this.Elements[j, i] > this.Elements[Max, i])
                    {
                        Max = j;
                    }
                }
                for (int x = 0; x < this.Columns; x++)
                {
                    Array [x,0] = c.Elements[x,j];
                }
                for (int x = 0; x < this.Columns; x++)
                {
                    c.Elements[x, j] = c.Elements[x, Max];
                }
                for (int x = 0; x < this.Columns; x++)
                {
                    c.Elements[x, j] = Array[x, 0];
                }
                
                for (int i = j+1; i <c.Lines; i++ )
                {
                    k = c.Elements[j,i] / c.Elements[j,j];
                    for (int x = 0; x < c.Columns; x++)
                    {
                        c.Elements[x, i] -= k * c.Elements[x, j];
                    }
                    
                }

            }

            for (int x = 0; x < c.Lines; x++ )
            {
                det *= c.Elements[x, x];
            }
            return  det;
        }

        public Object Trace()
        {
            double Trac = 0;

            for (int i = 0; i < this.Lines; i++)
            {
                Trac += this.Elements[i, i];
            }
            return Trac ;
        }
        
        public Object Norme()
        {

            double sum = 0;
            double max = 0;

            SquareMatrix N = this;

            for (int j = 0 ; j < this.Columns; j++)
            {
                sum = 0;
                for (int i = 0 ; i < this .Lines; i++)
                {
                    sum += Math.Abs( N.Elements[j,i]); // norme 1
                }
                if ( max < sum)
                {
                    max = sum;
                }
            }
            return max;
        }

    }            
}


