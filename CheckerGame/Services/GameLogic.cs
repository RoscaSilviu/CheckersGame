using CheckerGame.Models;
using CheckerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace CheckerGame.Services
{
    class GameLogic
    {
        public ObservableCollection<ObservableCollection<CellVM>> Board { get; set; }
        public PieceColor CurrentTurn { get; private set; }

        public GameLogic()
        {
            CurrentTurn = PieceColor.Black; // Se începe cu jucătorul cu piesele negre
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

        public List<Position> GetPossibleMoves(Cell selectedCell)
        {
            List<Position> possibleMoves = new List<Position>();

            if (selectedCell.Piece.Type == PieceType.Checker)
            {
                int direction = (selectedCell.Piece.Color == PieceColor.Black) ? 1 : -1;

                // Verificăm dacă mutarea diagonală în față-stânga este validă
                if (IsMoveValid(selectedCell.Position.Row + direction, selectedCell.Position.Column - 1) == 1)
                {
                    possibleMoves.Add(new Position(selectedCell.Position.Row + direction, selectedCell.Position.Column - 1));
                }
                else if (IsMoveValid(selectedCell.Position.Row + direction, selectedCell.Position.Column - 1) == 2)
                {
                    if (IsMoveValid(selectedCell.Position.Row + 2*direction, selectedCell.Position.Column - 2) == 1)
                    possibleMoves.Add(new Position(selectedCell.Position.Row + 2* direction, selectedCell.Position.Column - 2));
                }
                // Verificăm dacă mutarea diagonală în față-dreapta este validă
                if (IsMoveValid(selectedCell.Position.Row + direction, selectedCell.Position.Column + 1) == 1)
                {
                    possibleMoves.Add(new Position(selectedCell.Position.Row + direction, selectedCell.Position.Column + 1));
                }
                else if (IsMoveValid(selectedCell.Position.Row + direction, selectedCell.Position.Column + 1) == 2)
                {
                    if (IsMoveValid(selectedCell.Position.Row + 2*direction, selectedCell.Position.Column + 2) == 1)
                    possibleMoves.Add(new Position(selectedCell.Position.Row + 2*direction, selectedCell.Position.Column + 2));
                }
            }

            return possibleMoves;
        }

        private void ModifyBoardWithPossibleMoves(List<Position> possibleMoves)
        {
            foreach (Position move in possibleMoves)
            {
                Board[move.Row][move.Column].SimpleCell.CellType = CellType.Green;
                Board[move.Row][move.Column].SimpleCell.DisplayedImage = "/CheckerGame;component/Resources/green.png";
            }
        }
        public void CapturePiece(Cell sourceCell, Cell targetCell)
        {
            // Calculăm poziția piesei capturate
            int capturedRow = (sourceCell.Position.Row + targetCell.Position.Row) / 2;
            int capturedColumn = (sourceCell.Position.Column + targetCell.Position.Column) / 2;

            // Obținem referința către celula piesei capturate
            Cell capturedCell = Board[capturedRow][capturedColumn].SimpleCell;

            // Eliminăm piesa capturată din joc
            capturedCell.Piece = null;
            capturedCell.CellType = CellType.Empty;
            capturedCell.DisplayedImage = "/CheckerGame;component/Resources/empty.png";
        }


        private void MovePiece(Cell selectedCell, Position newPosition)
        {
            Cell sourceCell = Board[selectedCell.Position.Row][selectedCell.Position.Column].SimpleCell;
            Cell targetCell = Board[newPosition.Row][newPosition.Column].SimpleCell;
            if (Math.Abs(targetCell.Position.Row - sourceCell.Position.Row) == 2)
                CapturePiece(sourceCell, targetCell);
            targetCell.Piece = sourceCell.Piece;
            targetCell.CellType = CellType.Occupied;
            targetCell.DisplayedImage = sourceCell.Piece.Image;
            sourceCell.Piece = null;
            sourceCell.CellType = CellType.Empty;
            sourceCell.DisplayedImage = "/CheckerGame;component/Resources/empty.png";

            CurrentTurn = (CurrentTurn == PieceColor.Black) ? PieceColor.White : PieceColor.Black;
        }

        private void RemoveGreenCells()
        {
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

        public Action<Position> OnClickCell;

        public void HandleBoardChange(Position pos)
        {
            CellVM selectedCell = Board[pos.Row][pos.Column];
            if (selectedCell.SimpleCell.CellType == CellType.Empty)
            {
                RemoveGreenCells();
                return;
            }
            if (selectedCell.SimpleCell.CellType == CellType.Occupied && selectedCell.SimpleCell.Piece.Color == CurrentTurn)
            {
                List<Position> possibleMoves = GetPossibleMoves(selectedCell.SimpleCell);
                if (possibleMoves.Count > 0)
                {
                    RemoveGreenCells();
                    ModifyBoardWithPossibleMoves(possibleMoves);
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
                RemoveGreenCells();
            }
        }

        public void ClickAction(Cell selectedCell)
        {
            OnClickCell(selectedCell.Position);
        }
    }
}