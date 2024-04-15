using CheckerGame.ViewModels;
using CheckerGame.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerGame.Services
{
     class ButtonService
    {
        GameVM gameVM;
        public void StartGame(Object obj)
        {
            GameWindow window = new GameWindow();
            window.Show();

        }
        public void LoadGame(Object obj)
        {
               
        }

        public void Statistics(Object obj)
        {
           StatsWindow window = new StatsWindow();
            window.Show();
        }

        public void Help(Object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
            
        }
    }
}
