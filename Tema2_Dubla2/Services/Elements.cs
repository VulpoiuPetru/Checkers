using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.ViewModels;
using Views.Tema2_Dubla2.Models;
using static Views.Tema2_Dubla2.Models.GamePiece;

namespace Tema2_Dubla2.Services
{
    public class Elements
    {
       
        public const string redPiece = "/Tema2_Dubla2;component/Resources/redPiece.png";

        public const string kingRedPiece = "/Tema2_Dubla2;component/Resources/kingRedPiece.png";
        public const string whitePiece = "/Tema2_Dubla2;component/Resources/whitePiece.png";
        public const string kingWhitePiece = "/Tema2_Dubla2;component/Resources/kingWhitePiece.png";
        public const string brownSquare = "/Tema2_Dubla2;component/Resources/squareBrown.png";
        public const string whiteBrownSquare = "/Tema2_Dubla2;component/Resources/squarewhiteBrown.png";
        public const string hintSquare = "/Tema2_Dubla2;component/Resources/squareHint.png";
        public const string evidenceSquare = "NULL";
        
        public string allowMoves = "../../Resources/allowMultipleMoves.txt";

       
        public const char no_Piece = 'N';
        public const char white = 'W';
        public const char red = 'R';
        public const char white_King = 'V';
        public const char red_King = 'K';
        public const char white_Turn = '1';
        public const char red_Turn = '2';
        public const char combo = 'C';
        public const char extra_Path = 'E';
        public const char allowMultipleMoves = 'A';
        public const char disallowMultipleMoves = 'D';


        private static GameVM _gameViewModel;

        public static GameVM GameViewModel
        {
            get { return _gameViewModel; }
            set { _gameViewModel = value; }
        }

       
        public const int boardSize = 8;

        public static GameSquare CurrentSquare { get; set; }

       

        private static Dictionary<GameSquare, GameSquare> currentNeighbours = new Dictionary<GameSquare, GameSquare>();
        private static PlayerTurn turn = new PlayerTurn(PieceColor.Red);
        private static bool extraMove = false;
        private static bool extraPath = false;
        private static int remainRedPieces = 12;
        private static int remainWhitePieces = 12;

        public static Dictionary<GameSquare, GameSquare> CurrentNeighbours
        {
            get { return currentNeighbours; }
            set { currentNeighbours = value; }
        }
        public static PlayerTurn Turn
        {
            get { return turn; }
            set { turn = value; }
        }
        public static int RemainWhitePieces
        {
            get { return remainWhitePieces; }
            set { remainWhitePieces = value; }
        }

        public static int RemainRedPieces
        {
            get { return remainRedPieces; }
            set { remainRedPieces = value; }
        }
        public static bool ExtraMove
        {
            get { return extraMove; }
            set { extraMove = value; }
        }
        public static bool ExtraPath
        {
            get { return extraPath; }
            set { extraPath = value; }
        }


      
        public static ObservableCollection<ObservableCollection<GameSquare>> initBoard()
        {
            ObservableCollection<ObservableCollection<GameSquare>> board = new ObservableCollection<ObservableCollection<GameSquare>>();
            const int boardSize = 8;
            for (int i = 0; i < boardSize; i++)
            {
                board.Add(new ObservableCollection<GameSquare>());
                for (int j = 0; j < boardSize; j++)
                {
                    
                    if ((i + j) % 2 == 0)
                    {
                        board[i].Add(new GameSquare(i, j, GameSquare.SquareShade.Light, null));
                    }
                    else
                   
                    if (i < 3)
                    {
                        board[i].Add(new GameSquare(i, j, GameSquare.SquareShade.Dark, new GamePiece(PieceColor.White)));
                    }
                    else
                   
                    if (i > 4)
                    {
                        board[i].Add(new GameSquare(i, j, GameSquare.SquareShade.Dark, new GamePiece(PieceColor.Red)));
                    }
                    else
                    {   
                        board[i].Add(new GameSquare(i, j, GameSquare.SquareShade.Dark, null));
                    }
                }
            }
            return board;
        }
        
        public static bool isInBound(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
        }
        public static void movePiece(GameSquare square, HashSet<Tuple<int, int>> movePiece)
        {
            if (square.Piece.Type == PieceType.King)
            {
                movePiece.Add(new Tuple<int, int>(-1, -1));
                movePiece.Add(new Tuple<int, int>(-1, 1));
                movePiece.Add(new Tuple<int, int>(1, -1));
                movePiece.Add(new Tuple<int, int>(1, 1));
            }
            else
                if (square.Piece.Color == PieceColor.Red)
            {
                movePiece.Add(new Tuple<int, int>(-1, -1));
                movePiece.Add(new Tuple<int, int>(-1, 1));
            }
            else
            {
                movePiece.Add(new Tuple<int, int>(1, -1));
                movePiece.Add(new Tuple<int, int>(1, 1));
            }

        }

