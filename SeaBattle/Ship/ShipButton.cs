using System;
using System.Windows.Forms;

namespace SeaBattle
{
    public class ShipButton : Button
    {
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
            private set => SetCoordinate(ref _x, value);
        }

        public int Y
        {
            get => _y;
            private set => SetCoordinate(ref _y, value);
        }

        public bool IsShipPart { get; set; } = false;

        public bool IsShot { get; set; } = false;

        public bool FreeForShipCreation => !IsShipPart && !_marked;

        private static void SetCoordinate(ref int coordinate, int value)
        {
            if (Field.CoordinateInside(value))
                coordinate = value;
            else
                throw new IndexOutOfRangeException();
        }

        public void Shoot()
        {
            const string ShotShipPartText = "X", ShotText = ".";
            Text = IsShipPart ? ShotShipPartText : ShotText;
            IsShot = true;
            Enabled = false;
        }

        public void Mark(Ship shipFrom)
        {
            _marked = true;
            shipFrom.MarkedParts.Add(this);
        }
    }
}