using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    interface IInteractable
    {
        bool CanInteractWith(PlayerControllerBehaviour player);

        void InteractWith(PlayerControllerBehaviour player);
    }
}
