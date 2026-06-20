using System;

namespace sudoku_maker.Models
{
    public class SudokuBoard
    {
        public const int Size = 9;
        public const int Empty = 0;

        private readonly int[,] _board;

        public SudokuBoard()
        {
            _board = new int[Size, Size];
        }

        public SudokuBoard(int[,] board)
        {
            if (board.GetLength(0) != Size || board.GetLength(1) != Size)
            {
                throw new ArgumentException("Board must be 9x9.");
            }
            _board = (int[,])board.Clone();
        }

        public int GetValue(int row, int column)
        {
            ValidatePosition(row, column);
            return _board[row, column];
        }

        public void SetValue(int row, int column, int value)
        {
            ValidatePosition(row, column);
            if (value < 0 || value > Size)
            {
                throw new ArgumentException("Value must be between 0 and 9.");
            }
            _board[row, column] = value;
        }

        public bool IsEmpty(int row, int column)
        {
            return _board[row, column] == Empty;
        }
       
        public bool IsComplete()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    if (_board[row, column] == Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public SudokuBoard Clone()
        {
            return new SudokuBoard(_board);
        }

        public int[,] toArray()
        {
            return (int[,])_board.Clone();
        }

        private void ValidatePosition(int row, int column)
        {
            if (row < 0 || row >= Size || column < 0 || column >= Size)
            {
                throw new ArgumentOutOfRangeException("Row and column must be between 0 and 8.");
            }
        }
    }
}