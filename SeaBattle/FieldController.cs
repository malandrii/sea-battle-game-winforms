using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class FieldController
    {
        private readonly MainForm _mainForm;
        public int StartingCoordinate { get; } = 0;
        public int FieldSize { get; } = 10;
        public int ButtonSize { get; } = 30;

        public FieldController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public void CreateField(ShipButton[,] field, int markingOffset)
        {
            int indent = 50;
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
                if (button != null) button.Dispose();
        }

        public bool AreCoordinatesInsideField(int x, int y)
        {
            return x >= StartingCoordinate && x < FieldSize
                && y >= StartingCoordinate && y < FieldSize;
        }

        public List<Point> GetCoordinatesAround(int x, int y)
        {
            int nextIndex = MainForm.NextIndex, lastCoordinate = FieldSize - nextIndex;
            List<Point> coordinatesToAdd = new List<Point>();
            if (x > StartingCoordinate && y > StartingCoordinate)
                coordinatesToAdd.Add(new Point(x - nextIndex, y - nextIndex));
            if (x > StartingCoordinate)
                coordinatesToAdd.Add(new Point(x - nextIndex, y));
            if (x > StartingCoordinate && y < lastCoordinate)
                coordinatesToAdd.Add(new Point(x - nextIndex, y + nextIndex));
            if (y < lastCoordinate)
                coordinatesToAdd.Add(new Point(x, y + nextIndex));
            if (x < lastCoordinate && y < lastCoordinate)
                coordinatesToAdd.Add(new Point(x + nextIndex, y + nextIndex));
            if (x < lastCoordinate)
                coordinatesToAdd.Add(new Point(x + nextIndex, y));
            if (y > StartingCoordinate && x < lastCoordinate)
                coordinatesToAdd.Add(new Point(x + nextIndex, y - nextIndex));
            if (y > StartingCoordinate)
                coordinatesToAdd.Add(new Point(x, y - nextIndex));
            return coordinatesToAdd;
        }

        private void AddMarking(int coordinate, int offset)
        {
            char[] englishLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            int buttonSize = ButtonSize, doubleButtonSize = buttonSize * 2,
                coordinateOffset = buttonSize * coordinate;
            Label letter = new Label
            {
                Location = new Point(doubleButtonSize + offset + coordinateOffset, buttonSize),
                Text = Convert.ToString(englishLetters[coordinate]),
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