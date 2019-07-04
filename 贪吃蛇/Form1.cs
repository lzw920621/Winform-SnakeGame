using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public partial class Form1 : Form
    {
        GameSnake gameSnake = new GameSnake();

        public Form1()
        {
            InitializeComponent();

        }

        void InitialGrid()//初始化表格
        {
            this.dataGridView1.Size = new Size(600, 600);//长宽
           
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowDrop = false;
            this.dataGridView1.AllowUserToOrderColumns = false;
            this.dataGridView1.AllowUserToResizeColumns = false;//禁止调整列的大小
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ReadOnly = true;

            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = ScrollBars.None;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;//设置表格元素选中后背景色设为白色 
            dataGridView1.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Rows.Add(30);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Height = 20;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 20;
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            InitialGrid();
            gameSnake.SendMessage += this.MessageCallback;
            gameSnake.UpdateGrid += this.UpdateGridCallback;
            gameSnake.UpdateGoals += this.UpdateGoals;
            this.MoveEvent += gameSnake.UserMove;

            gameSnake.Start();//开始游戏
        }

        protected override CreateParams CreateParams//解决控件加载时的闪烁问题
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        //protected override void WndProc(ref Message m)//解决控件更新时的闪烁问题
        //{
        //    if (m.Msg == 0x0014) // 禁掉清除背景消息
        //        return;
        //    base.WndProc(ref m);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//取消方向键对控件的焦点的控件，用自己自定义的函数处理各个方向键的处理函数
        {
            if (keyData == Keys.Up || keyData == Keys.Down ||
            keyData == Keys.Left || keyData == Keys.Right)
            {
                MoveEvent(keyData);
                return true;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
        }

        event Action<Keys> MoveEvent;//移动事件

        void UpdateGridCallback(int[,] map)//回调函数 更新表格
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if(map[i,j]==1)//蛇的身体
                    {
                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Blue;
                    }
                    else if(map[i,j]==2)//蛇头
                    {
                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Brown;
                    }
                    else if (map[i, j] == 3)//食物
                    {
                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                    }
                }
            }
        }

        void MessageCallback(string msg)//回调函数 业务层的消息
        {
            MessageBox.Show(msg);
        }

        void UpdateGoals(int goals)
        {
            lb_Goals.Text = goals+"";
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            gameSnake.Start();//开始游戏
        }
    }
}
