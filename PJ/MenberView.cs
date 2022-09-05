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
        SqlDataAdapter adapter, adapterCity;
        int _position;
        private void Form1_Load(object sender, EventArgs e)
        {
            Refrash();
        }

        DataSet Dset = new DataSet();
        DataSet Cityset = new DataSet();
        List<string> list = new List<string>();
        List<string> Clist = new List<string>();

        private void Refrash()
        {
            

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=MingSu;Integrated Security=True";
            conn.Open();

            adapter = new SqlDataAdapter(
                "SELECT * " +
                "FROM Member As M " 
                , conn);
            buider.DataAdapter = adapter;

            adapter.Fill(Dset);
            conn.Close();

            DataView dv = new DataView();
            dv.Table = Dset.Tables[0];

            

            dataGridView1.DataSource = dv;

            CBox.Items.Clear();
            foreach (DataColumn c in Dset.Tables[0].Columns)
            {
                CBox.Items.Add(c.ColumnName);
                list.Add(c.ColumnName);
            }
        }
        private void M_search(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    c.Style.BackColor = Color.White;
                }
               
            }

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {

                if (textBox1.Text == "")
                {
                    MessageBox.Show("請輸入關鍵字!!!");
                    break;
                }
                   
                if (CBox.Text =="")
                {
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.Value == null)
                            continue;
                        if (c.Value.ToString().ToUpper().Contains(textBox1.Text.ToUpper()))
                        {
                            c.Style.BackColor = Color.Yellow;
                        }
                    }
                }
                if (  list.IndexOf(CBox.Text)<0||r.Cells[list.IndexOf(CBox.Text)].Value == null)
                    break;
                if (r.Cells[list.IndexOf(CBox.Text)].Value.ToString().ToUpper().Contains(textBox1.Text.ToUpper()))
                {
                    r.Cells[list.IndexOf(CBox.Text)].Style.BackColor = Color.Yellow;
                }


            }
        }

        private void M_Delete(object sender, EventArgs e)
        {
            if (_position < 0)
                return;

            DataView dv = dataGridView1.DataSource as DataView;
            DataRow row = dv.Table.Rows[_position];
            row.Delete();
            databaseupdated();
        }
        private void databaseupdated()
        {
            DataView dv = dataGridView1.DataSource as DataView;
            if (dv.Count > 0)
            {
                adapter.Update(dv.Table);
                Refrash();
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
            DataView dv = dataGridView1.DataSource as DataView;
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
                //LargePhoto = (byte[])row["MemberImage"],
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
            databaseupdated();
            //Refrash();
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
