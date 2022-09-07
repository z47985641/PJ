using PJ.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PJ
{
    public partial class MenberChange : Form
    {
        public MenberChange()
        {
            InitializeComponent();
        }
        private void MenberChange_Load(object sender, EventArgs e)
        {
            txtMemberID.Visible = _menber.isaddornot;
            label9.Visible = _menber.isaddornot;

            foreach(string i in AuthorityLIST)
            {
                CBAuthorityName.Items.Add(i);
            }
            var q = from City in Minsu.City
                    select City.CityName;
            foreach(var i in q)
            {
                CBCityName.Items.Add(i);
            }

        }
        MingSuEntities Minsu =new MingSuEntities();
        List<string> AuthorityLIST = new List<string> { "普通會員","業主","管理員"};
        System.IO.MemoryStream ms = new System.IO.MemoryStream(); 
        Cmenber _menber = new Cmenber();
        bool _isOkClik = false;
        byte[] bytes;

        public Cmenber Change
        {
            get
            {
                if(_menber.isaddornot)
                    _menber.MemberID    = Convert.ToInt32(txtMemberID.Text); 
                _menber.MemberAccount   = txtMemberAccount.Text;
                _menber.MemberPassword  = txtMemberPassword.Text;
                _menber.MemberName      = txtMemberName.Text;
                _menber.BirthDate       = DTPBirthDate.Value;
                _menber.MemberPhone     = txtMemberPhone.Text;
                _menber.MemberEmail     = txtMemberEmail.Text;
                _menber.CityID = int.Parse(txtCityID.Text);
                _menber.Authority       = txtAuthorityID.Text;
                _menber.LargePhoto = bytes;
                return _menber; 
            }
            set 
            {
                _menber = value;
                if (_menber.isaddornot)
                    txtMemberID.Text    = _menber.MemberID.ToString();
                txtMemberAccount.Text   = _menber.MemberAccount;
                txtMemberPassword.Text  = _menber.MemberPassword;
                txtMemberName.Text      = _menber.MemberName;
                DTPBirthDate.Value = _menber.BirthDate;
                txtMemberPhone.Text     = _menber.MemberPhone;
                txtMemberEmail.Text     = _menber.MemberEmail;
                txtCityID.Text          = _menber.CityID.ToString();
                txtAuthorityID.Text       = _menber.Authority;
                pictureBox1.Image = Btyechangeimge(_menber.LargePhoto);
                CBAuthorityName.Text = AuthorityLIST[int.Parse(txtAuthorityID.Text)-1];
                var q = from i in Minsu.City
                        where i.CityID == _menber.CityID
                        select i;
                foreach(var i in q)
                CBCityName.Text = i.CityName;
            }
        }
        public bool isbuttonclik
        {
            get { return _isOkClik; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _isOkClik = true;
            this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.GetBuffer();
            this.Close();
        }
        private System.Drawing.Image Btyechangeimge(byte[] streamByte)
        {
            System.IO.MemoryStream BCI = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(BCI);
            return img;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image =System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
        }

        private void CBAuthorityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAuthorityID.Text = (AuthorityLIST.IndexOf(CBAuthorityName.Text)+1).ToString();
        }

        private void CBCityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = from n in Minsu.City
                    where n.CityName == CBCityName.Text
                    select n;
            foreach (var i in q)
                txtCityID.Text = i.CityID.ToString();

        }

        private void txtMemberPhone_TextChanged(object sender, EventArgs e)
        {
            LBphone.Visible = false;
            if (txtMemberPhone.Text.Length != 10)
            {
                LBphone.Visible = true;
                LBphone.Text = "格式錯誤";
            }
        }
    }
}
