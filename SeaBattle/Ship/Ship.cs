using System.Collections.Generic;

namespace SeaBattle
{
    public class Ship
    {
        private readonly ShipButton[] _shipParts;
        private int _size;

        public Ship(ShipButton[] shipParts)
        {
            _size = shipParts.Length;
            _shipParts = shipParts;
        }

        public HashSet<ShipButton> MarkedParts { get; set; } = new HashSet<ShipButton>();

        public bool IsDead => _size == 0;

        public IEnumerator<ShipButton> GetEnumerator()
        {
            return ((IEnumerable<ShipButton>)_shipParts).GetEnumerator();
        }

        public static explicit operator ShipButton[](Ship ship)
        {
            return ship._shipParts;
        }

        public void TakeDamage()
        {
            if (_size != 0) _size--;
        }

        public void Death()
        {
            foreach (ShipButton shipPart in _shipParts)
            {
                FormButtonController.SetButtonColorToShipHit(shipPart);
            }
            foreach (ShipButton button in MarkedParts)
            {
                if (button.IsShipPart) continue;
                button.Shoot();
            }
        }
    }
}