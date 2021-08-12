using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using ERP.Message_BOX;
using System.Windows;
namespace ERP
{
    public static class clsMessages
    {
        public static MainWindowViewModel Model = null;
        public static string currentMessage;
        public static MessageBOX Messagebox;

        public static void setMessageWithCaption(String Message)
        {

            if (Model != null && Message != null)
            {
                currentMessage = Message;
                Model.MessageLine = Properties.Resources.ERPCaption + " Say " + Message;
                Messagebox = new MessageBOX(Properties.Resources.ERPCaption + " Say ", Message);
                Messagebox.ShowDialog();
                Messagebox.Close();
            }
        }
        public static void setMessageWithCaption(String Message, Visibility Cancel)
        {

            if (Model != null && Message != null)
            {
                currentMessage = Message;
                Model.MessageLine = Properties.Resources.ERPCaption + " Say " + Message;
                Messagebox = new MessageBOX(Properties.Resources.ERPCaption + " Say ", Message, Cancel);
                Messagebox.ShowDialog();
                Messagebox.Close();
            }
        }
        public static void setMessage(String Message)
        {
            if (Model != null && Message != null)
            {
                currentMessage = Message;
                Model.MessageLine = Message;
                Messagebox = new MessageBOX("H2SO4", Message);
                Messagebox.ShowDialog();
                Messagebox.Close();
            }
        }
        public static void setMessage(String Message, Visibility Cancel)
        {
            if (Model != null && Message != null)
            {
                currentMessage = Message;
                Model.MessageLine = Message;
                Messagebox = new MessageBOX("H2SO4", Message, Cancel);
                Messagebox.ShowDialog();
                Messagebox.Close();
            }
        }

        #region SetMessageforLoginFormErros

        public static void setMessageLogin(String Message)
        {
            if (Message != null)
            {
                currentMessage = Message;
                if (Model != null)
                    Model.MessageLine = Message;
                Messagebox = new MessageBOX("H2SO4", Message);
                Messagebox.ShowDialog();
                Messagebox.Close();
            }
        }

        #endregion
    }
}
