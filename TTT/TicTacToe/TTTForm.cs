using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
            DisableAllSquares();
            ResetArray();
            ResetSquares();
            EnableAllSquares();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        const int SIZE = 5;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        // 2d array for game board
        private string[,] board = new string[5, 5];


        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as its parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        //* TODO: Modify this so it uses the array rather than a square in the UI 
        // This method takes a row (in the range of 0 - 4) and returns true
        // if the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        private bool IsRowWinner(int row)
        {
            string symbol = board[row, 0];

            if (String.Equals(symbol, EMPTY))
            {
                return false;
            }

            for (int col = 1; col < SIZE; col++)
            {
                if (!String.Equals(board[row, col], symbol))
                {
                    return false;
                }
            }
            return true;
        }
        
        //* TODO:  finish all of these that return true
        private bool IsAnyRowWinner()
        {
            return true;
        }

        private bool IsColumnWinner(int col)
        {
            string symbol = board[0, col];

            if (String.Equals(symbol, EMPTY))
            {
                return false;
            }
            for (int row = 1; row < SIZE; row++)
            {
                if (!String.Equals(board[row, col], symbol))
                    return false;
            }
            return true;
        }

        private bool IsAnyColumnWinner()
        {
            return true;
        }

        private bool IsDiagonal1Winner()
        {
            string symbol = board[0, 0];

            if (String.Equals(symbol, EMPTY))
            {
                return false;
            }

            for (int row = 1, col = 1; row < SIZE; row++, col++)
            {
                if (!String.Equals(board[row, col], symbol))
                    return false;
            }
            return true;
        }

        private bool IsDiagonal2Winner()
        {
            string symbol = board[0, SIZE - 1];

            if (String.Equals(symbol, EMPTY))
            {
                return false;
            }

            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                if (!String.Equals(board[row, col], symbol))
                    return false;
            }
            return true;
        }

        private bool IsAnyDiagonalWinner()
        {
            return true;
        }

        private bool IsFull()
        {
             /* starting at first row, increment through each column in row
              *     if square's text is empty, return false
              *     end if
              * increment row and go again
              * if no square's text is empty, return true
              */
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    if (String.Equals(square.Text, EMPTY))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            //increment through rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        private void ResetArray()
        {
            //increment through rows
            //increment through columns
            //set index to EMPTY
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    board[row, col] = EMPTY;
                }
            }
        }

        // Modify this so it uses the array rather than the UI to make the move.
        // Setting the text and disabling the square will happen in the SyncArrayAndSquares method
        private void MakeComputerMove()
        {
            Random gen = new Random();
            int row;
            int col;

            //generates random num between 0 and SIZE and if square is empty, set to comp symbol
            do
            {
                row = gen.Next(0, SIZE);
                col = gen.Next(0, SIZE);
                if (String.Equals(board[row, col], EMPTY))
                {
                    board[row, col] = COMPUTER_SYMBOL;
                    break;
                }
            } while (!String.Equals(board[row, col], EMPTY) && !IsFull());
        }

        // ALL OF THESE METHODS MANIPULATE THE UI AND SHOULDN'T CHANGE
        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        private void HighlightDiagonal1()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
            }
        }

        private void ResetSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Text = EMPTY;
                    square.ForeColor = Color.Black;
                }
            }
            // Clear the resultLabel
            resultLabel.Text = EMPTY;

        }

        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

       



        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }

        // This method should set the text property of each square in the UI
        // to the value in the corresponding element of the array and disable
        // the squares that are not empty (you don't have to enable the others
        // because they're enabled by default)
        private void SyncArrayAndSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Text = board[row, col];
                    if (!String.Equals(square.Text, EMPTY))
                    {
                        DisableSquare(square);
                    }
                }
            }
        }
        //* TODO:  Modify this so it uses the array and UI methods properly
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;


            /* Click label
             * Change clicked label text to USER_SYMBOL
             * Disable clicked label
             */
            int row, col;

            Label clickedLabel = (Label)sender;
            DisableSquare(clickedLabel);

            GetRowAndColumn(clickedLabel, out row, out col);
            board[row, col] = USER_SYMBOL;
            SyncArrayAndSquares();

            /* If user wins on this move,
             *     disable all squares
             *     call HighlightWinner method
             * Else if (IsFull()) is true
             *     disable all squares
             *     display "It's a tie!" in resultLabel
             * Else
             *     call MakeComputerMove() method
             */
            if (IsWinner(out winningDimension, out winningValue))
            {
                DisableAllSquares();
                HighlightWinner("User", winningDimension, winningValue);
            }
            else if (IsFull())
            {
                DisableAllSquares();
                resultLabel.Text = "It's a tie!";
            }
            else
            {
                MakeComputerMove();

                SyncArrayAndSquares();

                /* if IsWinner is true after computer move
                 *    disable all squares
                 *    call HighlightWinner() method
                 * else if IsFull is true
                 *    disable all squares
                 *    display "It's a tie!" in resultLabel text
                 */
                if (IsWinner(out winningDimension, out winningValue))
                {
                    DisableAllSquares();
                    HighlightWinner("Computer", winningDimension, winningValue);
                }
                else if (IsFull())
                {
                    resultLabel.Text = "It's a tie!";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        /* newGameButton click
        * calls DisableAllSquares()
        * calls ResetSquares()
        * calls EnableAllSquares();       
        */
        {
            DisableAllSquares();
            ResetArray();
            ResetSquares();
            EnableAllSquares();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
