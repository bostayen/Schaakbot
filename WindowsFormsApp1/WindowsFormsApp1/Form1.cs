using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //globals
        int WhiteWins;
        int BlackWins;
        bool rokeerWR = true;
        bool rokeerWL = true;
        bool rokeerZR = true;
        bool rokeerZL = true;
        bool slaan = false;
        bool rokeer = false;
        double StepSize = 1250;    //deze waarde moet het aantal stappen tussen 1 echt schaakvak zijn
        bool step = false; //0 is select piece, 1 is confirm new place
        bool turn = true; //true = wit false is zwart
        string piece;
        string[,] Board = new string[8, 8];
        int piecex;
        int piecey;
        System.IO.Ports.SerialPort port;
        //end of globals

        //functions
        public void ResetBoard()
        {
            BlackHorse1.Location = new Point(1 * 90, 0 * 90);
            BlackHorse2.Location = new Point(6 * 90, 0 * 90);
            BlackTower1.Location = new Point(0 * 90, 0 * 90);
            BlackTower2.Location = new Point(7 * 90, 0 * 90);
            BlackRook1.Location = new Point(2 * 90, 0 * 90);
            BlackRook2.Location = new Point(5 * 90, 0 * 90);
            BlackQueen.Location = new Point(3 * 90, 0 * 90);
            BlackKing.Location = new Point(4 * 90, 0 * 90);
            BlackPawn1.Location = new Point(0 * 90, 1 * 90);
            BlackPawn2.Location = new Point(1 * 90, 1 * 90);
            BlackPawn3.Location = new Point(2 * 90, 1 * 90);
            BlackPawn4.Location = new Point(3 * 90, 1 * 90);
            BlackPawn5.Location = new Point(4 * 90, 1 * 90);
            BlackPawn6.Location = new Point(5 * 90, 1 * 90);
            BlackPawn7.Location = new Point(6 * 90, 1 * 90);
            BlackPawn8.Location = new Point(7 * 90, 1 * 90);
            WhiteHorse1.Location = new Point(1 * 90, 7 * 90);
            WhiteHorse2.Location = new Point(6 * 90, 7 * 90);
            WhiteTower1.Location = new Point(0 * 90, 7 * 90);
            WhiteTower2.Location = new Point(7 * 90, 7 * 90);
            WhiteRook1.Location = new Point(2 * 90, 7 * 90);
            WhiteRook2.Location = new Point(5 * 90, 7 * 90);
            WhiteQueen.Location = new Point(3 * 90, 7 * 90);
            WhiteKing.Location = new Point(4 * 90, 7 * 90);
            WhitePawn1.Location = new Point(0 * 90, 6 * 90);
            WhitePawn2.Location = new Point(1 * 90, 6 * 90);
            WhitePawn3.Location = new Point(2 * 90, 6 * 90);
            WhitePawn4.Location = new Point(3 * 90, 6 * 90);
            WhitePawn5.Location = new Point(4 * 90, 6 * 90);
            WhitePawn6.Location = new Point(5 * 90, 6 * 90);
            WhitePawn7.Location = new Point(6 * 90, 6 * 90);
            WhitePawn8.Location = new Point(7 * 90, 6 * 90);
            Cancel.Location = new Point(9 * 90, 7 * 90);
            Cancel.Visible = false;
            foreach (Control c in this.Controls)
            {
                PictureBox pic = c as PictureBox;
                if (pic != null && (pic.Name.Contains("Target")))
                {
                    pic.Visible = false;
                }
            }
            step = false;
            label2.Text = "White";
            rokeerWL = true;
            rokeerWR = true;
            rokeerZL = true;
            rokeerZR = true;
            MoveHistory.Clear();
            MoveHistory.AppendText("Move History");
            label3.Text = "White Wins:" + Convert.ToString(WhiteWins) + " Black Wins: " + Convert.ToString(BlackWins);
        }   //ResetBoard
        public void ShowOptions()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i, j] = null;
                }
            }
            foreach (Control c in this.Controls)    //reset array
            {
                PictureBox pic = c as PictureBox;
                if (pic != null && (pic.Name.Contains("Black") || pic.Name.Contains("White")))
                {
                    if (pic.Location != new Point(0, 810)) //test voor of het stuk niet geslagen is
                    {
                        Board[pic.Location.X / 90, pic.Location.Y / 90] = pic.Name;
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] == piece)
                    {
                        piecex = i;
                        piecey = j;
                    }
                }
            }
            foreach (Control c in this.Controls)    //set all targets to 0,0
            {
                PictureBox pic = c as PictureBox;
                if (pic != null && (pic.Name.Contains("Target")))
                {
                    pic.Location = new Point(0, 0);
                }
            }
            Cancel.Visible = true;

            if (piece.Contains("King"))
            {
                Target1.Location = new Point(52 + piecex * 90 - 90, piecey * 90 - 90);
                Target2.Location = new Point(52 + piecex * 90, piecey * 90 - 90);
                Target3.Location = new Point(52 + piecex * 90 + 90, piecey * 90 - 90);
                Target4.Location = new Point(52 + piecex * 90 - 90, piecey * 90);
                Target5.Location = new Point(52 + piecex * 90 + 90, piecey * 90);
                Target6.Location = new Point(52 + piecex * 90 - 90, piecey * 90 + 90);
                Target7.Location = new Point(52 + piecex * 90, piecey * 90 + 90);
                Target8.Location = new Point(52 + piecex * 90 + 90, piecey * 90 + 90);
                if (piece.Contains("White"))
                {
                    if(Board[5,7] == null && Board[6,7] == null && rokeerWR == true)
                    {
                        Target9.Location = new Point(52 + piecex * 90 + 180, piecey * 90);
                    }
                    if(Board[3,7] == null && Board[2,7] == null && Board[1,7] == null && rokeerWL == true)
                    {
                        Target10.Location = new Point(52 + piecex * 90 - 180, piecey * 90);
                    }
                }
                if (piece.Contains("Black"))
                {
                    if (Board[5, 0] == null && Board[6, 0] == null && rokeerZR == true)
                    {
                        Target9.Location = new Point(52 + piecex * 90 + 180, piecey * 90);
                    }
                    if (Board[3, 0] == null && Board[2, 0] == null && Board[1, 0] == null && rokeerZL == true)
                    {
                        Target10.Location = new Point(52 + piecex * 90 - 180, piecey * 90);
                    }
                }
                ShowTargets();
            }//done
            else if (piece.Contains("Queen"))
            {
                bool loop = true;
                int t = 1;
                int i = 0;
                while (loop)//up
                {
                    i++;
                    if ((piecey - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex * 90), (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex * 90), (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//up
                i = 0;
                loop = true;
                while (loop)//right
                {
                    i++;
                    if ((piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//right
                i = 0;
                loop = true;
                while (loop)//down
                {
                    i++;
                    if ((piecey + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex * 90), (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex * 90), (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//down
                i = 0;
                loop = true;
                while (loop)//left
                {
                    i++;
                    if ((piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//left
                i = 0;
                loop = true;

                while (loop)//upleft
                {
                    i++;
                    if ((piecey - i) < 0 || (piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//up
                i = 0;
                loop = true;
                while (loop)//rightup
                {
                    i++;
                    if ((piecey - i) < 0 || (piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//right
                i = 0;
                loop = true;
                while (loop)//downright
                {
                    i++;
                    if ((piecey + i) > 7 || (piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//down
                i = 0;
                loop = true;
                while (loop)//left
                {
                    i++;
                    if ((piecey + i) > 7 || (piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//left

                ShowTargets();
            }
            else if (piece.Contains("Horse"))
            {
                Target1.Location = new Point(52 + piecex * 90 - 90, piecey * 90 - 180);
                Target2.Location = new Point(52 + piecex * 90 + 90, piecey * 90 - 180);
                Target3.Location = new Point(52 + piecex * 90 + 180, piecey * 90 - 90);
                Target4.Location = new Point(52 + piecex * 90 + 180, piecey * 90 + 90);
                Target5.Location = new Point(52 + piecex * 90 + 90, piecey * 90 + 180);
                Target6.Location = new Point(52 + piecex * 90 - 90, piecey * 90 + 180);
                Target7.Location = new Point(52 + piecex * 90 - 180, piecey * 90 + 90);
                Target8.Location = new Point(52 + piecex * 90 - 180, piecey * 90 - 90);
                ShowTargets();
            }//done
            else if (piece.Contains("Tower"))
            {
                bool loop = true;
                int t = 1;
                int i = 0;
                while (loop)//up
                {
                    i++;
                    if ((piecey - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex * 90), (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex * 90), (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//up
                i = 0;
                loop = true;
                while (loop)//right
                {
                    i++;
                    if ((piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//right
                i = 0;
                loop = true;
                while (loop)//down
                {
                    i++;
                    if ((piecey + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex * 90), (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex * 90), (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//down
                i = 0;
                loop = true;
                while (loop)//left
                {
                    i++;
                    if ((piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//left

                ShowTargets();
            }//done
            else if (piece.Contains("Rook"))
            {
                bool loop = true;
                int t = 1;
                int i = 0;
                while (loop)//upleft
                {
                    i++;
                    if ((piecey - i) < 0 || (piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//upleft
                i = 0;
                loop = true;
                while (loop)//rightup
                {
                    i++;
                    if ((piecey - i) < 0 || (piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey - i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey - i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey - i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey - i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey - i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey - i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//rightup
                i = 0;
                loop = true;
                while (loop)//downright
                {
                    i++;
                    if ((piecey + i) > 7 || (piecex + i) > 7)
                    {
                        loop = false;
                    }
                    else if (Board[piecex + i, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex + i) * 90, (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex + i, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex + i, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex + i, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex + i) * 90, (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//downright
                i = 0;
                loop = true;
                while (loop)//left
                {
                    i++;
                    if ((piecey + i) > 7 || (piecex - i) < 0)
                    {
                        loop = false;
                    }
                    else if (Board[piecex - i, piecey + i] == null)
                    {
                        foreach (Control c in this.Controls)
                        {
                            PictureBox pic = c as PictureBox;
                            if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                            {
                                pic.Location = new Point(52 + (piecex - i) * 90, (piecey + i) * 90);
                            }
                        }
                        t++;
                    }
                    else if (Board[piecex - i, piecey + i] != null)
                    {
                        if (piece.Contains("White") && Board[piecex - i, piecey + i].Contains("Black") ||
                            piece.Contains("Black") && Board[piecex - i, piecey + i].Contains("White"))
                        {
                            foreach (Control c in this.Controls)
                            {
                                PictureBox pic = c as PictureBox;
                                if (pic != null && pic.Name.Contains("Target" + Convert.ToString(t)))
                                {
                                    pic.Location = new Point(52 + (piecex - i) * 90, (piecey + i) * 90);
                                }
                            }
                            t++;
                        }
                        loop = false;
                    }
                }//leftdown

                ShowTargets();
            }//done
            else if (piece.Contains("BlackPawn"))
            {
                if (piecey != 7)
                {
                    Target1.Location = new Point(52 + piecex * 90, +piecey * 90 + 90);
                    if (Target1.Location.X < 720 && Target1.Location.Y < 720 && Target1.Location.X >= 0 && Target1.Location.Y >= 0 && Board[piecex, piecey + 1] == null) Target1.Visible = true;
                    Target2.Location = new Point(52 + piecex * 90, piecey * 90 + 180);
                    if (Target2.Location.X < 720 && Target2.Location.Y < 720 && Target2.Location.X >= 0 && Target2.Location.Y >= 0 && piecey == 1 && Board[piecex, piecey + 2] == null) Target2.Visible = true;
                    if (piecex - 1 >= 0 && Board[piecex - 1, piecey + 1] != null && Board[piecex - 1, piecey + 1].Contains("White"))
                    {
                        Target3.Location = new Point(52 + piecex * 90 - 90, +piecey * 90 + 90);
                        Target3.Visible = true;
                    }
                    if (piecex + 1 <= 7 && Board[piecex + 1, piecey + 1] != null && Board[piecex + 1, piecey + 1].Contains("White"))
                    {
                        Target4.Location = new Point(52 + piecex * 90 + 90, +piecey * 90 + 90);
                        Target4.Visible = true;
                    }
                }
            }//done
            else if (piece.Contains("WhitePawn"))
            {
                if (piecey != 0)
                {

                    Target1.Location = new Point(Target1.Location.X + 52 + piecex * 90, Target1.Location.Y + piecey * 90 - 90);
                    Target2.Location = new Point(52 + piecex * 90, piecey * 90 - 180);
                    if ((Target1.Location.X < 720) && (Target1.Location.Y < 720) && Target1.Location.X >= (0) && Target1.Location.Y >= (0) && Board[piecex, piecey - 1] == null) Target1.Visible = true;
                    if ((Target2.Location.X < 720) && (Target2.Location.Y < 720) && Target2.Location.X >= (0) && Target2.Location.Y >= (0) && piecey == 6 && Board[piecex, piecey - 2] == null) Target2.Visible = true;

                    if (piecex - 1 >= 0 && Board[piecex - 1, piecey - 1] != null && Board[piecex - 1, piecey - 1].Contains("Black"))
                    {
                        Target3.Location = new Point(52 + piecex * 90 - 90, +piecey * 90 - 90);
                        Target3.Visible = true;
                    }
                    if (piecex + 1 <= 7 && Board[piecex + 1, piecey - 1] != null && Board[piecex + 1, piecey - 1].Contains("Black"))
                    {
                        Target4.Location = new Point(52 + piecex * 90 + 90, +piecey * 90 - 90);
                        Target4.Visible = true;
                    }
                }
            }//done

            else
            {
                label1.Text = "error2";
            }
            step = !step;
        }   //showoptions
        public void HideOptions()
        {
            foreach (Control c in this.Controls)
            {
                PictureBox pic = c as PictureBox;
                if (pic != null && (pic.Name.Contains("Target")))
                {
                    pic.Visible = false;
                }
            }
            Cancel.Visible = false;
            step = !step;
        }   //hideoptions
        public void ShowTargets()
        {
            foreach (Control c in this.Controls)
            {
                PictureBox pic = c as PictureBox;
                if (pic != null && (pic.Name.Contains("Target")))
                {
                    if (pic.Location.X < 720 && pic.Location.Y < 720 && pic.Location.X >= 52 && pic.Location.Y >= 0 && pic.Location != new Point(0, 0))
                    {   //test of de target een leeg vak of een vak met andere kleur is
                        if (piece.Contains("Black") && (Board[(pic.Location.X - 52) / 90, (pic.Location.Y) / 90] == null || Board[(pic.Location.X - 52) / 90, (pic.Location.Y) / 90].Contains("White")))
                        {
                            pic.Visible = true;
                        }
                        if (piece.Contains("White") && (Board[(pic.Location.X - 52) / 90, (pic.Location.Y) / 90] == null || Board[(pic.Location.X - 52) / 90, (pic.Location.Y) / 90].Contains("Black")))
                        {
                            pic.Visible = true;
                        }
                    }
                }
            }
        }   //showtargets
        public void MoveToTarget(int x, int y)
        {
            //test voor rokeren
            if(piece == "WhiteKing")
            {
                rokeerWL = false;
                rokeerWR = false;
            }
            if(piece == "BlackKing")
            {
                rokeerZL = false;
                rokeerZR = false;
            }
            if(piece == "WhiteTower1")
            {
                rokeerWL = false;
            }
            if(piece == "WhiteTower2")
            {
                rokeerWR = false;
            }
            if(piece == "BlackTower1")
            {
                rokeerZL = false;
            }
            if(piece == "BlackTower2")
            {
                rokeerZR = false;
            }
            //rest van het programma
            char letter1 = Convert.ToChar(piecex + 65);
            char letterx2 = Convert.ToChar((x / 90) + 65);
            MoveHistory.AppendText("\n" + piece + " " + letter1 + Convert.ToString(8 - piecey) + " --> " + letterx2 + Convert.ToString(8 - (y / 90)));
            if (Board[x / 90, y / 90] == null)
            {
                foreach (Control c in this.Controls)
                {
                    PictureBox pic = c as PictureBox;
                    if (pic != null && pic.Name.Contains(piece))
                    {
                        pic.Location = new Point(x, y);
                    }
                }
            }   //alleen bewegen
            else
            {
               foreach (Control c in this.Controls)
                {
                    PictureBox pic = c as PictureBox;
                    //test voor slaan
                    if (pic != null && pic.Location == new Point(x, y))
                    {
                        pic.Location = new Point(0, 810);
                        if (pic.Name.Contains("WhitePawn"))
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, (Convert.ToInt32(pic.Name.Remove(0, 9)) - 1) * 0.5, 8);    //als witte pion stuur naar (pionnummer - 1, 8)
                        }
                        else if (pic.Name.Contains("BlackPawn"))
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, (Convert.ToInt32(pic.Name.Remove(0, 9)) - 1) * 0.5);    //als witte pion stuur naar 8, (pionnummer - 1)
                        }
                        else if (pic.Name == "WhiteTower1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 4, 8);
                        }
                        else if (pic.Name == "WhiteHorse1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 4.5, 8);
                        }
                        else if (pic.Name == "WhiteRook1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 5, 8);
                        }
                        else if (pic.Name == "WhiteQueen")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 5.5, 8);
                        }
                        else if (pic.Name == "WhiteKing")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 6, 8);
                            EndGame();
                        }
                        else if (pic.Name == "WhiteRook2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 6.5, 8);
                        }
                        else if (pic.Name == "WhiteHorse2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 7, 8);
                        }
                        else if (pic.Name == "WhiteTower2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 7.5, 8);
                        }
                        //end of white
                        //black
                        else if (pic.Name == "BlackTower1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 4);
                        }
                        else if (pic.Name == "BlackHorse1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 4.5);
                        }
                        else if (pic.Name == "BlackRook1")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 5);
                        }
                        else if (pic.Name == "BlackQueen")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 5.5);
                        }
                        else if (pic.Name == "BlackKing")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 6);
                            EndGame();
                        }
                        else if (pic.Name == "BlackRook2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 6.5);
                        }
                        else if (pic.Name == "BlackHorse2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 7);
                        }
                        else if (pic.Name == "BlackTower2")
                        {
                            slaan = true;
                            MoveMagnet(x / 90, y / 90, 8, 7.5);
                        }
                        //add to history
                        MoveHistory.AppendText("\t" + pic.Name + " X");
                    }
                }   //slaan
                foreach (Control c in this.Controls)
                {
                    PictureBox pic = c as PictureBox;
                    if (pic != null && pic.Name.Contains(piece))
                    {
                        pic.Location = new Point(x, y);
                    }
                }   //bewegen
            }   //slaan en bewegen
            if (piece.Contains("Pawn"))
            {
                if (piece.Contains("White"))
                {
                   if(y == 0)
                    {
                        if(WhiteQueen.Location== new Point(0, 810)) //als dame geslagen is wissel pion met dame
                        {
                            MoveMagnet(x / 90, y / 90, Convert.ToInt32(piece.Remove(0, 9)) - 1, 8);
                            MoveMagnet(3, 9, x / 90, y / 90);
                        }
                    }
                    else
                    {
                        label1.Text = "Kan niet promoveren, dame is niet geslagen";
                    }
                }
                else
                {   //black
                    if (y == 7) {
                        if (BlackQueen.Location == new Point(0, 810))
                        {
                            MoveMagnet(x / 90, y / 90, 8, Convert.ToInt32(piece.Remove(0, 9)) - 1);
                            MoveMagnet(9, 3, x / 90, y / 90);
                        }
                    }
                    else
                    {
                        label1.Text = "Kan niet promoveren, dame is niet geslagen";
                    }
                }
            }

            MoveMagnet(piecex, piecey, x / 90, y / 90); //gedeeld door 90 want is positie in interface
            turn = !turn;
            if (turn) label2.Text = "White";
            else label2.Text = "Black";
        }//Movetarget
        public void MoveMagnet(double xoud, double youd, double xnew, double ynew)
        {
            if (port != null && port.IsOpen)
            {
                //(xoud <= 9 && youd <= 9 && xoud >= 0 && youd >= 0 && xnew <= 9 && ynew <= 9 && xnew >= 0 && ynew >= 0)
                //TODO test of nodig is half vak en slaan
                if (slaan)
                {
                    if (piece.Contains("White"))
                    {
                        port.WriteLine("MoveTo;0;" + Convert.ToString(xoud * StepSize) + ";" + Convert.ToString(youd * StepSize) + ";");    //geef comando om te bewegen naar doel
                        while (port.ReadLine() != "Done\r") ;
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xoud * StepSize + 0.5 * StepSize) + ";" + Convert.ToString(youd * StepSize + 0.5 * StepSize) + ";");  //ga naar hoek van vak
                        while (port.ReadLine() != "Done\r") ;
                        //x
                        port.WriteLine("MoveTo;1;" + Convert.ToString(7.5 * StepSize) + ";" + Convert.ToString(youd * StepSize + StepSize * 0.5) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        //y
                        port.WriteLine("MoveTo;1;" + Convert.ToString(7.5 * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        //van (new - 0.5) naar (new)
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        slaan = false;
                    }
                    else
                    {
                        port.WriteLine("MoveTo;0;" + Convert.ToString(xoud * StepSize) + ";" + Convert.ToString(youd * StepSize) + ";");    //geef comando om te bewegen naar doel
                        while (port.ReadLine() != "Done\r") ;
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xoud * StepSize + 0.5 * StepSize) + ";" + Convert.ToString(youd * StepSize + 0.5 * StepSize) + ";");  //ga naar hoek van vak
                        while (port.ReadLine() != "Done\r") ;
                        //y
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xoud * StepSize + StepSize * 0.5) + ";" + Convert.ToString(7.5 * StepSize) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        //x
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(7.5 * StepSize) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        //van (new - 0.5) naar (new)
                        port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                        while (port.ReadLine() != "Done\r") ;
                        slaan = false;
                    }
                }
                else if (rokeer)
                {
                    port.WriteLine("MoveTo;0;" + Convert.ToString(xoud * StepSize) + ";" + Convert.ToString(youd * StepSize) + ";");    //geef comando om te bewegen naar doel
                    while (port.ReadLine() != "Done\r") ;
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xoud * StepSize + 0.5 * StepSize) + ";" + Convert.ToString(youd * StepSize + 0.5 * StepSize) + ";");  //ga naar hoek van vak
                    while (port.ReadLine() != "Done\r") ;
                    //x
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize + StepSize * 0.5) + ";" + Convert.ToString(youd * StepSize + StepSize * 0.5) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    //y
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize + StepSize * 0.5) + ";" + Convert.ToString(ynew * StepSize + StepSize * 0.5) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    //van (new - 0.5) naar (new)
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    rokeer = false;
                }
                else if (piece.Contains("Horse"))   
                {
                    port.WriteLine("MoveTo;0;" + Convert.ToString(xoud * StepSize) + ";" + Convert.ToString(youd * StepSize) + ";");    //geef comando om te bewegen naar doel
                    while (port.ReadLine() != "Done\r") ;
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xoud * StepSize + 0.5 * StepSize) + ";" + Convert.ToString(youd * StepSize + 0.5 * StepSize) + ";");  //ga naar hoek van vak
                    while (port.ReadLine() != "Done\r") ;
                    //x
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize + StepSize * 0.5) + ";" + Convert.ToString(youd * StepSize + StepSize * 0.5) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    //y
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize + StepSize * 0.5) + ";" + Convert.ToString(ynew * StepSize + StepSize * 0.5) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    //van (new - 0.5) naar (new)
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                    while (port.ReadLine() != "Done\r") ;
                }
                else
                {
                    label1.Text = ("Moving magnet from(" + Convert.ToString(xoud) + ", " + Convert.ToString(youd) + ") To (" + Convert.ToString(xnew) + ", " + Convert.ToString(ynew) + ".");
                    port.WriteLine("MoveTo;0;" + Convert.ToString(xoud * StepSize) + ";" + Convert.ToString(youd * StepSize) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    port.WriteLine("MoveTo;1;" + Convert.ToString(xnew * StepSize) + ";" + Convert.ToString(ynew * StepSize) + ";");
                    while (port.ReadLine() != "Done\r") ;
                    label1.Text = "Done Moving Magnet";
                }
            }
            else
            {
                label1.Text = "Can not move magnet; no serial connection";
            }
        }//MoveMagnet
        public void EndGame()
        {
            step = false;    //leg spel vast
            if (turn) label1.Text = "White Won";
            else label1.Text = "Black Won";
            if (turn) WhiteWins++;
            else BlackWins++;
            label3.Text = "White Wins:" + Convert.ToString(WhiteWins) + " Black Wins: " + Convert.ToString(BlackWins);
        }   //endgame
        //end of functions

        //all Actions
        public Form1()
        {
            InitializeComponent();
            ResetBoard();
            label1.Text = "Error: no serial connection";
        }
        private void comboBox1_Click(object sender, EventArgs e) //click action for combobox
        {
            comboBox1.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //comport list
        {
            port = new System.IO.Ports.SerialPort(Convert.ToString(comboBox1.SelectedItem), 115200);
            /*port.DtrEnable = true;
            port.RtsEnable = true;*/
            try
            {
                port.Open();
                System.Threading.Thread.Sleep(1000);
                if (port.IsOpen)
                {
                    label1.Text = "Serial connection is opened";
                    port.WriteLine("Zero;");
                    label1.Text = "ijken";
                    label2.Text = port.ReadLine();
                    while (port.ReadLine() != "Done\r") ;
                    label1.Text = "ijken compleet";
                }
            }
            catch (Exception ex)
            {
                label1.Text = "Could not open a serial cnnection";
            }
        }
        private void button3_Click(object sender, EventArgs e)  //Reset Board
        {
            ResetBoard();
            label1.Text = "Label1";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void pictureBox1_Click_3(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void pictureBox1_Click_4(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn3_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn4_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn5_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn6_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn7_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackPawn8_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void WhitePawn1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn3_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn4_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn5_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn6_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn7_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhitePawn8_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackRook2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackQueen_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackTower1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void BlackTower2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == false)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteHorse1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteHorse2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteKing_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteRook1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteRook2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteQueen_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteTower1_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        private void WhiteTower2_Click(object sender, EventArgs e)
        {
            if (step == false && turn == true)
            {
                PictureBox clicked = (PictureBox)sender;
                piece = clicked.Name;
                ShowOptions();
            }
        }
        //end of all actins

        //Tartget cliks
        private void Target1_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target1.Location.X - 52, Target1.Location.Y);
                HideOptions();
            }
        }
        private void Target2_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target2.Location.X - 52, Target2.Location.Y);
                HideOptions();
            }
        }
        private void Target3_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target3.Location.X - 52, Target3.Location.Y);
                HideOptions();
            }
        }
        private void Target4_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target4.Location.X - 52, Target4.Location.Y);
                HideOptions();
            }
        }
        private void Target5_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target5.Location.X - 52, Target5.Location.Y);
                HideOptions();
            }
        }
        private void Target6_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target6.Location.X - 52, Target6.Location.Y);
                HideOptions();
            }
        }
        private void Target7_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target7.Location.X - 52, Target7.Location.Y);
                HideOptions();
            }
        }
        private void Target8_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target8.Location.X - 52, Target8.Location.Y);
                HideOptions();
            }
        }
        private void Target9_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target9.Location.X - 52, Target9.Location.Y);
                HideOptions();
            }
            if (piece.Contains("King"))
            {
                if (piece.Contains("White"))
                {
                    piece = "WhiteTower2";
                    rokeer = true;
                    piecex = WhiteTower2.Location.X / 90;
                    piecey = WhiteTower2.Location.Y / 90;
                    MoveToTarget(5 * 90, 7 * 90);
                    
                }
                else
                {
                    piece = "BlackTower2";
                    rokeer = true;
                    piecex = BlackTower2.Location.X /90;
                    piecey = BlackTower2.Location.Y /90;
                    MoveToTarget(5 * 90, 0);
                    
                    
                }
                turn = !turn;
            }
        }
        private void Target10_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target10.Location.X - 52, Target10.Location.Y);
                HideOptions();
            }
            if (piece.Contains("King"))
            {
                if (piece.Contains("White"))
                {
                    piece = "WhiteTower1";
                    rokeer = true;
                    piecex = WhiteTower1.Location.X / 90;
                    piecey = WhiteTower1.Location.Y / 90;
                    MoveToTarget(3 * 90, 7 * 90);
                    
                }
                else
                {
                    piece = "BlackTower1";
                    rokeer = true;
                    piecex = BlackTower1.Location.X / 90;
                    piecey = BlackTower1.Location.Y / 90;
                    MoveToTarget(3 * 90, 0);
                    
                }
                turn = !turn;
            }
        }
        private void Target11_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target11.Location.X - 52, Target11.Location.Y);
                HideOptions();
            }
        }
        private void Target12_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target12.Location.X - 52, Target12.Location.Y);
                HideOptions();
            }
        }
        private void Target13_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target13.Location.X - 52, Target13.Location.Y);
                HideOptions();
            }
        }
        private void Target14_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target14.Location.X - 52, Target14.Location.Y);
                HideOptions();
            }
        }
        private void Target15_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target15.Location.X - 52, Target15.Location.Y);
                HideOptions();
            }
        }
        private void Target16_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target16.Location.X - 52, Target16.Location.Y);
                HideOptions();
            }
        }
        private void Target17_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target17.Location.X - 52, Target17.Location.Y);
                HideOptions();
            }
        }
        private void Target18_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target18.Location.X - 52, Target18.Location.Y);
                HideOptions();
            }
        }
        private void Target19_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target19.Location.X - 52, Target19.Location.Y);
                HideOptions();
            }
        }
        private void Target20_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target20.Location.X - 52, Target20.Location.Y);
                HideOptions();
            }
        }
        private void Target21_Click(object sender, EventArgs e)
        {
            if (step == true)
            {
                MoveToTarget(Target21.Location.X - 52, Target21.Location.Y);
                HideOptions();
            }
        }
        //end of target cliks
        private void Cancel_Click(object sender, EventArgs e)
        {
            HideOptions();
        }
        private void label2_Click(object sender, EventArgs e)//turn
        {

        }
        //new spultyb



    }
}