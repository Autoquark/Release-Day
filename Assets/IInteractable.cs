using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    interface IInteractable
    {
        string Message { get; }

        bool CanInteract();

        void Interact();
    }
}
