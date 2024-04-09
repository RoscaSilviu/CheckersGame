using System.Collections.ObjectModel;

namespace CheckerGame.Models
{
    class Board
    {
        public const int Size = 8;
        public int GetSize()
        {
            return Size;
        }

        private ObservableCollection<ObservableCollection<Cell>> cells;
        public Cell selectedCell { get; set; }
        public ObservableCollection<ObservableCollection<Cell>> Cells
        {
            get { return cells; }
            set { cells = value; }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public Board(string image)
        {
            InitializeBoard();
            Image = image;
        }

        private void InitializeBoard()
        {
            // Inițializarea tablă cu celule în poziția de început
            Cells = new ObservableCollection<ObservableCollection<Cell>>();

            // Adăugarea celulelor goale
            for (int row = 0; row < Size; row++)
            {
                ObservableCollection<Cell> rowCells = new ObservableCollection<Cell>();
                for (int col = 0; col < Size; col++)
                {
                    rowCells.Add(new Cell(CellType.Empty, new Position(row, col)));
                }
                Cells.Add(rowCells);
            }

            // Adăugarea pieselor negre
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < Size; col++)
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
            for (int row = Size - 3; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        Cells[row][col].Piece = new Piece(PieceColor.White, PieceType.Checker, "/CheckerGame;component/Resources/whitePiece.png");
                        Cells[row][col].CellType = CellType.Occupied;
                        Cells[row][col].DisplayedImage = Cells[row][col].Piece.Image;
                    }
                }
            }
        }
    }
}
