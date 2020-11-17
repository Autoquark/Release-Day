using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Behaviours.LevelSpecific
{
    class BuggyInGameMenuBehaviour : InGameMenuBehavior
    {
        public BuggyInGameMenuBehaviour()
        {
            PauseWhenOpen = false;
        }

        public void SetMenuVisible(bool value)
        {
            if (Visible != value)
            {
                ToggleMenu();
            }
        }
    }
}
