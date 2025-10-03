using System;
using System.Windows.Forms;

namespace WinFormsAuthApp
{
    public class MainForm : Form
    {
        private readonly Button _btnLogin = new Button { Text = "Iniciar sesión", Left = 20, Top = 20, Width = 150 };
        private readonly Button _btnRegister = new Button { Text = "Registrarse", Left = 20, Top = 60, Width = 150 };

        public MainForm()
        {
            Text = "Auth App";
            Width = 320;
            Height = 160;
            Controls.Add(_btnLogin);
            Controls.Add(_btnRegister);

            _btnLogin.Click += (s, e) => new LoginForm().ShowDialog(this);
            _btnRegister.Click += (s, e) => new RegisterForm().ShowDialog(this);
        }
    }
}


