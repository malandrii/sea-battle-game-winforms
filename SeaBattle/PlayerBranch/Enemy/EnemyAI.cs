using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattle
{
    public class EnemyAI
    {
        private readonly Enemy _enemy;
        private readonly Field _humanPlayerField;
        private List<ShipButton> _buttonsAround;
        private ShipButton _foundShipButton;
        private bool _buttonsAroundButtonToAttackSet = false;
        private bool _shipHorizontalitySet = false;
        private bool _shipIsHorizontal;
        private bool _changeAttackSide = false;

        public EnemyAI(Enemy enemy, Field humanPlayerField)
        {
            _enemy = enemy;
            _humanPlayerField = humanPlayerField;
        }

        public bool FoundHumanPlayerShip { get; set; } = false;

        public bool HorizontalityDefined { get; set; } = false;

        public bool ChangeDefinedAttackSide { private get; set; } = false;

        public void SetButtonsAroundButtonToAttack(ShipButton button)
        {
            if (_buttonsAroundButtonToAttackSet) return;
            ResetVariables();
            FoundHumanPlayerShip = true;
            _foundShipButton = button;
            _buttonsAround.AddRange(from Point neighbourPoint in FieldController.GetCrossCoordiantes()
                                    let xShifted = _foundShipButton.X + neighbourPoint.X
                                    let yShifted = _foundShipButton.Y + neighbourPoint.Y
                                    where Field.CoordinatesInside(xShifted, yShifted)
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

        private void SetHorizontality(int x, int y)
        {
            _shipIsHorizontal = _humanPlayerField[x, y].X != _foundShipButton.X;
            _changeAttackSide = _shipIsHorizontal ? _foundShipButton.X < x : _foundShipButton.Y < y;
            _shipHorizontalitySet = true;
        }

        private void SetShiftedAttackingCoordinates(ref int x, ref int y)
        {
            Player.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);

            bool needAttackSideChange = !Field.CoordinatesInside(x, y)
                || ChangeDefinedAttackSide || _humanPlayerField[x, y].IsShot;

            if (needAttackSideChange)
            {
                SwitchAttackSide(ref x, ref y);
                Player.ShiftCoordinates(_shipIsHorizontal, _changeAttackSide, ref x, ref y);
                ChangeDefinedAttackSide = false;
            }
        }

        private void SwitchAttackSide(ref int x, ref int y)
        {
            x = _foundShipButton.X;
            y = _foundShipButton.Y;
            _changeAttackSide = !_changeAttackSide;
        }
    }
}