        public static bool AllowMultipleMoves { get; set; } = false;
        public static void ResetGameboard(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
            for (int i = 0; i < board.Count; i++)
                for (int j = 0; j < board[i].Count; j++)
                {
                    if ((i + j) % 2 == 0)
                        board[i][j].Piece = null;
                    else
                        if (i < 3)
                    {
                        board[i][j].Piece = new GamePiece(PieceColor.White);
                        board[i][j].Piece.Square = board[i][j];
                    }
                    else
                    if (i > 4)
                    {
                        board[i][j].Piece = new GamePiece(PieceColor.Red);
                        board[i][j].Piece.Square = board[i][j];
                    }
                    else
                    {
                        board[i][j].Piece = null;
                    }
                }
            
        }
       
        public static void ResetGame(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
            foreach (var square in CurrentNeighbours.Keys)
                square.SquareSymbol = null;
            if (CurrentSquare != null)
                CurrentSquare.Texture = brownSquare;

            currentNeighbours.Clear();
            CurrentSquare = null;
            ExtraMove = false;
            ExtraPath = false;
            RemainRedPieces = 12;
            RemainWhitePieces = 12;

            
            if (GameViewModel != null)
            {
                GameViewModel.RedPiecesScore = RemainRedPieces;
                GameViewModel.WhitePiecesScore = RemainWhitePieces;
               
                GameViewModel.IsCheckBoxEnabled = true;
            }

            Turn.PlayerColor = PieceColor.Red;
            ResetGameboard(board);
        }
        
        public static void SaveSettings(bool allowMultipleMoves)
        {
           
            Elements elements = new Elements();

           
            string filePath = elements.allowMoves;

           
            string content = allowMultipleMoves.ToString();

            
            File.WriteAllText(filePath, content);
        }
       
