using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace LudoGame.Figures
{
    enum FigureColor
    {
        Red,
        Green,
        Yellow,
        Blue
    }
    class Figure1
    {
        public int[] homePosition { get; set; }
        //public int[] startPosition { get; set; }
        public int startPos { get; set; }
        //public int[] currentPosition { get; set; } 

        public int previousPos { get; set; }

        public int currentPos { get; set; }
        public FigureColor figureColor { get; set; }
        public int stepsLeftToFinish { get; set; } = 55; //ukupan broj preostalih polja se smanjuje najmanje je 1
        public bool isInGame { get; set; } = false; //kada je figura na putanji
        public bool isAtFinish { get; set; } = false; //kada je figura na kraju igre
        public Figure1(FigureColor color)
        {
            figureColor = color;
            homePosition = new int[2];
            //startPosition = new int[2];
            //currentPosition = new int[2];
            switch (color)
            {
                case FigureColor.Red:
                    startPos = 1;
                    break;
                case FigureColor.Green:
                    startPos = 1+13;
                    break;
                case FigureColor.Yellow:
                    startPos = 1+13+13;
                    break;
                case FigureColor.Blue:
                    startPos = 1+13+13+13;
                    break;
            }
            currentPos = -1;
        }
        public Figure1 calculateMove(int steps)
        {
            Figure1 future = new Figure1(figureColor);
            //if (!isMoveable(steps)) return null;
            if (currentPos == -1)
            {
                future.currentPos = -1;
                future.previousPos = currentPos;
                future.stepsLeftToFinish = 55;
                future.figureColor = figureColor;
                future.isAtFinish = false;
                future.isInGame = false;
                return future;
            }
            
            future.currentPos = currentPos;
            future.previousPos = currentPos;
            future.figureColor = figureColor;
            future.stepsLeftToFinish = stepsLeftToFinish-steps;
            future.isInGame = isInGame;
            future.isAtFinish = isAtFinish;
            if (!isAtFinish) //pripada drugom nizu ako je atFinish
            {
                if (future.stepsLeftToFinish > 4)
                    //if ((currentPos + steps) % 52 < startPos - 1)
                    future.currentPos = (currentPos + steps) % 52;
                //else if ((currentPos + steps) % 52 <= startPos - 1 + 4)
                else
                {
                    //currentPos = (currentPos) % 52 - (startPos - 1) + steps;
                    future.currentPos = 4 - future.stepsLeftToFinish;
                    future.isAtFinish = true;
                }
            }
            else if (future.currentPos + steps < 4)
            {
                future.currentPos = currentPos + steps;
            }
            return future;
        }
        public bool Move(int steps) //ako se moze pomjeriti vrati true, ako ne false
        {
            //ako nije dosao do kucice
            //if (isAtFinish && currentPos + steps >= 4) return false; //provjera da li ima dovoljno preostalih polja
            if (!isMoveable(steps)) return false;
            previousPos = currentPos;
            stepsLeftToFinish -= steps;
            if (!isAtFinish) //pripada drugom nizu ako je atFinish
            {
                if (stepsLeftToFinish>4)
                //if ((currentPos + steps) % 52 < startPos - 1)
                    currentPos = (currentPos + steps) % 52;
                //else if ((currentPos + steps) % 52 <= startPos - 1 + 4)
                else
                {
                    //currentPos = (currentPos) % 52 - (startPos - 1) + steps;
                    currentPos = 4  - stepsLeftToFinish;
                    isAtFinish = true;
                }
            }
            else if (currentPos + steps < 4)
            {
                currentPos = currentPos + steps;
            }
            else return false;
            return true;
        }

        public void MoveToHome()
        {
            currentPos = -1;
            //currentPosition[0] = homePosition[0];
            //currentPosition[1] = homePosition[1];
            stepsLeftToFinish = 55;
            isInGame = false;
            isAtFinish = false;
        }
        public void MoveToStart()
        {
            currentPos = startPos;
            isInGame = true;
            isAtFinish = false;
            //currentPosition[0] = startPosition[0];
            //currentPosition[1] = startPosition[1];
        }
        public bool isMoveable(int steps)
        {
            if (stepsLeftToFinish - steps < 1) return false;
            else return true;
        }
    }
}
