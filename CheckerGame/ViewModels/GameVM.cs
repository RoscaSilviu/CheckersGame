using CheckerGame.Commands;
using CheckerGame.Models;
using CheckerGame.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckerGame.ViewModels
{
    class GameVM
    {
        private GameLogic gameLogic = new GameLogic();
        public ObservableCollection<ObservableCollection<CellVM>> GameBoard { get; set; }
        public ObservableCollection<ObservableCollection<Cell>> board { get; set; }
        public GameVM()
        {
            StartGame();
            gameLogic.OnClickCell += ExecuteClickAction;
        }
        //public void LoadGame(Object obj)
        //{
        //    (GameBoard, gameLogic.CurrentTurn) = FileHandler.LoadGame("gameData.json");
        //    gameLogic.Board = GameBoard;
        //}

        private ICommand loadGame;
        public ICommand LoadGame
        {
            get
            {
                if (loadGame == null)
                {
                    loadGame = new RelayCommand<object>(param =>
                    {
                        try
                        {
                            string filePath = "gameData.json"; 
                            (board, gameLogic.CurrentTurn) = FileHandler.LoadGame(filePath);
                            GameBoard[0][0].SimpleCell.DisplayedImage = "/CheckerGame;component/Resources/green.png";
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    GameBoard[i][j].SimpleCell.CellType = board[i][j].CellType;
                                    GameBoard[i][j].SimpleCell.DisplayedImage = board[i][j].DisplayedImage;
                                    GameBoard[i][j].SimpleCell.Piece = board[i][j].Piece;
                                    GameBoard[i][j].SimpleCell.Position = board[i][j].Position;
                                }
                            }
                            gameLogic.Board = GameBoard;
                        }
                        catch (Exception ex)
                        {
                            
                            Console.WriteLine($"Eroare la încărcarea jocului: {ex.Message}");
                        }
                    });
                }
                return loadGame;
            }
        }
        private ICommand saveGame;
        public ICommand SaveGame
        {
            get
            {
                if (saveGame == null)
                {
                    // Definește comanda de încărcare a jocului
                    saveGame = new RelayCommand<object>(param =>
                    {
                        // Apelează funcția LoadGame pentru a încărca datele jocului
                        try
                        {
                           FileHandler.SaveGame(CellVMBoardToCellBoard(GameBoard), gameLogic.CurrentTurn);
                            // Aici poți face ce dorești cu datele încărcate, cum ar fi actualizarea ViewModel-ului sau a altor componente din aplicație
                        }
                        catch (Exception ex)
                        {
                            // Tratează orice excepție care ar putea apărea în timpul încărcării jocului
                            // De obicei, ar fi bine să afișezi un mesaj utilizatorului sau să înregistrezi detaliile excepției în scopuri de depanare
                            Console.WriteLine($"Eroare la salvarea jocului: {ex.Message}");
                        }
                    });
                }
                return saveGame;
            }
        }
        public void StartGame()
        {
            board = Helper.InitializeBoard();
            GameBoard = CellBoardToCellVMBoard(board);
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
        private ObservableCollection<ObservableCollection<Cell>> CellVMBoardToCellBoard(ObservableCollection<ObservableCollection<CellVM>> boardVM)
        {
            ObservableCollection<ObservableCollection<Cell>> result = new ObservableCollection<ObservableCollection<Cell>>();
            for (int i = 0; i < boardVM.Count; i++)
            {
                ObservableCollection<Cell> line = new ObservableCollection<Cell>();
                for (int j = 0; j < boardVM[i].Count; j++)
                {
                    CellVM cellVM = boardVM[i][j];
                    Cell cell = cellVM.SimpleCell;
                    line.Add(cell);
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
