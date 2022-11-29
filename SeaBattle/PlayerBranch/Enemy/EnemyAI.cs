using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class EnemyAI
    {
        private static readonly Random _random = new Random();
        private static Point[] _crossCoordiantesOffset;
        private readonly Enemy _enemy;
        private readonly FieldController _enemyFieldController;
        private ShipButton[,] _userField;
        private ShipButton _foundShipButton;
        private List<ShipButton> _buttonsAround;
        private bool _firstCheck = true;
        private bool _shipHorizontalitySet = false;
        private bool _shipIsHorizontal;
        public bool FoundUserShip { get; set; } = false;
        public bool HorizontalityDefined { get; set; } = false;
        public bool ChangeGuessingAttackSide { get; set; } = false;
        public bool ChangeDefinedAttackSide { get; set; } = false;

        public EnemyAI(Enemy enemy, FieldController fieldController)
        {
            _enemy = enemy;
            _enemyFieldController = fieldController;
            const int crossUpRight = 1, crossDownLeft = -1, crossCoordinatesCount = 4;
            _crossCoordiantesOffset = new Point[crossCoordinatesCount]{
                new Point(crossUpRight, 0),
                new Point(0, crossUpRight),
                new Point(crossDownLeft, 0),
                new Point(0, crossDownLeft) };
        }

        public void SetButtonsAroundButtonToAttack(ShipButton button, ShipButton[,] Field)
        {
            if (!_firstCheck) return;
            ResetVariables();
            FoundUserShip = true;
            _foundShipButton = button;
            _userField = Field;
            for (int i = 0; i < _crossCoordiantesOffset.Length; i++)
            {
                int xShifted = _foundShipButton.X + _crossCoordiantesOffset[i].X,
                    yShifted = _foundShipButton.Y + _crossCoordiantesOffset[i].Y;
                if (_enemyFieldController.AreCoordinatesInsideField(xShifted, yShifted))
                {
                    ShipButton buttonAround = _userField[xShifted, yShifted];
                    if (!buttonAround.IsShot) _buttonsAround.Add(buttonAround);
                }
            }
            _firstCheck = false;
        }

        public void ResetVariables()
        {
            _shipHorizontalitySet = false;
            _foundShipButton = null;
            FoundUserShip = false;
            ChangeDefinedAttackSide = false;
            _firstCheck = true;
            _buttonsAround = new List<ShipButton>();
        }

        public void SetAttackCoordinates(ref int x, ref int y)
        {
            if (_enemy.RandomMoves || !FoundUserShip)
            {
                _enemy.MakeCoordinatesRandom(ref x, ref y);
                _firstCheck = true;
                HorizontalityDefined = false;
            }
            else GuessNextShipPart(ref x, ref y);
        }

        private void GuessNextShipPart(ref int x, ref int y)
        {
            if (!HorizontalityDefined)
            {
                ShipButton randomButtonAround = _buttonsAround[_random.Next(0, _buttonsAround.Count)];
                x = randomButtonAround.X;
                y = randomButtonAround.Y;
                _buttonsAround.Remove(randomButtonAround);
                return;
            }
            if (!_shipHorizontalitySet) SetHorizontality(x, y);
            _enemy.ShiftCoordinates(_shipIsHorizontal, ChangeGuessingAttackSide, ref x, ref y);
            bool needSideChange = !_enemyFieldController.AreCoordinatesInsideField(x, y)
                || ChangeDefinedAttackSide || _userField[x, y].IsShot;
            if (needSideChange)
            {
                ChangeTurningToOtherSide(ref x, ref y);
                _enemy.ShiftCoordinates(_shipIsHorizontal, ChangeGuessingAttackSide, ref x, ref y);
                ChangeDefinedAttackSide = false;
            }
        }

        private void SetHorizontality(int x, int y)
        {
            _shipIsHorizontal = _userField[x, y].X != _foundShipButton.X;
            ChangeGuessingAttackSide = _shipIsHorizontal ?
                _foundShipButton.X < x : _foundShipButton.Y < y;
            _shipHorizontalitySet = true;
        }

        private void ChangeTurningToOtherSide(ref int x, ref int y)
        {
            x = _foundShipButton.X;
            y = _foundShipButton.Y;
            ChangeGuessingAttackSide = !ChangeGuessingAttackSide;
        }
    }
}