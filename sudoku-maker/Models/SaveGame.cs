using System;

namespace sudoku_maker.Models;

public class SaveGame
{
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public Difficulty Difficulty {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public DateTime UpdatedAt {get; set;} = DateTime.Now;
    public int[,] Board {get; set;} = new int[9, 9];
    public int[,] CurrentBoard {get; set;} = new int[9, 9];
    public int[,] Solution {get; set;} = new int[9, 9];
    public bool IsCompleted {get; set;} = false;
}