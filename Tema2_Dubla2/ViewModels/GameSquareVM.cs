using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema2_Dubla2.Commands;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.Services;
using Views.Tema2_Dubla2.Models;

namespace Tema2_Dubla2.ViewModels
{
    public class GameSquareVM : BaseNotification
    {
        private GameLogic gameLogic;
        private GameSquare genericSquare;
        private ICommand clickPieceCommand;
        private ICommand movePieceCommand;

        public GameSquareVM(GameSquare square, GameLogic gameLogic)
        {
            if(square == null) 
            {
                throw new ArgumentNullException(nameof(square));
            }
            if(gameLogic == null) 
            {
                throw new ArgumentNullException(nameof(gameLogic));
            }
            genericSquare = square;
            this.gameLogic = gameLogic;
        }

        public GameSquare GenericSquare
        {
            get
            {
                return genericSquare;
            }
            set
            {
                genericSquare = value;
                NotifyPropertyChanged("GenericSquare");
            }
        }

        public ICommand ClickPieceCommand
        {
            get
            {
                if (clickPieceCommand == null)
                {
                    clickPieceCommand = new RelayCommand<GameSquare>(gameLogic.ClickPiece);
                }
                return clickPieceCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<GameSquare>(gameLogic.MovePiece);
                }
                return movePieceCommand;
            }
        }
    }
}
