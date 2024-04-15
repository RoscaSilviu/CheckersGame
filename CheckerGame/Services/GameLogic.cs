using CheckerGame.Models;
using CheckerGame.Properties;
using CheckerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Animation;

namespace CheckerGame.Services
{

    class GameLogic
    {
        public ObservableCollection<ObservableCollection<CellVM>> Board { get; set; }
        public PieceColor CurrentTurn { get; set; }

        public string Winner;
        
        public bool MultipleJumpsAllowed { get; set; }
        public int WhiteRemainingPieces { get; set; }
        public int BlackRemainingPieces { get; set; }
        public GameLogic()
        {
            CurrentTurn = PieceColor.Black; // Se începe cu jucătorul cu piesele negre
            MultipleJumpsAllowed = Settings.Default.IsCheckboxChecked;
        }


        public void UpdateRemainingPieces()
        {
            BlackRemainingPieces = 0;
            WhiteRemainingPieces = 0;
            foreach (ObservableCollection<CellVM> row in Board)
            {
                foreach (CellVM cell in row)
                {
                    if (cell.SimpleCell.Piece != null)
                    {
                        if (cell.SimpleCell.Piece.Color == PieceColor.Black)
                        {
                            BlackRemainingPieces++;
                        }
                        else WhiteRemainingPieces++;

                    }
                }
            }
            if (BlackRemainingPieces == 0 || WhiteRemainingPieces == 0)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            if (BlackRemainingPieces == 0)
            {
                Winner = "White";
                FileHandler.SaveStats(Winner, WhiteRemainingPieces);
            }
            else
            {
                Winner = "Black";
                FileHandler.SaveStats(Winner, BlackRemainingPieces);
            }
        }
        public int IsMoveValid(int row, int column)
        {
            if (row < 0 || row >= Board.Count || column < 0 || column >= Board.Count)
                return 0;
            if (Board[row][column].SimpleCell.Piece == null)
                return 1;
            if (Board[row][column].SimpleCell.Piece.Color != CurrentTurn)
                return 2;
            else return 0;
        }

