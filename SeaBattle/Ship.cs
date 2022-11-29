using System.Collections.Generic;
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
                shipPart.BackColor = Color.Red;
            for (int i = 0; i < MarkedParts.Count; i++)
            {
                foreach (ShipButton button in MarkedParts)
                {
                    if (!MarkedParts[i].IsShipPart)
                    {
                        button.IsShot = true;
                        MarkedParts[i].Text = button.ShotText;
                        MarkedParts[i].Enabled = false;
                    }
                }
            }
        }

        public void MakeShipUnabled()
        {
            List<ShipButton> allParts = ShipParts;
            allParts.AddRange(MarkedParts);
            foreach (ShipButton button in allParts) button.Enabled = false;
        }
    }
}