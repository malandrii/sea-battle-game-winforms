using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattle
{
    sealed public class EnemyAI
    {
        private readonly Enemy _enemy;
        private List<ShipButton> _buttonsAround;
        private ShipButton[,] _humanPlayerField;
        private ShipButton _foundShipButton;
        private bool _buttonsAroundButtonToAttackSet = false;
        private bool _shipHorizontalitySet = false;
        private bool _shipIsHorizontal;
        private bool _changeAttackSide = false;

        public EnemyAI(Enemy enemy)
        {
            _enemy = enemy;
        }

        public bool FoundHumanPlayerShip { get; set; } = false;

        public bool HorizontalityDefined { get; set; } = false;

        public bool ChangeDefinedAttackSide { private get; set; } = false;

        public void SetButtonsAroundButtonToAttack(ShipButton button, ShipButton[,] Field)
        {
            if (_buttonsAroundButtonToAttackSet) return;
            ResetVariables();
            const int crossUpRight = 1, crossDownLeft = -1, crossCoordinatesCount = 4;
            Point[] crossCoordiantesOffset = new Point[crossCoordinatesCount]{
                new Point(crossUpRight, 0),
                new Point(0, crossUpRight),
                new Point(crossDownLeft, 0),
                new Point(0, crossDownLeft) };
            FoundHumanPlayerShip = true;
            _foundShipButton = button;
            _humanPlayerField = Field;
            Point point = new Point(button.X, button.Y);
            _buttonsAround.AddRange(from Point neighbourPoint in crossCoordiantesOffset
                                    let xShifted = _foundShipButton.X + neighbourPoint.X
                                    let yShifted = _foundShipButton.Y + neighbourPoint.Y
                                    where FieldController.CoordinatesInsideField(xShifted, yShifted)
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
            Randomizer.MakeCoordinatesRandom(ref x, ref y);
            _buttonsAroundButtonToAttackSet = false;
            HorizontalityDefined = false;
        }

        private void GuessNextShipPart(ref int x, ref int y)
        {
            if (!HorizontalityDefined)
            {
                Randomizer.GetRandomCoordinateAroundShip(out x, out y, _buttonsAround);
                return;
            }
            if (!_shipHorizontalitySet) SetHorizontality(x, y);
            SetShiftedAttackingCoordinates(ref x, ref y);
        }

        private void SetShiftedAttackingCoordinates(ref int x, ref int y)
        {
            Player.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);
            bool needAttackSideChange = !FieldController.CoordinatesInsideField(x, y)
                || ChangeDefinedAttackSide || _humanPlayerField[x, y].IsShot;
            if (needAttackSideChange)
            {
                SwitchAttackSide(ref x, ref y);
                Player.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);
                ChangeDefinedAttackSide = false;
            }
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