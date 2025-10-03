using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinFormsAuthApp.Config;

namespace WinFormsAuthApp
{
    public class DataViewerForm : Form
    {
        private readonly Button _btnLoad = new Button { Text = "Cargar CSV", Left = 20, Top = 20, Width = 120 };
        private readonly DataGridView _grid = new DataGridView { Left = 20, Top = 60, Width = 740, Height = 380, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly Button _btnBack = new Button { Text = "Volver", Left = 150, Top = 20, Width = 100 };

        public DataViewerForm()
        {
            Text = "Visor de Datos";
            Width = 800;
            Height = 500;

            Controls.Add(_btnLoad);
            Controls.Add(_grid);
            Controls.Add(_btnBack);

            _btnLoad.Click += (s, e) => OnLoadCsv();
            _btnBack.Click += (s, e) => Close();
        }

        private void OnLoadCsv()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Selecciona archivo CSV"
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            var dt = ReadCsvToDataTable(ofd.FileName);
            ApplyRoleProjection(dt);
        }

        private static DataTable ReadCsvToDataTable(string path)
        {
            var dt = new DataTable();
            using var sr = new StreamReader(path);
            var header = sr.ReadLine();
            if (string.IsNullOrEmpty(header)) return dt;
            var columns = header.Split(',').Select(h => h.Trim()).ToArray();
            foreach (var col in columns) dt.Columns.Add(col);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                var cells = line.Split(',').Select(c => c.Trim()).ToArray();
                dt.Rows.Add(cells);
            }
            return dt;
        }

        private void ApplyRoleProjection(DataTable source)
        {
            DataTable projected;
            if (WinFormsAuthApp.Config.Config.Session.IsAdmin)
            {
                projected = source;
            }
            else
            {
                var keep = source.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .Where(n => string.Equals(n, "Name", StringComparison.OrdinalIgnoreCase) || string.Equals(n, "RoleTitle", StringComparison.OrdinalIgnoreCase) || string.Equals(n, "Cargo", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (keep.Length == 0)
                {
                    MessageBox.Show("El CSV no contiene columnas reconocidas (Name/Cargo/RoleTitle). Se mostrará todo.");
                    projected = source;
                }
                else
                {
                    projected = source.DefaultView.ToTable(false, keep);
                }
            }
            _grid.DataSource = projected;
        }
    }
}


