using System.Threading.Tasks.Dataflow;

namespace PACMAN_ENHANCED
{


    public partial class Form1 : Form
    {
        bool goup, godown, goleft, goright;
        bool noup, nodown, noleft, noright;
        List<PictureBox> walls = new List<PictureBox>();
        List<PictureBox> coins = new List<PictureBox>();
        int speed = 12;
        int score = 0;

        Ghost red, yellow, blue, pink;
        List<Ghost> ghosts = new List<Ghost>();

        public Form1()

        {

            InitializeComponent();
            SetUp();

        }

        private void KeyIsDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Left && !noleft)
            {
                goright = godown = goup = false;
                noright = nodown = noup = false;
                goleft = true;
                pac.Image = Properties.Resources.pacman_left;

            }
            if (e.KeyCode == Keys.Right && !noright)

            {
                goleft = goup = godown = false;
                noleft = noup = nodown = false;
                goright = true;
                pac.Image = Properties.Resources.pacman_right;
            }
            if (e.KeyCode == Keys.Up && !noup)
            {
                goleft = goright = godown = false;
                noleft = noright = noup = false;
                goup = true;
                pac.Image = Properties.Resources.pacman_up;

            }
            if (e.KeyCode == Keys.Down && !nodown)
            {
                goleft = goright = goup = false;
                noleft = noright = noup = false;
                godown = true;
                pac.Image = Properties.Resources.pacman_down;
            }


        }

        private void GameTimerEvent(object sender, EventArgs e)


        {

            PlayerMovements();

            foreach (PictureBox wall in walls)
            {
                CheckBoundaries(pac, wall);

            }
            foreach (PictureBox coin in coins)
            {
                CollectingCoins(pac, coin);
            }
            if (score == coins.Count())
            {
                GameOver();

                GAMEWIN.Visible = true;

            }

            red.GhostMovement(pac);
            blue.GhostMovement(pac);
            yellow.GhostMovement(pac);
            pink.GhostMovement(pac);

            foreach (Ghost ghost in ghosts)
            {
                GhostCollision(ghost, pac, ghost.image);
            }

        }

        private void StartButtonClick(object sender, EventArgs e)

        {

            panelMenu.Enabled = false;

            panelMenu.Visible = false;

            goleft = goright = goup = godown = false;
            noleft = noright = noup = nodown = false;
            score = 0;
            red.image.Location = new Point(100, 100);
            blue.image.Location = new Point(848, 597);
            yellow.image.Location = new Point(132, 584);
            pink.image.Location = new Point(877, 130);

            GameTimer.Start();

        }
        private void SetUp()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "wall")
                {

                    walls.Add((PictureBox)x);
                }
                if (x is PictureBox && x.Tag == "coin")
                {
                    coins.Add((PictureBox)x);
                }
            }
            red = new Ghost(this, Properties.Resources.red, 100, 100);
            ghosts.Add(red);
            blue = new Ghost(this, Properties.Resources.blue, 848, 597);
            ghosts.Add(blue);
            yellow = new Ghost(this, Properties.Resources.yellow, 132, 584);
            ghosts.Add(yellow);
            pink = new Ghost(this, Properties.Resources.pink, 877, 130);
            ghosts.Add(pink);


            this.Text = walls.Count + "" + coins.Count;


        }


        private void PlayerMovements()
        {
            if (goleft) { pac.Left -= speed; }
            if (goright) { pac.Left += speed; }
            if (goup) { pac.Top -= speed; }
            if (godown) { pac.Top += speed; }

            if (pac.Left < -30)
            {
                pac.Left = this.ClientSize.Width - pac.Width;
            }
            if (pac.Left + pac.Width > this.ClientSize.Width)
            {
                pac.Left = -10;
            }
            if (pac.Top < -30)
            {
                pac.Top = this.ClientSize.Height - pac.Height;
            }
            if (pac.Top + pac.Width > this.ClientSize.Height)
            {
                pac.Top = -10;
            }
        }
        private void ShowCoins()
        {
            foreach (PictureBox coin in coins)
            {
                coin.Visible = true;
            }


        }
        private void CheckBoundaries(PictureBox pacman, PictureBox wall)

        {
            if (pac.Bounds.IntersectsWith(wall.Bounds))
            {
                if (goleft)

                {
                    noleft = true;
                    goleft = false;
                    pac.Left = wall.Right + 2;
                }
                if (goright)
                {
                    noright = true;
                    goright = false;
                    pac.Left = wall.Left - pac.Width - 2;

                }
                if (goup)
                {
                    noup = true;
                    goup = false;
                    pac.Top = wall.Bottom + 2;

                }

                if (godown)
                {
                    noup = true;
                    goup = false;
                    pac.Top = wall.Top - pac.Height - 2;
                }


            }


        }
        private void CollectingCoins(PictureBox pacman, PictureBox coin)
        {
            if (pac.Bounds.IntersectsWith(coin.Bounds))
            {
                if (coin.Visible)
                {
                    coin.Visible = false;
                    score += 1;

                }

            }

        }
        private void GhostCollision(Ghost g, PictureBox pacman, PictureBox ghost)
        {

            if (pac.Bounds.IntersectsWith(ghost.Bounds))
            {

                g.ChangeDirection();
                GAMELOSS.Visible = true;
                GAMEWIN.Visible = true;
                GAMELOSS.BringToFront();

                GameOver();
            }

        }

        private void GameOver()
        {
            panelMenu.Visible = true;
            panelMenu.Enabled = true;
            GameTimer.Stop();
            ShowCoins();
            pac.Location = new Point(493, 327);


        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            GAMELOSS.Visible = false;
            GAMEWIN.Visible = false;
            panelMenu.Enabled = false;

            panelMenu.Visible = false;

            goleft = goright = goup = godown = false;
            noleft = noright = noup = nodown = false;
            score = 0;
            red.image.Location = new Point(100, 100);
            blue.image.Location = new Point(848, 597);
            yellow.image.Location = new Point(132, 584);
            pink.image.Location = new Point(877, 130);

            GameTimer.Start();
            this.Focus();
        }

        private void btnRestartLoss_Click(object sender, EventArgs e)
        {
            GAMEWIN.Visible = false;
            GAMELOSS.Visible = false ;
            panelMenu.Enabled = false;

            panelMenu.Visible = false;

            goleft = goright = goup = godown = false;
            noleft = noright = noup = nodown = false;
            score = 0;
            red.image.Location = new Point(100, 100);
            blue.image.Location = new Point(848, 597);
            yellow.image.Location = new Point(132, 584);
            pink.image.Location = new Point(877, 130);

            GameTimer.Start();
            this.Focus();
        }
    }
}




