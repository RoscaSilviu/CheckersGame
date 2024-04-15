using CheckerGame.Models;
using CheckerGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerGame.ViewModels
{
    class StatsVM : BaseNotification
    {

        public StatsVM() {
          (string whiteWinsBoxText, string blackWinsBoxText, string highScoreBoxText) = FileHandler.LoadStats();
          WhiteWinsBoxText = whiteWinsBoxText;
          BlackWinsBoxText = blackWinsBoxText;
          HighScoreBoxText = highScoreBoxText;
        }
        private string whiteWinsBoxText;
        public string WhiteWinsBoxText
        {
            get { return whiteWinsBoxText; }
            set
            {
                if (whiteWinsBoxText != value)
                {
                    whiteWinsBoxText = value;
                    NotifyPropertyChanged("WhiteWinsBoxText"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }

        private string blackWinsBoxText;
        public string BlackWinsBoxText
        {
            get { return blackWinsBoxText; }
            set
            {
                if (blackWinsBoxText != value)
                {
                    blackWinsBoxText = value;
                    NotifyPropertyChanged("BlackWinsBoxText"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }

        private string highScoreBoxText;
        public string HighScoreBoxText
        {
            get { return highScoreBoxText; }
            set
            {
                if (highScoreBoxText != value)
                {
                    highScoreBoxText = value;
                    NotifyPropertyChanged("HighScoreBoxText"); // Notifică View-ul că s-a schimbat proprietatea
                }
            }
        }

    }
}
