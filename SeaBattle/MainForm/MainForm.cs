using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class MainForm : Form
    {
        private const int ProgressBarMaximumValue = 100;
        private readonly PreGameController _preGameController;
        private readonly GameController _gameController;
        private readonly HumanPlayerShipController _humanPlayerShipController;
        private Button[] _chooseSizeButtons;
        private Label[] _chooseSizeLabels;

        public MainForm()
        {
            InitializeComponent();
            _preGameController = new PreGameController(this);
            _gameController = new GameController(this);
            _humanPlayerShipController = new HumanPlayerShipController(this);
            HumanPlayerStatus = new MainFormHumanPlayerStatus(this);
        }

        public MainFormHumanPlayerStatus HumanPlayerStatus { get; }

        public PreGameController PreGameController => _preGameController;

        public GameController GameController => _gameController;

        public ComboBox EnemyMoveSpeedComboBox => comboBoxEnemyMoveSpeed;

        public bool MarkComputerMovesToolStripMenuItemChecked => MarkComputerMovesToolStripMenuItem.Checked;

        public bool ComputerTurnLabelVisible { get; private set; } = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            const int mediumSpeedSelectedIndex = 1;
            comboBoxEnemyMoveSpeed.SelectedIndex = mediumSpeedSelectedIndex;
            comboBoxEnemyMoveSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            _preGameController.StartPreGame();
        }

        public void CreateShipSizeChoosingPanel()
        {
            _chooseSizeButtons = new Button[GameController.ShipSizesAmount];
            _chooseSizeLabels = new Label[GameController.ShipSizesAmount];
            for (int shipSizeIndex = 0; shipSizeIndex < GameController.ShipSizesAmount; shipSizeIndex++)
            {
                _chooseSizeButtons[shipSizeIndex] =
                    (Button)Controls[GetControlText("button", shipSizeIndex + FieldController.NextIndex)];
                _chooseSizeLabels[shipSizeIndex] =
                    (Label)Controls[GetControlText("label", shipSizeIndex + FieldController.NextIndex)];
                _chooseSizeButtons[shipSizeIndex].Click += new EventHandler(ButtonToChooseShipSize_Click);
                ComputerMovesSpeedToolStripMenuItem.DropDownItems[shipSizeIndex].Click +=
                    new EventHandler(ToolStripItemToChooseComputerSpeed_Click);
            }
        }

        private string GetControlText(string controlName, int index)
        {
            return (controlName + (index) + "x").ToString();
        }

        public void SetButtonsEvents()
        {
            foreach (ShipButton shipButton in _gameController.HumanPlayer.Field)
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
            var chosenButton = (Button)sender;
            SetShipSizeChoosingButtonsColors(chosenButton);
        }

        private void SetShipSizeChoosingButtonsColors(Button chosenButton)
        {
            for (int i = 0; i < GameController.ShipSizesAmount; i++)
            {
                if (chosenButton == _chooseSizeButtons[i])
                {
                    _preGameController.ChosenShipSize = i + FieldController.NextIndex;
                    _chooseSizeButtons[i].BackColor = Color.LightSkyBlue;
                }
                else FormButtonController.ButtonColorToStandart(_chooseSizeButtons[i]);
            }
        }

        private void ToolStripItemToChooseComputerSpeed_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GameController.ShipSizesAmount; i++)
                if ((ToolStripItem)sender == ComputerMovesSpeedToolStripMenuItem.DropDownItems[i])
                    comboBoxEnemyMoveSpeed.SelectedIndex = i;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GameController.ShipSizesAmount; i++)
                CheckComputerMovesSpeedToolStrip(i, check: false);
            CheckComputerMovesSpeedToolStrip(comboBoxEnemyMoveSpeed.SelectedIndex, check: true);
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
            if (!HumanPlayerShipController.ButtonIsShipPreviewPart(senderShipButton)) return;
            ChangeShipsLeft((Button)Controls[GetControlText("button", _preGameController.ChosenShipSize)],
                (Label)Controls[GetControlText("label", _preGameController.ChosenShipSize).ToString()]);
            _gameController.HumanPlayer.SetShip(senderShipButton);
            _preGameController.TryToChangeChosenShipSize();
        }

        private void ChangeShipsLeft(Button selectedButton, Label currentSizeShipsLeft)
        {
            const int progressBarIncrement = 10;
            if (progressBar.Value < ProgressBarMaximumValue - progressBarIncrement)
            {
                progressBar.Value += progressBarIncrement;
                currentSizeShipsLeft.Text =
                    Convert.ToString(Convert.ToInt32(currentSizeShipsLeft.Text) - FieldController.NextIndex);
                if (currentSizeShipsLeft.Text != Field.StartingCoordinate.ToString()) return;
                selectedButton.Enabled = false;
                FormButtonController.ButtonColorToStandart(selectedButton);
                _preGameController.ShipHasBeenPlaced = true;
            }
            else UnableShipsArrangePanel();
        }

        public void UnableShipsArrangePanel()
        {
            foreach (Button button in _chooseSizeButtons)
            {
                FormButtonController.ButtonColorToStandart(button);
                button.Enabled = false;
            }
            foreach (Label label in _chooseSizeLabels)
                label.Text = Field.StartingCoordinate.ToString();
            progressBar.Value = ProgressBarMaximumValue;
            SetShipArrangeButtonEnables(shipsPlaced: true);
            _gameController.HumanPlayer.Field.Unable();
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
            _humanPlayerShipController.SetShipVisibility((ShipButton)sender, _preGameController.ChosenShipSize,
                toAppear: true, _preGameController.ChosenShipIsHorizontal);
        }

        private void PlayerButton_MouseLeave(object sender, EventArgs e)
        {
            _humanPlayerShipController.SetShipVisibility((ShipButton)sender, _preGameController.ChosenShipSize, 
                toAppear: false, _preGameController.ChosenShipIsHorizontal);
        }

        private void ButtonRotate_Click(object sender, EventArgs e)
        {
            _preGameController.RotateShipHorizontality();
            labelShipPlacement.Text = MainFormHumanPlayerStatus.GetShipPositionPlacementLabel(_preGameController.ChosenShipIsHorizontal);
        }

        private void ButtonGameStart_Click(object sender, EventArgs e)
        {
            SetControlsVisibility(visible: false);
            labelStatus.Visible = true;
            labelEnemyField.Visible = true;
            _preGameController.StartGame(checkBoxEnemyRandomMoves.Checked);
        }

        public void ShipButton_TextChanged(object sender, EventArgs e)
        {
            FormButtonController.SetShipButtonFontStyle(sender as ShipButton);
        }

        public void SetLabelStatus(string status, Color color)
        {
            labelStatus.Text = MainFormHumanPlayerStatus.StatusText + status;
            labelStatus.ForeColor = color;
        }

        public void SetComputerMovesToolStripsEnables(bool enable)
        {
            for (int i = 0; i < GameController.ShipSizesAmount; i++)
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
            MarkComputerMovesToolStripMenuItem.Checked = checkBoxMarkEnemyMoves.Checked;
            _gameController.RefreshEnemyMovesMarking();
        }

        private void MarkComputerMovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkComputerMovesToolStripMenuItem.Checked = !MarkComputerMovesToolStripMenuItem.Checked;
            _gameController.RefreshEnemyMovesMarking();
            checkBoxMarkEnemyMoves.Checked = MarkComputerMovesToolStripMenuItem.Checked;
            foreach (ShipButton button in _gameController.HumanPlayer.Field)
                FormButtonController.RefreshShipButtonMarking(button, mark: MarkComputerMovesToolStripMenuItem.Checked);
        }

        public void SetFocus()
        {
            labelYourField.Focus();
        }

        public void SetRestartButtonVisibile()
        {
            buttonRestart.Visible = true;
        }

        private void SetControlsVisibility(bool visible)
        {
            Control[] controlsToSetVisibility = {
                button1x, button2x, button3x,
                button4x, buttonRotate, label1x, label2x, label3x,
                label4x, labelPlaceShips, labelShipsPlaceLeft, progressBar,
                buttonGameStart, labelComputerMovingSpeed,
                comboBoxEnemyMoveSpeed, checkBoxMarkEnemyMoves, panel };
            Label[] labelsToSetOppositeVisibility = { labelStatus, labelEnemyField };
            foreach (Control control in controlsToSetVisibility) control.Visible = visible;
            foreach (Label label in labelsToSetOppositeVisibility) label.Visible = !visible;
        }

        public void ResetControls()
        {
            for (int i = 0; i < GameController.ShipSizesAmount; i++)
            {
                SetStartingShipAmountControlSettings(choosingSizeControlIndex: i);
            }
            SetControlsVisibility(visible: true);
            HumanPlayerStatus.SetStandartLabelStatus();
            progressBar.Value = 0;
            SetShipArrangeButtonEnables(shipsPlaced: false);
            buttonRestart.Visible = false;
            _preGameController.ResetProperties();
        }

        private void SetStartingShipAmountControlSettings(int choosingSizeControlIndex)
        {
            _chooseSizeButtons[choosingSizeControlIndex].Enabled = true;
            ((Label)Controls[GetControlText("label", choosingSizeControlIndex + FieldController.NextIndex)]).Text
                = Convert.ToString(GameController.ShipSizesAmount - choosingSizeControlIndex);
            Button choosingSizeButton = (Button)Controls[GetControlText("button", choosingSizeControlIndex + FieldController.NextIndex)];
            FormButtonController.ButtonColorToStandart(choosingSizeButton);
        }

        private void ButtonArrangeShipsRandomly_Click(object sender, EventArgs e)
        {
            _preGameController.ArrangeHumanPlayerShipsRandomly();
        }

        private void ButtonRestart_Click(object sender, EventArgs e)
        {
            _gameController.RestartGame();
        }

        private void GameRestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameController.RestartGame();
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            labelPrompt.Visible = false;
        }
    }
}