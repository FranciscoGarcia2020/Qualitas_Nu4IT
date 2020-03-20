/*Fernando Rivelino Ortiz Martínez
 Nü4ItAutomation
 En está clase se lleva todo lo referente a la automatización de Outlook:
 --Envío de correos eléctrónicos
 --Revisión de bandejas de entrada
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace QPil
{
    class AutomatizadorOutlook
    {
        const int CTE_INT_SI = 1;
        const int CTE_INT_NO = 0;

        public static void MandarCorreoGral(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.Subject = Asunto;
            mail.Body = CuerpoMensaje;
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

        public static void MandarCorreoGral(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje, string RutaArchivoAdjunto)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.Subject = Asunto;
            mail.Body = CuerpoMensaje;
            mail.Attachments.Add(RutaArchivoAdjunto);
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

        public static void MandarCorreoGralVariosArchivosAdjuntos(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje, List<string> RutaArchivosAdjunto)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.BCC = Copiados;
            mail.Subject = Asunto;
            mail.Body = CuerpoMensaje;

            foreach (string ArchvioAdjuntar in RutaArchivosAdjunto)
            {
                mail.Attachments.Add(ArchvioAdjuntar);
            }

            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

        public static void MandarCorreoFormatoHTML(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.Subject = Asunto;
            mail.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
            mail.HTMLBody = CuerpoMensaje;
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

        public static void MandarCorreoFormatoHTML(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje, string RutaArchivoAdjunto, bool EsFirmaElArchivoAdjunto)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.Subject = Asunto;
            mail.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
            mail.HTMLBody = CuerpoMensaje;
            if (EsFirmaElArchivoAdjunto) { mail.Attachments.Add(RutaArchivoAdjunto, Outlook.OlAttachmentType.olEmbeddeditem, null, "Firma"); }
            else { mail.Attachments.Add(RutaArchivoAdjunto); }
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

        public static void MandarCorreoFHTMLVariosArchivosArjuntos(string Destinatarios, string Copiados, string Asunto, string CuerpoMensaje, List<string> RutaArchivosAdjunto, bool EsFirmaElUltimoArchivoAdjunto)
        {
            int CantidadArchivosAdjuntos, ArchivosAdjuntados;
            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            mail.To = Destinatarios;
            if (!string.IsNullOrEmpty(Copiados)) { mail.BCC = Copiados; }
            mail.Subject = Asunto;
            mail.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
            mail.HTMLBody = CuerpoMensaje;
            ArchivosAdjuntados = 0;
            CantidadArchivosAdjuntos = RutaArchivosAdjunto.Count;
            foreach (string ArchvioAdjuntar in RutaArchivosAdjunto)
            {
                if (EsFirmaElUltimoArchivoAdjunto)
                {
                    if (ArchivosAdjuntados < CantidadArchivosAdjuntos - 1) { mail.Attachments.Add(ArchvioAdjuntar); }
                    else { mail.Attachments.Add(ArchvioAdjuntar, Outlook.OlAttachmentType.olEmbeddeditem, null, "Firma"); }
                    ArchivosAdjuntados++;
                }
                else { mail.Attachments.Add(ArchvioAdjuntar); }
            }
            mail.Importance = Outlook.OlImportance.olImportanceNormal;
            ((Outlook.MailItem)mail).Send();
        }

    }
}