        public static void SaveGame(ObservableCollection<ObservableCollection<GameSquare>>squares)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            bool? answers = saveFileDialog.ShowDialog();
            if (answers == true) 
            {
                var path=saveFileDialog.FileName;
                using(var writer=new StreamWriter(path)) 
                {
                   
                    if(CurrentSquare!= null)
                    {
                       
                        writer.Write(CurrentSquare.Row.ToString()+CurrentSquare.Column.ToString());
                    }
                    else
                    {
                        writer.Write(no_Piece);
                    }
                    writer.WriteLine();
                   
                    if(ExtraMove)
                    {
                        writer.Write(combo);
                    }
                    else
                    {
                        writer.Write(no_Piece);
                    }
                    writer.WriteLine();
                    
                    if (CurrentSquare != null && CurrentSquare.Piece != null)
                    {
                        if (CurrentSquare.Piece.Color == PieceColor.Red)
                        {
                            writer.Write(red_Turn);
                        }
                        else
                        {
                            writer.Write(white_Turn);
                        }
                    }
                    else
                    {
                       
                        writer.Write(Turn.PlayerColor == PieceColor.Red ? red_Turn : white_Turn);
                    }

                   


                   
                    writer.WriteLine();
                    if(AllowMultipleMoves==true) 
                    {
                        writer.Write(allowMultipleMoves);
                    }
                    else
                    {
                        writer.Write(disallowMultipleMoves);
                    }
                    writer.WriteLine();
                    
                    foreach (var line in squares)
                    {
                        foreach(var line1 in line)
                        {
                            if (line1.Piece == null)
                            {
                                writer.Write(no_Piece);
                            }
                            else if (line1.Piece.Color == PieceColor.Red && line1.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(red);
                            }
                            else if (line1.Piece.Color == PieceColor.White && line1.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(white);
                            }
                            else if (line1.Piece.Color == PieceColor.White && line1.Piece.Type == PieceType.King)
                            {
                                writer.Write(white_King);
                            }
                            else if (line1.Piece.Color == PieceColor.Red && line1.Piece.Type == PieceType.King)
                            {
                                writer.Write(red_King);
                            }
                        }
                        writer.WriteLine();
                    }

                    foreach(var square in CurrentNeighbours.Keys)
                    {
                        if (CurrentNeighbours[square] == null)
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + no_Piece);
                        }
                        else
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + CurrentNeighbours[square].Row.ToString() + CurrentNeighbours[square].Column.ToString());
                        }
                        writer.WriteLine();
                    }
                    writer.Write("-\n");
                }
            }
        }
        public static void LoadGame(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
           
            OpenFileDialog openDialog = new OpenFileDialog();
            
            bool? answer = openDialog.ShowDialog();
            if (answer == true) 
            {
                string path=openDialog.FileName;
                using (var reader = new StreamReader(path)) 
                {
                    string text;
                   

                    if(CurrentSquare!=null)
                    {
                        CurrentSquare.Texture = brownSquare;
                    }
                    text=reader.ReadLine();
                    
                    if (text!=no_Piece.ToString()) 
                    {
                        CurrentSquare = board[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])];
                        CurrentSquare.Texture = evidenceSquare;
                    }
                    else
                    {
                        CurrentSquare = null;
                    }
                   
                    text=reader.ReadLine();
                    if(text!=no_Piece.ToString()) 
                    {
                        ExtraMove=true;
                    }
                    else 
                    {
                        ExtraMove = false;
                    }
                    
                    text=reader.ReadLine();
                    if(text==red_Turn.ToString()) 
                    {
                        Turn.PlayerColor=PieceColor.Red;
                        Turn.TurnImage=redPiece;
                    }
                    else 
                    {
                        Turn.PlayerColor = PieceColor.White;
                        Turn.TurnImage = whitePiece;
                    }
                   
                    text = reader.ReadLine();
                    
                    if(text==allowMultipleMoves.ToString())
                    {
                        AllowMultipleMoves=true;
                    }
                    else
                    if(text==disallowMultipleMoves.ToString())
                    {
                        AllowMultipleMoves=false;
                    }
                    int redCount = 0;
                    int whiteCount = 0;
                    
                    for (int i = 0;i<boardSize;i++) 
                    {
                        text=reader.ReadLine();
                        for(int j = 0;j<boardSize;j++)
                        {
                            board[i][j].SquareSymbol = null;
                            if (text[j] == no_Piece)
                            {
                                board[i][j].Piece = null;
                            }
                            else if (text[j] == red)
                            {
                                board[i][j].Piece = new GamePiece(PieceColor.Red, PieceType.Regular);
                                board[i][j].Piece.Square = board[i][j];
                                redCount++;
                                
                            }
                            else if (text[j] == red_King)
                            {
                                board[i][j].Piece = new GamePiece(PieceColor.Red, PieceType.King);
                                board[i][j].Piece.Square = board[i][j];
                                redCount++;
                               
                            }
                            else if (text[j] == white)
                            {
                                board[i][j].Piece = new GamePiece(PieceColor.White, PieceType.Regular);
                                board[i][j].Piece.Square = board[i][j];
                                whiteCount++;
                            }
                            else if (text[j] == white_King)
                            {
                                board[i][j].Piece = new GamePiece(PieceColor.White, PieceType.King);
                                board[i][j].Piece.Square = board[i][j];
                                whiteCount++;
                            }
                        }
                    }
                    foreach(var square in CurrentNeighbours.Keys)
                    {
                        square.SquareSymbol = null;
                    }
                    CurrentNeighbours.Clear();
                    do
                    {
                        text = reader.ReadLine();
                        if (text == "-")
                        {
                            if (text.Length == 1)
                            {
                                break;
                            }
                            CurrentNeighbours.Add(board[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])], null);
                        }
                        else
                        {
                            if (text.Length >= 4)
                            {
                                int index1 = (int)char.GetNumericValue(text[0]);
                                int index2 = (int)char.GetNumericValue(text[1]);
                                int index3 = (int)char.GetNumericValue(text[2]);
                                int index4 = (int)char.GetNumericValue(text[3]);

                                
                                if (index1 >= 0 && index1 < board.Count &&
                                    index2 >= 0 && index2 < board[index1].Count &&
                                    index3 >= 0 && index3 < board.Count &&
                                    index4 >= 0 && index4 < board[index3].Count)
                                {
                                   
                                    CurrentNeighbours.Add(board[index1][index2], board[index3][index4]);
                                }
                                else
                                {
                                    
                                    Console.WriteLine("Indicii obținuți din text sunt în afara limitelor colecției board.");
                                }
                            }
                            else
                            {
                                
                                Console.WriteLine("Șirul text nu are suficiente caractere pentru a extrage indicii.");
                            }


                           
                        }
                    } while (text != "-");
                   
                    RemainRedPieces = whiteCount;
                    RemainWhitePieces = redCount;

                  
                    if (GameViewModel != null)
                    {
                        GameViewModel.RedPiecesScore = RemainRedPieces;
                        GameViewModel.WhitePiecesScore = RemainWhitePieces;
                    }

                    GameViewModel.AllowMultipleMoves = AllowMultipleMoves;

                   
                    bool extraMove = false; 
                    for (int i = 0; i < boardSize; i++)
                    {
                        for (int j = 0; j < boardSize; j++)
                        {
                            if (board[i][j].Piece != null && board[i][j].SquareSymbol == hintSquare)
                            {
                                extraMove = true;
                            }
                        }
                    }
                   
                    ExtraMove = extraMove;

                   
                    bool pieceMoved = false;
                    for(int i=0;i<boardSize;i++)
                    {
                        for(int j=0;j<boardSize;j++) 
                        {
                            
                            if ((i + j) % 2 != 0 && i < 3 && board[i][j].Piece?.Color != PieceColor.White)
                            {
                                pieceMoved = true;
                                break;
                            }
                            if ((i + j) % 2 != 0 && i > 4 && board[i][j].Piece?.Color != PieceColor.Red)
                            {
                                pieceMoved = true;
                                break;
                            }
                        }
                        if (pieceMoved)
                            break;
                    }
                    if(!pieceMoved) 
                    {
                        GameViewModel.IsCheckBoxEnabled = true;
                    }
                    else
                    {
                        GameViewModel.IsCheckBoxEnabled = false;
                    }

                   

                }

            }
        }
        public static bool GetSetAllowMultipleMoves
        {
            get
            {
                string filePath = "allowMultipleMoves.txt";
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
            set
            {
                string filePath = "allowMultipleMoves.txt";

                try
                {
                    File.WriteAllText(filePath, value.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare la actualizarea fișierului: " + ex.Message);
                }
            }
        }

        
    }
}