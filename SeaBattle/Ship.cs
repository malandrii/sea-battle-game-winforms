using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SeaBattle
{
    public class Ship
    {
        private int _size;
        public List<ShipButton> ShipParts { get; set; }
        public List<ShipButton> MarkedParts { get; set; } = new List<ShipButton>();

        public Ship(List<ShipButton> shipParts)
        {
            _size = shipParts.Count;
            ShipParts = shipParts;
        }

        public void TakeDamage()
        {
            if (_size != 0) _size--;
        }

        public bool IsDead()
        {
            return _size == 0;
        }

        public void Death()
        {
            foreach (ShipButton shipPart in ShipParts)
            {
                shipPart.BackColor = Color.Red;
            }
            foreach (ShipButton button in MarkedParts)
            {
                if (button.IsShipPart) continue;
                button.IsShot = true;
                button.Text = ShipButton.ShotText;
                button.Enabled = false;
            }
        }
    }
}