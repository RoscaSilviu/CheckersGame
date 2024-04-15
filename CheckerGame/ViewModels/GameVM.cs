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
    class GameVM : INotifyPropertyChanged
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
        //    (GameBoards, gameLogic.CurrentTurn) = FileHandler.LoadGame("gameData.json");
        //    gameLogic.Board = GameBoard;
        //}
        private string _textBoxText;
        public string TextBoxText
        {
            get { return _textBoxText; }
            set
            {
                if (_textBoxText != value)
                {
                    _textBoxText = value;
                    OnPropertyChanged("TextBoxText"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }
        private string currentImagePath = "/CheckerGame;component/Resources/blackPiece.png";

        public string CurrentImagePath
        {
            get { return currentImagePath; }
            set
            {
                if (currentImagePath != value)
                {
                    currentImagePath = value;
                    OnPropertyChanged("CurrentImagePath");
                }
            }
        }
        private string winnerText;
        public string WinnerText
        {
            get { return winnerText; }
            set
            {
                if (winnerText != value)
                {
                    winnerText = value;
                    OnPropertyChanged("WinnerText"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }
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
                            (board, gameLogic.CurrentTurn) = FileHandler.LoadGame();
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
                    saveGame = new RelayCommand<object>(param =>
                    {
                        try
                        {
                           FileHandler.SaveGame(CellVMBoardToCellBoard(GameBoard), gameLogic.CurrentTurn);
                        }
                        catch (Exception ex)
                        {
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
        private bool _isTextBoxVisible = false;
        public bool IsTextBoxVisible
        {
            get { return _isTextBoxVisible; }
            set
            {
                if (_isTextBoxVisible != value)
                {
                    _isTextBoxVisible = value;
                    OnPropertyChanged("IsTextBoxVisible"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }
        private void ExecuteClickAction(Position pos)
        {
            gameLogic.Board = GameBoard;
            gameLogic.HandleBoardChange(pos);
            gameLogic.UpdateRemainingPieces();
            CurrentTurn = gameLogic.CurrentTurn.ToString();
           
            if (CurrentTurn == "White" && DisplayWhiteRemainingText!= "0")
                CurrentImagePath = "/CheckerGame;component/Resources/whitePiece.png";
            else if (CurrentTurn == "Black" && DisplayBlackRemainingText != "0")
                CurrentImagePath = "/CheckerGame;component/Resources/blackPiece.png";
            WinnerText = gameLogic.Winner;
            if (gameLogic.Winner != null)
            {
                CurrentTurnLabel = "Winner: ";
                if (gameLogic.Winner == "White")
                    CurrentImagePath = "/CheckerGame;component/Resources/whitePiece.png";
                else if (gameLogic.Winner == "Black")
                    CurrentImagePath = "/CheckerGame;component/Resources/blackPiece.png";
            }
            ChangeWhiteRemainingText(gameLogic.WhiteRemainingPieces.ToString());
            ChangeBlackRemainingText(gameLogic.BlackRemainingPieces.ToString());
        }

        private string displayWhiteRemainingText = "12";

        public string DisplayWhiteRemainingText
        {
            get { return displayWhiteRemainingText; }
            set
            {
                displayWhiteRemainingText = value;
                OnPropertyChanged("DisplayWhiteRemainingText");
            }
        }
        private string currentTurn = "Black";

        public string CurrentTurn
        {
            get { return currentTurn; }
            set
            {
                currentTurn = value;
                OnPropertyChanged("CurrentTurn");
            }
        }

        public void ChangeWhiteRemainingText(string text)
        {
            DisplayWhiteRemainingText = text;
        }
        private string currentTurnLabel = "Current Turn:";

        public string CurrentTurnLabel
        {
            get { return currentTurnLabel; }
            set
            {
                currentTurnLabel = value;
                OnPropertyChanged("CurrentTurnLabel");
            }
        }
        private string displayBlackRemainingText = "12";

        public string DisplayBlackRemainingText
        {
            get { return displayBlackRemainingText; }
            set
            {
                displayBlackRemainingText = value;
                OnPropertyChanged("DisplayBlackRemainingText");
            }
        }

        public void ChangeBlackRemainingText(string text)
        {
            DisplayBlackRemainingText = text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }






    }
}
