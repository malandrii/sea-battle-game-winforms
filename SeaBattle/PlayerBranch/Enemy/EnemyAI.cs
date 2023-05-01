using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattle
{
    sealed public class EnemyAI
    {
        private static Point[] _crossCoordiantesOffset;
        private readonly Enemy _enemy;
        private readonly FieldController _enemyFieldController;
        private List<ShipButton> _buttonsAround;
        private ShipButton[,] _humanPlayerField;
        private ShipButton _foundShipButton;
        private bool _buttonsAroundButtonToAttackSet = false;
        private bool _shipHorizontalitySet = false;
        private bool _shipIsHorizontal;
        private bool _changeAttackSide = false;

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

        public bool FoundHumanPlayerShip { get; set; } = false;

        public bool HorizontalityDefined { get; set; } = false;

        public bool ChangeDefinedAttackSide { private get; set; } = false;

        public void SetButtonsAroundButtonToAttack(ShipButton button, ShipButton[,] Field)
        {
            if (_buttonsAroundButtonToAttackSet) return;
            ResetVariables();
            FoundHumanPlayerShip = true;
            _foundShipButton = button;
            _humanPlayerField = Field;
            _buttonsAround.AddRange(from Point crossCoordianteOffset in _crossCoordiantesOffset
                                    let xShifted = _foundShipButton.X + crossCoordianteOffset.X
                                    let yShifted = _foundShipButton.Y + crossCoordianteOffset.Y
                                    where _enemyFieldController.AreCoordinatesInsideField(xShifted, yShifted)
                                    let buttonAround = _humanPlayerField[xShifted, yShifted]
                                    where !buttonAround.IsShot
                                    select buttonAround);
            _buttonsAroundButtonToAttackSet = true;
        }

        public void ResetVariables()
        {
            _shipHorizontalitySet = false;
            _foundShipButton = null;
            FoundHumanPlayerShip = false;
            ChangeDefinedAttackSide = false;
            _buttonsAroundButtonToAttackSet = false;
            _buttonsAround = new List<ShipButton>();
        }

        public void SetAttackCoordinates(ref int x, ref int y)
        {
            if (_enemy.RandomMoves || !FoundHumanPlayerShip)
            {
                SetRandomAttackingCoordinates(ref x, ref y);
            }
            else GuessNextShipPart(ref x, ref y);
        }

        private void SetRandomAttackingCoordinates(ref int x, ref int y)
        {
            _enemy.MakeCoordinatesRandom(ref x, ref y);
            _buttonsAroundButtonToAttackSet = false;
            HorizontalityDefined = false;
        }

        private void GuessNextShipPart(ref int x, ref int y)
        {
            if (!HorizontalityDefined)
            {
                GetRandomCoordinateAroundShip(out x, out y);
                return;
            }
            if (!_shipHorizontalitySet) SetHorizontality(x, y);
            SetShiftedAttackingCoordinates(ref x, ref y);
        }

        private void SetShiftedAttackingCoordinates(ref int x, ref int y)
        {
            _enemy.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);
            bool needAttackSideChange = !_enemyFieldController.AreCoordinatesInsideField(x, y)
                || ChangeDefinedAttackSide || _humanPlayerField[x, y].IsShot;
            if (needAttackSideChange)
            {
                SwitchAttackSide(ref x, ref y);
                _enemy.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);
                ChangeDefinedAttackSide = false;
            }
        }

        private void GetRandomCoordinateAroundShip(out int x, out int y)
        {
            ShipButton randomButtonAround = _buttonsAround[new Random().Next(0, _buttonsAround.Count)];
            x = randomButtonAround.X;
            y = randomButtonAround.Y;
            _buttonsAround.Remove(randomButtonAround);
        }

        private void SetHorizontality(int x, int y)
        {
            _shipIsHorizontal = _humanPlayerField[x, y].X != _foundShipButton.X;
            _changeAttackSide = _shipIsHorizontal ? _foundShipButton.X < x : _foundShipButton.Y < y;
            _shipHorizontalitySet = true;
        }

        private void SwitchAttackSide(ref int x, ref int y)
        {
            x = _foundShipButton.X;
            y = _foundShipButton.Y;
            _changeAttackSide = !_changeAttackSide;
        }
    }
}