using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class UseHistory : Form
    {
        public Button BackButton { get; private set; }

        public UseHistory(MainMenu mainMenu)
        {
            InitializeComponent();
            BackButton = btnBack;
            mainMenu.ResizeEnd += (object sender, EventArgs e) => SizeLastColumn();
            listView.BackColor = PaixntColors.PrimaryClick;
            SizeLastColumn();
        }

        private void SizeLastColumn()
        {
            for (int i = 0; i < listView.Columns.Count - 1; i++)
                listView.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);

            listView.Columns[listView.Columns.Count - 1].Width = -2;
        }

        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView.Columns[e.ColumnIndex].Width;
        }

        private void UseHistory_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            var db = new Database("history", "select * from history order by date desc");
            db.DocBang();

            if (db.Rows.Count == 0)
                return;

            listView.Items.Clear();

            var items = new List<ListViewItem>();
            foreach (DataRow row in db.Rows)
            {
                string[] props = new string[]
                {
                        ((DateTime)row["DATE"]).ToString("dd/MM/yyyy hh:mm:ss:fff tt"),
                        row["USERNAME"].ToString(),
                        row["WHAT"].ToString(),
                };

                var item = new ListViewItem(props);
                items.Add(item);
            }

            listView.Items.AddRange(items.ToArray());
            SizeLastColumn();
        }
    }
}
