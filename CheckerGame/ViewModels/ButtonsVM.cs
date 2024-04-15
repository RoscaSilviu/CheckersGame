using System;
using CheckerGame.Models;
using CheckerGame.Services;
using CheckerGame.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckerGame.Properties;

namespace CheckerGame.ViewModels
{
     class ButtonsVM
    {
        private ButtonService buttonService = new ButtonService();
        private ICommand startGame;
        public ICommand StartGame
        {
            get
            {
                if (startGame == null)
                {
                    startGame = new RelayCommand<object>(buttonService.StartGame);
                }
                return startGame;
            }
        }
        public bool IsCheckboxChecked
        {
            get { return Settings.Default.IsCheckboxChecked; }
            set
            {
                if (Settings.Default.IsCheckboxChecked != value)
                {
                    Settings.Default.IsCheckboxChecked = value;
                    Settings.Default.Save();
                }
            }
        }

        private ICommand statistics;
        public ICommand Statistics
        {
            get
            {
                if (statistics == null)
                {
                    statistics = new RelayCommand<object>(buttonService.Statistics);
                }
                return statistics;
            }
        }

        private ICommand help;
        public ICommand Help
        {
            get
            {
                if (help == null)
                {
                    help = new RelayCommand<object>(buttonService.Help);
                }
                return help;
            }
        }

    }
}
