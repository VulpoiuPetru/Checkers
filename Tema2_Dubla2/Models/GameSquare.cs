using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tema2_Dubla2.Services;

namespace Views.Tema2_Dubla2.Models
{
    public class GameSquare : INotifyPropertyChanged
    {
        public enum SquareShade
        {
            Dark,
            Light
        }
        private int row;
        private int column;
        private SquareShade shade;
        private string texture;
        private GamePiece piece;
        private string squareSymbol;

        //constructor pt patratele de pe tabla(linia ,coloana,nuanta,piesa(care se afla pe ea))
        public GameSquare(int row,int column,SquareShade shade,GamePiece piece)
        {
            this.row = row;
            this.column = column;
            this.shade = shade;
           if(shade==SquareShade.Dark)
            {
               texture=Elements.brownSquare;
            }
           else 
            {
                texture=Elements.whiteBrownSquare;
            }
            this.piece = piece;
        }
        //propietati
        public int Row
        { get { return row; } }
        public int Column { get { return column; } }
        //NotifyPropertyChanged - folosit pt a notifica interfata de anumite schimbari(din cauza texturii,piesei,simbolul de pe patrat) 
        public SquareShade Shade { get { return shade; } }
        public string Texture { 
            get { return texture; }
            set { texture = value; NotifyPropertyChanged("Texture"); }
        }
        public GamePiece Piece
        {
            get { return piece; }
            set { piece = value;NotifyPropertyChanged("Piece"); }
        }
        public string SquareSymbol
        {
            get { return squareSymbol; }
            set { squareSymbol = value; NotifyPropertyChanged("SquareSymbol"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //CallerMemberName - un atribut folosit pentru a returna numele proprietatii modificate catre metoda OnPropertyChanged
        //OnPropertyChanged se apeleaza in set-ul unei proprietati, deci acel nume de proprietate va fi returnat
        //Invoke - apeleaza delegatul care asociaza handler-ul de evenimente lui PropertyChanged
        //a se vedea parametrii lui Invoke - sunt cei ai unei metode care se executa la declansarea unui eveniment
        //PropertyChanged? inlocuieste if (PropertyChanged != null)
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
