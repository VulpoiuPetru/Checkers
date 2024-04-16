using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.Services;

namespace Tema2_Dubla2.ViewModels
{
    public class WinnerVM:BaseNotification
    {
        private GameLogic gameLogic;
        private Winner winner;
        public WinnerVM(GameLogic gameLogic,Winner winner)
        {
            this.winner = winner;  
            this.gameLogic=gameLogic;
        }
        public Winner WinnerPlayer { 
            get { return winner; }
            set { winner = value;NotifyPropertyChanged("WinnerPlayer"); } }
    }
}
