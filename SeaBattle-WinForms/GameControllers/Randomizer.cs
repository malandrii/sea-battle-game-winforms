using System;
using System.Collections.Generic;

namespace SeaBattle
{
    public static class Randomizer
    {
        public static Random _random = new Random(); // stackoverflow if not a single static random

        public static bool FiftyFifty => _random.Next(1, 3) == 1;

        public static void MakeCoordinatesRandom(ref int x, ref int y)
        {
            x = _random.Next(0, Field.Size);
            y = _random.Next(0, Field.Size);
        }

        public static void GetRandomCoordinateAroundShip(out int x, out int y, List<ShipButton> buttonsAround)
        {
            ShipButton randomButtonAround = buttonsAround[new Random().Next(0, buttonsAround.Count)];
            x = randomButtonAround.X;
            y = randomButtonAround.Y;
            buttonsAround.Remove(randomButtonAround);
        }
    }
}
