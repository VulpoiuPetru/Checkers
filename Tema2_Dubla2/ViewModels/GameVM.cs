using System;
using System.IO;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Views.Tema2_Dubla2.Models;
using Tema2_Dubla2.Services;
using static Views.Tema2_Dubla2.Models.GamePiece;
using Tema2_Dubla2.Models;
using System.Reflection;
using System.Windows.Input;
using Tema2_Dubla2.Commands;
using System.ComponentModel;

namespace Tema2_Dubla2.ViewModels
{
    public class GameVM:INotifyPropertyChanged
    {
      
        public ObservableCollection<ObservableCollection<GameSquareVM>> Board { get; set; }
        public GameLogic Logic { get; set; }
        public ButtonInteractionVM Interactions { get; set; }

        public WinnerVM WinnerVM { get; set; }

        public PlayerTurnVM PlayerTurnVM { get; set; }

        public string Red_Piece { get; set; }
        public string White_Piece { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _whitePiecesScore;
        public int WhitePiecesScore
        {
            get { return _whitePiecesScore; }
            set
            {
                _whitePiecesScore = value;
                OnPropertyChanged(nameof(WhitePiecesScore));
            }
        }

        private int _redPiecesScore;
        public int RedPiecesScore
        {
            get { return _redPiecesScore; }
            set
            {
                _redPiecesScore = value;
                OnPropertyChanged(nameof(RedPiecesScore));
            }
        }

        private bool _isCheckBoxEnabled = true;
        public bool IsCheckBoxEnabled
        {
            get { return _isCheckBoxEnabled; }
            set
            {
                _isCheckBoxEnabled = value;
                OnPropertyChanged(nameof(IsCheckBoxEnabled));
            }
        }
      
        public ICommand NewGameCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand StatisticsCommand { get; private set; }

        private bool _allowMultipleMoves;
        public bool AllowMultipleMoves
        {
            get { return _allowMultipleMoves; }
            set
            {
                if (_allowMultipleMoves != value)
                {
                    _allowMultipleMoves = value;
                    OnPropertyChanged(nameof(AllowMultipleMoves));
                    Elements.AllowMultipleMoves = value;
                    UpdateAllowMultipleMovesFile(value);
                }
            }
        }

       
        private void UpdateAllowMultipleMovesFile(bool value)
        {

            string filePath = "../../Resources/allowMultipleMoves.txt";

            try
            {
                File.WriteAllText(filePath, value.ToString());
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Eroare la actualizarea fișierului: " + ex.Message);
            }
        }

       
        private bool LoadAllowMultipleMovesFromFile()
        {
            string filePath = "../../Resources/allowMultipleMoves.txt";
            bool defaultValue = false;
            
            try
            {
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    if (bool.TryParse(fileContent, out bool result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
              
                Console.WriteLine("Eroare la citirea fișierului: " + ex.Message);
            }

            return defaultValue;
        }

        public GameVM()
        {
           
            AllowMultipleMoves = LoadAllowMultipleMovesFromFile();
            
            ObservableCollection<ObservableCollection<GameSquare>> board = Elements.initBoard();
            PlayerTurn playerTurn = new PlayerTurn(PieceColor.Red);
           
            Winner winner = new Winner(0, 0);
            Logic = new GameLogic(board, playerTurn, winner,this);
            PlayerTurnVM = new PlayerTurnVM(Logic, playerTurn);
            WinnerVM = new WinnerVM(Logic, winner);
            Board = CellBoardToCellVMBoard(board);
            Interactions = new ButtonInteractionVM(Logic);
            Red_Piece = Elements.redPiece;
            White_Piece = Elements.whitePiece;
           WhitePiecesScore = Elements.RemainWhitePieces;
            RedPiecesScore=Elements.RemainRedPieces;

          
            Elements.GameViewModel = this;

         
        }
        private ObservableCollection<ObservableCollection<GameSquareVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
            ObservableCollection<ObservableCollection<GameSquareVM>> result = new ObservableCollection<ObservableCollection<GameSquareVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<GameSquareVM> line = new ObservableCollection<GameSquareVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    GameSquare c = board[i][j];
                    GameSquareVM cellVM = new GameSquareVM(c, Logic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
        

    }
}