# PAC-MAN WITH PROPERLY PROGRAMMED GHOSTS USING ONLY C##

## Overview
**PAC-MAN WITH PROPERLY PROGRAMMED GHOSTS USING C#** has been made purely on C# WPF without any other editors like unity. The Ghosts has been well programmed move as accurately as a game.They will also start randomly seeking the player like the original game. You have to collect all the coins on the map to win the game. 
If a ghost touches the PAC-MAN then you will lose the game.

## How to Run
1. Clone the repository or download the script file.
2. Open the file using visual studio.
3.Run the build and play the game.
   

## Controls 
- Use the Arrow Keys to move the pac-man around to colllect all the coins
- Avoid the ghosts.
- The ghosts will move around and will randomely start seeking the player like in the original game.
- Collect all the coins to win the game and get touched by the ghosts to lose the game.



## Overview of Steps
- Imported the assets for the pac man, coins and the ghosts.
- Designed the UI of the game in Windows Forms.
- Made the Pac-Man move around with the Arrow Keys.
- Added the ghosts to the game.
- Made the coins which are visible become invisible when their hitbox intersects with the Pac-Man.
- 



Credits to Moo ICT for the assets and inspiration.
Tutorial:
https://youtu.be/ZDw-GQ8j1Uk?si=RUz5Nvg8P8_lW2aF
