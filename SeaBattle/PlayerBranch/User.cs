using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    sealed public class User : Player
    {
        private Enemy _enemy;

        public User(MainForm mainForm) : base(mainForm)
        {
            const int userMarkingOffset = 0;
            _markingOffset = userMarkingOffset;
        }

        public void SetEnemy(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void SetShip(ShipButton senderShipButton)
        {
            List<Point> shipCoordinates = GetShipCoordinates(size: _mainForm.ChosenSize, 
                senderShipButton.X, senderShipButton.Y, 
                isHorizontal: _mainForm.ChosenShipIsHorizontal, randomShip: false);

            Ship newPlayerShip = DeclareShip(shipCoordinates);
            foreach (ShipButton shipPart in newPlayerShip.ShipParts)
                shipPart.BackColor = Color.Blue;

            foreach (var shipButton in newPlayerShip.ShipParts)
                shipButton.Enabled = false;
            foreach (var shipButton in newPlayerShip.MarkedParts)
                shipButton.Enabled = false;
        }

        public void UnableField()
        {
            foreach (ShipButton button in Field) 
                button.Enabled = false;
        }

        public void Attack(object sender)
        {
            bool canMove = !_mainForm.ComputerMovingLabelVisible;
            if (!canMove) return;
            _mainForm.SetFocus();
            ShipButton senderButton = sender as ShipButton;
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
            if (senderButton.ShipFrom.IsDead)
            {
                _mainForm.SetLabelStatus("Dead", Color.Red);
                senderButton.ShipFrom.Death();
            }
            _enemy.CheckDeath();
        }
    }
}