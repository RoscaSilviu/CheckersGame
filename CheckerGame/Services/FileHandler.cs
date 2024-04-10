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

namespace CheckerGame.Services
{
     static class FileHandler
    {
        public static void SaveGame(ObservableCollection<ObservableCollection<Cell>> board, PieceColor CurrentTurn)
        {
            // Crează un obiect care să conțină datele pe care vrei să le salvezi
            var gameData = new
            {
                Board = board,
                CurrentTurn = CurrentTurn
            };

            // Converteste obiectul în format JSON
            string json = JsonConvert.SerializeObject(gameData);

            // Specifică calea și numele fișierului în care vrei să salvezi datele
            string filePath = "gameData.json";

            // Scrie datele JSON în fișier
            File.WriteAllText(filePath, json);
        }

        public static (ObservableCollection<ObservableCollection<Cell>> board, PieceColor currentTurn) LoadGame(string filePath)
        {
            // Verificăm dacă fișierul există
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Fișierul specificat nu a fost găsit.", filePath);
            }

            try
            {
                // Citim datele JSON din fișier
                string json = File.ReadAllText(filePath);

                // Deserializăm datele JSON în obiectul corespunzător
                var gameData = JsonConvert.DeserializeObject<dynamic>(json);

                // Extragem valorile necesare din obiectul deserializat
                var board = gameData["Board"].ToObject<ObservableCollection<ObservableCollection<Cell>>>();
                var currentTurn = gameData["CurrentTurn"].ToObject<PieceColor>();

                // Returnăm obiectele încărcate din fișier
                return (board, currentTurn);
            }
            catch (JsonException ex)
            {
                // Handle any JSON parsing errors
                throw new JsonException("Eroare în parsarea fișierului JSON.", ex);
            }
        }
    }
}
