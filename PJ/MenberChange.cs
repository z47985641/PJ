using PJ.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            MenberView View = new MenberView();
            _isOkClik = View.isaddornot;
            txtMemberID.Enabled = false;
            txtMemberID.Visible = _isOkClik;
            label9.Visible = _isOkClik;
        }

        Cmenber _menber = new Cmenber();
        bool _isOkClik = false;

        public Cmenber Change
        {
            get
            {
                if(_isOkClik)
                    _menber.MemberID = Convert.ToInt32(txtMemberID.Text); 
                _menber.MemberAccount   = txtMemberAccount.Text;
                _menber.MemberPassword  = txtMemberPassword.Text;
                _menber.MemberName      = txtMemberName.Text;
                _menber.BirthDate = DateTime.Parse(txtBirthDate.Text);
                _menber.MemberPhone     = txtMemberPhone.Text;
                _menber.MemberEmail     = txtMemberEmail.Text;
                _menber.CityID          = Convert.ToInt32(txtCityID.Text);
                _menber.Authority       = txtAuthority.Text;
                return _menber; 
            }
            set 
            { 
                _menber = value;
                if (_isOkClik)
                    txtMemberID.Text = _menber.MemberID.ToString();
                txtMemberAccount.Text = _menber.MemberAccount;
                txtMemberPassword.Text = _menber.MemberPassword;
                txtMemberName.Text = _menber.MemberName;
                txtBirthDate.Text = _menber.BirthDate.ToString();
                txtMemberPhone.Text = _menber.MemberPhone;
                txtMemberEmail.Text = _menber.MemberEmail;
                txtCityID.Text = _menber.CityID.ToString();
                txtAuthority.Text = _menber.Authority;

            }
        }
        public bool isbuttonclik
        {
            get { return _isOkClik; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _isOkClik = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
