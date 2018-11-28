using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.DirectoryServices;
using System.Configuration;


namespace VIEW
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// CodeBehind da tela de login
    /// </summary>
    public partial class LoginScreen : Window
    {
        /// <summary>
        /// Inicialização do componente.
        /// </summary>
        public LoginScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bloqueio das funções ALT+F4
        /// Tecla de atalho para sair do sistema CTRL+A+P
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && Keyboard.IsKeyDown(Key.F4))
                e.Handled = true;

            if ((Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.A) && Keyboard.IsKeyDown(Key.P))
                Application.Current.Shutdown(); 
        }

        //Caso usuário pressione Enter
        /// <summary>
        /// Captura do botão Enter, caso o usuário pressione Enter
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void pressEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string username = matriculabox.Text.ToString();
                string pass = passwdbox.Password;

                string loginMod = username;
                try
                {
                    Convert.ToDecimal(username);
                    loginMod = AdjustLoginNumber(username);
                    username = loginMod;
                }
                catch { }

                string domain = ConfigurationManager.AppSettings["domain"].ToString();
                bool resultDomain = IsAuthenticated(domain, username, pass);

                try
                {
                    if (resultDomain)
                    {
                        // Now you log in!
                        Window w2 = new TOTEM.MainWindow(username);
                        w2.Show();
                        
                        //Fecha a janela de login
                        Application.Current.Windows[0].Close();
                    }
                    else
                    {
                        //limpa o campo de senha
                        passwdbox.Password = "";
                    }
                }
                catch (Exception ex) {
                   throw ex;
                }
            } 
        }

        /// <summary>
        /// Captura do clique no botão Entrar
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void LoginButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

            

            string username = matriculabox.Text.ToString();
            string pass = passwdbox.Password;
            string loginMod = username;
            try
            {
                Convert.ToDecimal(username);
                 loginMod = AdjustLoginNumber(username);
                 username = loginMod;
            }
            catch { }
            
            string domain = ConfigurationManager.AppSettings["domain"].ToString();
            bool resultDomain = IsAuthenticated(domain, username, pass);

            try
            {
                if (resultDomain)
                {
                    // Now you log in!
                    Window w2 = new TOTEM.MainWindow(username);
                    w2.Show();

                    //Fecha a janela de login
                    Application.Current.Windows[0].Close();
                }
                else
                {
                    //limpa o campo de senha
                    passwdbox.Password = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        /// <summary>
        /// Verifica se o login é válido no active directory
        /// </summary>
        /// <param name="domain">Domínio eepsa.com.br</param>
        /// <param name="usr">login do usuário</param>
        /// <param name="pwd">senha do usuário</param>
        /// <returns></returns>
        public bool IsAuthenticated(string domain, string usr, string pwd)
        {
            bool authenticated = false;

            try
            {
                DirectoryEntry entry = new DirectoryEntry(domain, usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //MessageBox.Show( cex.Message);
                lblErrorText.Style = (Style)FindResource("lblErrorText_Active");
                lblErrorText.Text = "Matrícula ou senha incorreta";
            }
            catch (Exception ex)
            {
                //not authenticated due to some other exception [this is optional]
            }
            return authenticated;
        }

        /// <summary>
        /// Método para limpar campos da tela de login
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void lblClear_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            matriculabox.Text = "";
            passwdbox.Password = "";
            lblErrorText.Style = (Style)FindResource("lblErrorText_Inactive");
            lblErrorText.Text = "";
            matriculabox.Focus();
        }

        private string AdjustLoginNumber(string loginNumber)
        {
            int count = 8 - loginNumber.Count();


            for (int i = 0; i < count; i++)
            {
                loginNumber = "0" + loginNumber;

            }

            return loginNumber;
        }

        // I've noticed that sometimes we can't see the blinking cursor (hard to tell)
        // Let's focus it in and focus it out accordingly, webforms style!
        // Unfortunately, WPF has a very strange definition of "focus" for form inputs...
        //private void matriculabox_GotFocus_1(object sender, RoutedEventArgs e)
        //{
        //    matriculabox.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#16BA16");
        //    // matriculabox.BorderThickness = ;
        //}

        //private void matriculabox_LostFocus_1(object sender, RoutedEventArgs e)
        //{
        //    matriculabox.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#1c3d50");
        //}

        
    }
}


// Open Start Menu > Run and enter gpedig.msc. Navigate to: User Configuration > Administrative Templates > System > CTRL+ALT+DELETE Options.This is the place where you normally set the behaviour of the key combo. Select Remove Task Manager > Double-click the Remove Task Manager option.
// If you change it’s value, the following registry entry gets created/modified: Software\Microsoft\Windows\CurrentVersion\Policies\System and the value of DisableTaskMgr gets set to 1.
//ALT+F4 desativado e tecla de atalha para sair Ctrl+A+P não me pergunte porque!
// Extender para cobrir as seguintes sequências de escape:
// Alt+Tab
// Win+Tab
// Win+D
// Acho melhor desabilitar o botão do Windows

// O BLOQUEIO DAS TECLAS DE ATALHOS DO WIN E CTRL + ALT + DEL SÃO DE RESPONSABILIDADE DO SO, SERÃO BLOQUEADOS POR GPO
