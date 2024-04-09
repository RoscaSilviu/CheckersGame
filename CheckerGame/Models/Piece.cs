using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerGame.Models
{
    public enum PieceColor { White, Black };
    public enum PieceType { Checker, King };

        class Piece:  BaseNotification
    {
        public Piece(PieceColor color, PieceType type, string image)
        {
            Color = color;
            Type = type;
            Image = image;
        }


        private string image;
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                NotifyPropertyChanged("Image");
            }
        }
        private PieceColor color;
        public PieceColor Color
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }
        private PieceType type;
        public PieceType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

    }
}
