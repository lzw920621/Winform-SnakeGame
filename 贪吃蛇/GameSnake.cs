using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    enum Direction
    {
        Up,
        Down,       
        Left,
        Right,

    }
    class GameSnake
    {
        int[,] map = new int[30, 30];

        List<Node> snake = new List<Node>();

        int moveSpan = 400;//移动一次的时间间隔

        int goals = 0;

        Direction direction;

        Timer timer = new Timer();//定时器

        public GameSnake()
        {
            timer.Interval = moveSpan;
            timer.Tick += TimerTick;//定时事件 触发蛇移动
        }

        void InitialMap()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    map[i, j] = 0;
                }
            }

            map[15, 14] = 2;//蛇头
            snake.Add(new Node(15, 14));
            map[15, 13] = 1;
            snake.Add(new Node(15, 13));
            map[15, 12] = 1;
            snake.Add(new Node(15, 12));
            map[15, 11] = 1;
            snake.Add(new Node(15, 11));
            map[15, 10] = 1;
            snake.Add(new Node(15, 10));
                     
            direction = Direction.Right;//蛇的方向
        }

        void GenerateFood()
        {
            Random rd = new Random();
            int x = rd.Next(0, 30);
            int y = rd.Next(0, 30);
            while(map[x,y]==1)//食物的位置不能和蛇重叠
            {
                x = rd.Next(0, 30);
                y = rd.Next(0, 30);
            }
            map[x, y] = 3;//值3表示食物
        }

        public void Start()
        {
            goals = 0;//分数归零
            snake.Clear();//蛇初始化
            InitialMap();//初始化map
            GenerateFood();//生成食物
            UpdateGrid(map);//更新表格
            UpdateGoals(goals);//更新分数
            timer.Start();//定时器 开启
        }

        public void UserMove(Keys key)//根据用户按下的方向键 改变蛇的运动方向
        {
            switch(key)
            {
                case Keys.Up:
                    direction = (direction == Direction.Down) ? Direction.Down : Direction.Up;
                    break;

                case Keys.Down:
                    direction = (direction == Direction.Up) ? Direction.Up : Direction.Down;
                    break;

                case Keys.Right:
                    direction = (direction == Direction.Left) ? Direction.Left : Direction.Right;
                    break;

                case Keys.Left:
                    direction = (direction == Direction.Right) ? Direction.Right : Direction.Left;
                    break;

                default:break;
            }
        }

        void TimerTick(object sender, EventArgs e)
        {
            SankeMove(direction);
        }

        void SankeMove(Direction direction)
        {
            Node newNode = new Node();
            Node snakeHead = snake.First();//蛇头节点
            Node snakeTail = snake.Last();//蛇尾节点
            switch (direction)
            {
                case Direction.Up:
                    newNode.x = snakeHead.x - 1;
                    newNode.y = snakeHead.y;
                    break;
                case Direction.Down:
                    newNode.x = snakeHead.x + 1;
                    newNode.y = snakeHead.y;
                    break;
                case Direction.Left:
                    newNode.x = snakeHead.x;
                    newNode.y = snakeHead.y - 1;
                    break;
                case Direction.Right:
                    newNode.x = snakeHead.x;
                    newNode.y = snakeHead.y + 1;
                    break;
            }

            if(newNode.x<0||newNode.x>=30||newNode.y<0||newNode.y>=30||(map[newNode.x,newNode.y]==1))//超出map数组范围或者撞到自身 蛇死亡
            {
                SnakeDead();
            }
            else
            {
                if(map[newNode.x,newNode.y]==3)//吃到食物
                {
                    goals++;
                    UpdateGoals(goals);//更新分数

                    //map[snakeTail.x, snakeTail.y] = 1;
                    map[snakeHead.x, snakeHead.y] = 1;
                    map[newNode.x, newNode.y] = 2;

                    //snake.Remove(snakeTail);//去掉蛇尾节点
                    snake.Insert(0, newNode);//新蛇头节点

                    GenerateFood();//生成新的食物
                }
                else
                {                    
                    map[snakeTail.x, snakeTail.y] = 0;
                    map[snakeHead.x, snakeHead.y] = 1;
                    map[newNode.x, newNode.y] = 2;

                    snake.Remove(snakeTail);//去掉蛇尾节点
                    snake.Insert(0, newNode);//新蛇头节点
                }

                UpdateGrid(map);
            }
        }

        void SnakeDead()
        {
            timer.Stop();
            SendMessage("YOU ARE DEAD!!!");
        }

        public event Action<int[,]> UpdateGrid;//更新界面

        public event Action<string> SendMessage;//向界面发送消息

        public event Action<int> UpdateGoals;//更新分数
    }

    
    struct Node
    {
        public int x;
        public int y;
        public Node(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
