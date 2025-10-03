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
        private readonly Button _btnRegister = new Button { Text = "Registrar", Left = 120, Top = 140, Width = 100 };

        public RegisterForm()
        {
            Text = "Registro";
            Width = 360;
            Height = 230;

            Controls.Add(new Label { Text = "Nombre:", Left = 20, Top = 20, Width = 90 });
            Controls.Add(new Label { Text = "Correo:", Left = 20, Top = 60, Width = 90 });
            Controls.Add(new Label { Text = "Contraseña:", Left = 20, Top = 100, Width = 90 });
            Controls.Add(_txtName);
            Controls.Add(_txtEmail);
            Controls.Add(_txtPassword);
            Controls.Add(_btnRegister);

            _btnRegister.Click += async (s, e) => await OnRegisterAsync();
        }

        private async Task OnRegisterAsync()
        {
            var name = _txtName.Text.Trim();
            var email = _txtEmail.Text.Trim();
            var password = _txtPassword.Text;

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

                var userId = await repo.RegisterAsync(name, email, password);
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


