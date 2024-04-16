using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.Services;

namespace Tema2_Dubla2.ViewModels
{
    public class PlayerTurnVM : BaseNotification
    {
        private GameLogic gameLogic;
        private PlayerTurn playerTurn;

        public PlayerTurnVM(GameLogic gameLogic, PlayerTurn playerTurn)
        {
            this.gameLogic = gameLogic;
            this.playerTurn = playerTurn;
        }
        public PlayerTurn PlayerIcon
        {
            get
            {
                return playerTurn;
            }
            set {
            playerTurn = value;
                NotifyPropertyChanged("PlayerIcon");
            }
        }
    }
}
