using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program{
        // Set game variables
        static int boardWidth = 30;
        static int boardHeight = 20;
        static int score = 0;
        static int speed = 100;
        static bool gameOver = false;
        static List<(int x, int y)> snake = new List<(int x, int y)>();
        static (int x, int y) food;

        static void Main(string[] args){
            // Initialize the game
            InitializeGame();
            
            // Main game loop
            while (!gameOver)
            {
                DrawBoard();
                Input();
                Logic();
                Thread.Sleep(speed);
            }

            // Game Over Message
            Console.Clear();
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Final Score: {score}");
            Console.ReadKey();
        }

        static void InitializeGame(){
            // Snake starting position
            snake.Add((boardWidth / 2, boardHeight / 2));

            // Generate first food
            GenerateFood();
        }

        static void DrawBoard()
        {
            Console.Clear();
            
            // Draw the top border
            for (int i = 0; i < boardWidth + 2; i++)
                Console.Write("#");
            Console.WriteLine();

            // Draw the game area
            for (int y = 0; y < boardHeight; y++)
            {
                Console.Write("#"); // Left border
                for (int x = 0; x < boardWidth; x++)
                {
                    if (snake[0] == (x, y))
                        Console.Write("O"); // Snake head
                    else if (snake.Contains((x, y)))
                        Console.Write("o"); // Snake body
                    else if (food == (x, y))
                        Console.Write("F"); // Food
                    else
                        Console.Write(" ");
                }
                Console.Write("#"); // Right border
                Console.WriteLine();
            }

            // Draw the bottom border
            for (int i = 0; i < boardWidth + 2; i++)
                Console.Write("#");
            Console.WriteLine();

            // Display Score
            Console.WriteLine($"Score: {score}");
        }

        static void Input()
        {
            // Check for user input
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                // Control snake direction
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (snake.Count < 2 || snake[1].y != snake[0].y - 1)
                            ChangeDirection(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        if (snake.Count < 2 || snake[1].y != snake[0].y + 1)
                            ChangeDirection(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (snake.Count < 2 || snake[1].x != snake[0].x - 1)
                            ChangeDirection(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        if (snake.Count < 2 || snake[1].x != snake[0].x + 1)
                            ChangeDirection(1, 0);
                        break;
                }
            }
        }

        static void Logic()
        {
            // Move the snake by adding new head and removing the tail
            var newHead = (snake[0].x + dx, snake[0].y + dy);
            snake.Insert(0, newHead);
            
            // Check for food collision
            if (newHead == food)
            {
                score++;
                GenerateFood();
                // Increase snake length and speed as you score
                if (speed > 10)
                    speed -= 5;
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); // Remove tail if no food eaten
            }

            // Check for border collisions
            if (newHead.Item1 < 0 || newHead.Item1 >= boardWidth || newHead.Item2 < 0 || newHead.Item2 >= boardHeight)
            {
                gameOver = true;
            }

            // Check for snake body collisions
            if (snake.GetRange(1, snake.Count - 1).Contains(newHead))
            {
                gameOver = true;
            }
        }

        static int dx = 1, dy = 0;
        static void ChangeDirection(int x, int y){
            dx = x;
            dy = y;
        }

        static void GenerateFood(){
            Random rnd = new Random();
            food = (rnd.Next(0, boardWidth), rnd.Next(0, boardHeight));
        }
    }
}
