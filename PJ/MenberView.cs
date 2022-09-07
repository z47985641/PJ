using PJ.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PJ
{
    public partial class MenberView : Form
    {
        public MenberView()
        {
            InitializeComponent();
        }
        SqlCommandBuilder buider = new SqlCommandBuilder();
        SqlDataAdapter adapter;
        int _position;
        private void Form1_Load(object sender, EventArgs e)
        {
            Refrash();
        }
        DataView dv = new DataView();
        DataSet Dset = new DataSet();
        List<string> list = new List<string>();
        string key = "", keyword = ""; 
        private void RefrashData()
        {
            CleanTable();
            Refrash();
        }

        private void Refrash()
        {
            

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=MingSu;Integrated Security=True";
            conn.Open();
            if (keyword == "")
            {
                adapter = new SqlDataAdapter("SELECT * FROM Member", conn);
                buider.DataAdapter = adapter;
            }
            else 
            {
                adapter = new SqlDataAdapter($"SELECT * FROM Member {keyword}", conn);
                buider.DataAdapter = adapter;

            }


            adapter.Fill(Dset);
            conn.Close();

            dv.Table = Dset.Tables[0];
            dataGridView1.DataSource = dv;

            CBox.Items.Clear();
            foreach (DataColumn c in Dset.Tables[0].Columns)
            {
                CBox.Items.Add(c.ColumnName);
                list.Add(c.ColumnName);
            }
            keyword = "";
        }

        private void CleanTable()
        {
            if (dv.Table != null)
            {
                dv.Table.Rows.Clear();
                dv.Table.NewRow();
            }
        }

        private void M_search(object sender, EventArgs e)
        {
            RefrashData();

            int searchkey = 0;
            if (list.IndexOf(CBox.Text) < 0)
            {
                MessageBox.Show("請選取欄位");
                return;
            }

            key = textBox1.Text;
            if (
                    key == "")
            {
                MessageBox.Show("請輸入關鍵字!!!");
                return;
            }
            //CB = X , TXT = X OR CB = V, TXT = X
            

            //foreach (DataGridViewRow r in dataGridView1.Rows)
            //{
            //    foreach (DataGridViewCell c in r.Cells)
            //    {
            //        c.Style.BackColor = Color.White;
            //    }
            //}
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[list.IndexOf(CBox.Text)].Value == null)
                    break;
                if (r.Cells[list.IndexOf(CBox.Text)].Value.ToString().ToUpper().Contains(key.ToUpper()))
                {
                    keyword = $"where {CBox.Text} like '%{key}%' ";
                    r.Cells[list.IndexOf(CBox.Text)].Style.BackColor = Color.Yellow;
                    searchkey = 1;
                    //CB =V, TXT = V 單攔查詢
                }
            }
            if (searchkey == 0)
                MessageBox.Show("查無資料");
            RefrashData();
        }

        private void M_Delete(object sender, EventArgs e)
        {
            if (_position < 0)
                return;
            DataRow row = dv.Table.Rows[_position];
            row.Delete();
            databaseupdated();
        }
        private void databaseupdated()
        {

            if (dv.Count > 0)
            {
                adapter.Update(dv.Table);
                RefrashData();
            }
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _position = e.RowIndex;
        }
        private void M_Change(object sender, EventArgs e)
        {
            if (_position < 0)
                return;
            DataRow row = dv.Table.Rows[_position];
            Cmenber p = new Cmenber()
            {
                MemberID        = (int)row["MemberID"],
                MemberAccount   = row["MemberAccount"].ToString(),
                MemberPassword  = row["MemberPassword"].ToString(),
                MemberName      = row["MemberName"].ToString(),
                MemberPhone     = row["MemberPhone"].ToString(),
                MemberEmail     = row["MemberEmail"].ToString(),
                CityID          = (int)row["CityID"],
                Authority       = row["Authority"].ToString(),
                BirthDate =DateTime.Parse(row["BirthDate"].ToString()),
                LargePhoto = (byte[])row["MemberImage"],
                isaddornot = true ,
            };
            MenberChange Pgchange = new MenberChange();
            Pgchange.Change = p;
            Pgchange.ShowDialog();

            if (!Pgchange.isbuttonclik)
                return;

            row["MemberID"] = Pgchange.Change.MemberID;
            row["MemberAccount"] = Pgchange.Change.MemberAccount;
            row["MemberPassword"] = Pgchange.Change.MemberPassword;
            row["MemberName"] = Pgchange.Change.MemberName;
            row["MemberPhone"] = Pgchange.Change.MemberPhone;
            row["MemberEmail"] = Pgchange.Change.MemberEmail;
            row["CityID"] = Pgchange.Change.CityID;
            row["Authority"] = Pgchange.Change.Authority;
            row["BirthDate"] = Pgchange.Change.BirthDate;
            row["MemberImage"] = Pgchange.Change.LargePhoto;
            databaseupdated();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefrashData();
        }

        private void M_Add(object sender, EventArgs e)
        {
            MenberChange Pgchange = new MenberChange();
            Pgchange.ShowDialog();

            if (!Pgchange.isbuttonclik)
                return;

            Cmenber p = Pgchange.Change;
            DataView dv = dataGridView1.DataSource as DataView;
            DataRow row = dv.Table.NewRow();

            row["MemberAccount"] = p.MemberAccount;
            row["MemberPassword"] = p.MemberPassword;
            row["MemberName"] = p.MemberName;
            row["MemberPhone"] = p.MemberPhone;
            row["MemberEmail"] = p.MemberEmail;
            row["CityID"] = p.CityID;
            row["Authority"] = p.Authority;
            row["BirthDate"] = p.BirthDate;
            row["MemberImage"] = p.LargePhoto;
            dv.Table.Rows.Add(row);
            
            databaseupdated();
        }

    }
}
