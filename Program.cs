using System;
using System.Linq;

class TicTacToe
{
    static string[] board = { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
    static string huPlayer = "X";
    static string aiPlayer = "O";
    static int round = 0;
    static int score_bot = 0;
    static int score_pl = 0;
    static int score_ties = 0;

    static void Main()
    {
        Console.WriteLine("TicTacToe: You're Player 'X'. The Bot is Player 'O'. \n");
        Console.WriteLine("press any key to continue..");
        Console.ReadLine();
        while (true)
        {
            DrawBoard();
            PlayerMove();
            if (CheckWin(board, huPlayer))
            {
                score_pl += 1;
                DrawBoard();
                Console.WriteLine("YOU WIN!");
                Console.ReadLine();
                Reset();
                continue;
            }
            else if (round > 8)
            {
                DrawBoard();
                Console.WriteLine("It's a TIE!");
                Console.ReadLine();
                Reset();
                continue;
            }

            round++;
            AIBotMove();
            if (CheckWin(board, aiPlayer))
            {
                score_bot += 1;
                DrawBoard();
                Console.WriteLine("YOU LOSE!");
                Console.ReadLine();
                Reset();
                continue;
            }
            else if (round > 8)
            {
                score_ties += 1;
                DrawBoard();
                Console.WriteLine("It's a TIE!");
                Console.ReadLine();
                Reset();
                continue;
            }

            round++;
        }
    }

    static void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine("Score:");
        Console.WriteLine($"Wins: {score_pl} | Bot wins: {score_bot} | Ties: {score_ties} \n");
        Console.WriteLine($" {board[0]} | {board[1]} | {board[2]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {board[3]} | {board[4]} | {board[5]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {board[6]} | {board[7]} | {board[8]} ");
    }

    static void PlayerMove()
    {
        Console.Write("Your move (1-9): ");
        int move = int.Parse(Console.ReadLine());

        if (board[move] != huPlayer && board[move] != aiPlayer)
        {
            board[move] = huPlayer;
        }
        else
        {
            Console.WriteLine("Invalid move, try again.");
            PlayerMove();
        }
    }

    static void AIBotMove()
    {
        var move = Minimax(board, aiPlayer).Index;
        board[move] = aiPlayer;
    }

    static bool CheckWin(string[] board, string player)
    {
        int[,] winCombos = new int[,] {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
            { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        for (int i = 0; i < 8; i++)
        {
            if (board[winCombos[i, 0]] == player && board[winCombos[i, 1]] == player && board[winCombos[i, 2]] == player)
            {
                return true;
            }
        }
        return false;
    }

    static void Reset()
    {
        round = 0;
        board = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
    }

    // Funkce Minimax je algoritmus využívaný pro rozhodování v hrách, jako je Tic Tac Toe. Slouží k nalezení nejlepšího možného tahu, a to tak, že zkoumá všechny možné tahy a hodnotí je podle výsledného stavu hry.
    
    // Pozor rekurze.

    // Kontroluje výherní podmínky: Pokud některý hráč vyhraje, vrátí skóre (+10 pro AI, -10 pro hráče, 0 pro remízu).
    // Simulace tahů: Pro každý dostupný tah:
    // Simuluje tah aktuálního hráče.
    // Rekurzivně volá Minimax pro druhého hráče.
    // Hodnotí tahy: Pro každý tah vypočítá skóre.
    // AI hledá tah s nejvyšším skóre.
    // Hráč hledá tah s nejnižším skóre.
    // Vrací nejlepší tah s odpovídajícím skóre.
    static MinimaxMove Minimax(string[] newBoard, string player)
    {
        var availSpots = newBoard.Where(s => s != huPlayer && s != aiPlayer).ToArray();

        // Base conditions for recursion
        if (CheckWin(newBoard, huPlayer))
        {
            return new MinimaxMove { Score = -10 };
        }
        else if (CheckWin(newBoard, aiPlayer))
        {
            return new MinimaxMove { Score = 10 };
        }
        else if (availSpots.Length == 0)
        {
            return new MinimaxMove { Score = 0 };
        }

        var moves = new System.Collections.Generic.List<MinimaxMove>();

        for (int i = 0; i < availSpots.Length; i++)
        {
            MinimaxMove move = new MinimaxMove();
            move.Index = int.Parse(newBoard[int.Parse(availSpots[i])]);

            newBoard[int.Parse(availSpots[i])] = player;

            if (player == aiPlayer)
            {
                move.Score = Minimax(newBoard, huPlayer).Score;
            }
            else
            {
                move.Score = Minimax(newBoard, aiPlayer).Score;
            }

            newBoard[int.Parse(availSpots[i])] = move.Index.ToString();
            moves.Add(move);
        }

        int bestMove = 0;
        if (player == aiPlayer)
        {
            int bestScore = -10000;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Score > bestScore)
                {
                    bestScore = moves[i].Score;
                    bestMove = i;
                }
            }
        }
        else
        {
            int bestScore = 10000;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Score < bestScore)
                {
                    bestScore = moves[i].Score;
                    bestMove = i;
                }
            }
        }

        return moves[bestMove];
    }

    class MinimaxMove
    {
        public int Index { get; set; }
        public int Score { get; set; }
    }
}
