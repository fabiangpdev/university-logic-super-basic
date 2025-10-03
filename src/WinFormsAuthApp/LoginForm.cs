using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAuthApp.Data;

namespace WinFormsAuthApp
{
    public class LoginForm : Form
    {
        private readonly TextBox _txtEmail = new TextBox { Left = 120, Top = 20, Width = 200 };
        private readonly TextBox _txtPassword = new TextBox { Left = 120, Top = 60, Width = 200, PasswordChar = '•' };
        private readonly Button _btnLogin = new Button { Text = "Entrar", Left = 120, Top = 100, Width = 100 };

        public LoginForm()
        {
            Text = "Login";
            Width = 360;
            Height = 190;

            Controls.Add(new Label { Text = "Correo:", Left = 20, Top = 20, Width = 90 });
            Controls.Add(new Label { Text = "Contraseña:", Left = 20, Top = 60, Width = 90 });
            Controls.Add(_txtEmail);
            Controls.Add(_txtPassword);
            Controls.Add(_btnLogin);

            _btnLogin.Click += async (s, e) => await OnLoginAsync();
        }

        private async Task OnLoginAsync()
        {
            var email = _txtEmail.Text.Trim();
            var password = _txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            try
            {
                var repo = new UserRepository();
                var ok = await repo.ValidateLoginAsync(email, password);
                MessageBox.Show(ok ? "Login exitoso." : "Credenciales inválidas.");
                if (ok) Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}


