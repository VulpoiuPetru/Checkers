using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tema2_Dubla2.Models;
using Tema2_Dubla2.ViewModels;
using Views.Tema2_Dubla2.Models;
using static Views.Tema2_Dubla2.Models.GamePiece;


namespace Tema2_Dubla2.Services
{
    public class GameLogic
    {
        private ObservableCollection<ObservableCollection<GameSquare>> board;
        public PlayerTurn playerTurn;
        private Winner winner;
        private GameVM gameViewModel;
        public GameLogic(ObservableCollection<ObservableCollection<GameSquare>> board, PlayerTurn playerTurn, Winner winner,GameVM gameViewModel)
        {
            this.board = board;
            this.playerTurn = playerTurn;
            this.winner = winner;
            this.gameViewModel = gameViewModel;
        }
        private void SwitchTurns(GameSquare square)
        {
            if (square != null && square.Piece != null)
            {
                if (square.Piece.Color == GamePiece.PieceColor.Red)
                {
                    Elements.Turn.PlayerColor = GamePiece.PieceColor.White;
                    Elements.Turn.TurnImage = Elements.whitePiece;
                    playerTurn.PlayerColor = GamePiece.PieceColor.White;
                    playerTurn.TurnImage = Elements.whitePiece;
                }
                else
                {
                    Elements.Turn.PlayerColor = GamePiece.PieceColor.Red;
                    Elements.Turn.TurnImage = Elements.redPiece;
                    playerTurn.PlayerColor = GamePiece.PieceColor.Red;
                    playerTurn.TurnImage = Elements.redPiece;
                }
            }
        }

       
        public void MovePiece(GameSquare gameSquare)
        {
            gameSquare.Piece = Elements.CurrentSquare.Piece;
            gameSquare.Piece.Square = gameSquare;

            if (Elements.CurrentNeighbours[gameSquare] != null)
            {
                Elements.CurrentNeighbours[gameSquare].Piece = null;
                Elements.ExtraMove = true;

                SwitchTurns(Elements.CurrentSquare);
            }
            else
            {
                Elements.ExtraMove = false;
                SwitchTurns(Elements.CurrentSquare);
            }

           

            SwitchTurns(Elements.CurrentSquare);

            
            board[Elements.CurrentSquare.Row][Elements.CurrentSquare.Column].Texture = Elements.brownSquare;

            
            foreach (GameSquare selectedSquare in Elements.CurrentNeighbours.Keys)
            {
                selectedSquare.SquareSymbol = null;
            }
            Elements.CurrentNeighbours.Clear();
            Elements.CurrentSquare.Piece = null;
            Elements.CurrentSquare = null;
           
            if (gameSquare.Piece.Type == PieceType.Regular)
            {
                if (gameSquare.Row == 0 && gameSquare.Piece.Color == PieceColor.Red)
                {
                    gameSquare.Piece.Type = PieceType.King;
                    gameSquare.Piece.Texture = Elements.kingRedPiece;
                }
                else if (gameSquare.Row == board.Count - 1 && gameSquare.Piece.Color == PieceColor.White)
                {
                    gameSquare.Piece.Type = PieceType.King;
                    gameSquare.Piece.Texture = Elements.kingWhitePiece;
                }
            }
            if (Elements.ExtraMove)
            {
                if (playerTurn.TurnImage == Elements.redPiece)
                {
                    Elements.RemainWhitePieces--;
                    gameViewModel.WhitePiecesScore = Elements.RemainWhitePieces; 
                }
                else
            if (playerTurn.TurnImage == Elements.whitePiece)
                {
                    Elements.RemainRedPieces--;
                    gameViewModel.RedPiecesScore = Elements.RemainRedPieces; 
                }
               
                DispayMoves(gameSquare);
            }
            
            gameViewModel.IsCheckBoxEnabled = false;
            if (Elements.RemainRedPieces == 0 || Elements.RemainWhitePieces == 0)
            {
                GameOver();
            }


        }



       
        private void FindNeighbours(GameSquare square)
        {
            
            Elements.CurrentNeighbours.Clear();

            
            var neighboursCheck = new HashSet<Tuple<int, int>>();
            
            Elements.movePiece(square, neighboursCheck);

            foreach (Tuple<int, int> neighbour in neighboursCheck)
            {
                
                if (Elements.isInBound(square.Row + neighbour.Item1, square.Column + neighbour.Item2))
                {
                    
                    if (board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece == null)
                    {
                        
                        if (!Elements.CurrentNeighbours.ContainsKey(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]))
                        {
                            Elements.CurrentNeighbours.Add(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2], null);
                        }
                    }
                    else
                   
                        if (Elements.isInBound(square.Row + neighbour.Item1 * 2, square.Column + neighbour.Item2 * 2) &&
                        board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece.Color != square.Piece.Color &&
                        board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2].Piece == null)
                    {
                        var newKey = board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2];
                        var newValue = board[square.Row + neighbour.Item1][square.Column + neighbour.Item2];

