using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeansClustering
{
    class KMeans
    {
        private List<Node> nodes;
        private List<Cluster> clusters;
        private int k, smallestX, biggestX, smallestY, biggestY;
        private Random random;
        public KMeans(List<Node> nodes, int k)
        {
            this.nodes=nodes;
            this.k=k;
            random=new Random();
            clusters=new List<Cluster>(k);
            findBiggestAndSmallest();
        }
        private void findBiggestAndSmallest()
        {
            smallestX = nodes[0].X;
            biggestX = nodes[0].X;
            smallestY = nodes[0].Y;
            biggestY = nodes[0].Y;
            for (int i = 1; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (smallestX > node.X)
                    smallestX = node.X;
                if (biggestX < node.X)
                    biggestX = node.X;
                if (smallestY > node.Y)
                    smallestY = node.Y;
                if (biggestY < node.Y)
                    biggestY = node.Y;
            }
        }
        public List<Cluster> run()
        {
            for(int i=0;i<k;i++)
            {
                int centerX=random.Next(smallestX,biggestX);
                int centerY=random.Next(smallestY,biggestY);
                Cluster cluster=new Cluster(centerX,centerY);
                clusters.Add(cluster);
            }
            bool hasChanged;
            do
            {
                hasChanged=false;
                foreach (Node node in nodes)
                {
                    Cluster closestCluster=ClosestCluster(node);
                    Cluster clusterOfNode=ClusterOfNode(node);
                    if(clusterOfNode!=null&&closestCluster!=clusterOfNode)
                    {
                        clusterOfNode.Remove(node);
                        closestCluster.Add(node);
                    }
                    else if(clusterOfNode==null)
                        closestCluster.Add(node);
                }
                foreach (Cluster cluster in clusters)
                {
                    cluster.ReCalculateCenter();
                    if(cluster.HasChanged)
                        hasChanged=true;
                }
                foreach (Cluster cluster in clusters)
                    cluster.HasChanged=false;
            }
            while(hasChanged);
            return clusters;
        }
        private Cluster ClusterOfNode(Node node)
        {
            foreach (Cluster cluster in clusters)
            {
                if(cluster.Contains(node))
                    return cluster;
            }
            return null;
        }
        private Cluster ClosestCluster(Node node)
        {
            Cluster closestCluster = clusters[0];
            double closestDistance = Distance(node, closestCluster);
            for (int i = 1; i < clusters.Count; i++)
            {
                Cluster cluster = clusters[i];
                double distance = Distance(node, cluster);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCluster = cluster;
                }
            }
            return closestCluster;
        }
        private double Distance(Node node, Cluster cluster)
        {
            double x = Math.Abs(node.X - cluster.CenterX);
            double y = Math.Abs(node.Y - cluster.CenterY);
            return Math.Sqrt(x * x + y * y);
        }
    }
}
