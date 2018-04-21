using System.Windows.Forms;

namespace Uvv.TesteAgil.WebForms.Util
{
    public class Mensagens
    {
        public static void mensagemSucesso(string mensagem, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBox.Show(mensagem, "Sucesso", buttons, icon);
        }

        public static void mensagemErro(string mensagem, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(mensagem, "Erro", buttons, icon);
        }

        public static void mensagemAlerta(string mensagem, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Exclamation)
        {
            MessageBox.Show(mensagem, "Alerta", buttons, icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}