using System.Data.SqlClient;
using System.Data;

namespace DocumentsSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFile(txtFilePath.Text);
            MessageBox.Show("Saved");
        }

        private void SaveFile(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var fi = new FileInfo(filePath); 
                string extension = fi.Extension;
                string name = fi.Name;

                string query = "INSERT INTO Document(Data,Extension)VALUES(@data,@extension)";

                using (SqlConnection cn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                   // cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = buffer;
                    cmd.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                    cmd.Parameters.Add("@extension", SqlDbType.Char).Value = extension;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=LAPTOP-PHU5KCBD;Database=DocumentsSystem;Trusted_Connection=true;");
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            txtFilePath.Text = openFile.FileName;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
            using (SqlConnection cn = GetConnection())
            {
                string query = "SELECT ID, FileName, Extenseion FROM Documents";
                SqlDataAdapter adapter = new SqlDataAdapter(query, cn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                    dgvDocuments.DataSource = dataTable;
            }
        }
    }
}