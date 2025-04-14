using System.Collections.Concurrent;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace ConstraintSatisfactionAlgorith
{
    internal class Program
    {
        static List<Color> colors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Purple };


        public static List<int>[,] GetPossibilityTable
        {
            get { 
                var res = new List<int>[9, 9];

                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        // Dette er domænerne
                        res[x, y] = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                }
                return res;
            }
        }

        public static List<int>[,] CopyPossibilitySpace(List<int>[,] origin)
        {
            List<int>[,] res = new List<int>[9,9];

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    res[x, y] = new List<int>();
                    foreach (var item in origin[x,y])
                    {
                        res[x, y].Add(item);
                    }
                    
                }
            }
            return res;
        }



        static void Main(string[] args)
        {
            #region colormap
            List<CanadaState> list = new List<CanadaState>();
            CanadaState WA = new CanadaState("WA");
            CanadaState SA = new CanadaState("SA");
            CanadaState NT = new CanadaState("NT");
            CanadaState Q = new CanadaState("Q");
            CanadaState V = new CanadaState("V");
            CanadaState NSQ = new CanadaState("NSQ");
            CanadaState T = new CanadaState("T");


            WA.Neighbours.Add(NT);
            WA.Neighbours.Add(SA);

            SA.Neighbours.Add(NT);
            SA.Neighbours.Add(WA);
            SA.Neighbours.Add(Q);
            SA.Neighbours.Add(NSQ);
            SA.Neighbours.Add(V);

            Q.Neighbours.Add(NSQ);
            Q.Neighbours.Add(SA);
            Q.Neighbours.Add(NT);

            NSQ.Neighbours.Add(SA);
            NSQ.Neighbours.Add(Q);
            NSQ.Neighbours.Add(V);

            V.Neighbours.Add(SA);
            V.Neighbours.Add(NSQ);

            list.AddRange(new List<CanadaState>() { WA, SA, NT, Q, V, NSQ, T });

            PlaceColorSimpleBackTracking(0, 0, list);

            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Name + item.Color);
            //}

            #endregion

            #region SudokuSolver

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    
                    Sudoku[x, y] = 0;
                }
            }

            Sudoku[0, 2] = 9;
            Sudoku[0, 3] = 1;
            Sudoku[1,6] = 2;
            Sudoku[2, 4] = 6;
            Sudoku[2, 6] = 3;
            Sudoku[3, 7] = 5;
            Sudoku[4, 0] = 3;
            Sudoku[5, 3] = 9;
            Sudoku[5, 5] = 5;
            Sudoku[5, 6] = 7;
            Sudoku[5, 6] = 1;
            Sudoku[6, 0] = 6;
            Sudoku[6, 4] = 3;
            Sudoku[7, 2] = 4;
            Sudoku[7, 7] = 9;
            Sudoku[8, 4] = 2;
            Sudoku[8, 5] = 8;

            InitialiseSudokuSolveFlexness(Sudoku);
            
            #endregion


        }

        public static List<int>[,] InitialiseSudokuSolveFlexness( int[,] puzzle)
        {
            var possibilities = GetPossibilityTable;
            //Start her

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    UpdatePossibilitySpace(x, y, puzzle[x,y],possibilities);
                }
            }


            Solve(possibilities,0,1);

            return possibilities;

        }

        public static int Convert2DPositionTo1DIndex(int x, int y, int width)
        {
            return (y * width) + x;
        }

        public static (int x, int y) Convert1DIndexTo2DPosition(int index, int width)
        {
            int y = index / width;
            int x = index % width;
            return (x, y);
        }
        public static List<int>[,] Solution;
        public static bool Solve(List<int>[,] crntpossibilities, int crntcell, int numberToSlot)
        {
            var mittlerweile = CopyPossibilitySpace(crntpossibilities);
            (int x, int y) res = Convert1DIndexTo2DPosition(crntcell,9);
            UpdatePossibilitySpace(res.x, res.y, numberToSlot, mittlerweile);
            

            if (IsLegal(mittlerweile))
            {
                if (crntcell == 80)
                {
                    Console.WriteLine("Solution!");
                    Solution = CopyPossibilitySpace(mittlerweile);

                    for (int i = 0; i <9; i++)
                    {
                        string re = "";
                        for (int y = 0; y < 9; y++)
                        {
                            re = Solution[i, y][0] + " ";
                        }
                        Console.WriteLine(re);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    return true;
                }
                return Solve(mittlerweile, crntcell + 1, 1);
            }
            else
            {
                if (numberToSlot==9)
                {
                    Console.WriteLine("No such solution");
                    return false;
                }

                return Solve(crntpossibilities, crntcell, numberToSlot + 1);
            }

        }

        public static bool IsLegal(List<int>[,] crntpossibilities)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (crntpossibilities[x,y].Count==0)
                    {
                        return false;
                    }
                }


            }
            return true;
        }




        public static List<int>[,] UpdatePossibilitySpace(int x,int y,int newValue, List<int>[,] possibilitySpace)
        {
            possibilitySpace[x, y].Clear();
            possibilitySpace[x, y].Add(newValue);
            int boxindex = GetBoxIndex(x, y);

            for (int ex = 0; ex < 9; ex++)
            {
                if (ex== x)
                {
                    continue;
                }
                possibilitySpace[ex,y].Remove(newValue); 
            }
            for (int yh = 0; yh < 9; yh++)
            {
                if (yh==y)
                {
                    continue;
                }
                possibilitySpace[x, yh].Remove(newValue);

            }
            for (int ex = 0; ex < 9; ex++)
            {
                for (int yh = 0; yh < 9; yh++)
                {
                    if (yh == y && ex == x)
                    {
                        continue;
                    }
                    if (GetBoxIndex(ex,yh) == boxindex)
                    {
                        possibilitySpace[ex, yh].Remove(newValue);
                    }
                }
            }
            return possibilitySpace;
        }
        public static int GetBoxIndex(int row, int col)
        {
            int boxRow = row / 3;
            int boxCol = col / 3;
            return (boxRow * 3) + boxCol;
        }

        public static int[] Convert2DArrayTo1D(int[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);
            int[] array1D = new int[rows * cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int index1D = (row * cols) + col;
                    array1D[index1D] = array2D[row, col];
                }
            }

            return array1D;
        }

        public static List<int>[] Convert2DArrayTo1D(List<int>[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);
            List<int>[] array1D = new List<int>[rows * cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int index1D = (row * cols) + col;
                    array1D[index1D] = array2D[row, col];
                }
            }

            return array1D;
        }



        public static int[,] Convert1DArrayTo2D(int[] array1D, int rows, int cols)
        {
            if (array1D.Length != rows * cols)
            {
                throw new ArgumentException("The size of the 1D array does not match the dimensions of the 2D array.");
            }

            int[,] array2D = new int[rows, cols];

            for (int i = 0; i < array1D.Length; i++)
            {
                int row = i / cols;
                int col = i % cols;
                array2D[row, col] = array1D[i];
            }

            return array2D;
        }



        #region Colour placement Simple Backtracking


        public static bool CheckIfAllright(CanadaState candidate)
        {
            if (candidate.Neighbours.Count == 0)
            {
                return true;
            }


            foreach (var item in candidate.Neighbours)
            {
                if (IsNotSameColour(candidate, item) == 0)
                {
                    return false;
                }
            }

            return true;
        }
        public static bool CheckIfNetworkIsOk(List<CanadaState> states)
        {
            foreach (var item in states)
            {
                if (!CheckIfAllright(item))
                {
                    return false;
                }

            }

            return true;
        }
    
        

        public static bool PlaceColorSimpleBackTracking(int curnt, int color, List<CanadaState> liste)
        {
            Console.WriteLine("Trying to place the " + colors[color] + " on the " + liste[curnt].Name);

            liste[curnt].Color = colors[color];

            if (CheckIfNetworkIsOk(liste))
            {
                if (liste.Count - 1 > curnt)
                {
                    PlaceColorSimpleBackTracking(curnt + 1, 0, liste);
                }
                return true;
            }
            else
            {
                if (colors.Count - 1 > color)
                {
                    Console.WriteLine("No good!");
                    PlaceColorSimpleBackTracking(curnt, color + 1, liste);
                }
                else
                {

                    return false;
                }
            }
            return false;
        }


        public static int IsNotSameColour(CanadaState A, CanadaState B)
        {

            if (A.Color == Color.White || B.Color == Color.White)
            {
                return 1;
            }

            if (A.Color == B.Color)
            {
                return 0;
            }
            return 1;
        }
        #endregion


        #region Sudoku Look Ahead Algorithm
        public static  List<int>[,] possibilitySpace = new List<int>[9, 9];
        public static int[,] Sudoku = new int[9, 9];

       
        
        
        
        #endregion


    }

    public class CanadaState
    {
        public string Name;
        public List<CanadaState> Neighbours = new List<CanadaState>();
        public Color Color = Color.White;


        public CanadaState(string name)
        {
            Name = name;
        }

    }

}
