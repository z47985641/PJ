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

        private void Refrash()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=MingSu;Integrated Security=True";
            conn.Open();

            adapter = new SqlDataAdapter(
                "SELECT * " +
                "FROM Member As M " 

                //"SELECT M.MemberID,M.MemberName,M.MemberAccount,M.MemberPassword,M.BirthDate,M.MemberPhone,M.MemberEmail,C.CityID,C.CityName,M.Authority,M.MemberImage " +
                //"FROM Member As M " +
                //"INNER JOIN  City As C on M.CityID = C.CityID"
                , conn);
            buider.DataAdapter = adapter;

            DataSet Dset = new DataSet();
            adapter.Fill(Dset);
            conn.Close();

            DataView dv = new DataView();
            dv.Table = Dset.Tables[0];

            dataGridView1.DataSource = dv;
        }

        private void button3_Click(object sender, EventArgs e)
        {

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
                //LargePhoto      =Byte.Parse(row["MemberImage"].ToString()),
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
            databaseupdated();
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
            dv.Table.Rows.Add(row);

            databaseupdated();
        }
    }
}
