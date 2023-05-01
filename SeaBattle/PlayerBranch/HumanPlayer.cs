using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    sealed public class HumanPlayer : Player
    {
        private Enemy _enemy;

        public HumanPlayer(MainForm mainForm) : base(mainForm)
        {
            const int humanPlayerMarkingOffset = 0;
            _markingOffset = humanPlayerMarkingOffset;
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
            MainFormButtonController.ColorHumanPlayerShip(newPlayerShip.ShipParts);
            MainFormButtonController.UnableRegion(newPlayerShip.ShipParts);
            MainFormButtonController.UnableRegion(newPlayerShip.MarkedParts);
        }

        public void UnableField()
        {
            foreach (ShipButton button in Field)
                button.Enabled = false;
        }

        public void Attack(object sender)
        {
            bool canMove = !_mainForm.ComputerTurnLabelVisible;
            if (!canMove) return;
            _mainForm.SetFocus();
            ShipButton senderButton = sender as ShipButton;
            senderButton.Shoot();
            if (!senderButton.IsShipPart)
            {
                _mainForm.HumanPlayerStatus.SetStandartLabelStatus();
                if (!_enemy.CheckDeath() && !CheckDeath()) _enemy.StartNewAttack();
                return;
            }
            senderButton.ShipFrom.TakeDamage();
            _enemy.TakeDamage();
            SetHumanPlayerStatus(senderButton);
            _enemy.CheckDeath();
        }

        private void SetHumanPlayerStatus(ShipButton senderButton)
        {
            _mainForm.HumanPlayerStatus.SetHitEnemyShipStatus();
            if (senderButton.ShipFrom.IsDead)
            {
                _mainForm.HumanPlayerStatus.SetDeadEnemyShipStatus();
                senderButton.ShipFrom.Death();
            }
        }
    }
}