using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Circle
    {
        private CircleType type;
        private int x, y, width, height;
        private System.Windows.Forms.Panel parent;

        public Circle(CircleType t, int t_x, int t_y, int t_width, int t_height, System.Windows.Forms.Panel p)
        {
            type = t;
            x = t_x; y = t_y;
            parent = p;
            width = t_width;
            height = t_height;
            if(parent == null)
            {
                int a = 3;
            }
        }


        public void draw()
        {
            //if (parent == null)
            //    return;
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush((type == CircleType.RED)?System.Drawing.Color.Red : System.Drawing.Color.Yellow);
            System.Drawing.Graphics formGraphics = parent.CreateGraphics();
            formGraphics.FillEllipse(myBrush, new System.Drawing.Rectangle(x, y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }
    }
}
