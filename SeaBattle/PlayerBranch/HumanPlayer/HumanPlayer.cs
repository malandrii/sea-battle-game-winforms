using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class HumanPlayer : Player
    {
        private Enemy _enemy;

        public HumanPlayer(MainForm mainForm) : base(mainForm) { }

        public void SetEnemy(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void SetShip(ShipButton senderShipButton)
        {
            List<Point> shipCoordinates = GetShipCoordinates(size: _mainForm.PreGameController.ChosenShipSize, 
                senderShipButton.X, senderShipButton.Y, 
                isHorizontal: _mainForm.PreGameController.ChosenShipIsHorizontal, randomShip: false);

            Ship newPlayerShip = DeclareShip(shipCoordinates);
            HumanPlayerShipController.ColorShip(newPlayerShip);

            HashSet<ShipButton> allShipParts = new HashSet<ShipButton>((ShipButton[])newPlayerShip);
            allShipParts.UnionWith(newPlayerShip.MarkedParts);
            FieldController.UnableRegion(allShipParts);
        }

        public void Attack(object sender)
        {
            bool canMove = !_mainForm.GameController.EnemyTurn;
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