using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class FieldController
    {
        private const string EnglishLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int NextIndex = MainForm.NextIndex;
        public const int StartingCoordinate = 0;
        public const int FieldSize = 10;
        private readonly MainForm _mainForm;
        private List<Point> _coordinatesAround;
        private int _initialX;
        private int _initialY;

        public FieldController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public void CreateField(ShipButton[,] field, int markingOffset)
        {
            const int indent = 50;
            for (int y = 0; y < FieldSize; y++)
            {
                for (int x = 0; x < FieldSize; x++)
                {
                    _mainForm.ButtonController.CreateNewShipButton(field, markingOffset, indent, y, x);
                }
                AddMarking(y, markingOffset);
            }
        }

        public void ClearField(ShipButton[,] field)
        {
            foreach (ShipButton button in field) 
                button?.Dispose();
        }

        public bool AreCoordinatesInsideField(int x, int y)
        {
            return IsCoordinateInsideField(x) && IsCoordinateInsideField(y);
        }

        public static bool IsCoordinateInsideField(int coordinate)
        {
            return coordinate >= StartingCoordinate && coordinate < FieldSize;
        }

        public List<Point> GetCoordinatesAround(int x, int y)
        {
            int lastCoordinate = FieldSize - NextIndex;
            _coordinatesAround = new List<Point>();
            _initialX = x;
            _initialY = y;
            AddDimensionRelatedCoordinates(lastCoordinate, coordinateIsX: true);
            AddDimensionRelatedCoordinates(lastCoordinate, coordinateIsX: false);
            return _coordinatesAround;
        }

        private void AddDimensionRelatedCoordinates(int lastCoordinate, bool coordinateIsX)
        {
            int coordinate = coordinateIsX ? _initialX : _initialY;
            if (coordinate > StartingCoordinate)
                AddSpecificDimensionRelatedCoordinates(lastCoordinate, coordinateIsX, offsetToDownLeft: true);
            if (coordinate < lastCoordinate)
                AddSpecificDimensionRelatedCoordinates(lastCoordinate, coordinateIsX, offsetToDownLeft: false);
        }

        private void AddSpecificDimensionRelatedCoordinates(int lastCoordinate,
            bool coordinateIsX, bool offsetToDownLeft)
        {
            int x = _initialX, y = _initialY,
                offsetCoordinate = coordinateIsX ? x : y;
            offsetCoordinate += offsetToDownLeft ? -NextIndex : NextIndex;
            if (!coordinateIsX)
            {
                _coordinatesAround.Add(new Point(x, offsetCoordinate));
                return;
            }
            _coordinatesAround.Add(new Point(offsetCoordinate, y));
            if (y > StartingCoordinate)
                _coordinatesAround.Add(new Point(offsetCoordinate, y - NextIndex));
            if (y < lastCoordinate)
                _coordinatesAround.Add(new Point(offsetCoordinate, y + NextIndex));
        }

        private void AddMarking(int coordinate, int offset)
        {
            int buttonSize = MainFormButtonController.ButtonSize, doubleButtonSize = buttonSize * 2,
                coordinateOffset = buttonSize * coordinate;
            var letter = new Label
            {
                Location = new Point(doubleButtonSize + offset + coordinateOffset, buttonSize),
                Text = Convert.ToString(EnglishLetters[coordinate]),
                AutoSize = true
            };
            var digit = new Label
            {
                Location = new Point(buttonSize + offset, doubleButtonSize + coordinateOffset),
                Text = Convert.ToString(coordinate + MainForm.NextIndex),
                AutoSize = true
            };
            _mainForm.Controls.Add(digit);
            _mainForm.Controls.Add(letter);
        }

        public static void RevealShips(ShipButton[,] Field)
        {
            foreach (ShipButton button in Field)
            {
                if (button.IsShipPart) MainFormButtonController.ButtonColorShipHit(button);
            }
            MainFormButtonController.UnableField(Field);
        }
    }
}