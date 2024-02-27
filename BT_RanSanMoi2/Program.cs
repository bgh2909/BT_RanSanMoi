using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading;

class Program
{
    // Kích thước map trò chơi
    public static int width = 20;
    public static int height = 20;

    // Khởi tạo các biến khai báo tọa độ của rắn và mồi
    public static int[] snakeX = new int[100];
    public static int[] snakeY = new int[100];
    public static int snakeLength; //khai báo biến độ dài của rắn
    public static int fruitX; //tọa độ mồi
    public static int fruitY; // tọa độ mồi

    // Hướng di chuyển của rắn
    public static string direction = "RIGHT";

    // Khởi tại biến tính điểm số
    public static int score;

    // Bool kiểm tra trạng thái của trò chơi
    public static bool gameOver;

    public static void Main(string[] args)
    {
        Setup();
        while (!gameOver)
        {
            Draw();
            Input();
            Logic();
            Thread.Sleep(100); // Delay để trò chơi không quá nhanh
        }

        //thông báo sau khi kết thúc trò chơi
        Console.Clear();
        Console.WriteLine("Game Over");
        Console.WriteLine();
        Console.WriteLine("Your score: " + score);
        Console.ReadKey();
        
    }

    public static void Setup()
    {
        Console.CursorVisible = false; // Ẩn con trỏ
        gameOver = false;
        direction = "RIGHT"; // hướng di chuyển khởi đầu 
        snakeLength = 1;
        //điểm xuất phát của rắn ở giữa màn hình trò chơi
        snakeX[0] = width / 2; 
        snakeY[0] = height / 2;
        score = 0; 
        GenerateFruit(); //khởi tạo mồi 
    }

    public static void Draw()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red; // Đặt màu cho tường
        //Tạo tường cho bản đồ trò chơi
        for (int i = 0; i < width + 2; i++) //vẽ trường trên
        {
            Console.Write("#");
        }
        Console.WriteLine();

        for (int i = 0; i < height; i++) // vẽ từng dòng trong bản đồ
        {
            for (int j = 0; j < width; j++)
            {
                if (j == 0) //vẽ tường trái
                    Console.Write("#");

                if (i == snakeY[0] && j == snakeX[0]) //vẽ đầu rắn
                {
                    
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("O");
                    Console.ForegroundColor = ConsoleColor.Red; // Đặt lại màu cho tường
                }
                else if (i == fruitY && j == fruitX) //vẽ mồi 
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Đặt màu cho mồi
                    Console.Write("@");
                    Console.ForegroundColor = ConsoleColor.Red; // Đặt lại màu cho tường
                }
                else
                {
                    bool print = false;
                    for (int k = 1; k < snakeLength; k++) //vẽ thân rắn
                    {
                        if (snakeX[k] == j && snakeY[k] == i)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("o");
                            print = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                    if (!print) // vẽ khoảng trống
                        Console.Write(" ");
                }

                if (j == width - 1) //vẽ tường phải
                    Console.Write("#");
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.Red; // Đặt lại màu cho tường
        for (int i = 0; i < width + 2; i++) // vẽ tường dưới
        {
            Console.Write("#"); 
        }
            Console.WriteLine();
       Console.WriteLine("Score: " + score);
    }

    public static void Input() //nhập hướng di chuyển từ bàn phím 
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    direction = "LEFT";
                    break;
                case ConsoleKey.RightArrow:
                    direction = "RIGHT";
                    break;
                case ConsoleKey.UpArrow:
                    direction = "UP";
                    break;
                case ConsoleKey.DownArrow:
                    direction = "DOWN";
                    break;
            }
        }
    }

    public static void Logic() // Cập nhật vị trí của rắn và mồi, kiểm tra va chạm và cập nhật điểm số
    {
        // Cập nhật vị trí của rắn:
        //vị trí đầu rắn
        int prevX = snakeX[0];
        int prevY = snakeY[0];
        int prev2X, prev2Y;
        snakeX[0] = snakeX[0] + (direction == "RIGHT" ? 1 : direction == "LEFT" ? -1 : 0);
        snakeY[0] = snakeY[0] + (direction == "DOWN" ? 1 : direction == "UP" ? -1 : 0);

        if (snakeX[0] == fruitX && snakeY[0] == fruitY) //kiểm tra vị trí đầu rắn trùng với vị trí mồi
        {
            score += 10; //cập nhật điểm số
            snakeLength++; //cập nhật độ dài rắn
            GenerateFruit(); //khởi tạo mồi mới
        }

        for (int i = 1; i < snakeLength; i++) //vòng lặp cập nhật vị trí thân rắn
        {
            prev2X = snakeX[i];
            prev2Y = snakeY[i];
            snakeX[i] = prevX;
            snakeY[i] = prevY;
            prevX = prev2X;
            prevY = prev2Y;
        }

        if (snakeX[0] < 0 || snakeX[0] >= width || snakeY[0] < 0 || snakeY[0] >= height) // kiểm tra điều kiệu va trạm với tường
            gameOver = true;

        for (int i = 1; i < snakeLength; i++) //kiểm tra điều kiện rắn va chạm với thân
        {
            if (snakeX[i] == snakeX[0] && snakeY[i] == snakeY[0])
                gameOver = true;
        }
    }

    public static void GenerateFruit() //tạo mồi ngẫu nhiên trên map
    {
        Random rnd = new Random();
        fruitX = rnd.Next(1, width);
        fruitY = rnd.Next(1, height);
    }
}