                        if (!Elements.CurrentNeighbours.ContainsKey(newKey))
                        {
                            Elements.CurrentNeighbours.Add(newKey, newValue);
                        }
                        if (Elements.AllowMultipleMoves)
                        {
                            Elements.ExtraPath = true;
                        }
                        else
                        {
                            Elements.ExtraPath = false;
                        }

                    }
                }
            }
        }

      
        private void DispayMoves(GameSquare square)
        {
            if (Elements.CurrentSquare != square)
            {
                
                if (Elements.CurrentSquare != null)
                {
                    board[Elements.CurrentSquare.Row][Elements.CurrentSquare.Column].Texture = Elements.brownSquare;

                    foreach (GameSquare selectedSquare in Elements.CurrentNeighbours.Keys)
                    {
                        selectedSquare.SquareSymbol = null;
                    }
                    Elements.CurrentNeighbours.Clear();
                }
               
                FindNeighbours(square);
                
                if (Elements.ExtraMove && !Elements.ExtraPath)
                {
                    Elements.ExtraMove = false;
                    SwitchTurns(square);
                }
                else
               

                {

                    foreach (GameSquare neighbour in Elements.CurrentNeighbours.Keys)
                    {
                        board[neighbour.Row][neighbour.Column].SquareSymbol = Elements.hintSquare;
                    }

                    Elements.CurrentSquare = square;
                    Elements.ExtraPath = false;
                }
            }
           
            else
            {
                board[square.Row][square.Column].Texture = Elements.brownSquare;

                foreach (GameSquare selectedSquare in Elements.CurrentNeighbours.Keys)
                {
                    selectedSquare.SquareSymbol = null;
                }
                Elements.CurrentNeighbours.Clear();
                Elements.CurrentSquare = null;
            }

        }
        public void GameOver()
        {
            
            if (Elements.RemainRedPieces == 0)
            {
                string text = "Red wins";
                MessageBox.Show(text);
            }
            else
                if (Elements.RemainWhitePieces == 0)
            {
                string text = "White wins";
                MessageBox.Show(text);
            }
            UpdateStatisticsFile();
            
            Elements.RemainRedPieces = 12;
            Elements.RemainWhitePieces = 12;
            gameViewModel.IsCheckBoxEnabled = true;
            Elements.ResetGame(board);
        }
        public void UpdateStatisticsFile()
        {
            string filePath = "../../Resources/statistics.txt";

            try
            {
               
                string[] lines = File.ReadAllLines(filePath);
                int redWinners = int.Parse(lines[0].Split(':')[1].Trim());
                int whiteWinners = int.Parse(lines[1].Split(':')[1].Trim());
                int maxRedPieces = int.Parse(lines[2].Split(':')[1].Trim());
                int maxWhitePieces = int.Parse(lines[3].Split(':')[1].Trim());

               
                if (Elements.RemainRedPieces == 0)
                {
                    redWinners++;
                    maxRedPieces = Math.Max(maxRedPieces,Elements.RemainWhitePieces); 
                }
                else if (Elements.RemainWhitePieces == 0)
                {
                    whiteWinners++;
                    maxWhitePieces = Math.Max(maxWhitePieces,Elements.RemainRedPieces); 
                }

               
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Red Winners: {redWinners}");
                    writer.WriteLine($"White Winners: {whiteWinners}");
                    writer.WriteLine($"Max Red Pieces Remaining: {maxRedPieces}");
                    writer.WriteLine($"Max White Pieces Remaining: {maxWhitePieces}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating statistics file: " + ex.Message);
            }
        }
        public void ClickPiece(GameSquare gameSquare)
        {
            if ((Elements.Turn.PlayerColor == GamePiece.PieceColor.Red && gameSquare.Piece.Color == GamePiece.PieceColor.Red ||
                 Elements.Turn.PlayerColor == GamePiece.PieceColor.White && gameSquare.Piece.Color == GamePiece.PieceColor.White) &&
                 !Elements.ExtraMove)
            {
                DispayMoves(gameSquare);
            }

        }
        public void ResetGame()
        {
            Elements.ResetGame(board);
           gameViewModel.IsCheckBoxEnabled = true;

        }

        public void SaveGame()
        {
            Elements.SaveGame(board);
        }
        public void LoadGame()
        {
            Elements.LoadGame(board);
            playerTurn.TurnImage=Elements.Turn.TurnImage;
            
        }

    }
}