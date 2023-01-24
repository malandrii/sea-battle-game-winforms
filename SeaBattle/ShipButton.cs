using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle
{
    public class ShipButton : Button
    {
        private const string ShotShipPartText = "X";
        public const string ShotText = ".";
        private bool _marked = false;

        public ShipButton(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Ship ShipFrom { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsShipPart { get; set; } = false;
        public bool IsShot { get; set; } = false;
        public bool CanMakeShip { get => !IsShipPart && !_marked; }

        public void Shoot()
        {
            IsShot = true;
            Text = IsShipPart ? ShotShipPartText : ShotText;
        }

        public void Mark(Ship shipFrom)
        {
            _marked = true;
            shipFrom.MarkedParts.Add(this);
        }

        public void RefreshMarking(bool mark)
        {
            if (!IsShipPart)
            {
                MainForm.ButtonColorToStandart(this);
                return;
            }
            if (IsShot)
            {
                BackColor = ShipFrom.IsDead ? Color.Red : (mark ? Color.DarkRed : Color.Blue);
            }
        }
    }
}