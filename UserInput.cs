using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CalMat
{
     class UserInput    
         
    {
         private static Regex cal_rgx     = new Regex(@"^(\w+)\=(\w)([\+\-\*])(\w)$");
         private static Regex creat_rgx   = new Regex(@"^(\w+)\=\[(\w+),(\w+)\]$");
         private static Regex showCal_rgx = new Regex(@"^(\w+)([\+\-\*])(\w+)$");
         private static Regex exit_rgx    = new Regex(@"(exit)");
         private static Regex meth_rgx    = new Regex(@"^(\w+)\((\w)\)$");
         private static Regex methaff_rgx = new Regex(@"^(\w+)\=(\w+)\((\w)\)$");
         private static Regex affMat_rgx  = new Regex(@"^(\w+)$");
         //private static Regex delete_rgx  = new Regex(@"^(delete)\((\w+)\)$");

         static MatchCollection matches;
         private static void Create( string operande1, string operande2, string Mtrx_assign_name)
         {
             string mtrx_operande = Calculatrice.listMatrix.ContainsKey(operande1) ? operande1 : operande2;
             if (Calculatrice.listMatrix[mtrx_operande].Lines == Calculatrice.listMatrix[mtrx_operande].Columns)
             {
                 SquareMatrix matrix_assign = new SquareMatrix(Calculatrice.listMatrix[mtrx_operande].Lines, Mtrx_assign_name);
             }
             else
             {
                 Calculatrice.listMatrix.Add(Mtrx_assign_name, null);
                 //Matrix matrix_assign = new Matrix(CalMat.listMatrix[operande2].Columns, CalMat.listMatrix[operande1].Lines, Mtrx_assign_name);
             }
         }

         public static String AssignCalcul ()
         {
             string operateur = matches[0].Groups[3].ToString();
             string operande1 = matches[0].Groups[2].ToString();
             string operande2 = matches[0].Groups[4].ToString();
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             switch (operateur)
             {
                 case "+":
                     if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                     {
                         Create(operande1, operande2, Mtrx_assign_name);
                     }

                     Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] + Calculatrice.listMatrix[operande2];

                     return Calculatrice.listMatrix[Mtrx_assign_name].ToString();
                 case "-":
                     if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                     {
                         Create(operande1, operande2, Mtrx_assign_name);
                     }
                     Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] - Calculatrice.listMatrix[operande2];

                     return Calculatrice.listMatrix[Mtrx_assign_name].ToString();
                 case "*":
                     if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                     {
                         Create(operande1, operande2, Mtrx_assign_name);
                     }

                     if (!Calculatrice.listMatrix.ContainsKey(operande1))
                     {
                             Calculatrice.listMatrix[Mtrx_assign_name] = operande1 * Calculatrice.listMatrix[operande2];
                     }
                     else if (!Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                             Calculatrice.listMatrix[Mtrx_assign_name] = operande2 * Calculatrice.listMatrix[operande1];
                     }
                     else
                     {
                             Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] * Calculatrice.listMatrix[operande2];
                     }
                     return Calculatrice.listMatrix[Mtrx_assign_name].ToString();
             }
             return "";

         }

         public static String Init () 
         {
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             string dim1 = matches[0].Groups[2].ToString();
             string dim2 = matches[0].Groups[3].ToString();

             if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
             {
                 if (dim1 == dim2)
                 {
                     int dim;
                     if (int.TryParse(dim1, out dim))
                     {
                         SquareMatrix matrix_creat = new SquareMatrix(dim, Mtrx_assign_name);
                     }
                     else
                     {
                         throw new Exception ("Les dimentions données ne sont pas valide\n");
                     }

                 }
                 else
                 {
                     int dim_1, dim_2;
                     if (int.TryParse(dim1, out dim_1) && int.TryParse(dim2, out dim_2))
                     {
                         Matrix matrix_creat = new Matrix(dim_1, dim_2, Mtrx_assign_name);
                     }
                     else
                     {
                         return "Les dimentions données ne sont pas valide\n";
                     }
                 }
                 Calculatrice.listMatrix[Mtrx_assign_name].Input();
                 return "";
                 //return calculatrice.listMatrix[Mtrx_assign_name].ToString();
             }
             else
             {
                 throw new Exception("Il existe deja une matrice " + Mtrx_assign_name + "\n");
             }
         }

         public static String PrintCal ()
         {
             string operande1 = matches[0].Groups[1].ToString();
             string operateur = matches[0].Groups[2].ToString();
             string operande2 = matches[0].Groups[3].ToString();

             switch (operateur)
             {
                 case "+":
                 
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         return (Calculatrice.listMatrix[operande1] + Calculatrice.listMatrix[operande2]).ToString();
                     }
                     
                     
                     break;
                   
                 case "-":
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         return (Calculatrice.listMatrix[operande1] - Calculatrice.listMatrix[operande2]).ToString();
                     }
                     break;
                     
                 case "*":
                     if (Calculatrice.listMatrix.ContainsKey(operande1) || Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                        if (!Calculatrice.listMatrix.ContainsKey(operande1))
                        {
                             return (operande1 * Calculatrice.listMatrix[operande2]).ToString();
                        }
                        else if (!Calculatrice.listMatrix.ContainsKey(operande2))
                        {
                             return (operande2 * Calculatrice.listMatrix[operande1]).ToString();
                        }
                        else
                        {
                             return (Calculatrice.listMatrix[operande1] * Calculatrice.listMatrix[operande2]).ToString();
                        }
                     }
                     break;
                     
             }
             throw new Exception("Au moins une des deux matrices n'existe pas\n");
         }

         public static String Method ()
         {
             string method = matches[0].Groups[1].ToString();
             string matrix = matches[0].Groups[2].ToString();

             if (method == "trans")
             {
                 if((Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns))
                 {
                     SquareMatrix m = (SquareMatrix) Calculatrice.listMatrix[matrix];
                    return (m.Trans()).ToString();
                 }
                 else
                 {
                     return (Calculatrice.listMatrix[matrix].Trans()).ToString();
                 }
             }
             else if (method == "det")
             {
                 if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                 {
                     return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Det()).ToString();
                 }
                 else
                 {
                     throw new Exception ("La matrice " + matrix +" n'est pas une matrice carre\n");
                 }
             }
             else if (method == "trace")
             {
                 if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                 {
                     return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Trace()).ToString();
                 }

             }
             else if (method == "norme")
             {
                 if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                 {
                     return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Norme()).ToString();
                 }
             }
             throw new Exception ("Cette fonction n'est pas definie.\n");
         }

         public static String MethodAffectation()
         {
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             string method = matches[0].Groups[2].ToString();
             string matrix = matches[0].Groups[3].ToString();
             if (method == "trans")
             {
                 if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                 {
                     if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                     {
                         SquareMatrix matrix_assign = new SquareMatrix(Calculatrice.listMatrix[matrix].Lines, Mtrx_assign_name);
                     }
                     else
                     {
                         Calculatrice.listMatrix.Add(Mtrx_assign_name, null);
                     }
                 }
                 Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[matrix].Trans();
                
             }
              return (Calculatrice.listMatrix[Mtrx_assign_name]).ToString(); // pour l'afficher
         }

         public static String Print ()
         {
             String matrix = matches[0].Groups[1].ToString();
             if (Calculatrice.listMatrix.ContainsKey(matrix))
             {
                 return Calculatrice.listMatrix[matrix].ToString();
             }
             else
             {
                 throw new Exception("La matrice n'existe pas\n");
             }
         }

        /* public static String Delete () //a finir
         {
             String matrix = matches[0].Groups[2].ToString();
             if (Calculatrice.listMatrix.ContainsKey(matrix))
             {
                 Matrix m = Calculatrice.listMatrix[matrix];
                 //Calculatrice.mainWindow.ListBox_display.Items.Remove();
                 int index = Calculatrice.mainWindow.ListBox_display.Items.IndexOf( m);
                 //int index = Calculatrice.mainWindow.ListBox_display(m.name);
                 //Calculatrice.mainWindow.ListBox_display.Items.Refresh(); //A finir 
                 Calculatrice.mainWindow.ListBox_display.Items.RemoveAt(index);
                 Calculatrice.listMatrix.Remove(matrix);
                 return "Fait!\n";
             }
             throw new Exception("La matrice n'existe pas.\n");
         } 
         */

         public static String parse(string control, ref string error)
         {
             control = control.Replace(" ", "");
             matches = cal_rgx.Matches(control);

             if (matches.Count > 0)
             {
                 try
                 {
                     return AssignCalcul();
                 }
                 catch (Exception e)
                 {
                    return error = e.Message;
                 }
             }
             /*matches = delete_rgx.Matches(control);

             if (matches.Count > 0)
             {
                 try
                 {
                     return Delete();
                 }
                 catch (Exception e)
                 {
                     return error = e.Message;
                 }
             }*/

             matches = creat_rgx.Matches(control);

             if (matches.Count > 0)
             {
                 try
                 {
                     return Init();
                 }
                 catch (Exception e)
                 {
                    return error = e.Message;
                 }
             }

             matches = showCal_rgx.Matches(control);

             if (matches.Count > 0)
             {
                 try
                 {
                     return PrintCal();
                 }
                 catch (Exception e)
                 {
                     return error = e.Message;
                 }
             }

             matches = exit_rgx.Matches(control);

             if (matches.Count>0)
             {
                 System.Diagnostics.Process.GetCurrentProcess().Kill();
             }

             matches = meth_rgx.Matches(control);

             if(matches.Count > 0)
             {
                 try
                 {
                     return Method();
                 }
                 catch (Exception e)
                 {
                     return error = e.Message;
                 }
             }

             matches = methaff_rgx.Matches(control);

             if(matches.Count >0)
             {
                 return MethodAffectation();
             }

             matches = affMat_rgx.Matches(control);

             if (matches.Count > 0)
             {
                 try
                 {
                     return Print();
                 }
                 catch(Exception e)
                 {
                    return error = e.Message;
                 }
             }
             
             if (control.Length == 0)
             {
                 return error = "\n";
             }

             return error = "L'expression n'est pas valable.\n";
         }
    }
}

