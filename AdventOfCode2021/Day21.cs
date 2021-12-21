using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
    internal static class Day21
    {
        private static IEnumerator<int> DeterministicDice()
        {
            IEnumerable<int> inner()
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    yield return i % 100 + 1;
                }
            }

            return inner().GetEnumerator();
        }

        internal static void Run()
        {
            // What's this??!?!? No input string??!?! This is a first!

            var dice = DeterministicDice();

            var player1 = new Player(4, dice);
            var player2 = new Player(8, dice);

            player1 = new Player(6, dice);
            player2 = new Player(3, dice);


            for (int rollNumber = 0; rollNumber < int.MaxValue; rollNumber += 3 * 2)
            {
                player1.ApplyDiceRoll();
                if (player1.Score >= 1000)
                {
                    // hack. this loop consumes 6 rolls for player 1 + 2.
                    // player 1 could rely on the loop counter, we know we have to include
                    // player 1's 3 rolls before us if we win
                    rollNumber += 3;

                    Console.WriteLine($"{nameof(player1)} scored {player1.Score} in {rollNumber} rolls");
                    Console.WriteLine($"{player2.Score} × {rollNumber} = {player2.Score * rollNumber}");
                    break;
                }

                player2.ApplyDiceRoll();
                if (player2.Score >= 1000)
                {
                    Console.WriteLine($"{nameof(player2)} scored {player2.Score} in {rollNumber} rolls");
                    Console.WriteLine($"{player1.Score} × {rollNumber} = {player1.Score * rollNumber}");
                    break;
                }
            }
        }

        private record Player
        {
            public Player(int startingPosition, IEnumerator<int> dice)
            {
                Position = startingPosition;
                this.dice = dice;
            }

            private readonly IEnumerator<int> dice;

            public int Position { get; private set; }
            public int Score { get; private set; }

            public void ApplyDiceRoll()
            {
                var positionsToMove = Roll();
                Position = (Position - 1 + positionsToMove) % 10 + 1;
                Score += Position;
            }

            private int Roll()
            {
                int result = 0;
                for (int i = 0; i < 3; i++)
                {
                    dice.MoveNext();
                    result += dice.Current;
                }

                return result;
            }
        }
    }
}
