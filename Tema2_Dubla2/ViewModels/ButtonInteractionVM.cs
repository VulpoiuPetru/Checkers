using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tema2_Dubla2.Commands;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.Services;
using Tema2_Dubla2.Views;

namespace Tema2_Dubla2.ViewModels
{
    public class ButtonInteractionVM:BaseNotification
    {
        public GameLogic gameLogic;
       // private static bool permissionAsked = false;
        public ICommand newGameCommand;
        public ICommand saveCommand;
        public ICommand loadCommand;
        public ICommand aboutCommand;
        public ICommand statisticsCommand;
        private AboutWindow _aboutWindow;
        public ButtonInteractionVM(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
          
        }
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                {
                    newGameCommand = new RelayCommand<object>(ExecuteNewGame);
                }
                return newGameCommand;
            }
        }

        private void ExecuteNewGame(object parameter)
        {
            gameLogic.ResetGame();
            
        }


        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    
                    saveCommand=new NonGenericCommand(gameLogic.SaveGame);
                }
                return saveCommand;
            }
        }
        public ICommand LoadCommand
        {
            get 
            {
                if(loadCommand == null)
                {
                    loadCommand=new NonGenericCommand(gameLogic.LoadGame);
                }
                return loadCommand;
            }
        }
        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new RelayCommand<object>(ExecuteAbout);
                }
                return aboutCommand;
            }
        }

        private void ExecuteAbout(object parameter)
        {
            
            if (_aboutWindow == null)
            {
                
                _aboutWindow = new AboutWindow();
                _aboutWindow.Closed += AboutWindow_Closed;
                _aboutWindow.Show();
            }
            else
            {
                _aboutWindow.Activate();
            }
        }

        private void AboutWindow_Closed(object sender, EventArgs e)
        {
           
            _aboutWindow = null;
        }

        public ICommand StatisticsCommand
        {
            get
            {
                if (statisticsCommand == null)
                {
                   
                    statisticsCommand = new RelayCommand<object>(ExecuteStatistics);
                }
                return statisticsCommand;
            }
        }
        private void ExecuteStatistics(object parameter)
        {
            
            string filePath = "../../Resources/statistics.txt";
            try
            {
                string statisticsContent = File.ReadAllText(filePath);
                
                MessageBox.Show(statisticsContent, "Statistics");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading statistics file: " + ex.Message);
            }
           
        }

        


    }
}