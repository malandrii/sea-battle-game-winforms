using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattle
{
    public class FieldController
    {
        public const int StartingCoordinate = 0;
        public const int NextIndex = 1;
        public const int FieldSize = 10;
        private readonly MainForm _mainForm;

        public FieldController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public void CreateField(ShipButton[,] field, int markingOffset)
        {
            MainFormButtonController mainFormButtonController = new MainFormButtonController(_mainForm);
            const int indent = 50;
            for (int y = 0; y < FieldSize; y++)
            {
                for (int x = 0; x < FieldSize; x++)
                {
                    mainFormButtonController.CreateNewShipButton(field, markingOffset, indent, y, x);
                }
                AddMarking(y, markingOffset);
            }
        }

        private void AddMarking(int coordinate, int offset)
        {
            const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int buttonSize = MainFormButtonController.ButtonSize, doubleButtonSize = buttonSize * 2,
                coordinateOffset = buttonSize * coordinate;
            var letter = new Label
            {
                Location = new Point(doubleButtonSize + offset + coordinateOffset, buttonSize),
                Text = Convert.ToString(EnglishAlphabet[coordinate]),
                AutoSize = true
            };
            var digit = new Label
            {
                Location = new Point(buttonSize + offset, doubleButtonSize + coordinateOffset),
                Text = Convert.ToString(coordinate + NextIndex),
                AutoSize = true
            };
            _mainForm.Controls.Add(digit);
            _mainForm.Controls.Add(letter);
        }

        public static void DisposeField(ShipButton[,] field)
        {
            foreach (ShipButton button in field)
                button?.Dispose();
        }

        public static bool CoordinatesInsideField(int x, int y)
        {
            return CoordinateInsideField(x) && CoordinateInsideField(y);
        }

        public static bool CoordinateInsideField(int coordinate)
        {
            return coordinate >= StartingCoordinate && coordinate < FieldSize;
        }

        public static HashSet<Point> GetCoordinatesAround(int x, int y)
        {
            Point point = new Point(x, y);
            int[] cartesianSet = { -1, 0, 1 };
            return cartesianSet
                .SelectMany(dx => cartesianSet.Select(dy => new Point(point.X + dx, point.Y + dy)))
                .Where(selectedPoint => !point.Equals(selectedPoint))
                .Where(selectedPoint => CoordinatesInsideField(selectedPoint.X, selectedPoint.Y))
                .ToHashSet();
        }

        public static void ColorShips(ShipButton[,] Field, bool humanField)
        {
            foreach (ShipButton shipPart in Field)
            {
                if (!shipPart.IsShipPart) continue;
                if (humanField) HumanPlayerShipController.ColorShipPart(shipPart);
                else MainFormButtonController.ButtonColorShipHit(shipPart);
            }
        }

        public static void UnableRegion(HashSet<ShipButton> region)
        {
            foreach (var shipButton in region)
                shipButton.Enabled = false;
        }

        public static void UnableField(ShipButton[,] Field)
        {
            foreach (ShipButton button in Field)
                button.Enabled = false;
        }
    }
}