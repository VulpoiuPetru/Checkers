using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tema2_Dubla2.Services;

namespace Views.Tema2_Dubla2.Models
{
    public class GamePiece: INotifyPropertyChanged
    {
        public enum PieceColor
        {
            Red,
            White
        }
        public enum PieceType
        {
            Regular,
            King
        }
        private PieceColor color;
        private PieceType type;
        private string texture;
        private GameSquare square;

        //constructor doar pt culoare
        public GamePiece(PieceColor color)
        {
            this.color = color;
            type = PieceType.Regular;
            if(color == PieceColor.Red) 
            {
                texture = Elements.redPiece;

            }
            else 
            {
                texture = Elements.whitePiece;
            }
        }
        //constructor pt culoare si tipul sau
        public GamePiece(PieceColor color,PieceType type)
        {
            this.color = color;
            this.type = type;
            if (color == PieceColor.Red)
            {
                texture = Elements.redPiece;

            }
            else
            {
                texture = Elements.whitePiece;
            }
            if(type == PieceType.King&&color==PieceColor.Red)
            {
                texture = Elements.kingRedPiece;
            }
            else
                if(type==PieceType.King&&color == PieceColor.White) 
            {
                texture = Elements.kingWhitePiece;
            }
        }
        //propietatile(set+get)
        public PieceColor Color
        {
            get { return color; }
        }
        //NotifyPropertyChanged(aceasta notifica interfata de utilizator ca propietatea clasei
        //i s-a schimbat,ajuta la reactualizarea paginii,folosind Data biding,acest lucru este pentru spre ex pt
        //jeton,cand ajunge la capatul celalat acesta sa se schimba in rege)
        public PieceType Type
        {
            get { return type; }
            set { type = value;NotifyPropertyChanged("Type"); }
        }
        public string Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                NotifyPropertyChanged("Texture");
            }
        }
        public GameSquare Square
        {
            get
            {
                return square;
            }
            set
            {
                square = value;
                NotifyPropertyChanged("Square");
            }
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
