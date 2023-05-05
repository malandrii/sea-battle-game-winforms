using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class FieldController
    {
        private const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
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

        public static List<Point> GetCoordinatesAround(int x, int y)
        {
            int lastCoordinate = FieldSize - NextIndex, initialX = x, initialY = y;
            var coordinatesAround = new List<Point>();
            AddDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, initialX, initialY, coordinateIsX: true);
            AddDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, initialX, initialY, coordinateIsX: false);
            return coordinatesAround;
        }

        private static void AddDimensionRelatedCoordinates(List<Point> coordinatesAround, int lastCoordinate, int initialX, 
            int initialY, bool coordinateIsX)
        {
            int coordinate = coordinateIsX ? initialX : initialY;
            if (coordinate > StartingCoordinate)
                AddSpecificDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, initialX, initialY, 
                    coordinateIsX, offsetToDownLeft: true);
            if (coordinate < lastCoordinate)
                AddSpecificDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, initialX, initialY,
                    coordinateIsX, offsetToDownLeft: false);
        }

        private static void AddSpecificDimensionRelatedCoordinates(List<Point> coordinatesAround, int lastCoordinate,
            int initialX, int initialY, bool coordinateIsX, bool offsetToDownLeft)
        {
            int x = initialX, y = initialY,
                offsetCoordinate = coordinateIsX ? x : y;
            offsetCoordinate += offsetToDownLeft ? -NextIndex : NextIndex;
            if (!coordinateIsX)
            {
                coordinatesAround.Add(new Point(x, offsetCoordinate));
                return;
            }
            coordinatesAround.Add(new Point(offsetCoordinate, y));
            if (y > StartingCoordinate)
                coordinatesAround.Add(new Point(offsetCoordinate, y - NextIndex));
            if (y < lastCoordinate)
                coordinatesAround.Add(new Point(offsetCoordinate, y + NextIndex));
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

        public static void UnableRegion(List<ShipButton> region)
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