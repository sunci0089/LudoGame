using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;
using LudoGame.GUI;

namespace LudoGame.Figures
{
    class Player
    {
        public bool isTurn { get; set; } = false;
        public FigureColor color { get; set; } = FigureColor.Red;
        public Figure1[] figs { get; set; } = new Figure1[4];
    }
    class LoadGame
    {
        public Player[] players { get; set; } = new Player[StartPage.NUM_PLAYERS];
        public Figure1[] figures { get; set; }
        public void load()
        {
            LoadPlayers();
            figures = new Figure1[StartPage.NUM_PLAYERS * 4];

            for (int i = 0; i < 4; i++)
            {
                figures[i] = new Figure1(FigureColor.Red);

                /*figures[i].startPosition[0] = 1;
                figures[i].startPosition[1] = 6;*/

                figures[i + 4] = new Figure1(FigureColor.Green);

                /*figures[i + 4].startPosition[0] = 14 - 6;
                figures[i + 4].startPosition[1] = 1;*/

                if (StartPage.NUM_PLAYERS == 3 || StartPage.NUM_PLAYERS == 4)
                    figures[i + 8] = new Figure1(FigureColor.Blue);

                /* figures[i+8].startPosition[0] = 6;
                 figures[i+8].startPosition[1] = 14-1;*/

                if (StartPage.NUM_PLAYERS == 4)
                    figures[i + 12] = new Figure1(FigureColor.Yellow);

                /*figures[i + 12].startPosition[0] = 14 - 1;
                figures[i + 12].startPosition[1] = 14 - 6;*/


            }

            //homes position
            //red
            figures[0].homePosition[0] = 2;
            figures[0].homePosition[1] = 2;
            figures[1].homePosition[0] = 3;
            figures[1].homePosition[1] = 2;
            figures[2].homePosition[0] = 2;
            figures[2].homePosition[1] = 3;
            figures[3].homePosition[0] = 3;
            figures[3].homePosition[1] = 3;

            //green
            figures[4].homePosition[0] = 14 - 3;
            figures[4].homePosition[1] = 2;
            figures[5].homePosition[0] = 14 - 2;
            figures[5].homePosition[1] = 2;
            figures[6].homePosition[0] = 14 - 3;
            figures[6].homePosition[1] = 3;
            figures[7].homePosition[0] = 14 - 2;
            figures[7].homePosition[1] = 3;
            //blue
            if (StartPage.NUM_PLAYERS == 3 || StartPage.NUM_PLAYERS == 4)
            {
                figures[8].homePosition[0] = 2;
                figures[8].homePosition[1] = 14 - 3;
                figures[9].homePosition[0] = 2;
                figures[9].homePosition[1] = 14 - 2;
                figures[10].homePosition[0] = 3;
                figures[10].homePosition[1] = 14 - 3;
                figures[11].homePosition[0] = 3;
                figures[11].homePosition[1] = 14 - 2;
            }
            //yellow
            if (StartPage.NUM_PLAYERS == 4)
            {
                figures[12].homePosition[0] = 14 - 3;
                figures[12].homePosition[1] = 14 - 3;
                figures[13].homePosition[0] = 14 - 3;
                figures[13].homePosition[1] = 14 - 2;
                figures[14].homePosition[0] = 14 - 2;
                figures[14].homePosition[1] = 14 - 3;
                figures[15].homePosition[0] = 14 - 2;
                figures[15].homePosition[1] = 14 - 2;
            }
            loadPlayersFigures();
        }
        private void loadPlayersFigures()
        {
            for (int i = 0; i < players.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    players[i].figs[j] = figures[i * 4 + j];
                }
            }
        }
        private void LoadPlayers()
        {
            players = new Player[StartPage.NUM_PLAYERS];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
            players[0].color = FigureColor.Red;
            players[1].color = FigureColor.Green;
            if (players.Length >= 3)
                players[2].color = FigureColor.Blue;
            if (players.Length == 4)
                players[3].color = FigureColor.Yellow;
            players[players.Length-1].isTurn = true;
        }
        public void NextPlayer()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isTurn == true)
                {
                    players[i].isTurn = false;
                    players[(i + 1) % players.Length].isTurn = true;
                    return;
                }
            }
        }
        public Player currentPlayer()
        {
            foreach (Player one in players)
            {
                if (one.isTurn) return one;
            }            
            return players[0];

        }

    }
    }

