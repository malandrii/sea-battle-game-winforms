using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class MainForm : Form
    {
        private const int ProgressBarMaximumValue = 100;
        private const int FirstIndex = FieldController.StartingCoordinate;
        public const int NextIndex = 1;
        public const int ShipSizesAmount = 4;
        public const string StandartLabelStatusText = "----";
        public static readonly Color StandartLabelStatusColor = Color.Black;
        private static int s_fieldSize;
        private Button[] _chooseSizeButtons;
        private Label[] _chooseSizeLabels;
        private FieldController _fieldController;
        private User _user;
        private Enemy _enemy;
        private bool _makeSizeZero = false;
        private int _chosenSize = 0;
        
        public MainForm()
        {
            InitializeComponent();
        }

        public int ChosenSize 
        { 
            get => _chosenSize; 
            private set 
            { 
                if (value >= FirstIndex && value <= ShipSizesAmount) 
                    _chosenSize = value; 
            } 
        }

        public int ComputerMoveSpeedSelectedIndex { get => comboBoxComputerMoveSpeed.SelectedIndex; }

        public bool ChosenShipIsHorizontal { get; private set; } = false;

        public bool ComputerTurnLabelVisible { get; private set; } = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            const int mediumSpeedSelectedIndex = 1;
            comboBoxComputerMoveSpeed.SelectedIndex = mediumSpeedSelectedIndex;
            comboBoxComputerMoveSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            StartGame();
        }

        private void StartGame()
        {
            const string clearFieldButtonText = "Clear Field";
            ContextMenuStrip.Items[FirstIndex].Text = clearFieldButtonText;
            _fieldController = new FieldController(this);
            s_fieldSize = FieldController.FieldSize;
            CreateShipSizeChoosingPanel();
            DeclarePlayers();
            SetButtonsEvents();
            _enemy.MarkMoves = MarkComputerMovesToolStripMenuItem.Checked;
        }

        private void CreateShipSizeChoosingPanel()
        {
            _chooseSizeButtons = new Button[ShipSizesAmount];
            _chooseSizeLabels = new Label[ShipSizesAmount];
            for (int shipSizeIndex = 0; shipSizeIndex < ShipSizesAmount; shipSizeIndex++)
            {
                _chooseSizeButtons[shipSizeIndex] =
                    (Button)Controls[GetControlText("button", shipSizeIndex + NextIndex)];
                _chooseSizeLabels[shipSizeIndex] =
                    (Label)Controls[GetControlText("label", shipSizeIndex + NextIndex)];
                _chooseSizeButtons[shipSizeIndex].Click += new EventHandler(ButtonToChooseShipSize_Click);
                ComputerMovesSpeedToolStripMenuItem.DropDownItems[shipSizeIndex].Click +=
                    new EventHandler(ToolStripItemToChooseComputerSpeed_Click);
            }
        }

        private void DeclarePlayers()
        {
            _user = new User(this);
            _enemy = new Enemy(this, _user);
            _user.SetEnemy(_enemy);
            _user.Field = new ShipButton[s_fieldSize, s_fieldSize];
            _user.DeclareField();
        }

        private string GetControlText(string controlName, int index)
        {
            return (controlName + (index) + "x").ToString();
        }

        private void SetButtonsEvents()
        {
            foreach (ShipButton shipButton in _user.Field)
            {
                shipButton.Click += new EventHandler(PlayerButton_Click);
                shipButton.MouseEnter += new EventHandler(PlayerButton_MouseEnter);
                shipButton.MouseLeave += new EventHandler(PlayerButton_MouseLeave);
                shipButton.TextChanged += new EventHandler(ShipButton_TextChanged);
            }
        }

        private void ButtonToChooseShipSize_Click(object sender, EventArgs e)
        {
            SetFocus();
            int chooseSizeButtonNumber = 0;
            var chosenButton = (Button)sender;
            SetShipSizeChoosingButtonsColors(chosenButton, ref chooseSizeButtonNumber);
        }

        private void SetShipSizeChoosingButtonsColors(Button chosenButton, ref int chooseSizeButtonNumber)
        {
            for (int i = 0; i < ShipSizesAmount; i++)
            {
                if (chosenButton == _chooseSizeButtons[i])
                {
                    ChosenSize = i + NextIndex;
                    _chooseSizeButtons[i].BackColor = Color.LightSkyBlue;
                    chooseSizeButtonNumber = i;
                }
                else FieldController.ButtonColorToStandart(_chooseSizeButtons[i]);
            }
        }

        private void ToolStripItemToChooseComputerSpeed_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ShipSizesAmount; i++)
                if ((ToolStripItem)sender == ComputerMovesSpeedToolStripMenuItem.DropDownItems[i])
                    comboBoxComputerMoveSpeed.SelectedIndex = i;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ShipSizesAmount; i++)
                CheckComputerMovesSpeedToolStrip(i, check: false);
            CheckComputerMovesSpeedToolStrip(comboBoxComputerMoveSpeed.SelectedIndex, check: true);
        }

        private void CheckComputerMovesSpeedToolStrip(int index, bool check)
        {
            ((ToolStripMenuItem)ComputerMovesSpeedToolStripMenuItem.
                DropDownItems[index]).Checked = check;
        }

        private void PlayerButton_Click(object sender, EventArgs e)
        {
            SetFocus();
            var senderShipButton = sender as ShipButton;
            bool buttonChosen = senderShipButton.BackColor == Color.LightBlue;
            if (!buttonChosen) return;
            ChangeShipsLeft((Button)Controls[GetControlText("button", ChosenSize)],
                (Label)Controls[GetControlText("label", ChosenSize).ToString()]);
            _user.SetShip(senderShipButton);
            if (_makeSizeZero)
            {
                ChosenSize = 0;
                _makeSizeZero = false;
            }
        }

        private void ChangeShipsLeft(Button selectedButton, Label currentSizeShipsLeft)
        {
            const int progressBarIncrement = 10;
            if (progressBar.Value < ProgressBarMaximumValue - progressBarIncrement)
            {
                progressBar.Value += progressBarIncrement;
                currentSizeShipsLeft.Text =
                    Convert.ToString(Convert.ToInt32(currentSizeShipsLeft.Text) - NextIndex);
                if (currentSizeShipsLeft.Text != FirstIndex.ToString()) return;
                selectedButton.Enabled = false;
                FieldController.ButtonColorToStandart(selectedButton);
                _makeSizeZero = true;
            }
            else UnableShipsArrangePanel();
        }

        private void UnableShipsArrangePanel()
        {
            foreach (Button button in _chooseSizeButtons)
            {
                FieldController.ButtonColorToStandart(button);
                button.Enabled = false;
            }
            foreach (Label label in _chooseSizeLabels)
                label.Text = FirstIndex.ToString();
            progressBar.Value = ProgressBarMaximumValue;
            SetShipArrangeButtonEnables(shipsPlaced: true);
            _user.UnableField();
        }

        private void SetShipArrangeButtonEnables(bool shipsPlaced)
        {
            labelShipPlacement.Visible = !shipsPlaced;
            buttonRotate.Enabled = !shipsPlaced;
            buttonGameStart.Enabled = shipsPlaced;
            buttonArrangeShipsRandomly.Enabled = !shipsPlaced;
        }

        private void PlayerButton_MouseEnter(object sender, EventArgs e)
        {
            SetShipVisibility((ShipButton)sender, toAppear: true);
        }

        private void PlayerButton_MouseLeave(object sender, EventArgs e)
        {
            SetShipVisibility((ShipButton)sender, toAppear: false);
        }

        private void SetShipVisibility(ShipButton button, bool toAppear)
        {
            int previousSize = ChosenSize - NextIndex;
            bool spaceIsFree = ChosenShipIsHorizontal ?
                (button.X + previousSize) < s_fieldSize :
                (button.Y - previousSize) >= FieldController.StartingCoordinate;
            ShipToAppear(button, ref spaceIsFree, toAppear, spaceIsFreeSet: false);
            ShipToAppear(button, ref spaceIsFree, toAppear, spaceIsFreeSet: true);
        }

        private void ShipToAppear(ShipButton button, ref bool spaceIsFree, bool toAppear, bool spaceIsFreeSet)
        {
            int x = button.X, y = button.Y;
            if (!spaceIsFree) return;
            for (int i = 0; i < ChosenSize; i++)
            {
                if (!spaceIsFreeSet)
                {
                    spaceIsFree = _user.Field[x, y].Enabled;
                }
                else
                {
                    if (toAppear) _user.Field[x, y].BackColor = Color.LightBlue;
                    else FieldController.ButtonColorToStandart(_user.Field[x, y]);
                }
                _user.ShiftCoordinates(isHorizontal: ChosenShipIsHorizontal,
                    Add: ChosenShipIsHorizontal, ref x, ref y);
            }
        }

        private void ButtonRotate_Click(object sender, EventArgs e)
        {
            ChosenShipIsHorizontal = !ChosenShipIsHorizontal;
            string shipPositionLabel = "Placement: ";
            shipPositionLabel += ChosenShipIsHorizontal ? "—" : "|";
            labelShipPlacement.Text = shipPositionLabel;
        }

        private void ButtonGameStart_Click(object sender, EventArgs e)
        {
            SetControlsVisibility(visible: false);
            labelStatus.Visible = true;
            labelEnemyField.Visible = true;
            _enemy.RandomMoves = checkBoxEnemyRandomMoves.Checked;
            ContextMenuStrip.Items[FirstIndex].Text = "Restart Game";
            _enemy.DeclareField();
            _enemy.SpawnRandomShips();
        }

        public void ShipButton_TextChanged(object sender, EventArgs e)
        {
            var button = sender as ShipButton;
            int fontSize = button.Text == "." ? 15 : 8;
            FontStyle fontStyle = button.Text == "." ? FontStyle.Bold : FontStyle.Regular;
            button.Font = new Font(button.Font.Name, fontSize, fontStyle);
        }

        public void SetLabelStatus(string status, Color color)
        {
            labelStatus.Text = "Status: " + status;
            labelStatus.ForeColor = color;
        }

        public void SetComputerMovesToolStripsEnables(bool enable)
        {
            for (int i = 0; i < ShipSizesAmount; i++)
                ComputerMovesSpeedToolStripMenuItem.DropDownItems[i].Enabled = enable;
            GameRestartToolStripMenuItem.Enabled = enable;
        }

        public void SetLabelComputerMoveVisibility(bool visible)
        {
            labelComputerMove.Visible = visible;
        }

        private void LabelComputerMove_VisibleChanged(object sender, EventArgs e)
        {
            ComputerTurnLabelVisible = labelComputerMove.Visible;
        }

        private void CheckBoxMarkEnemyMoves_CheckedChanged(object sender, EventArgs e)
        {
            _enemy.MarkMoves = checkBoxMarkEnemyMoves.Checked;
            MarkComputerMovesToolStripMenuItem.Checked = checkBoxMarkEnemyMoves.Checked;
        }

        private void MarkComputerMovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkComputerMovesToolStripMenuItem.Checked = !MarkComputerMovesToolStripMenuItem.Checked;
            _enemy.MarkMoves = MarkComputerMovesToolStripMenuItem.Checked;
            checkBoxMarkEnemyMoves.Checked = MarkComputerMovesToolStripMenuItem.Checked;
            foreach (ShipButton button in _user.Field)
                button.RefreshMarking(MarkComputerMovesToolStripMenuItem.Checked);
        }

        public void SetFocus()
        {
            labelYourField.Focus();
        }

        public void FinishGame()
        {
            bool userWon = _enemy.ShipPartsAlive == 0;
            string gameFinishTitle = "You ";
            gameFinishTitle += userWon ? "won" : "lost";
            Color gameFinishTitleColor = userWon ? Color.Green : Color.Red;
            SetLabelStatus(gameFinishTitle, gameFinishTitleColor);
            _enemy.FinishGame();
            buttonRestart.Visible = true;
        }

        private void SetControlsVisibility(bool visible)
        {
            Control[] controlsToSetVisibility = {
                button1x, button2x, button3x,
                button4x, buttonRotate, label1x, label2x, label3x,
                label4x, labelPlaceShips, labelShipsPlaceLeft, progressBar,
                buttonGameStart, labelComputerMovingSpeed,
                comboBoxComputerMoveSpeed, checkBoxMarkEnemyMoves, panel };
            Label[] labelsToSetOppositeVisibility = { labelStatus, labelEnemyField };
            foreach (Control control in controlsToSetVisibility) control.Visible = visible;
            foreach (Label label in labelsToSetOppositeVisibility) label.Visible = !visible;
        }

        private void RestartGame()
        {
            for (int i = 0; i < ShipSizesAmount; i++)
            {
                SetStartingShipAmountControlSettings(choosingSizeControlIndex: i);
            }
            SetControlsVisibility(visible: true);
            SetLabelStatus(StandartLabelStatusText, StandartLabelStatusColor);
            ChosenSize = FirstIndex;
            progressBar.Value = FirstIndex;
            SetShipArrangeButtonEnables(shipsPlaced: false);
            buttonRestart.Visible = false;
            _makeSizeZero = false;
            _fieldController.ClearField(_user.Field);
            _fieldController.ClearField(_enemy.Field);
            StartGame();
        }

        private void SetStartingShipAmountControlSettings(int choosingSizeControlIndex)
        {
            _chooseSizeButtons[choosingSizeControlIndex].Enabled = true;
            ((Label)Controls[GetControlText("label", choosingSizeControlIndex + NextIndex)]).Text
                = Convert.ToString(ShipSizesAmount - choosingSizeControlIndex);
            FieldController.ButtonColorToStandart((Button)Controls[GetControlText("button", choosingSizeControlIndex + NextIndex)]);
        }

        private void ButtonArrangeShipsRandomly_Click(object sender, EventArgs e)
        {
            RestartGame();
            _user.SpawnRandomShips();
            UnableShipsArrangePanel();
            foreach (ShipButton button in _user.Field)
            {
                if (!button.IsShipPart) continue;
                button.BackColor = Color.Blue;
            }
        }

        private void ButtonRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void GameRestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            labelPrompt.Visible = false;
        }
    }
}