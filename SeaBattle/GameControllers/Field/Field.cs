using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public class Field
    {
        public const int StartingCoordinate = 0;
        public const int Size = 10;
        private readonly ShipButton[,] _shipButtons;

        public Field() => _shipButtons = new ShipButton[Size, Size];

        public ShipButton this[int x, int y]
        {
            get => _shipButtons[x, y];
            set => _shipButtons[x, y] = value;
        }

        public IEnumerator<ShipButton> GetEnumerator()
        {
            for (int row = 0; row < _shipButtons.GetLength(0); row++)
            {
                for (int column = 0; column < _shipButtons.GetLength(1); column++)
                {
                    yield return _shipButtons[row, column];
                }
            }
        }

        public static explicit operator ShipButton[,](Field field) => field._shipButtons;

        public static bool CoordinatesInside(int x, int y) => CoordinateInside(x) && CoordinateInside(y);

        public static bool CoordinateInside(int coordinate)
        {
            return coordinate >= StartingCoordinate && coordinate < Size;
        }

        public void Declare(Form form, int markingOffset)
        {
            const int indent = 50;
            FormButtonController formButtonController = new FormButtonController(form);

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    formButtonController.CreateNewShipButton(this, markingOffset, indent, y, x);
                }
                AddMarking(form, y, markingOffset);
            }
        }

        private void AddMarking(Form form, int coordinate, int offset)
        {
            const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int buttonSize = FormButtonController.ButtonSize,
                doubleButtonSize = buttonSize * 2,
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
                Text = Convert.ToString(coordinate + FieldController.NextIndex),
                AutoSize = true
            };

            form.Controls.Add(digit);
            form.Controls.Add(letter);
        }

        public void Dispose()
        {
            foreach (ShipButton button in _shipButtons)
                button?.Dispose();
        }

        public void Unable()
        {
            foreach (ShipButton button in _shipButtons)
                button.Enabled = false;
        }
    }
}
