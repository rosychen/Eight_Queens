using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        private Brush[] brush_colors = new Brush[] { Brushes.White, Brushes.Red, Brushes.Black };  // colors used for 8 Queens
        private int BLOCK_WIDTH = 50;                       // width of each block
        private int BLOCK_HEIGHT = 50;                      // height of each block
        private bool[,] takenBlocks = new bool[8, 8];       // keeps track of queens on the board
        private int QueensOnBoard;                          // number of queens on board
        private bool HintOn;                                // indicates whether Hint Checkbox is checked
        private Font QFont = new Font("Arial", 30, FontStyle.Bold);

        public Form1()
        {
            InitializeComponent();
        }

        /** Paints the Chess Board */
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int color = 0;

            // Drawing Chess Board
            for (int row = 0; row < 8; row++)               // y-axis
            {
                for (int column = 0; column < 8; column++)  // x-axis
                {
                    if (HintOn == true && isValidMove(column, row) == false)    // if hint is on and block is invalid
                    {
                        color = 1;  // red box
                    }
                    else if ((row + column) % 2 == 0)
                    {
                        color = 0; // white box
                    }
                    else
                    {
                        color = 2; // black box
                    }
                    g.FillRectangle(brush_colors[color], 100 + (50 * column), 100 + (50 * row), BLOCK_WIDTH, BLOCK_HEIGHT);
                    g.DrawRectangle(Pens.Black, new Rectangle(100 + (50 * column), 100 + (50 * row), BLOCK_WIDTH, BLOCK_HEIGHT));

                    if (takenBlocks[column, row] == true)
                    {
                        // centering text in the box
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        Rectangle rect1 = new Rectangle(100 + (50 * column), 100 + (50 * row), BLOCK_WIDTH, BLOCK_HEIGHT);

                        if (color == 2)
                        {
                            // font is white for black background
                            g.DrawString("Q", QFont, brush_colors[0], rect1, stringFormat);
                        }
                        else
                        {
                            // font is black for white and red background
                            g.DrawString("Q", QFont, brush_colors[2], rect1, stringFormat);
                        }
                    }
                }
                string QueenString = String.Format("You have {0} queens on the board.", QueensOnBoard);
                g.DrawString(QueenString, Font, brush_colors[2], 200, 20);
            }
        }

        /** Clears the Chess board (starts a new game) */
        private void ClearButton_Click(object sender, EventArgs e)
        {
            QueensOnBoard = 0;                  // reset the # of queen count
            takenBlocks = new bool[8, 8];       // reset the board
            this.Invalidate();
        }

        /** Changes HintOn according to the checkBox1 */
        private void HintBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                HintOn = true;
            }
            else
            {
                HintOn = false;
            }
            this.Invalidate();
        }

        /** Mouse event */
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // coordinates of the mouse click
            int x = e.X;
            int y = e.Y;

            // check to see if the click was inside the chess board
            if (x >= 100 && x < 500 && y >= 100 && y < 500)
            {
                // figure out the row and column on the board
                x = (x - 100) / 50;
                y = (y - 100) / 50;
                //System.Diagnostics.Debug.WriteLine(x + " " + y);

                if (e.Button == MouseButtons.Left)
                {
                    // check to see if it's valid spot on the board
                    if (isValidMove(x, y) == true)
                    {
                        // place the queen on the board if valid
                        QueensOnBoard++;
                        takenBlocks[x, y] = true;

                        // user has won the game 
                        if (QueensOnBoard == 8)
                        {
                            MessageBox.Show("You did it! :)");
                        }
                    }
                    else
                    {
                        // invalid entry
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
                else
                {
                    // if right click on a Queen: it removes it from the board, o/w nothing is done
                    if (takenBlocks[x, y] == true)
                    { 
                        takenBlocks[x, y] = false;
                        QueensOnBoard--;
                    }
                }
                this.Invalidate();
            }
        }

        /** method to check if it's a valid spot*/
        private bool isValidMove(int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                // checks the entire row and column
                if (takenBlocks[i, y] == true || takenBlocks[x, i] == true)
                {
                    return false;
                }
            }

            // check left down diagonal and left up diagonal
            for (int i = x; i < 8; i++)
            {
                // checks left up diagonal
                if ((y + i - x) < 8 && (y + i - x) >= 0 && i >= 0 && i < 8)
                {
                    if (takenBlocks[i, y + i - x] == true)
                    {
                        return false;
                    }
                }
                // checks left down diagonal
                if (y - (i - x) >= 0 && y - (i - x) < 8 && i >= 0 && i < 8)
                {
                    if (takenBlocks[i, y - (i - x)] == true)
                    {
                        return false;
                    }
                }
            }

            // check right down diagonal and right up diagonal
            for (int i = x; i >= 0; i--)
            {
                // checks right up diagonal
                if (y + x - i >= 0 && y + x - i < 8 && i >= 0 && i < 8)
                {
                    if (takenBlocks[i, y + x - i] == true)
                    {
                        return false;
                    }
                }

                // checks right down diagonal
                if (y - (x - i) >= 0 && y - (x - i) < 8 && i >= 0 && i < 8)
                {
                    if (takenBlocks[i, y - (x - i)])
                    {
                        return false;
                    }
                }
            }
            // otherwise valid move
            return true;
        }
    }
}