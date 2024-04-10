using CheckerGame.Models;
using CheckerGame.Services;
using CheckerGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace CheckerGame.ViewModels
{
    class CellVM
    {
        public Cell SimpleCell { get; set; }
        GameLogic gameLogic;

        public CellVM() { }
        public CellVM(Cell cell, GameLogic gameLogic)
        {
            SimpleCell = new Cell(cell.CellType, cell.Position, cell.Piece, cell.DisplayedImage);
            this.gameLogic = gameLogic;
            
        }

        //am adus celula din Model in VM
        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Cell>(gameLogic.ClickAction);
                }
                return clickCommand;
            }
        }

    }
}
