using System;
using CheckerGame.Models;
using CheckerGame.Services;
using CheckerGame.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckerGame.ViewModels
{
     class ButtonsVM
    {
        private ButtonService buttonService = new ButtonService();
        private GameVM gameVM= new GameVM();
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

        //private ICommand loadGame;
        //public ICommand LoadGame
        //{
        //    get
        //    {
        //        if (loadGame == null)
        //        {
        //            loadGame = new RelayCommand<object>(gameVM.LoadGame);
        //        }
        //        return loadGame;
        //    }
        //}

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
