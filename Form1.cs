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

namespace PhoneBookApp
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\PhoneBookApp\PhoneBookDB.mdf;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        DataSet ds = new DataSet();
        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка "сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Пожалуйста, введите имя и номер телефона.");
            }
            else
            {
                cmd = new SqlCommand("SELECT * FROM TblPhoneBook WHERE MobileNo='"+textBox2.Text+"'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i > 0)
                {
                    MessageBox.Show("Мобильный телефон " + textBox2.Text + " занят");
                    ds.Clear();
                }
                else
                {


                    try
                    {
                        con.Open();
                        cmd = new SqlCommand("INSERT INTO TblPhoneBook(Name, MobileNO) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "')", con);
                        cmd.ExecuteNonQuery();

                        da = new SqlDataAdapter("SELECT * FROM TblPhoneBook ORDER BY Id", con);
                        dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        MessageBox.Show("Контакт сохранён.");
                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }   

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }

        // Кнопка "обновить"
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                da = new SqlDataAdapter(
                    "UPDATE TblPhoneBook set Name='"+textBox1.Text+"', MobileNo='"+textBox2.Text+"' WHERE Id='"+textBox4.Text+"'", con);
                cmd.ExecuteNonQuery();

                da = new SqlDataAdapter("SELECT * FROM TblPhoneBook ORDER BY Id", con);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                MessageBox.Show("Контакт был обновлён...");
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[i];
            textBox1.Text = row.Cells[1].Value.ToString();  
            textBox2.Text = row.Cells[2].Value.ToString();  
            textBox4.Text = row.Cells[0].Value.ToString();  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            updateFunction();
        }

        // Кнопка "удалить"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("DELETE FROM TblPhoneBook WHERE Id='"+textBox4.Text+"'", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Выбранный контакт удалён...");

                updateFunction();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }

        // Поиск по имени
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = "Name like '%"+textBox3.Text+"%'";
            dataGridView1.DataSource = bs;
        }

        // Поиск по номеру телефона
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = "MobileNo like '%" + textBox5.Text + "%'";
            dataGridView1.DataSource = bs;
        }

        // Функция обновления
        private void updateFunction()
        {
            da = new SqlDataAdapter("SELECT * FROM TblPhoneBook ORDER BY Id", con);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}
