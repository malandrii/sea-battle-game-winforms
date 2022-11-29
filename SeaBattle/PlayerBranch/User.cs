using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class User : Player
    {
        private Enemy _enemy;

        public User(MainForm mainForm) : base(mainForm)
        {
            int userMarkingOffset = 0;
            _markingOffset = userMarkingOffset;
        }

        public void SetEnemy(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void SetShip(ShipButton senderShipButton)
        {
            int x = senderShipButton.X,
                y = senderShipButton.Y,
                xCopy = x,
                yCopy = y;
            List<ShipButton> shipParts = new List<ShipButton>();
            TuneShip(ref x, ref y, new Ship(shipParts), setButtons: true);
            y = yCopy; x = xCopy;
            TuneShip(ref x, ref y, new Ship(shipParts), setButtons: false);
        }

        public void UnableField()
        {
            foreach (ShipButton button in Field) button.Enabled = false;
        }

        private void TuneShip(ref int x, ref int y, Ship newPlayerShip, bool setButtons)
        {
            for (int i = 0; i < _mainForm.ChosenSize; i++)
            {
                Field[x, y].IsShipPart = true;
                if (setButtons)
                {
                    Field[x, y].Enabled = false;
                    Field[x, y].BackColor = Color.Blue;
                    newPlayerShip.ShipParts.Add(Field[x, y]);
                }
                else
                {
                    MakeShipPart(x, y, newPlayerShip);
                    newPlayerShip.MakeShipUnabled();
                }
                ShiftCoordinates(isHorizontal: _mainForm.ChosenShipIsHorizontal,
                        Sum: _mainForm.ChosenShipIsHorizontal, ref x, ref y);
            }
        }

        public void Attack(object sender)
        {
            bool canMove = !_mainForm.ComputerMovingLabelVisible;
            if (!canMove) return;
            _mainForm.SetFocus();
            ShipButton senderButton = (ShipButton)sender;
            senderButton.Enabled = false;
            senderButton.Shoot();
            if (!senderButton.IsShipPart)
            {
                _mainForm.SetLabelStatus(MainForm.StandartLabelStatusText,
                    MainForm.StandartLabelStatusColor);
                if (!_enemy.CheckDeath() && !CheckDeath()) _enemy.StartAttack();
                return;
            }
            senderButton.ShipFrom.TakeDamage();
            _enemy.ShipPartsAlive--;
            _mainForm.SetLabelStatus("Hit", Color.DarkRed);
            if (senderButton.ShipFrom.IsDead())
            {
                _mainForm.SetLabelStatus("Dead", Color.Red);
                senderButton.ShipFrom.Death();
            }
            _enemy.CheckDeath();
        }
    }
}