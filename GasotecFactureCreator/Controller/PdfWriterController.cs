﻿using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Controller;

public class PdfWriterController
{
    private PdfCreatorWriter _pdfWriter;

    public PdfWriterController()
    {
        _pdfWriter = new PdfCreatorWriter();
    }

    public void GenerarPdf( string pdfSalida, SalesChecker salesChecker, List<Domain.ServiceDomain> servicios, Dictionary<string, Tuple<float, float>> coordenadas)
    {
        _pdfWriter.OverwritePdf(pdfSalida, salesChecker, servicios, coordenadas);
    }
}