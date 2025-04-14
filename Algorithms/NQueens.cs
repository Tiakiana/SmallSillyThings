// See https://aka.ms/new-console-template for more information


class NQueens
{
    private int N;
    private int[] queens;

    public NQueens(int n)
    {
        N = n;
        queens = new int[N];
    }

    public void Solve()
    {
        if (PlaceQueen(0))
        {
            PrintSolution();
        }
        else
        {
            Console.WriteLine("No solution exists.");
        }
    }

    private bool PlaceQueen(int row)
    {
        if (row == N) // All queens are placed
            return true;

        for (int col = 0; col < N; col++)
        {
            if (IsSafe(row, col))
            {
                queens[row] = col; // Place queen
                if (PlaceQueen(row + 1)) // Recur to place the rest
                    return true;
                // If placing queen in this row and column leads to a solution, return true
                // If not, then backtrack: remove queen and try next column
            }
        }

        return false; // No place found for this row
    }

    private bool IsSafe(int row, int col)
    {
        for (int i = 0; i < row; i++)
        {
            int queenCol = queens[i];
            if (queenCol == col || // Same column
                queenCol - i == col - row || // Same major diagonal
                queenCol + i == col + row) // Same minor diagonal
            {
                return false;
            }
        }
        return true;
    }

    private void PrintSolution()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (queens[i] == j)
                    Console.Write("Q ");
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
        }
    }

    public static void Main(string[] args)
    {
    }


}


public class NodeNetwork
{
    public List<Node> nodes = new List<Node>();


    public float[,] AdjacencyMatrix = new float[4, 4];

    public NodeNetwork()
    {
        AdjacencyMatrix[0, 1] = 2.4f;
        AdjacencyMatrix[1, 0] = 2.4f;
        AdjacencyMatrix[0, 2] = 3.3f;
        AdjacencyMatrix[2, 0] = 3.3f;
        AdjacencyMatrix[1, 3] = 23.4f;
        AdjacencyMatrix[3, 1] = 23.4f;
        AdjacencyMatrix[2, 3] = 5.9f;
        AdjacencyMatrix[3, 2] = 5.9f;
    }




}

public class Node
{
    public int id;
    public Node(int id)
    {
        this.id = id;
    }
}





