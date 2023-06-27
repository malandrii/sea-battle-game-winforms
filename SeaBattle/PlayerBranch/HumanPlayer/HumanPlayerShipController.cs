using System.Drawing;

namespace SeaBattle
{
    public class HumanPlayerShipController
    {
        private readonly MainForm _mainForm;
        private static readonly Color HumanPlayerShipPreview = Color.LightBlue;
        public static readonly Color HumanPlayerShip = Color.Blue;

        public HumanPlayerShipController(MainForm mainForm) => _mainForm = mainForm;

        public void SetShipVisibility(ShipButton button, int chosenSize, bool toAppear, bool chosenShipIsHorizontal)
        {
            int previousSize = chosenSize - FieldController.NextIndex;
            bool spaceIsFree = chosenShipIsHorizontal ?
                (button.X + previousSize) < Field.Size :
                (button.Y - previousSize) >= Field.StartingCoordinate;
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: false);
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: true);
        }

        private void AppearShipPreview(ShipButton button, int chosenSize, ref bool spaceIsFree, bool toAppear,
            bool chosenShipIsHorizontal, bool spaceIsFreeSet)
        {
            if (!spaceIsFree) return;

            int x = button.X, y = button.Y;
            for (int i = 0; i < chosenSize; i++)
            {
                if (!spaceIsFreeSet)
                {
                    spaceIsFree = _mainForm.GameController.HumanPlayer.Field[x, y].Enabled;
                }
                else
                {
                    AppearShipPartPreview(_mainForm.GameController.HumanPlayer.Field[x, y], toAppear);
                }
                Player.ShiftCoordinates(chosenShipIsHorizontal, chosenShipIsHorizontal, ref x, ref y);
            }
        }

        public static void AppearShipPartPreview(ShipButton shipButton, bool toAppear)
        {
            if (toAppear)
                ColorShipPartPreview(shipButton);
            else
                FormButtonController.ButtonColorToStandart(shipButton);
        }

        private static void ColorShipPartPreview(ShipButton shipPart)
            => FormButtonController.SetShipButtonColor(shipPart, HumanPlayerShipPreview);

        public static void ColorShipPart(ShipButton shipPart)
            => FormButtonController.SetShipButtonColor(shipPart, HumanPlayerShip);

        public static void ColorShip(Ship ship)
        {
            foreach (ShipButton shipPart in ship)
                ColorShipPart(shipPart);
        }

        public static bool ButtonIsShipPreviewPart(ShipButton chosenButton)
            => chosenButton.BackColor == HumanPlayerShipPreview;
    }
}
