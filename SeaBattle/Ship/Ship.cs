using System.Collections.Generic;

namespace SeaBattle
{
    public class Ship
    {
        private int _size;

        public Ship(HashSet<ShipButton> shipParts)
        {
            _size = shipParts.Count;
            ShipParts = shipParts;
        }

        public HashSet<ShipButton> ShipParts { get; private set; }

        public HashSet<ShipButton> MarkedParts { get; set; } = new HashSet<ShipButton>();

        public bool IsDead { get => _size == 0; }

        public void TakeDamage()
        {
            if (_size != 0) _size--;
        }

        public void Death()
        {
            foreach (ShipButton shipPart in ShipParts)
            {
                MainFormButtonController.ButtonColorShipHit(shipPart);
            }
            foreach (ShipButton button in MarkedParts)
            {
                if (button.IsShipPart) continue;
                button.Shoot();
            }
        }
    }
}