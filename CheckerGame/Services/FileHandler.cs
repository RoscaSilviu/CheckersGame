using CheckerGame.Models;
using CheckerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace CheckerGame.Services
{
     static class FileHandler
    {
        public static void SaveGame(ObservableCollection<ObservableCollection<Cell>> board, PieceColor CurrentTurn)
        {
            var gameData = new
            {
                Board = board,
                CurrentTurn = CurrentTurn
            };

            string json = JsonConvert.SerializeObject(gameData);

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                File.WriteAllText(filePath, json);
            }
        }


        public static (ObservableCollection<ObservableCollection<Cell>> board, PieceColor currentTurn) LoadGame()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    string json = File.ReadAllText(filePath);

                    var gameData = JsonConvert.DeserializeObject<dynamic>(json);

                    var board = gameData["Board"].ToObject<ObservableCollection<ObservableCollection<Cell>>>();
                    var currentTurn = gameData["CurrentTurn"].ToObject<PieceColor>();

                    return (board, currentTurn);
                }
                catch (JsonException ex)
                {
                    throw new JsonException("Eroare în parsarea fișierului JSON.", ex);
                }
            }
            return (null, PieceColor.Black);
        }

        public static void SaveStats(string winner, int piecesLeft)
        {
            // Calea către fișierul text
            string filePath = "stats.txt";

            // Verificăm dacă fișierul există și îl citim
            if (File.Exists(filePath))
            {
                // Citim valorile actuale din fișier
                string[] lines = File.ReadAllLines(filePath);

                // Parsăm și stocăm valorile actuale
                int whiteScore = int.Parse(lines[0].Split(':')[1].Trim());
                int blackScore = int.Parse(lines[1].Split(':')[1].Trim());
                int highScore = int.Parse(lines[2].Split(':')[1].Trim());

                // Actualizăm scorul în funcție de câștigător
                if (winner == "White")
                {
                    whiteScore++;
                }
                else if (winner == "Black")
                {
                    blackScore++;
                }

                // Actualizăm scorul maxim
                if (piecesLeft > highScore)
                {
                    highScore = piecesLeft;
                }

                // Scriem valorile actualizate înapoi în fișier
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"White : {whiteScore}");
                    writer.WriteLine($"Black : {blackScore}");
                    writer.WriteLine($"HighScore: {highScore}");
                }
            }
            else
            {
                // Dacă fișierul nu există, creăm unul nou și scriem valorile în el
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("White : 0");
                    writer.WriteLine("Black : 0");
                    writer.WriteLine("HighScore: 0");
                }
            }
        }
        public static (string, string, string) LoadStats()
        {
            // Calea către fișierul text
            string filePath = "stats.txt";

            int whiteScore = 0;
            int blackScore = 0;
            int highScore = 0;

            // Verificăm dacă fișierul există și îl citim
            if (File.Exists(filePath))
            {
                // Citim valorile din fișier
                string[] lines = File.ReadAllLines(filePath);

                // Parsăm și stocăm valorile
                whiteScore = int.Parse(lines[0].Split(':')[1].Trim());
                blackScore = int.Parse(lines[1].Split(':')[1].Trim());
                highScore = int.Parse(lines[2].Split(':')[1].Trim());
            }

            // Returnăm valorile citite din fișier sau valorile implicite (0) dacă fișierul nu există
            return (whiteScore.ToString(), blackScore.ToString(), highScore.ToString());
        }

    }
}
