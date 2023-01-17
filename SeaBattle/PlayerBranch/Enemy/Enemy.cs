using System;
using System.Drawing;

namespace SeaBattle
{
    public class Enemy : Player
    {
        private readonly User _user;
        private readonly EnemyTimer _attackTimer;
        private readonly EnemyAI _enemyAI;
        private int _xToAttack = 0;
        private int _yToAttack = 0;
        public bool RandomMoves { get; set; } = false;
        public bool MarkMoves { get; set; } = true;

        public Enemy(MainForm mainForm, User user) : base(mainForm)
        {
            const int enemyMarkingOffset = 380;
            _markingOffset = enemyMarkingOffset;
            _user = user;
            Field = new ShipButton[_fieldSize, _fieldSize];
            _attackTimer = new EnemyTimer(this, _fieldController, mainForm);
            _enemyAI = new EnemyAI(this, _fieldController);
        }

        public override void DeclareField()
        {
            _fieldController.CreateField(Field, _markingOffset);
            foreach (ShipButton shipButton in Field)
            {
                shipButton.Click += new EventHandler(Button_Click);
                shipButton.TextChanged += new EventHandler(_mainForm.ShipButton_TextChanged);
            }
        }

        public void StartAttack()
        {
            _mainForm.SetComputerMovesToolStripsEnables(enable: false);
            _attackTimer.Start();
            _attackTimer.ResetTicks();
            _mainForm.SetLabelComputerMoveVisibility(visible: true);
        }

        public void ContinueTheAttack()
        {
            _enemyAI.SetAttackCoordinates(ref _xToAttack, ref _yToAttack);
            ShipButton buttonToAttack = _user.Field[_xToAttack, _yToAttack];
            if (buttonToAttack.IsShot)
            {
                if (_enemyAI.HorizontalityDefined)
                    _enemyAI.ChangeDefinedAttackSide = true;
                if (RandomMoves || !_enemyAI.FoundUserShip)
                {
                    ContinueTheAttack();
                    return;
                }
            }
            Attack(buttonToAttack);
        }

        private void Attack(ShipButton buttonToAttack)
        {
            buttonToAttack.Shoot();
            if (MarkMoves) buttonToAttack.BackColor = Color.DarkRed;
            if (!buttonToAttack.IsShipPart)
            {
                if (_enemyAI.HorizontalityDefined) 
                    _enemyAI.ChangeDefinedAttackSide = true;
                _attackTimer.Stop();
                return;
            }
            if (_enemyAI.FoundUserShip) _enemyAI.HorizontalityDefined = true;
            _enemyAI.FoundUserShip = true;
            buttonToAttack.ShipFrom.TakeDamage();
            _user.ShipPartsAlive--;
            if (buttonToAttack.ShipFrom.IsDead())
            {
                buttonToAttack.ShipFrom.Death();
                _enemyAI.ResetVariables();
                if (_user.CheckDeath()) return;
            }
            else _enemyAI.SetButtonsAroundButtonToAttack(buttonToAttack, _user.Field);
            StartAttack();
        }

        public void FinishGame()
        {
            foreach (ShipButton button in Field)
            {
                if (button.IsShipPart) button.BackColor = Color.Red;
                button.Enabled = false;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            _user.Attack(sender);
        }
    }
}