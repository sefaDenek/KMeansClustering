using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMeansClustering
{
    public partial class Form1 : Form
    {
        private bool isDragged = false;
        private Point firstPoint;
        private Random random;
        private List<Button> buttons;
        private List<ClusterCenterView> clusterCenterViews;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
            buttons = new List<Button>();
            clusterCenterViews = new List<ClusterCenterView>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ClusterCenterView center in clusterCenterViews)
                panel1.Controls.Remove(center);
            clusterCenterViews=new List<ClusterCenterView>();
            List<Node> nodes=new List<Node>();
            foreach (Button button in buttons)
            {
                Node node=new Node(button.Location.X,button.Location.Y);
                nodes.Add(node);
            }
            int k=Convert.ToInt32(numericUpDown1.Value);
            KMeans kMeans=new KMeans(nodes,k);
            List<Cluster> clusters=kMeans.run();
            Color[] colors={Color.Red,Color.Blue,Color.Green,Color.Yellow,Color.Black};
            for(int i=0;i<clusters.Count;i++)
            {
                Cluster cluster=clusters[i];
                ClusterCenterView center = new ClusterCenterView();
                center.Location = new Point(cluster.CenterX, cluster.CenterY);
                panel1.Controls.Add(center);
                clusterCenterViews.Add(center);
                foreach (Node node in cluster.Nodes)
                {
                    Button button=getNode(node.X,node.Y);
                    button.BackColor = colors[i];
                }
            }
        }
        private Button getNode(int x,int y)
        {
            foreach (Button button in buttons)
            {
                if(button.Location.X==x&&button.Location.Y==y)
                    return button;
            }
            return null;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int numberOfButtons = Convert.ToInt32(numericUpDown2.Value);
            for (int i = 0; i < numberOfButtons; i++)
            {
                int x = random.Next(panel1.Width-10);
                int y = random.Next(panel1.Height-10);
                createButton(x, y);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            createButton(e.X,e.Y);
        }

        private void createButton(int x, int y)
        {
            Button button = new Button();
            button.Location = new Point(x, y);
            button.Size = new System.Drawing.Size(15,15);
            button.MouseDown += button_MouseDown;
            button.MouseMove+=button_MouseMove;
            button.MouseUp+=button_MouseUp;
            panel1.Controls.Add(button);
            buttons.Add(button);
        }

        void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                panel1.Controls.Remove((Button)sender);
                buttons.Remove((Button)sender);
            }
            else if (e.Button == MouseButtons.Left)
            {
                Button button = (Button)sender;
                isDragged = true;
                Point ptStartPosition = button.PointToScreen(new Point(e.X, e.Y));
                firstPoint = new Point();
                firstPoint.X = button.Location.X - ptStartPosition.X;
                firstPoint.Y = button.Location.Y - ptStartPosition.Y;
            }
            else
                isDragged = false;
        }
        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Button button = (Button)sender;
                Point newPoint = button.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(firstPoint);
                button.Location = newPoint;
            }
        }

        private void button_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttons.Clear();
            panel1.Controls.Clear();
        }
    }
}
