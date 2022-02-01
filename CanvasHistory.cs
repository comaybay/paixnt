using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Paixnt
{
    class CanvasHistory
    {
        private List<Image> history;
        private int index;
        private static int Limit = 21;

        public CanvasHistory(Image initial)
        {
            history = new List<Image>() { initial };
            index = 0;
        }

        public void Add(Image image)
        {
            if (index != history.Count - 1)
            {
                int start = index + 1;

                for (int i = start; i < history.Count; i++)
                    history[i].Dispose();

                history.RemoveRange(start, history.Count - start);
            }

            history.Add(image);

            if (history.Count > Limit)
            {
                history.RemoveAt(0);
            }

            index = history.Count - 1;
        }

        public Image Next()
        {
            index = Math.Min(history.Count - 1, index + 1);
            return GetImage();
        }
        public Image Prev()
        {
            index = Math.Max(0, index - 1);
            return GetImage();
        }
        public Image GetImage() => history[index];
    }
}
