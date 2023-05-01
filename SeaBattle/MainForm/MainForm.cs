using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class MainForm : Form
    {
        private const int ProgressBarMaximumValue = 100;
        public const int NextIndex = 1;
        private const int FirstIndex = FieldController.StartingCoordinate;
        private const int ShipSizesAmount = FieldController.ShipSizesAmount;
        private readonly GameController _gameController;
        private readonly FieldController _fieldController;
        private Button[] _chooseSizeButtons;
        private Label[] _chooseSizeLabels;
        private bool _makeSizeZero = false;
        private int _chosenSize = 0;

        public MainForm()
        {
            InitializeComponent();
            _gameController = new GameController(this);
            _fieldController = new FieldController(this);
            HumanPlayerStatus = new MainFormHumanPlayerStatus(this);
            ButtonController = new MainFormButtonController(this);
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

        public bool EnemyMarkMoves { get => MarkComputerMovesToolStripMenuItem.Checked; }

        public bool ChosenShipIsHorizontal { get; private set; } = false;

        public bool ComputerTurnLabelVisible { get; private set; } = false;

        public GameController GameController { get => _gameController; }

        public MainFormHumanPlayerStatus HumanPlayerStatus { get; }

        public MainFormButtonController ButtonController { get; }

        private void MainForm_Load(object sender, EventArgs e)
        {
            const int mediumSpeedSelectedIndex = 1;
            comboBoxComputerMoveSpeed.SelectedIndex = mediumSpeedSelectedIndex;
            comboBoxComputerMoveSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            _gameController.StartGame();
        }

        public void CreateShipSizeChoosingPanel()
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

        private string GetControlText(string controlName, int index)
        {
            return (controlName + (index) + "x").ToString();
        }

        public void SetButtonsEvents()
        {
            foreach (ShipButton shipButton in _gameController.HumanPlayerField)
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

        private void SetShipSizeChoosingButtonsColors(Button chosenButton, ref int chooseSizeButtonNumber) // what is this ref
        {
            for (int i = 0; i < ShipSizesAmount; i++)
            {
                if (chosenButton == _chooseSizeButtons[i])
                {
                    ChosenSize = i + NextIndex;
                    _chooseSizeButtons[i].BackColor = Color.LightSkyBlue;
                    chooseSizeButtonNumber = i;
                }
                else MainFormButtonController.ButtonColorToStandart(_chooseSizeButtons[i]);
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
            if (!MainFormButtonController.HumanPlayerButtonIsShipPreviewPart(senderShipButton)) return;
            ChangeShipsLeft((Button)Controls[GetControlText("button", ChosenSize)],
                (Label)Controls[GetControlText("label", ChosenSize).ToString()]);
            _gameController.HumanPlayer.SetShip(senderShipButton);
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
                MainFormButtonController.ButtonColorToStandart(selectedButton);
                _makeSizeZero = true;
            }
            else UnableShipsArrangePanel();
        }

        public void UnableShipsArrangePanel()
        {
            foreach (Button button in _chooseSizeButtons)
            {
                MainFormButtonController.ButtonColorToStandart(button);
                button.Enabled = false;
            }
            foreach (Label label in _chooseSizeLabels)
                label.Text = FirstIndex.ToString();
            progressBar.Value = ProgressBarMaximumValue;
            SetShipArrangeButtonEnables(shipsPlaced: true);
            _gameController.HumanPlayer.UnableField();
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
            _fieldController.SetShipVisibility((ShipButton)sender, ChosenSize, toAppear: true, ChosenShipIsHorizontal); // mb chosenshipishor to gamecontroller or fieldcontr
        }

        private void PlayerButton_MouseLeave(object sender, EventArgs e)
        {
            _fieldController.SetShipVisibility((ShipButton)sender, ChosenSize, toAppear: false, ChosenShipIsHorizontal);
        }

        private void ButtonRotate_Click(object sender, EventArgs e)
        {
            ChosenShipIsHorizontal = !ChosenShipIsHorizontal;
            labelShipPlacement.Text = MainFormHumanPlayerStatus.GetShipPositionPlacementLabel(ChosenShipIsHorizontal);
        }

        private void ButtonGameStart_Click(object sender, EventArgs e)
        {
            SetControlsVisibility(visible: false);
            labelStatus.Visible = true;
            labelEnemyField.Visible = true;
            ContextMenuStrip.Items[FirstIndex].Text = "Restart Game";
            _gameController.EnemyStartGame(checkBoxEnemyRandomMoves.Checked);
        }

        public void ShipButton_TextChanged(object sender, EventArgs e)
        {
            MainFormButtonController.SetShipButtonFontStyle(sender as ShipButton);
        }

        public void SetLabelStatus(string status, Color color)
        {
            labelStatus.Text = MainFormHumanPlayerStatus.StatusText + status;
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
            _gameController.RefreshEnemyMovesMarking(checkBoxMarkEnemyMoves.Checked);
            MarkComputerMovesToolStripMenuItem.Checked = checkBoxMarkEnemyMoves.Checked;
        }

        private void MarkComputerMovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkComputerMovesToolStripMenuItem.Checked = !MarkComputerMovesToolStripMenuItem.Checked;
            _gameController.RefreshEnemyMovesMarking(EnemyMarkMoves);
            checkBoxMarkEnemyMoves.Checked = MarkComputerMovesToolStripMenuItem.Checked;
            foreach (ShipButton button in _gameController.HumanPlayer.Field)
                MainFormButtonController.RefreshShipButtonMarking(button, mark: MarkComputerMovesToolStripMenuItem.Checked);
        }

        public void SetFocus()
        {
            labelYourField.Focus();
        }

        public void FinishGame()
        {
            _gameController.FinishGame();
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

        public void ResetControls()
        {
            for (int i = 0; i < FieldController.ShipSizesAmount; i++)
            {
                SetStartingShipAmountControlSettings(choosingSizeControlIndex: i);
            }
            SetControlsVisibility(visible: true);
            HumanPlayerStatus.SetStandartLabelStatus();
            ChosenSize = FirstIndex;
            progressBar.Value = FirstIndex;
            SetShipArrangeButtonEnables(shipsPlaced: false);
            buttonRestart.Visible = false;
            _makeSizeZero = false;
        }

        private void SetStartingShipAmountControlSettings(int choosingSizeControlIndex)
        {
            _chooseSizeButtons[choosingSizeControlIndex].Enabled = true;
            ((Label)Controls[GetControlText("label", choosingSizeControlIndex + NextIndex)]).Text
                = Convert.ToString(ShipSizesAmount - choosingSizeControlIndex);
            Button choosingSizeButton = (Button)Controls[GetControlText("button", choosingSizeControlIndex + NextIndex)];
            MainFormButtonController.ButtonColorToStandart(choosingSizeButton);
        }

        private void ButtonArrangeShipsRandomly_Click(object sender, EventArgs e)
        {
            _gameController.ArrangeHumanPlayerShipsRandomly();
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