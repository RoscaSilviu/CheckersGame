using CheckerGame.Models;
using CheckerGame.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerGame.ViewModels
{
    class GameVM 
    {
        private GameLogic gameLogic;
        public ObservableCollection<ObservableCollection<CellVM>> GameBoard { get; set; }
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Helper.InitializeBoard();
            gameLogic = new GameLogic();
            GameBoard = CellBoardToCellVMBoard(board);
            gameLogic.OnClickCell += ExecuteClickAction;
            

        }
        private ObservableCollection<ObservableCollection<CellVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Cell>> board)
        {
            ObservableCollection<ObservableCollection<CellVM>> result = new ObservableCollection<ObservableCollection<CellVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<CellVM> line = new ObservableCollection<CellVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Cell c = board[i][j];
                    CellVM cellVM = new CellVM(c, gameLogic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
        
        private void ExecuteClickAction(Position pos)
        {
            gameLogic.Board = GameBoard;
            gameLogic.HandleBoardChange(pos);
        }
        
    }
}
