using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAuthApp.Data;

namespace WinFormsAuthApp
{
    public class RegisterForm : Form
    {
        private readonly TextBox _txtName = new TextBox { Left = 120, Top = 20, Width = 200 };
        private readonly TextBox _txtEmail = new TextBox { Left = 120, Top = 60, Width = 200 };
        private readonly TextBox _txtPassword = new TextBox { Left = 120, Top = 100, Width = 200, PasswordChar = '•' };
        private readonly ComboBox _cmbRole = new ComboBox { Left = 120, Top = 140, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button _btnRegister = new Button { Text = "Registrar", Left = 120, Top = 180, Width = 100 };
        private readonly Button _btnBack = new Button { Text = "Volver", Left = 230, Top = 180, Width = 90 };

        public RegisterForm()
        {
            Text = "Registro";
            Width = 360;
            Height = 270;

            Controls.Add(new Label { Text = "Nombre:", Left = 20, Top = 20, Width = 90 });
            Controls.Add(new Label { Text = "Correo:", Left = 20, Top = 60, Width = 90 });
            Controls.Add(new Label { Text = "Contraseña:", Left = 20, Top = 100, Width = 90 });
            Controls.Add(_txtName);
            Controls.Add(_txtEmail);
            Controls.Add(_txtPassword);
            Controls.Add(new Label { Text = "Rol:", Left = 20, Top = 140, Width = 90 });
            _cmbRole.Items.AddRange(new object[] { "user", "admin" });
            _cmbRole.SelectedIndex = 0;
            Controls.Add(_cmbRole);
            Controls.Add(_btnRegister);
            Controls.Add(_btnBack);

            _btnRegister.Click += async (s, e) => await OnRegisterAsync();
            _btnBack.Click += (s, e) => Close();
        }

        private async Task OnRegisterAsync()
        {
            var name = _txtName.Text.Trim();
            var email = _txtEmail.Text.Trim();
            var password = _txtPassword.Text;
            var role = (_cmbRole.SelectedItem?.ToString() ?? "user").Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            try
            {
                var repo = new UserRepository();
                if (await repo.EmailExistsAsync(email))
                {
                    MessageBox.Show("El correo ya está registrado.");
                    return;
                }

                var userId = await repo.RegisterAsync(name, email, password, role);
                MessageBox.Show(userId > 0 ? "Usuario registrado." : "No se pudo registrar.");
                if (userId > 0) Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}


