using System;
using System.Drawing;
using System.Collections.Generic;

namespace SeaBattle
{
    public class Player
    {
        private static readonly Random _random = new Random();
        protected MainForm _mainForm;
        protected FieldController _fieldController;
        protected int _fieldSize;
        protected int _markingOffset;
        public int ShipPartsAlive { get; set; } = 20;
        public ShipButton[,] Field { get; set; }
        
        public Player(MainForm mainForm)
        {
            _mainForm = mainForm;
            _fieldController = new FieldController(_mainForm);
            _fieldSize = _fieldController.FieldSize;
        }

        public virtual void DeclareField()
        {
            _fieldController.CreateField(Field, _markingOffset);
        }

        public void MakeCoordinatesRandom(ref int x, ref int y)
        {
            x = _random.Next(0, _fieldSize);
            y = _random.Next(0, _fieldSize);
        }

        public void SpawnRandomShips()
        {
            for (int specificSizeShipSpawnAmount = _mainForm.ShipSizesAmount;
                specificSizeShipSpawnAmount > 0; specificSizeShipSpawnAmount--)
            {
                int sizeOfShip = 1;
                for (int i = 0; i < specificSizeShipSpawnAmount; i++)
                {
                    SpawnShip(sizeOfShip);
                    sizeOfShip++;
                }
            }
        }

        private void CheckForSameLocatedShips(ref bool sameCoordinates, int size,
            List<Point> coordinateCopies)
        {
            int minimumShipSizeToCheck = 2, nextIndex = MainForm.NextIndex;
            if (size > minimumShipSizeToCheck)
            {
                for (int specificShipPart = 0; specificShipPart < coordinateCopies.Count - nextIndex;
                        specificShipPart++)
                {
                    for (int nextShipPart = specificShipPart + 1;
                        nextShipPart < coordinateCopies.Count; nextShipPart++)
                    {
                        sameCoordinates = 
                            coordinateCopies[specificShipPart].X == coordinateCopies[nextShipPart].X
                         && coordinateCopies[specificShipPart].Y == coordinateCopies[nextShipPart].Y;
                        if (sameCoordinates) return;
                    }
                }
            }
        }

        public void ShiftCoordinates(bool isHorizontal, bool Sum, ref int x, ref int y)
        {
            if (isHorizontal) ShiftCoordinate(ref x, Sum);
            else ShiftCoordinate(ref y, Sum);
        }

        private void ShiftCoordinate(ref int coordinate, bool Sum)
        {
            coordinate = Sum ? ++coordinate : --coordinate;
        }

        private void SpawnShip(int size)
        {
            int x = 0, y = 0,
                oneSquareShipSize = 1;
            bool canMakeShip = true, sameCoordinates = false,
                 fiftyfiftyChance = _random.Next(1, 3) == 1,
                 isHorizontal = fiftyfiftyChance;
            MakeCoordinatesRandom(ref x, ref y);
            List<Point> coordinateCopies = new List<Point> { new Point(x, y) };
            if (size > oneSquareShipSize)
            {
                for (int i = oneSquareShipSize; i < size; i++)
                {
                    int sizeToCheck = _fieldSize - size - oneSquareShipSize,
                        coordinateToCheck = isHorizontal ? x : y;
                    bool insideField = coordinateToCheck < sizeToCheck;
                    ShiftCoordinates(isHorizontal, insideField, ref x, ref y);
                    coordinateCopies.Add(new Point(x, y));
                }
            }
            for (int i = 0; i < size; i++)
                if (!Field[coordinateCopies[i].X, coordinateCopies[i].Y].CanMakeShip())
                    canMakeShip = false;
            CheckForSameLocatedShips(ref sameCoordinates, size, coordinateCopies);
            if (canMakeShip && !sameCoordinates) DeclareShip(size, coordinateCopies);
            else SpawnShip(size);
        }

        private void DeclareShip(int size, List<Point> coordinatesCopies)
        {
            List<ShipButton> shipButtonList = new List<ShipButton>();
            for (int i = 0; i < size; i++) 
                shipButtonList.Add(Field[coordinatesCopies[i].X, coordinatesCopies[i].Y]);
            Ship ship = new Ship(shipButtonList);
            for (int i = 0; i < size; i++) 
                MakeShipPart(coordinatesCopies[i].X, coordinatesCopies[i].Y, ship);
        }

        public bool CheckDeath()
        {
            bool isDead = ShipPartsAlive == 0;
            if (isDead) _mainForm.FinishGame();
            return isDead;
        }

        protected void MakeShipPart(int x, int y, Ship ship)
        {
            Field[x, y].IsShipPart = true;
            Field[x, y].ShipFrom = ship;
            OccupyAroundShip(x, y);
        }

        private void OccupyAroundShip(int x, int y)
        {
            foreach (Point point in _fieldController.GetCoordinatesAround(x, y))
                Field[point.X, point.Y].Mark(x, y, Field);
        }
    }
}