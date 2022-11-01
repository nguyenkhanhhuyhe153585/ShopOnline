using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using ShopOnline.Models;
using ShopOnline.SignalRLab;

namespace ShopOnline.Pages.Admin.Orders
{
    public class ExportModel : PageModel
    {

        private readonly IWebHostEnvironment _env;
        private readonly PRN221DBContext dBContext;
 

        public ExportModel(IWebHostEnvironment _env, PRN221DBContext dBContext)
        {
            this._env = _env;
            this.dBContext = dBContext;
        }
        public IActionResult OnGet()
        {
            var orders = dBContext.Orders.ToList();

            //Create new Excel Workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel Sheet
            var sheet = workbook.CreateSheet();

            ////(Optional) set the width of the columns
            //sheet.SetColumnWidth(0, 20 * 256);//Title
            //sheet.SetColumnWidth(1, 20 * 256);//Section
            //sheet.SetColumnWidth(2, 20 * 256);//Views
            //sheet.SetColumnWidth(3, 20 * 256);//Downloads
            //sheet.SetColumnWidth(4, 20 * 256);//Status

            //Create a header row
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("OrderID");
            headerRow.CreateCell(1).SetCellValue("CustomerID");
            headerRow.CreateCell(2).SetCellValue("EmployeeID");
            headerRow.CreateCell(3).SetCellValue("OrderDate");
            headerRow.CreateCell(4).SetCellValue("RequiredDate");
            headerRow.CreateCell(5).SetCellValue("ShippedDate");
            headerRow.CreateCell(6).SetCellValue("Freight");
            headerRow.CreateCell(7).SetCellValue("ShipName");
            headerRow.CreateCell(8).SetCellValue("ShipAddress");
            headerRow.CreateCell(9).SetCellValue("ShipCity");
            headerRow.CreateCell(10).SetCellValue("ShipRegion");
            headerRow.CreateCell(11).SetCellValue("ShipPostalCode");
            headerRow.CreateCell(12).SetCellValue("ShipCountry");


            ////(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data

            foreach (Order order in orders)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(order.OrderId);
                row.CreateCell(1).SetCellValue(order.CustomerId);
                row.CreateCell(2).SetCellValue(order.EmployeeId.ToString());
                row.CreateCell(3).SetCellValue(order.OrderDate.ToString());
                row.CreateCell(4).SetCellValue(order.RequiredDate.ToString());
                row.CreateCell(5).SetCellValue(order.ShippedDate.ToString());
                row.CreateCell(6).SetCellValue(order.Freight.ToString());
                row.CreateCell(7).SetCellValue(order.ShipName);
                row.CreateCell(8).SetCellValue(order.ShipAddress);
                row.CreateCell(8).SetCellValue(order.ShipCity);
                row.CreateCell(8).SetCellValue(order.ShipRegion);
                row.CreateCell(8).SetCellValue(order.ShipPostalCode);
                row.CreateCell(8).SetCellValue(order.ShipCountry);

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel", //MIME type of Excel files
             "Orders.xls");
        }
    }   
}
