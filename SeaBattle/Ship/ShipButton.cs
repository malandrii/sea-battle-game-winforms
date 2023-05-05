using System.Windows.Forms;

namespace SeaBattle
{
    sealed public class ShipButton : Button
    {
        private const string ShotShipPartText = "X";
        private const string ShotText = ".";
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
                if (FieldController.CoordinateInsideField(value))
                    _x = value;
            }
        }

        public int Y
        {
            get => _y;
            private set
            {
                if (FieldController.CoordinateInsideField(value))
                    _y = value;
            }
        }

        public bool IsShipPart { get; set; } = false;

        public bool IsShot { get; set; } = false;

        public bool FreeForShipCreation { get => !IsShipPart && !_marked; }

        public void Shoot()
        {
            IsShot = true;
            Text = IsShipPart ? ShotShipPartText : ShotText;
            Enabled = false;
        }

        public void Mark(Ship shipFrom)
        {
            _marked = true;
            shipFrom.MarkedParts.Add(this);
        }
    }
}