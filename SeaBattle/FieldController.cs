using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class FieldController
    {
        public const int FieldSize = 10;
        private const int NextIndex = MainForm.NextIndex;
        private const string EnglishLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly MainForm _mainForm;
        private List<Point> _coordinatesAround;
        private int _initialX;
        private int _initialY;
        public int StartingCoordinate { get; } = 0;
        public int ButtonSize { get; } = 30;

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
                    int xIndent = indent + markingOffset;
                    field[x, y] = new ShipButton(x, y)
                    {
                        Location = new Point(x * ButtonSize + xIndent, y * ButtonSize + indent),
                        Size = new Size(ButtonSize, ButtonSize)
                    };
                    _mainForm.Controls.Add(field[x, y]);
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

        public bool IsCoordinateInsideField(int coordinate)
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
            int buttonSize = ButtonSize, doubleButtonSize = buttonSize * 2,
                coordinateOffset = buttonSize * coordinate;
            Label letter = new Label
            {
                Location = new Point(doubleButtonSize + offset + coordinateOffset, buttonSize),
                Text = Convert.ToString(EnglishLetters[coordinate]),
                AutoSize = true
            };
            Label digit = new Label
            {
                Location = new Point(buttonSize + offset, doubleButtonSize + coordinateOffset),
                Text = Convert.ToString(coordinate + MainForm.NextIndex),
                AutoSize = true
            };
            _mainForm.Controls.Add(digit);
            _mainForm.Controls.Add(letter);
        }
    }
}