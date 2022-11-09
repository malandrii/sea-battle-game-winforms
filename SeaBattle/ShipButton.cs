using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle
{
    public class ShipButton : Button
    {
        private bool _marked = false;
        private readonly string _shotShipPartText = "X";
        public string ShotText { get; } = ".";
        public bool IsShipPart { get; set; } = false;
        public bool IsShot { get; set; } = false;
        public Ship ShipFrom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public ShipButton(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Shoot()
        {
            IsShot = true;
            Text = IsShipPart ? _shotShipPartText : ShotText;
        }

        public bool CanMakeShip()
        {
            return (!IsShipPart && !_marked);
        }

        public void Mark(int y, int x, ShipButton[,] field)
        {
            if (!IsShipPart)
            {
                _marked = true;
                field[y, x].ShipFrom.MarkedParts.Add(this);
            }
        }

        public void RefreshMarking(bool mark)
        {
            if (ShipFrom != null)
            {
                if (IsShot)
                {
                    if (ShipFrom.IsDead()) BackColor = Color.Red;
                    else if (mark) BackColor = Color.DarkRed;
                    else BackColor = Color.Blue;
                }
            }
            else MainForm.ButtonColorToStandart(this);
        }
    }
}