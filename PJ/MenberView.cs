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
        SqlCommandBuilder buider2 = new SqlCommandBuilder();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        int _position;
        private void Form1_Load(object sender, EventArgs e)
        {
            RefrashData();
        }
        DataView dv = new DataView();
        DataSet Dset = new DataSet(); 
        DataView dv2 = new DataView();
        DataSet Dset2 = new DataSet();
        List<string> list = new List<string>();
        string key = "", keyword = ""; 
        private void RefrashData()
        {
            CleanTable();
            AdminRefrash();
            Refrash();
            setGridStyle();
        }
        private void setGridStyle()
        {
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 70;
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[6].Width = 130;
            dataGridView1.Columns[7].Width = 50;
            dataGridView1.Columns[8].Width = 100;
            dataGridView1.Columns[9].Width = 70;

            dataGridView2.Columns[0].Width = 70;
            dataGridView2.Columns[1].Width = 70;
            dataGridView2.Columns[2].Width = 70;

        }
        private void AdminRefrash()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=MingSu;Integrated Security=True";
            conn.Open();
            if(Admin)
            adapter2 = new SqlDataAdapter("SELECT * FROM Admin", conn);
            buider2.DataAdapter = adapter2;

            adapter2.Fill(Dset2);
            conn.Close();

            dv2.Table = Dset2.Tables[0];
            dataGridView2.DataSource = dv2;

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
                dv2.Table.Rows.Clear();
                dv2.Table.NewRow();
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
            if (key == "")
            {
                MessageBox.Show("請輸入關鍵字!!!");
                return;
            }
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[list.IndexOf(CBox.Text)].Value == null)
                    break;
                if (r.Cells[list.IndexOf(CBox.Text)].Value.ToString().ToUpper().Contains(key.ToUpper()))
                {
                    keyword = $"where {CBox.Text} like '%{key}%' ";
                    searchkey = 1;
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

        private void button7_Click(object sender, EventArgs e)
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
