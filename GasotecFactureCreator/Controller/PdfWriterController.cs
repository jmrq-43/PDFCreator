using GasotecFactureCreator.Domain;
using System.Collections.Generic;

namespace GasotecFactureCreator.Controller;

public class PdfWriterController
{
    private PdfCreatorWriter _pdfWriter;

    public PdfWriterController()
    {
        _pdfWriter = new PdfCreatorWriter();
    }

    public void GeneratePdf(string pdfSalidaDirectorio, string pdfNombreArchivo, SalesChecker salesChecker,
        List<ServiceDomain> servicios, Dictionary<string, Tuple<float, float>> coordenadas)
    {
        _pdfWriter.OverwritePdf(pdfSalidaDirectorio, pdfNombreArchivo, salesChecker, servicios, coordenadas);
    }
}