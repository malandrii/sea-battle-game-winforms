using System.Windows.Forms;
using System.Drawing;

namespace SeaBattle
{
    sealed public class ShipButton : Button
    {
        private const string ShotShipPartText = "X";
        public const string ShotText = ".";
        private bool _marked = false;
        private int _x;
        private int _y;

        public ShipButton(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Ship ShipFrom { get; set; }

        public int X
        {
            get => _x;
            private set
            {
                if (FieldController.IsCoordinateInsideField(value))
                    _x = value;
            }
        }

        public int Y
        {
            get => _y;
            private set
            {
                if (FieldController.IsCoordinateInsideField(value))
                    _y = value;
            }
        }

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