using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class FieldController
    {
        private const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int NextIndex = MainForm.NextIndex;
        public const int StartingCoordinate = 0;
        public const int ShipSizesAmount = 4;
        public const int FieldSize = 10;
        private readonly MainForm _mainForm;
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

        public static void ClearField(ShipButton[,] field)
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
            var coordinatesAround = new List<Point>();
            _initialX = x;
            _initialY = y;
            AddDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, coordinateIsX: true);
            AddDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, coordinateIsX: false);
            return coordinatesAround;
        }

        private void AddDimensionRelatedCoordinates(List<Point> coordinatesAround, int lastCoordinate, bool coordinateIsX)
        {
            int coordinate = coordinateIsX ? _initialX : _initialY;
            if (coordinate > StartingCoordinate)
                AddSpecificDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, coordinateIsX, offsetToDownLeft: true);
            if (coordinate < lastCoordinate)
                AddSpecificDimensionRelatedCoordinates(coordinatesAround, lastCoordinate, coordinateIsX, offsetToDownLeft: false);
        }

        private void AddSpecificDimensionRelatedCoordinates(List<Point> coordinatesAround, int lastCoordinate,
            bool coordinateIsX, bool offsetToDownLeft)
        {
            int x = _initialX, y = _initialY,
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
                Text = Convert.ToString(coordinate + MainForm.NextIndex),
                AutoSize = true
            };
            _mainForm.Controls.Add(digit);
            _mainForm.Controls.Add(letter);
        }

        public static void ColorHumanPlayerShips(ShipButton[,] humanField)
        {
            ColorShips(humanField, humanField: true);
        }

        public static void RevealEnemyShips(ShipButton[,] enemyField)
        {
            ColorShips(enemyField, humanField: false);
            MainFormButtonController.UnableField(enemyField);
        }

        private static void ColorShips(ShipButton[,] Field, bool humanField)
        {
            foreach (ShipButton shipPart in Field)
            {
                if (!shipPart.IsShipPart) continue;
                if (humanField) MainFormButtonController.ColorHumanPlayerShipPart(shipPart);
                else MainFormButtonController.ButtonColorShipHit(shipPart);
            }
        }

        public void SetShipVisibility(ShipButton button, int chosenSize, bool toAppear, bool chosenShipIsHorizontal)
        {
            int previousSize = chosenSize - NextIndex;
            bool spaceIsFree = chosenShipIsHorizontal ?
                (button.X + previousSize) < FieldSize :
                (button.Y - previousSize) >= StartingCoordinate;
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: false);
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: true);
        }

        private void AppearShipPreview(ShipButton button, int chosenSize, ref bool spaceIsFree, bool toAppear, 
            bool chosenShipIsHorizontal, bool spaceIsFreeSet)
        {
            int x = button.X, y = button.Y;
            if (!spaceIsFree) return;
            for (int i = 0; i < chosenSize; i++)
            {
                if (!spaceIsFreeSet)
                {
                    spaceIsFree = _mainForm.GameController.HumanPlayerField[x, y].Enabled;
                }
                else
                {
                    MainFormButtonController.AppearShipPartPreview(_mainForm.GameController.HumanPlayerField[x, y], toAppear);
                }
                _mainForm.GameController.HumanPlayer.ShiftCoordinates(chosenShipIsHorizontal, chosenShipIsHorizontal, ref x, ref y);
            }
        }
    }
}