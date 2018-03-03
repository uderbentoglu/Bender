using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        string[] directOrder = new string[] { "SOUTH", "EAST", "NORTH", "WEST" };
        string[] inputs = Console.ReadLine().Split(' ');
        int L = int.Parse(inputs[0]);
        int C = int.Parse(inputs[1]);
        Point startPoint = new Point();
        Point t1 = new Point();
        Point t2 = new Point();
        int xCount = 0;
        string[,] map = new string[C, L];
        for (int i = 0; i < L; i++)
        {
            string row = Console.ReadLine();
            for (int j = 0; j < row.Length; j++)
            {
                map[j, i] = row[j].ToString();
                if (row[j] == '@')
                {
                    startPoint.X = j;
                    startPoint.Y = i;
                }

                if (row[j] == 'X')
                {
                    xCount += 1;
                }

                if (row[j] == 'T')
                {
                    if (t1.X == 0 && t1.Y == 0)
                    {
                        t1.X = j;
                        t1.Y = i;
                    }
                    else
                    {
                        t2.X = j;
                        t2.Y = i;
                    }
                }
            }
        }
        StringBuilder result = new StringBuilder();
        string currDirection = "SOUTH";
        Point currPosition = startPoint;
        bool beerMode = false;
        bool isInLoop;
        List<Bender> prevStates = new List<Bender>();
        Bender bender = new Bender()
        {
            X = startPoint.X,
            Y = startPoint.Y,
            BeerMode = beerMode,
            Direction = currDirection,
            DirectOrder = string.Join(",", directOrder),
            XCount = xCount
        };
        prevStates.Add(bender);
        while (true)
        {
            isInLoop = false;
            Point targetPosition = currPosition;
            switch (currDirection)
            {
                case "SOUTH":
                    targetPosition.Y += 1;
                    break;
                case "NORTH":
                    targetPosition.Y -= 1;
                    break;
                case "WEST":
                    targetPosition.X -= 1;
                    break;
                case "EAST":
                    targetPosition.X += 1;
                    break;
                default:
                    break;
            }

            string targetSymbol = map[targetPosition.X, targetPosition.Y];
            if (targetSymbol == "#" || targetSymbol == "X")
            {
                if (targetSymbol == "X" && beerMode)
                {
                    map[targetPosition.X, targetPosition.Y] = " ";
                    xCount -= 1;
                }
                else
                {
                    string symbol = "#";
                    string newDirection = "";
                    int dirIndex = 0;
                    while (symbol == "#" || symbol == "X")
                    {
                        newDirection = directOrder[dirIndex];
                        Point nextPoint = currPosition;
                        switch (newDirection)
                        {
                            case "SOUTH":
                                nextPoint.Y += 1;
                                break;
                            case "NORTH":
                                nextPoint.Y -= 1;
                                break;
                            case "WEST":
                                nextPoint.X -= 1;
                                break;
                            case "EAST":
                                nextPoint.X += 1;
                                break;
                            default:
                                break;
                        }
                        targetPosition = nextPoint;
                        dirIndex += 1;
                        symbol = map[nextPoint.X, nextPoint.Y];
                    }
                    targetSymbol = symbol;
                    currDirection = newDirection;
                }
            }

            result.Append(currDirection + "\n");
            switch (targetSymbol)
            {
                case "S":
                    currDirection = "SOUTH";
                    break;
                case "N":
                    currDirection = "NORTH";
                    break;
                case "E":
                    currDirection = "EAST";
                    break;
                case "W":
                    currDirection = "WEST";
                    break;
                case "I":
                    directOrder = directOrder.Reverse().ToArray();
                    break;
                case "B":
                    beerMode = !beerMode;
                    break;
                case "T":
                    targetPosition = targetPosition == t1 ? t2 : t1;
                    break;
                default:
                    break;
            }

            currPosition = targetPosition;
            if (targetSymbol == "$")
            {
                break;
            }
            bender = new Bender
            {
                X = targetPosition.X,
                Y = targetPosition.Y,
                Direction = currDirection,
                BeerMode = beerMode,
                DirectOrder = string.Join(",", directOrder),
                XCount = xCount
            };
            var benderComparer = new BenderComparer();
            foreach (var item in prevStates)
            {
                if (benderComparer.Equals(bender, item))
                {
                    isInLoop = true;
                    break;
                }
            }
            prevStates.Add(bender);
            if (isInLoop)
            {
                break;
            }
        }
        Console.WriteLine(isInLoop ? "LOOP" : result.ToString().Trim());
    }
}

public class Bender
{
    public int X { get; set; }
    public int Y { get; set; }

    public string Direction { get; set; }
    public bool BeerMode { get; set; }
    public string DirectOrder { get; set; }
    public int XCount { get; set; }
}

class BenderComparer : IEqualityComparer<Bender>
{
    public bool Equals(Bender x, Bender y)
    {
        if (x == null || y == null)
            return false;

        return (x.X == y.X && x.Y == y.Y
            && x.Direction == y.Direction
            && x.BeerMode == y.BeerMode
            && x.DirectOrder == y.DirectOrder
            && x.XCount == y.XCount);
    }

    public int GetHashCode(Bender obj)
    {
        return obj.GetHashCode();
    }
}
