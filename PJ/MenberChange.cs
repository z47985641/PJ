﻿using PJ.Class;
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
            txtMemberID.Enabled = false;
            txtMemberID.Visible = _menber.isaddornot;
            label9.Visible = _menber.isaddornot;
        }

        System.IO.MemoryStream ms = new System.IO.MemoryStream(); 
        PictureBox pic = new PictureBox();
        MenberView View = new MenberView();
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
                _menber.BirthDate       = DateTime.Parse(txtBirthDate.Text);
                _menber.MemberPhone     = txtMemberPhone.Text;
                _menber.MemberEmail     = txtMemberEmail.Text;
                _menber.CityID = int.Parse(txtCityID.Text);
                _menber.Authority       = txtAuthority.Text;
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
                txtBirthDate.Text       = _menber.BirthDate.ToString();
                txtMemberPhone.Text     = _menber.MemberPhone;
                txtMemberEmail.Text     = _menber.MemberEmail;
                txtCityID.Text          = _menber.CityID.ToString();
                txtAuthority.Text       = _menber.Authority;
                pic.Image = System.Drawing.Image.FromStream(ms);

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        MingSuEntities MS = new MingSuEntities();
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image =System.Drawing.Image.FromFile(this.openFileDialog1.FileName);
            }
        }
    }
}
