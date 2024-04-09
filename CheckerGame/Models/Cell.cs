using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerGame.Models
{
    public enum CellType { Empty, Green, Occupied };

    class Cell : BaseNotification
    {
        private Piece _piece; // Piesa care ocupă această celulă (poate fi null)
        private Position _position; // Poziția acestei celule pe tablă
        private CellType _cellType; // Tipul de celulă (goală, verde, ocupată)
        private string displayedImage;
        public string DisplayedImage
        {
            get
            {
                return displayedImage;
            }
            set
            {
                displayedImage = value;
                NotifyPropertyChanged();
            }
        }
        public Piece Piece
        {
            get { return _piece; }
            set { _piece = value; NotifyPropertyChanged();
            }
        }

        public Position Position
        {
            get { return _position; }
            set { _position = value;
                NotifyPropertyChanged();
            }
        }

        public CellType CellType
        {
            get { return _cellType; }
            set { _cellType = value; NotifyPropertyChanged();
            }
        }

        public Cell(CellType cellType, Position position)
        {
            _cellType = cellType;
            _position = position;
            _piece = null; // Inițial, celula este goală
        }

        public Cell(CellType cellType, Piece piece, Position position)
        {
            _cellType = cellType;
            _piece = piece;
            _position = position;
        }
        public Cell(CellType cellType, Position position,Piece piece, string image)
        {
            _cellType = cellType;
            _position = position;
            _piece = piece;
            DisplayedImage = image;
        }
        public Cell(ViewModels.CellVM cellVM)
        {
            _cellType = cellVM.SimpleCell.CellType;
            _position = cellVM.SimpleCell.Position;
            _piece = cellVM.SimpleCell.Piece;
            DisplayedImage = cellVM.SimpleCell.DisplayedImage;
        }
        
    }
}

