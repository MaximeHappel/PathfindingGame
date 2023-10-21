using System.Drawing;
using System.Globalization;
using System;

namespace AgentMap2
{
    public enum MenuOption
    {
        AddGuard = 'g',
        AddFence = 'f',
        AddSensor = 's',
        AddCamera = 'c',
        AddMagicCross = 't',
        PrintSafeDirections = 'd',
        DrawMap = 'm',
        FindPath = 'p',
        Exit = 'x',
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a new map to manage obstacles and path.
            Map map = new();
            // Define the main menu text to be displayed to the user.
            string menuText = $"Select one of the following options\ng) Add 'Guard' obstacle\nf) Add 'Fence' obstacle\ns) Add 'Sensor' obstacle\nc) Add 'Camera' obstacle\nt) Add 'MagicCross' obstacle\nd) Show safe directions\nm) Display obstacle map\np) Find safe path\nx) Exit\nEnter code: ";
            // Flag if the display text is to be shown
            bool displayMenu = true;
            // Loop the program until user inputs 'x'
            while (true)
            {
                if (displayMenu)
                {
                    Console.WriteLine(menuText);
                }

                try
                {
                    displayMenu = true;
                    // Read the user input as a char value
                    char code = Char.ToLower(Char.Parse(Console.ReadLine() ?? ""));
                    switch (code)
                    {
                        case (char)MenuOption.AddGuard:
                            AddGuard(); // Call a function to add a 'Guard' obstacle.
                            break;
                        case (char)MenuOption.AddFence:
                            AddFence(); // Call a function to add a 'Fence' obstacle.
                            break;
                        case (char)MenuOption.AddSensor:
                            AddSensor(); // Call a function to add a 'Sensor' obstacle.
                            break;
                        case (char)MenuOption.AddCamera:
                            AddCamera(); // Call a function to add a 'Camera' obstacle.
                            break;
                        case (char)MenuOption.AddMagicCross:
                            AddMagicCross(); // Call a function to add a 'Magic Cross' obstacle.
                            break;
                        case (char)MenuOption.PrintSafeDirections:
                            PrintSafeDirections(); // Call a function to print safe directions.
                            break;
                        case (char)MenuOption.DrawMap:
                            DrawMap(); // Call a function to display the obstacle map.
                            break;
                        case (char)MenuOption.FindPath:
                            FindPath(); // Call a function to find a safe path.
                            break;
                        case (char)MenuOption.Exit:
                            return; // Exit the program.
                        default:
                            throw new Exception("Invalid option: Option does not exist");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Enter Code:");
                    // Set flag to false, so the Menu text does not show when this Exception occurs
                    displayMenu = false;
                }

            }
            /// <summary>
            /// Add a 'Guard' obstacle to the map at a specified location.
            /// </summary>
            void AddGuard()
            {
                map.obstacles.Add(new Guard(ReadPosition("Enter the guard's location (X,Y):")));
            }
            /// <summary>
            /// Add a 'Fence' obstacle to the map with specified starting and ending locations.
            /// </summary>
            void AddFence()
            {
                while (true)
                {
                    try
                    {                       
                        map.obstacles.Add(new Fence(ReadPosition("Enter the location where the fence starts (X,Y):"), ReadPosition("Enter the location where the fence ends (X,Y):")));
                        return;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Invalid Input: fence can only be vertical or horizontal");
                        continue;
                    }
                }
            }
            /// <summary>
            /// Add a 'Sensor' obstacle to the map at a specified location with a specified range.
            /// </summary>
            void AddSensor()
            {
                Point position = ReadPosition("Enter the sensor's location (X,Y):");
                Console.WriteLine("Enter the sensor's range (in klicks):");
                double range = double.Parse(Console.ReadLine() ?? "0.0", CultureInfo.InvariantCulture);
                map.obstacles.Add(new Sensor(position, range));
            }
            /// <summary>
            /// Add a 'Camera' obstacle to the map at a specified location with a specified direction.
            /// </summary>
            void AddCamera()
            {

                Point position = ReadPosition("Enter the camera's location (X,Y):");
                Console.WriteLine("Enter the direction the camera is facing (n, s, e or w):");
                char dir = Char.Parse(Console.ReadLine() ?? "");
                map.obstacles.Add(new Camera(position, dir));

            }
            /// <summary>
            /// Add a 'Magic Cross' obstacle to the map at a specified location.
            /// </summary>
            void AddMagicCross()
            {
                map.obstacles.Add(new MagicCross(ReadPosition("Enter the Cross location (X,Y):")));
            }
            /// <summary>
            /// Print safe directions from a specified location.
            /// </summary>
            void PrintSafeDirections()
            {
                map.PrintSafeDirections(ReadPosition("Enter your current location (X,Y):"));
            }
            /// <summary>
            /// Draw the map within specified boundaries.
            /// </summary>
            void DrawMap()
            {
                while (true)
                {
                   Point TopLeftCell = ReadPosition("Enter the location of the top-left cell of the map (X,Y):");
                   Point BottomRightCell = ReadPosition("Enter the location of the bottom-right cell of the map (X,Y):");
                    try
                    {
                        map.DrawMap(TopLeftCell, BottomRightCell);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid map specification.");
                    }
                }
            }
            /// <summary>
            /// Find a safe path from a starting location to a target location using the A* search algorithm.
            /// </summary>
            void FindPath()
            {
                // Create a pathfinder object
                Pathfinding pathfinder = new(map);
                Point start = ReadPosition("Enter your current location (X,Y):");
                Point target = ReadPosition("Enter the location of your objective(X, Y):");
                try
                {
                    // Print the path 
                    Console.WriteLine(pathfinder.PathToString(pathfinder.FindPath(start, target)));
                }

                catch (ArgumentException)
                {
                    Console.WriteLine("Agent, you are already at the objective.");
                    return;
                }
                catch (PathfindingTimeoutException)
                {
                    Console.WriteLine("Can't find safe path");
                }

            }
            /// <summary>
            /// Read and parse a position (X, Y) from the user, following the given prompt.
            /// </summary>
            /// <param name="prompt">The message to display to the user as a prompt for input.</param>
            /// <returns>A Point object representing the parsed X and Y coordinates.</returns>
            Point ReadPosition(string prompt)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine(prompt);
                        string input = Console.ReadLine() ?? "";
                        Point result;
                        // Get the X and Y coordinates
                        string[] values = input.Split(',');
                        if (values.Length != 2)
                        {
                            throw new ArgumentException("Input must contain exactly two values.");
                        }
                        if (!int.TryParse(values[0], out int x) || !int.TryParse(values[1], out int y))
                        {
                            throw new FormatException("Input values must be valid integers.");
                        }
                        result = new Point(Int32.Parse(values[0]), Int32.Parse(values[1]));
                        return result;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input");
                    }
                }
            }

        }
    }
}