using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.FP;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace SweetSpotDiscountGolfPOS.Misc
{
    //public class CreatePDF
    //{

    //    public static Document pdfD = new Document();
    //    public static TextFrame addressFrame = new TextFrame();
    //    public static Table table = new Table();
    //    static Invoice invoice = new Invoice();
    //    static LocationManager LM = new LocationManager();


    //    public static Document createDocument(Invoice i, object[] objPageDetails)
    //    {
    //        invoice = i;
    //        pdfD.Info.Title = invoice.varInvoiceNumber;
    //        pdfD.Info.Subject = "Receipt";
    //        pdfD.Info.Author = "SweetSpotDiscountGolf";
    //        DefineStyles();
    //        CreatePage(objPageDetails);
    //        FillContent(objPageDetails);

    //        return pdfD;
    //    }

    //    static void DefineStyles()
    //    {
    //        // Get the predefined style Normal.
    //        Style style = pdfD.Styles["Normal"];
    //        // Because all styles are derived from Normal, the next line changes the
    //        // font of the whole document. Or, more exactly, it changes the font of
    //        // all styles and paragraphs that do not redefine the font.
    //        style.Font.Name = "Verdana";
    //        style = pdfD.Styles[StyleNames.Header];
    //        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
    //        style = pdfD.Styles[StyleNames.Footer];
    //        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
    //        // Create a new style called Table based on style Normal
    //        style = pdfD.Styles.AddStyle("Table", "Normal");
    //        style.Font.Name = "Verdana";
    //        style.Font.Name = "Times New Roman";
    //        style.Font.Size = 9;
    //        // Create a new style called Reference based on style Normal
    //        style = pdfD.Styles.AddStyle("Reference", "Normal");
    //        style.ParagraphFormat.SpaceBefore = "5mm";
    //        style.ParagraphFormat.SpaceAfter = "5mm";
    //        style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
    //    }

    //    static void CreatePage(object[] objPageDetails)
    //    {
    //        // Each MigraDoc document needs at least one section.
    //        Section section = pdfD.AddSection();
    //        // Put a logo in the header
    //        Image image = section.Headers.Primary.AddImage("/Images/combinedLogo.jpg");
    //        image.Height = 2.5; //2.5cm
    //        image.LockAspectRatio = true;
    //        image.RelativeVertical = RelativeVertical.Line;
    //        image.RelativeHorizontal = RelativeHorizontal.Margin;
    //        image.Top = ShapePosition.Top;
    //        image.Left = ShapePosition.Left;
    //        image.WrapFormat.Style = WrapStyle.Through;
    //        // Create footer
    //        Paragraph paragraph = section.Footers.Primary.AddParagraph();
    //        paragraph.AddText(invoice.location.varLocationName + " · " + invoice.location.varAddress + " · "
    //            + invoice.location.varCityName + " " + invoice.location.varPostalCode + " · "
    //            + LM.CallReturnProvinceName(invoice.location.intProvinceID, objPageDetails) + " Canada");
    //        paragraph.Format.Font.Size = 9;
    //        paragraph.Format.Alignment = ParagraphAlignment.Center;

    //        // Create the text frame for the address
    //        addressFrame = section.AddTextFrame();
    //        addressFrame.Height = 3.0; //3.0cm
    //        addressFrame.Width = 7.0; //7.0cm
    //        addressFrame.Left = ShapePosition.Left;
    //        addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
    //        addressFrame.Top = 5.0; //5.0cm
    //        addressFrame.RelativeVertical = RelativeVertical.Page;
    //        // Put sender in address frame
    //        paragraph = addressFrame.AddParagraph("S and G Applications Inc · 943 Dutkowski Cresent · Regina S4N 6X7");
    //        paragraph.Format.Font.Name = "Times New Roman";
    //        paragraph.Format.Font.Size = 7;
    //        paragraph.Format.SpaceAfter = 3;
    //        // Add the print date field
    //        paragraph = section.AddParagraph();
    //        paragraph.Format.SpaceBefore = "8cm";
    //        paragraph.Style = "Reference";
    //        paragraph.AddFormattedText("INVOICE", TextFormat.Bold);
    //        paragraph.AddTab();
    //        paragraph.AddText("Cologne, ");
    //        paragraph.AddDateField("dd.MM.yyyy");
    //        // Create the item table
    //        table = section.AddTable();
    //        table.Style = "Table";
    //        table.Borders.Color = Colors.Black;
    //        table.Borders.Width = 0.25;
    //        table.Borders.Left.Width = 0.5;
    //        table.Borders.Right.Width = 0.5;
    //        table.Rows.LeftIndent = 0;
    //        // Before you can add a row, you must define the columns
    //        Column column = table.AddColumn("1cm");
    //        column.Format.Alignment = ParagraphAlignment.Center;
    //        column = table.AddColumn("2.5cm");
    //        column.Format.Alignment = ParagraphAlignment.Right;
    //        column = table.AddColumn("3cm");
    //        column.Format.Alignment = ParagraphAlignment.Right;
    //        column = table.AddColumn("3.5cm");
    //        column.Format.Alignment = ParagraphAlignment.Right;
    //        column = table.AddColumn("2cm");
    //        column.Format.Alignment = ParagraphAlignment.Center;
    //        column = table.AddColumn("4cm");
    //        column.Format.Alignment = ParagraphAlignment.Right;
    //        // Create the header of the table
    //        Row row = table.AddRow();
    //        row.HeadingFormat = true;
    //        row.Format.Alignment = ParagraphAlignment.Center;
    //        row.Format.Font.Bold = true;
    //        row.Shading.Color = Colors.Blue;
    //        row.Cells[0].AddParagraph("SKU #");
    //        //row.Cells[0].Format.Font.Bold = false;
    //        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
    //        //row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
    //        //row.Cells[0].MergeDown = 1;
    //        row.Cells[1].AddParagraph("Description");
    //        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
    //        //row.Cells[1].MergeRight = 3;
    //        //row = table.AddRow();
    //        //row.HeadingFormat = true;
    //        //row.Format.Alignment = ParagraphAlignment.Center;
    //        //row.Format.Font.Bold = true;
    //        //row.Shading.Color = TableBlue;

    //        row.Cells[1].AddParagraph("Retail Price");
    //        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

    //        row.Cells[2].AddParagraph("Discounts/Bonus Applied");
    //        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;

    //        row.Cells[3].AddParagraph("Quantity");
    //        row.Cells[3].Format.Alignment = ParagraphAlignment.Left;

    //        row.Cells[4].AddParagraph("Sale Price");
    //        row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

    //        row.Cells[5].AddParagraph("Extended Price");
    //        row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
    //        //row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
    //        //row.Cells[5].MergeDown = 1;
    //        table.SetEdge(0, 0, 6, 1, Edge.Box, BorderStyle.Single, 0.75, Colors.Red);
    //    }

    //    static void FillContent(object[] objPageDetails)
    //    {
    //        // Fill address in address text frame
    //        //XPathNavigator item = SelectItem("/invoice/to");
    //        Paragraph paragraph = addressFrame.AddParagraph();
    //        paragraph.AddText(invoice.customer.varFirstName + " " + invoice.customer.varLastName); //Customer Name
    //        paragraph.AddLineBreak();
    //        paragraph.AddText(invoice.customer.varAddress); //Customer Address
    //        paragraph.AddLineBreak();
    //        paragraph.AddText(invoice.customer.varCityName + ", " + LM.CallReturnProvinceName(invoice.customer.intProvinceID, objPageDetails)
    //            + " " + invoice.customer.varPostalCode); //1st PostalCode, 2nd City
    //        // Iterate the invoice items
    //        double totalExtendedPrice = 0;
    //        //XPathNodeIterator iter = this.navigator.Select("/invoice/items/*");
    //        //create iterative list of the items sold
    //        //object[] iter = { "", "", "" };
    //        foreach (InvoiceItems item in invoice.invoiceItems)
    //        {
    //            double quantity = Convert.ToDouble(item.intItemQuantity);
    //            double price = Convert.ToDouble(item.fltItemPrice);
    //            double discount = Convert.ToDouble(item.fltItemDiscount);
    //            // Each item fills two rows
    //            Row row1 = new Row();
    //            row1 = table.AddRow();
    //            //Row row2 = table.AddRow();
    //            row1.TopPadding = 1.5;
    //            row1.Cells[0].Shading.Color = Colors.Gray;
    //            row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
    //            //row1.Cells[0].MergeDown = 1;
    //            row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
    //            //row1.Cells[1].MergeRight = 3;
    //            row1.Cells[5].Shading.Color = Colors.Gray;
    //            //row1.Cells[5].MergeDown = 1;
    //            row1.Cells[0].AddParagraph(item.varSku.ToString()); //SKU
    //            paragraph = row1.Cells[1].AddParagraph();
    //            paragraph.AddFormattedText(item.varItemDescription.ToString(), TextFormat.Bold); //description
    //            //paragraph.AddFormattedText(" by ", TextFormat.Italic);
    //            //paragraph.AddText(item.varAdditionalInformation.ToString()); //this was another part of the description
    //            //row2.Cells[1].AddParagraph(quantity.ToString());
    //            //row2.Cells[2].AddParagraph(price.ToString("0.00"));
    //            //row2.Cells[3].AddParagraph(discount.ToString("0.0"));
    //            //row2.Cells[4].AddParagraph();
    //            //row2.Cells[5].AddParagraph(price.ToString("0.00"));
    //            double extendedPrice = quantity * price;
    //            extendedPrice = extendedPrice * (100 - discount) / 100;
    //            row1.Cells[5].AddParagraph(extendedPrice.ToString("0.00"));
    //            row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
    //            totalExtendedPrice += extendedPrice;
    //            table.SetEdge(0, table.Rows.Count - 2, 6, 1, Edge.Box, BorderStyle.Single, 0.75);
    //        }
    //        // Add an invisible row as a space line to the table
    //        Row row = table.AddRow();
    //        row.Borders.Visible = false;
    //        // Add the total price row
    //        row = table.AddRow();
    //        row.Cells[0].Borders.Visible = false;
    //        row.Cells[0].AddParagraph("Total Price");
    //        row.Cells[0].Format.Font.Bold = true;
    //        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
    //        row.Cells[0].MergeRight = 4;
    //        row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
    //        // Add the VAT row
    //        row = table.AddRow();
    //        row.Cells[0].Borders.Visible = false;
    //        row.Cells[0].AddParagraph("VAT (19%)");
    //        row.Cells[0].Format.Font.Bold = true;
    //        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
    //        row.Cells[0].MergeRight = 4;
    //        row.Cells[5].AddParagraph((0.19 * totalExtendedPrice).ToString("0.00") + " €");
    //        // Add the additional fee row
    //        row = table.AddRow();
    //        row.Cells[0].Borders.Visible = false;
    //        row.Cells[0].AddParagraph("Shipping and Handling");
    //        row.Cells[5].AddParagraph(0.ToString("0.00") + " €");
    //        row.Cells[0].Format.Font.Bold = true;
    //        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
    //        row.Cells[0].MergeRight = 4;
    //        // Add the total due row
    //        row = table.AddRow();
    //        row.Cells[0].AddParagraph("Total Due");
    //        row.Cells[0].Borders.Visible = false;
    //        row.Cells[0].Format.Font.Bold = true;
    //        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
    //        row.Cells[0].MergeRight = 4;
    //        totalExtendedPrice += 0.19 * totalExtendedPrice;
    //        row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
    //        // Set the borders of the specified cell range
    //        table.SetEdge(5, table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);
    //        // Add the notes paragraph
    //        paragraph = pdfD.LastSection.AddParagraph();
    //        paragraph.Format.SpaceBefore = "1cm";
    //        paragraph.Format.Borders.Width = 0.75;
    //        paragraph.Format.Borders.Distance = 3;
    //        paragraph.Format.Borders.Color = Colors.Black;
    //        paragraph.Format.Shading.Color = Colors.Gray;
    //        //item = SelectItem("/invoice"); //not sure if this line is needed??
    //        paragraph.AddText(invoice.varAdditionalInformation); //notes
    //    }
    //}


    class PdfCustomerInvoice
    {
        public void GenerateInvoiceSaveFile(Invoice invoice, object[] objPageDetails)
        {
            string id = invoice.varInvoiceNumber + "-" + invoice.intInvoiceSubNumber.ToString();
            //pdfD.ImagePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");

            Document pdfD = createInvoice(invoice, objPageDetails);
            pdfD.UseCmykColor = true;
            const bool unicode = false;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode);
            pdfRenderer.Document = pdfD.Clone();
            pdfRenderer.RenderDocument();
            // Send PDF to browser
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Desktop\\SweetSpotDiscountGolf_Invoices\\";

            string currYear = invoice.dtmInvoiceDate.Year.ToString(); //DateTime.Today.Year.ToString();
            string currMonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(invoice.dtmInvoiceDate.Month); //(DateTime.Today.Month);
            string currDay = invoice.dtmInvoiceDate.Day.ToString(); //DateTime.Today.Day.ToString();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                path += currYear + "_" + currMonth + "/";
                Directory.CreateDirectory(path);
            }
            else
            {
                path += currYear + "_" + currMonth + "/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    path += "Day_" + currDay + "/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
            id = path + id + ".pdf";
            pdfRenderer.Save(id);
            //View the document
            //Process.Start(id);
        }

        private Document createInvoice(Invoice invoice, object[] objPageDetails)
        {
            Document pdfD = new Document();
            pdfD.Info.Title = invoice.varInvoiceNumber;
            pdfD.Info.Author = "SweetSpotDiscountGolf";
            pdfD = createCustomerInvoice(invoice, pdfD, objPageDetails);
            return pdfD;
        }

        private Document createCustomerInvoice(Invoice invoice, Document pdfD, object[] objPageDetails)
        {
            // Get the predefined style Normal.
            Style style = pdfD.Styles["Normal"];
            style.Font.Color = Colors.Black;
            style.Font.Name = "Verdana";
            style = pdfD.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = pdfD.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            // Create a new style called Table based on style Normal
            style = pdfD.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;
            // Create a new style called Reference based on style Normal
            style = pdfD.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
            //************************************************************************
            //Page layout
            Section section = pdfD.AddSection();
            HeaderFooter header = section.Headers.Primary;
            Table headTable = header.AddTable();
            Column hColumn1 = headTable.AddColumn("8cm");
            hColumn1.Format.Alignment = ParagraphAlignment.Left;
            Column hColumn2 = headTable.AddColumn("8cm");
            hColumn2.Format.Alignment = ParagraphAlignment.Right;
            Row hRow1 = headTable.AddRow();
            hRow1.Cells[0].AddImage(System.Web.HttpContext.Current.Server.MapPath("~/Images/combinedLogo.jpg"));

            Paragraph headP = hRow1.Cells[1].AddParagraph();
            headP.AddText("Invoice Number: " + invoice.varInvoiceNumber + "-" + invoice.intInvoiceSubNumber);
            headP.AddLineBreak();
            headP.AddText("Tax Number: " + invoice.location.varTaxNumber);
            headP.AddLineBreak();
            headP.AddText("Date: " + invoice.dtmInvoiceDate.ToShortDateString() + " " + invoice.dtmInvoiceTime.ToShortTimeString());
            header.Format.Font.Size = 9;
            header.Format.Font.Color = Colors.Black;
            header.Format.Alignment = ParagraphAlignment.Right;
            headTable.AddRow();

            section.AddParagraph();
            Table userInfo = section.AddTable();
            //section.AddParagraph();
            Table invoiceItems = section.AddTable();
            Table invoiceTotals = section.AddTable();
            //section.AddParagraph();
            Table invoiceMops = section.AddTable();
            
            //HeaderFooter header = section.Headers.Primary;
            HeaderFooter footer = section.Footers.Primary;
            //************************************************************************
            //Header - 1:5 ratio
            //image.Height = 50;
            //image.Width = 250;
            //Header
            
            //Footer
            
            footer.AddParagraph("PLEASE NOTE: All used equipment is sold as is and it is understood that its' condition and usability may reflect prior use. "
                + invoice.location.varLocationName + " assumes no responsibility beyond the point of sale. ALL SALES FINAL. Thank you for shopping at " + invoice.location.varLocationName + ".");
            footer.Format.Font.Size = 6;
            footer.Format.Font.Color = Colors.Black;
            footer.Format.Font.Bold = true;
            footer.Format.Alignment = ParagraphAlignment.Center;
            //Aligning the info


            Column uColumn1 = userInfo.AddColumn("8cm");
            uColumn1.Format.Alignment = ParagraphAlignment.Left;
            Column uColumn2 = userInfo.AddColumn("8cm");
            uColumn2.Format.Alignment = ParagraphAlignment.Right;
            userInfo.Format.Font.Color = Colors.Black;
            userInfo.AddRow();
            Row rowInfo = userInfo.AddRow();

            LocationManager LM = new LocationManager();
            //Customer Info
            Paragraph customerInfo = rowInfo.Cells[0].AddParagraph();
            customerInfo.AddLineBreak();
            customerInfo.AddLineBreak();
            customerInfo.AddText(invoice.customer.varFirstName + " " + invoice.customer.varLastName);
            customerInfo.AddLineBreak();
            customerInfo.AddText(invoice.customer.varAddress);
            customerInfo.AddLineBreak();
            customerInfo.AddText(invoice.customer.varCityName + ", " + LM.CallReturnProvinceName(invoice.customer.intProvinceID, objPageDetails) + " " + invoice.customer.varPostalCode);
            customerInfo.AddLineBreak();
            customerInfo.AddText(invoice.customer.varContactNumber);
            customerInfo.AddLineBreak();
            customerInfo.AddLineBreak();

            //Location Info
            Paragraph locationInfo = rowInfo.Cells[1].AddParagraph();
            locationInfo.AddLineBreak();
            locationInfo.AddLineBreak();
            locationInfo.AddText(invoice.location.varLocationName);
            locationInfo.AddLineBreak();
            locationInfo.AddText(invoice.location.varAddress);
            locationInfo.AddLineBreak();
            locationInfo.AddText(invoice.location.varCityName + ", " + LM.CallReturnProvinceName(invoice.location.intProvinceID, objPageDetails) + " " + invoice.location.varPostalCode);
            locationInfo.AddLineBreak();
            locationInfo.AddText(invoice.location.varContactNumber);
            locationInfo.AddLineBreak();
            locationInfo.AddLineBreak();



            //invoiceItems table
            invoiceItems.Style = "Table";
            invoiceItems.Borders.Color = Colors.Black;
            invoiceItems.Borders.Width = 0.25;
            invoiceItems.Borders.Left.Width = 0.5;
            invoiceItems.Borders.Right.Width = 0.5;
            invoiceItems.Rows.LeftIndent = 0;
            invoiceItems.Format.Font.Color = Colors.Black;
            //invoiceItems columns
            //16.25
            Column column = invoiceItems.AddColumn("2.5cm");        //SKU
            column.Format.Alignment = ParagraphAlignment.Center;
            column = invoiceItems.AddColumn("3.75cm");              //Description
            column.Format.Alignment = ParagraphAlignment.Right;
            column = invoiceItems.AddColumn("2cm");                 //Retail Price
            column.Format.Alignment = ParagraphAlignment.Right;
            column = invoiceItems.AddColumn("2cm");                 //Discount
            column.Format.Alignment = ParagraphAlignment.Right;
            column = invoiceItems.AddColumn("2cm");                 //Quantity
            column.Format.Alignment = ParagraphAlignment.Right;
            column = invoiceItems.AddColumn("2cm");                 //Price
            column.Format.Alignment = ParagraphAlignment.Center;
            column = invoiceItems.AddColumn("2cm");                 //Extended Price
            column.Format.Alignment = ParagraphAlignment.Right;
            // Create the header of the table
            Row itemRow = invoiceItems.AddRow();
            itemRow.HeadingFormat = true;
            itemRow.Format.Alignment = ParagraphAlignment.Center;
            itemRow.Format.Font.Bold = true;

            FillHeaders(invoice.intInvoiceSubNumber, itemRow);

            invoiceItems.SetEdge(0, 0, 6, 1, Edge.Box, BorderStyle.Single, 0.75, Colors.Black);
            //Adding the items to the invoiceItems table
            double totalInvoicePrice = 0;
            foreach (InvoiceItems item in invoice.invoiceItems)
            {
                totalInvoicePrice += FillItemCalculations(invoice, invoiceItems, item);
            }

            invoiceItems.SetEdge(0, invoiceItems.Rows.Count - 2, 6, 1, Edge.Box, BorderStyle.Single, 0.75);
            //invoiceTotals table
            invoiceTotals.Style = "Table";
            invoiceTotals.Format.Font.Color = Colors.Black;
            Column totalsColumn = invoiceTotals.AddColumn("11.25cm");
            totalsColumn.Format.Alignment = ParagraphAlignment.Center;
            totalsColumn = invoiceTotals.AddColumn("2.5cm");
            totalsColumn.Format.Alignment = ParagraphAlignment.Center;
            totalsColumn = invoiceTotals.AddColumn("2.5cm");
            totalsColumn.Format.Alignment = ParagraphAlignment.Right;
            Row totalsRow = invoiceTotals.AddRow();
            totalsRow.Borders.Visible = false;

            //Discounts
            totalsRow = invoiceTotals.AddRow();
            totalsRow.Cells[1].Borders.Visible = false;
            totalsRow.Cells[1].AddParagraph("Discounts");
            totalsRow.Cells[1].Format.Font.Bold = true;
            totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            totalsRow.Cells[2].AddParagraph(invoice.fltTotalDiscount.ToString("C"));
            //TradeIn
            totalsRow = invoiceTotals.AddRow();
            totalsRow.Cells[1].Borders.Visible = false;
            totalsRow.Cells[1].AddParagraph("Trade-Ins");
            totalsRow.Cells[1].Format.Font.Bold = true;
            totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            totalsRow.Cells[2].AddParagraph(invoice.fltTotalTradeIn.ToString("C"));
            //Shipping
            totalsRow = invoiceTotals.AddRow();
            totalsRow.Cells[1].Borders.Visible = false;
            totalsRow.Cells[1].AddParagraph("Shipping");
            totalsRow.Cells[1].Format.Font.Bold = true;
            totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            totalsRow.Cells[2].AddParagraph(invoice.fltShippingCharges.ToString("C"));

            invoiceTotals.AddRow();

            //SubTotal
            totalsRow = invoiceTotals.AddRow();
            totalsRow.Cells[1].Borders.Visible = false;
            totalsRow.Cells[1].AddParagraph("Subtotal");
            totalsRow.Cells[1].Format.Font.Bold = true;
            totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            totalsRow.Cells[2].AddParagraph((invoice.fltSubTotal + invoice.fltShippingCharges).ToString("C"));

            //Taxes
            double governmentTax = 0;
            string governmentTaxName = "";
            double provincialTax = 0;
            string provincialTaxName = "";
            double liquorTax = 0;
            string liquorTaxName = "";

            foreach (var invoiceItem in invoice.invoiceItems)
            {
                foreach (var invoiceItemTax in invoiceItem.invoiceItemTaxes)
                {
                    if (invoiceItemTax.intTaxTypeID == 1 || invoiceItemTax.intTaxTypeID == 3)
                    {
                        if (invoiceItemTax.bitIsTaxCharged)
                        {
                            governmentTax += invoiceItemTax.fltTaxAmount;
                            governmentTaxName = invoiceItemTax.varTaxName;
                        }
                    }
                    else if (invoiceItemTax.intTaxTypeID == 2 || invoiceItemTax.intTaxTypeID == 4 || invoiceItemTax.intTaxTypeID == 5)
                    {
                        if (invoiceItemTax.bitIsTaxCharged)
                        {
                            provincialTax += invoiceItemTax.fltTaxAmount;
                            provincialTaxName = invoiceItemTax.varTaxName;
                        }
                    }
                    else if (invoiceItemTax.intTaxTypeID == 6)
                    {
                        if (invoiceItemTax.bitIsTaxCharged)
                        {
                            liquorTax += invoiceItemTax.fltTaxAmount;
                            liquorTaxName = invoiceItemTax.varTaxName;
                        }
                    }
                }
            }

            if (governmentTax != 0)
            {
                totalsRow = invoiceTotals.AddRow();
                totalsRow.Cells[1].Borders.Visible = false;
                totalsRow.Cells[1].AddParagraph(governmentTaxName);
                totalsRow.Cells[1].Format.Font.Bold = true;
                totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                totalsRow.Cells[2].AddParagraph(governmentTax.ToString("C"));
                totalInvoicePrice += governmentTax;
            }
            if (provincialTax != 0)
            {
                totalsRow = invoiceTotals.AddRow();
                totalsRow.Cells[1].Borders.Visible = false;
                totalsRow.Cells[1].AddParagraph(provincialTaxName);
                totalsRow.Cells[1].Format.Font.Bold = true;
                totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                totalsRow.Cells[2].AddParagraph(provincialTax.ToString("C"));
                totalInvoicePrice += provincialTax;
            }
            if (liquorTax != 0)
            {
                totalsRow = invoiceTotals.AddRow();
                totalsRow.Cells[1].Borders.Visible = false;
                totalsRow.Cells[1].AddParagraph(liquorTaxName);
                totalsRow.Cells[1].Format.Font.Bold = true;
                totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                totalsRow.Cells[2].AddParagraph(liquorTax.ToString("C"));
                totalInvoicePrice += liquorTax;
            }

            //Total Paid
            totalsRow = invoiceTotals.AddRow();
            totalsRow.Cells[1].AddParagraph("Total Paid");
            totalsRow.Cells[1].Borders.Visible = false;
            totalsRow.Cells[1].Format.Font.Bold = true;
            totalsRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            totalsRow.Cells[2].AddParagraph(totalInvoicePrice.ToString("C"));

            invoiceTotals.AddRow();
            
            //invoiceMops table
            invoiceMops.Style = "Table";
            invoiceMops.Format.Font.Color = Colors.Black;
            Column mopsColumn = invoiceMops.AddColumn("11.25cm");
            mopsColumn.Format.Alignment = ParagraphAlignment.Right;
            mopsColumn = invoiceMops.AddColumn("2.5cm");
            mopsColumn.Format.Alignment = ParagraphAlignment.Right;
            mopsColumn = invoiceMops.AddColumn("2.5cm");
            mopsColumn.Format.Alignment = ParagraphAlignment.Right;
            Row mopRow = invoiceMops.AddRow();
            mopRow.HeadingFormat = true;
            mopRow.Format.Alignment = ParagraphAlignment.Right;
            mopRow.Format.Font.Bold = true;
            mopRow.Cells[1].AddParagraph("Payment Type");
            mopRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            mopRow.Cells[2].AddParagraph("Amount Paid");
            mopRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            //Adding the mops to the invoiceMops table
            foreach (InvoiceMOPs mop in invoice.invoiceMops)
            {
                Row row = new Row();
                row = invoiceMops.AddRow();
                row.TopPadding = 1.5;
                Paragraph methodsOfPayment = section.AddParagraph();
                row.Cells[1].AddParagraph(mop.varPaymentName.ToString());
                row.Cells[2].AddParagraph(mop.fltAmountPaid.ToString("C"));
            }

            //Notes
            Paragraph invoiceNotes = pdfD.LastSection.AddParagraph();
            invoiceNotes.Format.Font.Color = Colors.Black;
            invoiceNotes.Format.SpaceBefore = "1cm";
            invoiceNotes.Format.Borders.Width = 0.75;
            invoiceNotes.Format.Borders.Distance = 3;
            invoiceNotes.Format.Borders.Color = Colors.Black;
            invoiceNotes.Format.Shading.Color = Colors.LightGray;
            invoiceNotes.AddText("Comments: " + invoice.varAdditionalInformation);

            return pdfD;
        }

        private void FillHeaders(int invoiceSub, Row itemRow)
        {
            ParagraphAlignment pa = ParagraphAlignment.Left;

            string desc1 = "Retail Price";
            string desc2 = "Discounts/ Bonus Applied";
            string desc3 = "Retail Price";

            if (invoiceSub > 1)
            {
                desc1 = "Sold At";
                desc2 = "Non Refundable";
                desc3 = "Returned At";
            }

            itemRow.Cells[0].AddParagraph("SKU #");
            itemRow.Cells[1].AddParagraph("Description");
            itemRow.Cells[2].AddParagraph(desc1);
            itemRow.Cells[3].AddParagraph(desc2);
            itemRow.Cells[4].AddParagraph("Quantity");
            itemRow.Cells[5].AddParagraph(desc3);
            itemRow.Cells[6].AddParagraph("Extended Price");

            itemRow.Cells[0].Format.Alignment = pa;
            itemRow.Cells[1].Format.Alignment = pa;
            itemRow.Cells[2].Format.Alignment = pa;
            itemRow.Cells[3].Format.Alignment = pa;
            itemRow.Cells[4].Format.Alignment = pa;
            itemRow.Cells[5].Format.Alignment = pa;
            itemRow.Cells[6].Format.Alignment = pa;
        }
        private double FillItemCalculations(Invoice invoice, Table invoiceItems, InvoiceItems item)
        {
            double quantity = Convert.ToDouble(item.intItemQuantity);

            double priceSold = 0;
            double discountNonRefund = 0;
            double retailReturned = 0;
            double extendedPrice = 0;

            if (invoice.intInvoiceSubNumber > 1)
            {
                if (item.bitIsDiscountPercent)
                {
                    priceSold = (item.fltItemDiscount / 100) * item.fltItemPrice;
                }
                else
                {
                    priceSold = item.fltItemPrice - item.fltItemDiscount;
                }
                discountNonRefund = priceSold + item.fltItemRefund;
                retailReturned = item.fltItemRefund;
            }
            else
            {
                priceSold = Convert.ToDouble(item.fltItemPrice);
                if (item.bitIsDiscountPercent)
                {
                    discountNonRefund = Math.Floor(priceSold * (item.fltItemDiscount / 100));
                }
                else
                {
                    discountNonRefund = item.fltItemDiscount;
                }
                retailReturned = priceSold - discountNonRefund;
            }
            extendedPrice = retailReturned * quantity;

            Row itemDetailsRow = new Row();
            itemDetailsRow = invoiceItems.AddRow();
            itemDetailsRow.TopPadding = 1.5;
            itemDetailsRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            itemDetailsRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            itemDetailsRow.Cells[0].AddParagraph(item.varSku.ToString());           //SKU
            itemDetailsRow.Cells[1].AddParagraph(item.varItemDescription.ToString());   //Description
            itemDetailsRow.Cells[2].AddParagraph(priceSold.ToString("C"));           //Price
            itemDetailsRow.Cells[3].AddParagraph(discountNonRefund.ToString("C"));         //Discount
            itemDetailsRow.Cells[4].AddParagraph(quantity.ToString());              //Quantity
            itemDetailsRow.Cells[5].AddParagraph(retailReturned.ToString("C"));       //Retail Price
            itemDetailsRow.Cells[6].AddParagraph(extendedPrice.ToString("C"));   //Extended Price
            return extendedPrice;
        }
    }
}
