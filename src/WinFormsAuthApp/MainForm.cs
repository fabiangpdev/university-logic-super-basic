using System;
using System.Windows.Forms;

namespace WinFormsAuthApp
{
    public class MainForm : Form
    {
        private readonly Button _btnLogin = new Button { Text = "Iniciar sesión", Left = 20, Top = 20, Width = 150 };
        private readonly Button _btnRegister = new Button { Text = "Registrarse", Left = 20, Top = 60, Width = 150 };
        private readonly Button _btnData = new Button { Text = "Visor de Datos", Left = 20, Top = 100, Width = 150 };

        public MainForm()
        {
            Text = "Auth App";
            Width = 320;
            Height = 200;
            Controls.Add(_btnLogin);
            Controls.Add(_btnRegister);
            Controls.Add(_btnData);

            _btnLogin.Click += (s, e) => OpenReplacing(new LoginForm());
            _btnRegister.Click += (s, e) => OpenReplacing(new RegisterForm());
            _btnData.Click += (s, e) => OpenReplacing(new DataViewerForm());
        }

        private void OpenReplacing(Form next)
        {
            Hide();
            next.FormClosed += (s, e) => { Show(); }; // vuelve al main al cerrar
            next.Show();
        }
    }
}


