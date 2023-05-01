using System;

namespace SeaBattle
{
    sealed public class Enemy : Player
    {
        private readonly HumanPlayer _humanPlayer;
        private readonly EnemyTimer _attackTimer;
        private readonly EnemyAI _enemyAI;
        private int _xToAttack = 0;
        private int _yToAttack = 0;

        public Enemy(MainForm mainForm, HumanPlayer humanPlayer) : base(mainForm)
        {
            const int enemyMarkingOffset = 380;
            _markingOffset = enemyMarkingOffset;
            _humanPlayer = humanPlayer;
            Field = new ShipButton[_fieldSize, _fieldSize];
            _attackTimer = new EnemyTimer(this, mainForm);
            _enemyAI = new EnemyAI(this, _fieldController);
        }

        public bool RandomMoves { get; set; } = false;

        public bool MarkMoves { private get; set; } = true;

        public override void DeclareField()
        {
            base.DeclareField();
            foreach (ShipButton shipButton in Field)
            {
                shipButton.Click += new EventHandler(Button_Click);
                shipButton.TextChanged += new EventHandler(_mainForm.ShipButton_TextChanged);
            }
        }

        public void StartNewAttack()
        {
            _mainForm.SetComputerMovesToolStripsEnables(enable: false);
            _attackTimer.Start();
            _attackTimer.ResetTicks();
            _mainForm.SetLabelComputerMoveVisibility(visible: true);
        }

        public void ContinueAttack()
        {
            _enemyAI.SetAttackCoordinates(ref _xToAttack, ref _yToAttack);
            ShipButton buttonToAttack = _humanPlayer.Field[_xToAttack, _yToAttack];
            if (buttonToAttack.IsShot)
            {
                if (_enemyAI.HorizontalityDefined)
                    _enemyAI.ChangeDefinedAttackSide = true;
                if (RandomMoves || !_enemyAI.FoundHumanPlayerShip)
                {
                    ContinueAttack();
                    return;
                }
            }
            Attack(buttonToAttack);
        }

        private void Attack(ShipButton buttonToAttack)
        {
            buttonToAttack.Shoot();
            if (MarkMoves) MainFormButtonController.ButtonColorEnemyMarkedHit(buttonToAttack);
            if (buttonToAttack.IsShipPart)
            {
                AttackShipPart(buttonToAttack);
            }
            else
            {
                StopAttack();
            }
        }

        private void AttackShipPart(ShipButton buttonToAttack)
        {
            if (_enemyAI.FoundHumanPlayerShip) _enemyAI.HorizontalityDefined = true;
            _enemyAI.FoundHumanPlayerShip = true;
            buttonToAttack.ShipFrom.TakeDamage();
            _humanPlayer.TakeDamage();
            if (buttonToAttack.ShipFrom.IsDead)
            {
                buttonToAttack.ShipFrom.Death();
                _enemyAI.ResetVariables();
                if (_humanPlayer.CheckDeath()) return;
            }
            else _enemyAI.SetButtonsAroundButtonToAttack(buttonToAttack, _humanPlayer.Field);
            StartNewAttack();
        }

        private void StopAttack()
        {
            if (_enemyAI.HorizontalityDefined)
                _enemyAI.ChangeDefinedAttackSide = true;
            _attackTimer.Stop();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            _humanPlayer.Attack(sender);
        }
    }
}