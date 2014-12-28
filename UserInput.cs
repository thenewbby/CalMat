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
         // defini des modeles de commande
         private static Regex cal_rgx           = new Regex(@"^(\w+)\=(\w)([\+\-\*])(\w)$"); // A=B+C
         private static Regex init_rgx         = new Regex(@"^(\w+)\=\[(\w+),(\w+)\]$"); //A=[2,2]
         private static Regex showCal_rgx       = new Regex(@"^(\w+)([\+\-\*])(\w+)$"); //A*B
         private static Regex exit_rgx          = new Regex(@"(exit)"); //exit
         private static Regex meth_rgx          = new Regex(@"^(\w+)\((\w)\)$"); // trans(A)
         private static Regex methaff_rgx       = new Regex(@"^(\w+)\=(\w+)\((\w)\)$"); //B=trans(A)
         private static Regex affMat_rgx        = new Regex(@"^(\w+)$"); //N
         private static Regex power_rgx         = new Regex(@"^(\w+)\^(\w+)$"); //A^3
         private static Regex powerAssigne_rgx  = new Regex(@"^(\w+)\=(\w+)\^(\w+)$"); //B=A^5

         static MatchCollection matches;
         private static void Create( string operande1, string operande2, string Mtrx_assign_name) // méthode pour crée une matrice "principal"
         {
             string mtrx_operande = Calculatrice.listMatrix.ContainsKey(operande1) ? operande1 : operande2; // si l'un des deux matrices existe
             if (Calculatrice.listMatrix[mtrx_operande].Lines == Calculatrice.listMatrix[mtrx_operande].Columns) 
             {
                 SquareMatrix matrix_assign = new SquareMatrix(Calculatrice.listMatrix[mtrx_operande].Lines, Mtrx_assign_name); //si c'est une matrice carrée
             }
             else
             {
                 Calculatrice.listMatrix.Add(Mtrx_assign_name, null); //si c'est une matrice classique
             }
         }

         public static String AssignCalcul () // méthode pour affecter une valeur venant d'un calcul matriciel à une autre matrice
         {
             //defini les paramètres donnés par l'utilisateur
             string operateur        = matches[0].Groups[3].ToString();
             string operande1        = matches[0].Groups[2].ToString();
             string operande2        = matches[0].Groups[4].ToString();
             string Mtrx_assign_name = matches[0].Groups[1].ToString();

             switch (operateur)
             {
                 case "+": // cas si l'opérateur est +
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                         {
                             Create(operande1, operande2, Mtrx_assign_name); // si la matrice de gauche n'existe pas on la créer
                         }

                         Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] + Calculatrice.listMatrix[operande2]; // on lui affecte la valeur du calcul

                         return Calculatrice.listMatrix[Mtrx_assign_name].ToString(); // on retourne la valeur sous forme de string
                     }
                     break;

                 case "-": // cas si l'opérateur est -
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                         {
                             Create(operande1, operande2, Mtrx_assign_name); // si la matrice de gauche n'existe pas on la créer
                         }
                         Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] - Calculatrice.listMatrix[operande2]; // on lui affecte la valeur du calcul

                         return Calculatrice.listMatrix[Mtrx_assign_name].ToString(); // on retourne la valeur sous forme de string
                     }
                     break;

                 case "*": // cas si l'opérateur est *
                     if (Calculatrice.listMatrix.ContainsKey(operande1) || Calculatrice.listMatrix.ContainsKey(operande2)) // si au moins l'un des deux existe
                     {

                         if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name))
                         {
                             Create(operande1, operande2, Mtrx_assign_name); // si la matrice de gauche n'existe pas on la créer
                         }

                         if (!Calculatrice.listMatrix.ContainsKey(operande1)) //si c'est l'opérande 1 qui n'est pas dans le dictionnaire
                         {
                             Calculatrice.listMatrix[Mtrx_assign_name] = operande1 * Calculatrice.listMatrix[operande2]; // on affecte la valeur (on considère que l'opérande 1 est un nombre)
                         }
                         else if (!Calculatrice.listMatrix.ContainsKey(operande2)) //si c'est l'opérande 2 qui n'est pas dans le dictionnaire
                         {
                             Calculatrice.listMatrix[Mtrx_assign_name] = operande2 * Calculatrice.listMatrix[operande1]; // on affecte la valeur (on considère que l'opérande 2 est un nombre)
                         }
                         else // si les deux y sont
                         {
                             Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[operande1] * Calculatrice.listMatrix[operande2]; //on affecte la valeur du produit des deux matrices
                         }
                         return Calculatrice.listMatrix[Mtrx_assign_name].ToString();
                     }
                     break;

             }
             throw new Exception("Au moins une des deux matrices n'existe pas\n"); // sinon on affiche l'erreur
         }

         public static String Init () //méthode pour créer et initialiser une matrice
         {
             //defini les paramètres donnés par l'utilisateur
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             string dim1 = matches[0].Groups[2].ToString();
             string dim2 = matches[0].Groups[3].ToString();

             if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name)) //si la matrice n'existe pas
             {
                 if (dim1 == dim2)
                 {
                     int dim;
                     if (int.TryParse(dim1, out dim))
                     {
                         SquareMatrix matrix_creat = new SquareMatrix(dim, Mtrx_assign_name); // on crée une matrice carrée si les deux dimension son égale
                     }
                     else
                     {
                         throw new Exception ("Les dimentions données ne sont pas valide\n"); // si on arrvie pas à convertir les dimensions en int, on affiche l'erreur
                     }

                 }
                 else
                 {
                     int dim_1, dim_2;
                     if (int.TryParse(dim1, out dim_1) && int.TryParse(dim2, out dim_2))
                     {
                         Matrix matrix_creat = new Matrix(dim_1, dim_2, Mtrx_assign_name); //sinon on crée une matrice classique
                     }
                     else
                     {
                         return "Les dimentions données ne sont pas valide\n"; // si on arrvie pas à convertir les dimensions en int, on affiche l'erreur
                     }
                 }
                 Calculatrice.listMatrix[Mtrx_assign_name].Input(); // on appelle la méthode Input dans la classe Matrix pour initialiser la matrice
                 return ""; // sert juste pour que le programme compile
             }
             else
             {
                 throw new Exception("Il existe deja une matrice " + Mtrx_assign_name + "\n"); // sinon on affiche l'erreur
             }
         }

         public static String PrintCal () // méthode pour affichier les résultats d'un calcul matriciel
         {
             //defini les paramètres donnés par l'utilisateur
             string operande1 = matches[0].Groups[1].ToString();
             string operateur = matches[0].Groups[2].ToString();
             string operande2 = matches[0].Groups[3].ToString();

             switch (operateur)
             {
                 case "+": // cas si l'opérateur est +
                 
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         return (Calculatrice.listMatrix[operande1] + Calculatrice.listMatrix[operande2]).ToString(); // si les deux matrices existent, on retourne le résultat sous forme de string
                     }
                     break;

                 case "-": // cas si l'opérateur est -
                     if (Calculatrice.listMatrix.ContainsKey(operande1) && Calculatrice.listMatrix.ContainsKey(operande2))
                     {
                         return (Calculatrice.listMatrix[operande1] - Calculatrice.listMatrix[operande2]).ToString(); // si les deux matrices existent, on retourne le résultat sous forme de string
                     }
                     break;

                 case "*": // cas si l'opérateur est *
                     if (Calculatrice.listMatrix.ContainsKey(operande1) || Calculatrice.listMatrix.ContainsKey(operande2)) // si au moins l'un des deux existe
                     {
                        if (!Calculatrice.listMatrix.ContainsKey(operande1)) //si c'est l'opérande 1 qui n'est pas dans le dictionnaire
                        {
                             return (operande1 * Calculatrice.listMatrix[operande2]).ToString(); // on retourne la valeur sous forme de string (on considère que l'opérande 1 est un nombre)
                        }
                        else if (!Calculatrice.listMatrix.ContainsKey(operande2)) //si c'est l'opérande 2 qui n'est pas dans le dictionnaire
                        {
                            return (operande2 * Calculatrice.listMatrix[operande1]).ToString();// on retourne la valeur sous forme de string (on considère que l'opérande 2 est un nombre)
                        }
                        else // si les deux y sont
                        {
                             return (Calculatrice.listMatrix[operande1] * Calculatrice.listMatrix[operande2]).ToString(); //on retourne la valeur du produit des deux matrices sous forme de string
                        }
                     }
                     break;       
             }
             throw new Exception("Au moins une des deux matrices n'existe pas\n"); // sinon on affiche l'erreur
         }

         public static String Method() // méthode pour affiher les valeurs de méthode de calcul
         {
             //defini les paramètres donnés par l'utilisateur
             string method = matches[0].Groups[1].ToString();
             string matrix = matches[0].Groups[2].ToString();
             if (Calculatrice.listMatrix.ContainsKey(matrix))
             {
                 switch (method)
                 {
                     case "trans": // cas si la methode est trans

                         return (Calculatrice.listMatrix[matrix].Trans()).ToString(); //retourne la valeur de la transposée sour forme de string


                     case "det": // cas si la methode est det
                         if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                         {
                             return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Det()).ToString(); //si la matrice est carré, retourne la valeur du determinant sous forme de string
                         }
                         else
                         {
                             throw new Exception("La matrice " + matrix + " n'est pas une matrice carre\n"); // sinon retourne l'erreur
                         }

                     case "trace": // cas si la methode est trace
                         if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                         {
                             return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Trace()).ToString(); //si la matrice est carré, retourne la valeur de la trace sous forme de string
                         }
                         else
                         {
                             throw new Exception("La matrice " + matrix + " n'est pas une matrice carre\n"); // sinon retourne l'erreur
                         }

                     case "norme": // cas si la methode est norme
                         if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns)
                         {
                             return (((SquareMatrix)Calculatrice.listMatrix[matrix]).Norme()).ToString(); //si la matrice est carré, retourne la valeur de la norme infini sous forme de string
                         }
                         else
                         {
                             throw new Exception("La matrice " + matrix + " n'est pas une matrice carre\n"); // sinon retourne l'erreur
                         }
                     default:
                         throw new Exception("Cette fonction n'est pas definie.\n"); //sinon affiche l'erreur
                 }
             }
              throw new Exception("La matrice " + matrix + " n'existe pas\n"); // sinon retourne l'erreur
         }

         public static String MethodAffectation() //méthode pour affecter à une matrice une valeur avec une methode
         {
             //defini les paramètres donnés par l'utilisateur
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             string method = matches[0].Groups[2].ToString();
             string matrix = matches[0].Groups[3].ToString();
             if (Calculatrice.listMatrix.ContainsKey(matrix))
             {
                 if (method == "trans") //si la méthode est trans
                 {
                     if (!Calculatrice.listMatrix.ContainsKey(Mtrx_assign_name)) // si la matrice d'assignation n'existe pas 
                     {
                         if (Calculatrice.listMatrix[matrix].Lines == Calculatrice.listMatrix[matrix].Columns) //si la matrice de droite est carrée
                         {
                             SquareMatrix matrix_assign = new SquareMatrix(Calculatrice.listMatrix[matrix].Lines, Mtrx_assign_name); //on crée une nouvelle matrice carrée de même dimension
                         }
                         else
                         {
                             Calculatrice.listMatrix.Add(Mtrx_assign_name, null); //sinon on crée une matrice classique
                         }
                     }
                     Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[matrix].Trans(); // on donne les valeurs à la matrice de gauche

                 }
                 return (Calculatrice.listMatrix[Mtrx_assign_name]).ToString(); // on retourne la valeur sous string
             }
             throw new Exception("La matrice " + matrix + " n'existe pas\n"); // sinon retourne l'erreur
         }

         public static String Print () // méthode pour afficher la valeur de la matrice
         {
             //defini les paramètres donnés par l'utilisateur
             string matrix = matches[0].Groups[1].ToString();

             if (Calculatrice.listMatrix.ContainsKey(matrix))
             {
                 return Calculatrice.listMatrix[matrix].ToString(); //si la matrice existe, on retourne sa valeur sous forme de string
             }
             else
             {
                 throw new Exception("La matrice n'existe pas\n"); //sinon on retourne l'erreur
             }
         }

         public static String Power() //méthode pour faire la puissance d'une matrix
         {
             //defini les paramètres donnés par l'utilisateur
             string matrix = matches[0].Groups[1].ToString(); 
             string power  = matches[0].Groups[2].ToString();

            if (Calculatrice.listMatrix.ContainsKey(matrix))
            {
                return Calculatrice.listMatrix[matrix].Pow(power).ToString(); // si la matrice existe, on retourne la valeur de matrix^power sous forme de string
            }
            throw new Exception("La matrice n'existe pas"); // sinon, on affiche l'erreur
         }

         public static String PowerAssigne() // methode pour faire un puissance d'une matrice et affecter la valeur
         {
             //defini les paramètres donnés par l'utilisateur
             string Mtrx_assign_name = matches[0].Groups[1].ToString();
             string matrix           = matches[0].Groups[2].ToString();
             string power            = matches[0].Groups[3].ToString();

             Create(matrix, matrix, Mtrx_assign_name); //on crée la matrice

             Calculatrice.listMatrix[Mtrx_assign_name] = Calculatrice.listMatrix[matrix].Pow(power); // on iniialise la matrice avec le calcul matrix^power
             return Calculatrice.listMatrix[Mtrx_assign_name].ToString(); // on retourne la valeur de matrix^power sous forme de string

         }

         public static String parse(string commande, ref string error) // procédure de parsing
         {
             commande = commande.Replace(" ", ""); // on enlève tout les espace
             matches = cal_rgx.Matches(commande); // on regarde si la commande est de la forme A=B+C

             if (matches.Count > 0)
             {
                 try
                 {
                     return AssignCalcul(); //si oui on essaye la méthode AssignCalcul
                 }
                 catch (Exception e)
                 {
                    return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = init_rgx.Matches(commande); // on regarde si la commande est de la forme A=[2,2]

             if (matches.Count > 0)
             {
                 try
                 {
                     return Init(); //si oui on essaye la méthode Init
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = showCal_rgx.Matches(commande); // on regarde si la commande est de la forme B+C

             if (matches.Count > 0)
             {
                 try
                 {
                     return PrintCal(); //si oui on essaye la méthode PrintCal
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = exit_rgx.Matches(commande); // on regarde si la commande est de la forme exit

             if (matches.Count>0)
             {
                 System.Diagnostics.Process.GetCurrentProcess().Kill(); // on ferme le programme
             }

             matches = meth_rgx.Matches(commande); // on regarde si la commande est de la forme trans(A)

             if(matches.Count > 0)
             {
                 try
                 {
                     return Method(); //si oui on essaye la méthode Method
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = methaff_rgx.Matches(commande); // on regarde si la commande est de la forme B=trans(A)

             if(matches.Count >0)
             {
                 try
                 {
                     return MethodAffectation();  //si oui on essaye la méthode MethodAffectation
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = affMat_rgx.Matches(commande); // on regarde si la commande est de la forme A

             if (matches.Count > 0)
             {
                 try
                 {
                     return Print(); //si oui on essaye la méthode Print
                 }
                 catch(Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }

             matches = power_rgx.Matches(commande); // on regarde si la commande est de la forme A^2

             if (matches.Count > 0)
             {
                 try
                 {
                     return Power(); //si oui on essaye la méthode Power
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }
             matches = powerAssigne_rgx.Matches(commande); // on regarde si la commande est de la forme A=B^2
              
             if (matches.Count > 0)
             {
                 try
                 {
                     return PowerAssigne(); //si oui on essaye la méthode PowerAssigne
                 }
                 catch (Exception e)
                 {
                     return error = e.Message; // si il y a un problème, on récupère l'erreur et on l'affiche
                 }
             }
             
             if (commande.Length == 0) // on regarde si la commande a une longueur null
             {
                 return error = "\n";
             }

             return error = "L'expression n'est pas valable.\n"; // si la commande ressemble à rien
         }
    }
}

