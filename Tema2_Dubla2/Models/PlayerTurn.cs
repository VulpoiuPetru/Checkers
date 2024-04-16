using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2_Dubla2.Services;
using Views.Tema2_Dubla2.Models;

namespace Tema2_Dubla2.Models
{
    public class PlayerTurn:BaseNotification
    {
        public GamePiece.PieceColor pieceColor;
        public string image;

        public PlayerTurn(GamePiece.PieceColor piece)
        {
            this.pieceColor = piece;
            loadImage();
        }

        public void loadImage()
        {
            if(pieceColor==GamePiece.PieceColor.Red)
            {
                image = Elements.redPiece;
                return;
            }
           
                image= Elements.whitePiece;
        }
        public GamePiece.PieceColor PlayerColor
        { get { return pieceColor; }
        set { pieceColor = value; NotifyPropertyChanged("PlayerColor"); }
        }
        public string TurnImage
        { get { return image; } set {  image = value; NotifyPropertyChanged("TurnImage"); } }
    }
}