        List<Position> possibleMoves = new List<Position>();
        private void GetNormalMoves(Cell selectedCell)
        {
            int direction = (selectedCell.Piece.Color == PieceColor.Black) ? 1 : -1;
            Position CurrentPosition = selectedCell.Position;
            if (selectedCell.Piece.Type == PieceType.Checker)
            {

                if (IsMoveValid(CurrentPosition.Row + direction, CurrentPosition.Column - 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row + direction, CurrentPosition.Column - 1));
                }
                if (IsMoveValid(CurrentPosition.Row + direction, CurrentPosition.Column + 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row + direction, CurrentPosition.Column + 1));
                }
            }
            else if (selectedCell.Piece.Type == PieceType.King)
            {
                if (IsMoveValid(CurrentPosition.Row + 1, CurrentPosition.Column - 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row + 1, CurrentPosition.Column - 1));
                }
                if (IsMoveValid(CurrentPosition.Row + 1, CurrentPosition.Column + 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row + 1, CurrentPosition.Column + 1));
                }
                if (IsMoveValid(CurrentPosition.Row -1, CurrentPosition.Column - 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row -1 , CurrentPosition.Column - 1));
                }
                if (IsMoveValid(CurrentPosition.Row -1, CurrentPosition.Column + 1) == 1)
                {
                    possibleMoves.Add(new Position(CurrentPosition.Row -1 , CurrentPosition.Column + 1));
                }
            }
        }
        private void GetCaptureMoves(Cell selectedCell)
        {
            Position CurrentPosition = selectedCell.Position;

            if (selectedCell.Piece.Type == PieceType.Checker)
            {
                int direction = (selectedCell.Piece.Color == PieceColor.Black) ? 1 : -1;

                if (IsMoveValid(CurrentPosition.Row + direction, CurrentPosition.Column - 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row + 2 * direction, CurrentPosition.Column - 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row + 2 * direction, CurrentPosition.Column - 2));
                    }
                }


                if (IsMoveValid(CurrentPosition.Row + direction, CurrentPosition.Column + 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row + 2 * direction, CurrentPosition.Column + 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row + 2 * direction, CurrentPosition.Column + 2));
                    }
                }

            }
            else if (selectedCell.Piece.Type == PieceType.King)
            {
                // Verifică dacă se poate deplasa în sus-stânga
                if (IsMoveValid(CurrentPosition.Row - 1, CurrentPosition.Column - 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row - 2, CurrentPosition.Column - 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row - 2, CurrentPosition.Column - 2));
                    }
                }

                // Verifică dacă se poate deplasa în sus-dreapta
                if (IsMoveValid(CurrentPosition.Row - 1, CurrentPosition.Column + 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row - 2, CurrentPosition.Column + 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row - 2, CurrentPosition.Column + 2));
                    }
                }

                // Verifică dacă se poate deplasa în jos-stânga
                if (IsMoveValid(CurrentPosition.Row + 1, CurrentPosition.Column - 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row + 2, CurrentPosition.Column - 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row + 2, CurrentPosition.Column - 2));
                    }
                }

                // Verifică dacă se poate deplasa în jos-dreapta
                if (IsMoveValid(CurrentPosition.Row + 1, CurrentPosition.Column + 1) == 2)
                {
                    if (IsMoveValid(CurrentPosition.Row + 2, CurrentPosition.Column + 2) == 1)
                    {
                        possibleMoves.Add(new Position(CurrentPosition.Row + 2, CurrentPosition.Column + 2));
                    }
                }
            }

        }

        private void ModifyBoardWithPossibleMoves()
        {
            foreach (Position move in possibleMoves)
            {
                Board[move.Row][move.Column].SimpleCell.CellType = CellType.Green;
                Board[move.Row][move.Column].SimpleCell.DisplayedImage = "/CheckerGame;component/Resources/green.png";
            }
        }
        public bool PieceCaptured = false;
        public void CapturePieces(Cell sourceCell, Cell targetCell)
        {
            // Calculăm poziția piesei capturate
            int capturedRow = (sourceCell.Position.Row + targetCell.Position.Row) / 2;
            int capturedColumn = (sourceCell.Position.Column + targetCell.Position.Column) / 2;

            // Obținem referința către celula piesei capturate
            Cell capturedCell = Board[capturedRow][capturedColumn].SimpleCell;
            capturedCell.Piece = null;
            capturedCell.CellType = CellType.Empty;
            capturedCell.DisplayedImage = "/CheckerGame;component/Resources/empty.png";

            SwapPieces(sourceCell, targetCell);
            if (MultipleJumpsAllowed)
            {
            GetCaptureMoves(Board[targetCell.Position.Row][targetCell.Position.Column].SimpleCell);
            ModifyBoardWithPossibleMoves();
            if (possibleMoves.Count > 0)
                PieceCaptured = true;
            else if (WhiteRemainingPieces !=0 && BlackRemainingPieces !=0)
                    CurrentTurn = (CurrentTurn == PieceColor.Black) ? PieceColor.White : PieceColor.Black;
            }
            else if (WhiteRemainingPieces != 0 && BlackRemainingPieces != 0)
                CurrentTurn = (CurrentTurn == PieceColor.Black) ? PieceColor.White : PieceColor.Black;
            Helper.PreviousCell = Board[targetCell.Position.Row][targetCell.Position.Column].SimpleCell;

        }
        private void SwapPieces(Cell sourceCell, Cell targetCell)
        {
            targetCell.Piece = sourceCell.Piece;
            targetCell.CellType = CellType.Occupied;
            targetCell.DisplayedImage = sourceCell.Piece.Image;
            sourceCell.Piece = null;
            sourceCell.CellType = CellType.Empty;
            sourceCell.DisplayedImage = "/CheckerGame;component/Resources/empty.png";
            RemoveGreenCells();
        }
        private void MovePiece(Cell selectedCell, Position newPosition)
        {
            Cell sourceCell = Board[selectedCell.Position.Row][selectedCell.Position.Column].SimpleCell;
            Cell targetCell = Board[newPosition.Row][newPosition.Column].SimpleCell;
            if (Math.Abs(targetCell.Position.Row - sourceCell.Position.Row) == 1)
            {
                SwapPieces(sourceCell, targetCell);
                if (WhiteRemainingPieces != 0 && BlackRemainingPieces != 0)
                    CurrentTurn = (CurrentTurn == PieceColor.Black) ? PieceColor.White : PieceColor.Black;

            }
            else
            {
                CapturePieces(sourceCell, targetCell);
            }
            if (targetCell.Piece.Type == PieceType.Checker && targetCell.Piece.Color == PieceColor.Black && targetCell.Position.Row == 7)
            {
                targetCell.Piece.Type = PieceType.King;
                targetCell.Piece.Image = "/CheckerGame;component/Resources/blackPieceKing.png";
                targetCell.DisplayedImage = "/CheckerGame;component/Resources/blackPieceKing.png";
            }
            if (targetCell.Piece.Type == PieceType.Checker && targetCell.Piece.Color == PieceColor.White && targetCell.Position.Row == 0)
            {
                targetCell.Piece.Type = PieceType.King;
                targetCell.Piece.Image = "/CheckerGame;component/Resources/whitePieceKing.png";
                targetCell.DisplayedImage = "/CheckerGame;component/Resources/whitePieceKing.png";
            }
        }


        private void RemoveGreenCells()
        {
            possibleMoves.Clear();
            foreach (ObservableCollection<CellVM> row in Board)
            {
                foreach (CellVM cell in row)
                {
                    if (cell.SimpleCell.CellType == CellType.Green)
                    {
                        cell.SimpleCell.CellType = CellType.Empty;
                        cell.SimpleCell.DisplayedImage = "/CheckerGame;component/Resources/empty.png";
                    }
                }
            }
        }


        public void HandleBoardChange(Position pos)
        {
            CellVM selectedCell = Board[pos.Row][pos.Column];
            if (PieceCaptured == false)
            {
                if (selectedCell.SimpleCell.CellType == CellType.Empty)
                {
                    RemoveGreenCells();
                    return;
                }
                if (selectedCell.SimpleCell.CellType == CellType.Occupied && selectedCell.SimpleCell.Piece.Color == CurrentTurn)
                {
                    RemoveGreenCells();
                    GetNormalMoves(selectedCell.SimpleCell);
                    GetCaptureMoves(selectedCell.SimpleCell);
                    if (possibleMoves.Count > 0)
                    {
                        ModifyBoardWithPossibleMoves();
                        Helper.PreviousCell = selectedCell.SimpleCell;
                    }
                    else
                    {
                        RemoveGreenCells();
                    }
                }
                if (selectedCell.SimpleCell.CellType == CellType.Green)
                {
                    MovePiece(Helper.PreviousCell, pos);
                    // RemoveGreenCells();
                }
            }
            else
            {
                if (selectedCell.SimpleCell.CellType == CellType.Green)
                {
                    PieceCaptured = false;
                    MovePiece(Helper.PreviousCell, pos);
                    RemoveGreenCells();
                }
            }
        }

        public Action<Position> OnClickCell;
        public void ClickAction(Cell selectedCell)
        {
            OnClickCell(selectedCell.Position);
        }
    }
}