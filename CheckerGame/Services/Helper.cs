using CheckerGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace CheckerGame.Services
{
    class Helper
    {
        public static Cell CurrentCell { get; set; }
        public static Cell PreviousCell { get; set; }

        
        public static ObservableCollection<ObservableCollection<Cell>> InitializeBoard()
        {
            // Inițializarea tablă cu celule în poziția de început
            ObservableCollection<ObservableCollection<Cell>>  Cells = new ObservableCollection<ObservableCollection<Cell>>();

            // Adăugarea celulelor goale
            for (int row = 0; row < 8; row++)
            {
                ObservableCollection<Cell> rowCells = new ObservableCollection<Cell>();
                for (int col = 0; col < 8; col++)
                {
                    rowCells.Add(new Cell(CellType.Empty, new Position(row, col)));
                    rowCells[col].DisplayedImage="/CheckerGame;component/Resources/empty.png";
                }
                Cells.Add(rowCells);
            }

            // Adăugarea pieselor negre
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        Cells[row][col].Piece = new Piece(PieceColor.Black, PieceType.Checker, "/CheckerGame;component/Resources/blackPiece.png");
                        Cells[row][col].CellType = CellType.Occupied;
                        Cells[row][col].DisplayedImage = Cells[row][col].Piece.Image;
                    }
                }
            }

            // Adăugarea pieselor albe
            for (int row = 8 - 3; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        Cells[row][col].Piece = new Piece(PieceColor.White, PieceType.Checker, "/CheckerGame;component/Resources/whitePiece.png");
                        Cells[row][col].CellType = CellType.Occupied;
                        Cells[row][col].DisplayedImage = Cells[row][col].Piece.Image;
                    }
                }
            }
            return Cells;
        }


    }
}
