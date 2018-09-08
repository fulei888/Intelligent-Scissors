using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;

namespace VisualIntelligentScissors
{
    public class DijkstraScissors : Scissors
    {
        Pen redpen = new Pen(Color.Red);
        Pen yellowpen = new Pen(Color.Yellow);
        public DijkstraScissors() { }
        /// <summary>
        /// constructor for intelligent scissors. 
        /// </summary>
        /// <param name="image">the image you are oging to segment.  has methods for getting gradient information</param>
        /// <param name="overlay">an overlay on which you can draw stuff by setting pixels.</param>
		public DijkstraScissors(GrayBitmap image, Bitmap overlay) : base(image, overlay) { }

        // this is the class you need to implement in CS 312

        /// <summary>
        /// this is the class you implement in CS 312. 
        /// </summary>
        /// <param name="points">these are the segmentation points from the pgm file.</param>
        /// <param name="pen">this is a pen you can use to draw on the overlay</param>
		public override void FindSegmentation(IList<Point> points, Pen pen)
        {
            if (Image == null) throw new InvalidOperationException("Set Image property first.");
            // this is the entry point for this class when the button is clicked
            // to do the image segmentation with intelligent scissors.
            Program.MainForm.RefreshImage();
            // PriorityQueue p = new PriorityQueue();
            // Dictionary<Point, int> dictionary = new Dictionary<Point, int>();

            CircleStartPoints(points);
            for (int i = 0; i < points.Count; i++)
            // for(int i=0; i<8; i++)
            {
                
                bool stop = false;
                Point pp = new Point();
                pp = points[i];
                Node shortpath = null;
                shortpath = new Node(shortpath, pp, GetPixelWeight(pp));
               Dictionary<Point, int> dictionary = new Dictionary<Point, int>();
                HashSet<Point> diction2 = new HashSet<Point>();
                PriorityQueue p = new PriorityQueue();
                diction2.Add(pp);
                p.Enqueue(shortpath, GetPixelWeight(pp));
                //when we find the end point, it will stop
                while (stop == false)
                {
                    diction2.Remove(pp);
                    shortpath = (Node)p.Dequeue();
                    pp = shortpath.current;
                    dictionary.Add(pp, GetPixelWeight(pp));
                    List<Point> aroundp = new List<Point>();

                    Point tl = new Point(pp.X - 1, pp.Y - 1);

                    Point tm = new Point(pp.X, pp.Y - 1);
                    Point tr = new Point(pp.X + 1, pp.Y - 1);
                    Point ml = new Point(pp.X - 1, pp.Y);
                    Point mr = new Point(pp.X + 1, pp.Y);
                    Point bl = new Point(pp.X - 1, pp.Y + 1);
                    Point bm = new Point(pp.X, pp.Y + 1);
                    Point br = new Point(pp.X + 1, pp.Y + 1);
                    if (WithinPicture(tl, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(tl);
                    }
                    if (WithinPicture(tm, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(tm);
                    }
                    if (WithinPicture(tr, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(tr);
                    }
                    if (WithinPicture(ml, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(ml);
                    }
                    if (WithinPicture(mr, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(mr);
                    }
                    if (WithinPicture(bl, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(bl);
                    }
                    if (WithinPicture(bm, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(bm);
                    }
                    if (WithinPicture(br, points[i], points[(i + 1) % points.Count]))
                    {
                        aroundp.Add(br);
                    }

                    foreach (Point a in aroundp)
                    {

                        if (a == points[(i + 1) % points.Count])
                        {

                            shortpath = new Node(shortpath, a, GetPixelWeight(a));
                            stop = true;

                        }
                        else if (!dictionary.ContainsKey(a)&&!diction2.Contains(a))
                        {
                            shortpath = new Node(shortpath, a, GetPixelWeight(a));
                            p.Enqueue(shortpath, int.MaxValue - GetPixelWeight(a));

                            diction2.Add(a);


                        }

                        

                    }

                   
                    
                }
                Node pare = null;
                //go back and draw the shortest path
                while (shortpath != null)
                {
                    using (Graphics g = Graphics.FromImage(Overlay))
                    {
                        g.DrawEllipse(redpen, shortpath.current.X, shortpath.current.Y, 1, 1);
                    }
                    pare = shortpath.parent;
                    shortpath = pare;


                }


                //check if the node is the next (selected) node
                //Node n <-- the node
                //Point p <-- n.Point
                //draw rectangle for that node
                //n = n.Parents
                //and this is looping until the parent is null

            }


        }
        //make sure the point will not go out the image
        private Boolean WithinPicture(Point point, Point start, Point end)
        {
            // return (point.X < (Overlay.Width - 1) && point.Y < (Overlay.Height - 1) && point.X > 1 && point.Y > 1);
            //  return (point.X < Math.Max(start.X+20,end.X+ 20) && point.Y < Math.Max(start.Y+ 20, end.Y+ 20) && point.X > Math.Min(start.X- 20, end.X- 20) && point.Y > Math.Min(start.Y- 20, end.Y- 20));
             return (point.X <Math.Min( Math.Max(start.X+10,end.X + 10), Overlay.Width-1) && point.Y <Math.Min( Math.Max(start.Y + 10, end.Y + 10), Overlay.Height-1) && point.X > Math.Min(Math.Min(start.X-10, end.X - 10),Overlay.Width-1) && point.Y > Math.Min(Math.Min(start.Y - 10, end.Y - 10),Overlay.Height-1));
            //return (point.X < Math.Min(Math.Max(start.X + 5, end.X + 5), Overlay.Width - 1) && point.Y < Math.Min(Math.Max(start.Y + 5, end.Y + 5), Overlay.Height - 1) && point.X > Math.Min(Math.Min(start.X - 5, end.X - 5), Overlay.Width - 1) && point.Y > Math.Min(Math.Min(start.Y - 5, end.Y - 5), Overlay.Height - 1));

        }

        //draw the start point
        private void CircleStartPoints(IList<Point> points)
        {
            using (Graphics g = Graphics.FromImage(Overlay))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Point start = points[i];
                    g.DrawEllipse(yellowpen, start.X - 3, start.Y - 3, 5, 5);
                }
                Program.MainForm.RefreshImage();
            }
        }
    }
}
