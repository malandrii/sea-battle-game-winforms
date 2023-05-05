using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    public class HumanPlayerShipController
    {
        private readonly MainForm _mainForm;
        private static readonly Color HumanPlayerShipPreview = Color.LightBlue;
        public static readonly Color HumanPlayerShip = Color.Blue;

        public HumanPlayerShipController(MainForm mainForm)
        { 
            _mainForm = mainForm;
        }

        public void SetShipVisibility(ShipButton button, int chosenSize, bool toAppear, bool chosenShipIsHorizontal)
        {
            int previousSize = chosenSize - FieldController.NextIndex;
            bool spaceIsFree = chosenShipIsHorizontal ?
                (button.X + previousSize) < FieldController.FieldSize :
                (button.Y - previousSize) >= FieldController.StartingCoordinate;
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: false);
            AppearShipPreview(button, chosenSize, ref spaceIsFree, toAppear, chosenShipIsHorizontal, spaceIsFreeSet: true);
        }

        private void AppearShipPreview(ShipButton button, int chosenSize, ref bool spaceIsFree, bool toAppear,
            bool chosenShipIsHorizontal, bool spaceIsFreeSet)
        {
            int x = button.X, y = button.Y;
            if (!spaceIsFree) return;
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
                MainFormButtonController.ButtonColorToStandart(shipButton);
        }

        private static void ColorShipPartPreview(ShipButton shipPart)
        {
            MainFormButtonController.SetShipButtonColor(shipPart, HumanPlayerShipPreview);
        }

        public static void ColorShipPart(ShipButton shipPart)
        {
            MainFormButtonController.SetShipButtonColor(shipPart, HumanPlayerShip);
        }

        public static void ColorShip(List<ShipButton> shipParts)
        {
            foreach (ShipButton shipPart in shipParts)
                ColorShipPart(shipPart);
        }

        public static bool ButtonIsShipPreviewPart(ShipButton chosenButton)
        {
            return chosenButton.BackColor == HumanPlayerShipPreview;
        }
    }
}
