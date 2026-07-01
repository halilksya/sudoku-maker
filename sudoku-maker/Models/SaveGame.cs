using System;

namespace sudoku_maker.Models;

public class SaveGame
{
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public Difficulty Difficulty {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public DateTime UpdatedAt {get; set;} = DateTime.Now;
    public int TimeElapsed {get; set;} = 0;
    public int[][] Board {get; set;} = CreateBoard();
    public int[][] CurrentBoard {get; set;} = CreateBoard();
    public int[][] Solution {get; set;} = CreateBoard();
    public bool IsCompleted {get; set;} = false;

    private static int[][] CreateBoard()
    {
        var board = new int[9][];

        for (int row = 0; row < 9; row++)
        {
            board[row] = new int[9];
        }

        return board;
    }
}