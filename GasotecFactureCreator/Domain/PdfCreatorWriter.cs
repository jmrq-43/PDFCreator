using System;
using System.Globalization;
using System.IO;
using System.Windows;
using iTextSharp.text.pdf;

namespace GasotecFactureCreator.Domain;

public class PdfCreatorWriter
{
    public void OverwritePdf(string outputFile, SalesChecker salesChecker, List<ServiceDomain> service,
        Dictionary<string, Tuple<float, float>> coordenate)
    {
        try
        {
            string resourcePath = "resources/templategasotec.pdf".ToLower();
            Uri resourceUri = new Uri(resourcePath, UriKind.Relative);

            Stream templateStream = Application.GetResourceStream(resourceUri)?.Stream;

            if (templateStream == null)
            {
                MessageBox.Show($"No se encontró la plantilla PDF en: {resourcePath}");
                return;
            }

            using (PdfReader lector = new PdfReader(templateStream))
            {
                using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write,
                           FileShare.None))
                {
                    using (PdfStamper stamper = new PdfStamper(lector, fs))
                    {
                        PdfContentByte contentByte = stamper.GetOverContent(1);

                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA,
                            BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12);

                        InsertData(contentByte, salesChecker.Name, coordenate["NOMBRE"]);
                        InsertData(contentByte, salesChecker.Address, coordenate["LOCATION"]);
                        InsertData(contentByte, salesChecker.Email, coordenate["EMAIL"]);
                        InsertData(contentByte, salesChecker.PhoneNumber.ToString(), coordenate["PHONE"]);
                        InsertData(contentByte, salesChecker.Nit, coordenate["NIT"]);
                        InsertData(contentByte, salesChecker.Balance.ToString(), coordenate["BALANCE"]);
                        InsertData(contentByte, salesChecker.Payment.ToString(), coordenate["PAYMENT"]);
                        InsertData(contentByte, salesChecker.Total.ToString(), coordenate["TOTAL"]);

                        if (service != null && service.Count > 0)
                        {
                            for (int i = 0; i < service.Count; i++)
                            {
                                float serviceY = coordenate["SERVICE"].Item2 - (i * 15);
                                float serviceDescriptionY = coordenate["SERVICEDESCRIPTION"].Item2 - (i * 15);
                                float servicePriceY = coordenate["SERVICEPRICE"].Item2 - (i * 15);

                                InsertData(contentByte, service[i].serviceType, new Tuple<float, float>(coordenate["SERVICE"].Item1, serviceY));
                                InsertData(contentByte, service[i].serviceDescription, new Tuple<float, float>(coordenate["SERVICEDESCRIPTION"].Item1, serviceDescriptionY));
                                InsertData(contentByte, service[i].servicePrice.ToString(CultureInfo.CurrentCulture), new Tuple<float, float>(coordenate["SERVICEPRICE"].Item1, servicePriceY));
                            }
                        }
                        contentByte.EndText();
                    }
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            throw;
        }
    }

    public void InsertData(PdfContentByte contentByte, string value, Tuple<float, float> coordenate)
    {
        if (!string.IsNullOrEmpty(value))
        {
            contentByte.SetTextMatrix(coordenate.Item1, coordenate.Item2);
            contentByte.ShowText(value);
        }
    }
}