using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ShopOnline.Models;
using ShopOnline.SignalRLab;
using System.Text;

namespace ShopOnline.Pages.Admin.Products
{
    public class UploadModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly PRN221DBContext dBContext;
        private IHubContext<SignalrServer> signalR;

        public UploadModel(IWebHostEnvironment _env, PRN221DBContext dBContext, IHubContext<SignalrServer> signalR)
        {
            this._env = _env;
            this.dBContext = dBContext;
            this.signalR = signalR;
        }


        public IActionResult OnGet()
        {
            var products = dBContext.Products.Include(e => e.Category).ToList();

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
            headerRow.CreateCell(0).SetCellValue("ProductID");
            headerRow.CreateCell(1).SetCellValue("ProductName");
            headerRow.CreateCell(2).SetCellValue("Category");
            headerRow.CreateCell(3).SetCellValue("QuantityPerUnit");
            headerRow.CreateCell(4).SetCellValue("UnitPrice");
            headerRow.CreateCell(5).SetCellValue("UnitsInStock");
            headerRow.CreateCell(6).SetCellValue("UnitsOnOrder");
            headerRow.CreateCell(7).SetCellValue("ReorderLevel");
            headerRow.CreateCell(8).SetCellValue("Discontinued");


            ////(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data

            foreach (Product product in products)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(product.ProductId);
                row.CreateCell(1).SetCellValue(product.ProductName);
                row.CreateCell(2).SetCellValue(product.Category.CategoryName);
                row.CreateCell(3).SetCellValue(product.QuantityPerUnit);
                row.CreateCell(4).SetCellValue(product.UnitPrice.ToString());
                row.CreateCell(5).SetCellValue(product.UnitsInStock.ToString());
                row.CreateCell(6).SetCellValue(product.UnitsOnOrder.ToString());
                row.CreateCell(7).SetCellValue(product.ReorderLevel.ToString());
                row.CreateCell(8).SetCellValue(product.Discontinued);

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel", //MIME type of Excel files
             "Products.xls");
        }

        async public Task<IActionResult> OnPost()
        {
            IFormFile file = Request.Form.Files[0];

            string folderName = "UploadExcel";

            string webRootPath = _env.WebRootPath;

            string newPath = Path.Combine(webRootPath, folderName);

            StringBuilder sb = new StringBuilder();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                ISheet sheet;

                string fullPath = Path.Combine(newPath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))

                {

                    file.CopyTo(stream);

                    stream.Position = 0;

                    if (sFileExtension == ".xls")

                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats                        
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format                        
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook                     
                    }

                    IRow headerRow = sheet.GetRow(0); //Get Header Row

                    int cellCount = headerRow.LastCellNum;

                    sb.Append("Product List:\n");

                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        sb.Append(cell.ToString() + " | ");
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                            sb.Append("     " + " | ");

                    }
                    sb.Append("\n");
                    StringBuilder message = new StringBuilder();
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File                        
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        Product newProduct = new Product();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                ICell cell = row.GetCell(j);

                                switch (j)
                                {
                                    case 0:
                                        if (cell.ToString().Length == 0)
                                            message.Append("ProductName do not empty\n");
                                        else
                                        {
                                            newProduct.ProductName = cell.ToString();
                                        }
                                        break;
                                    case 1:
                                        if (cell.ToString().Length == 0)
                                        {
                                            message.Append("CategoryId do not empty\n");
                                        }
                                        else
                                        {
                                            newProduct.CategoryId = int.Parse(cell.ToString());
                                        }
                                        break;
                                    case 2:
                                        if (cell.ToString().Length == 0)
                                        {
                                            message.Append("QuantityPerUnit do not empty\n");
                                        }
                                        else
                                        {
                                            newProduct.QuantityPerUnit = cell.ToString();
                                        }
                                        break;
                                    case 3:
                                        if (cell.ToString().Length == 0)
                                        {
                                            message.Append("UnitPrice do not empty\n");
                                        }
                                        else
                                        {
                                            newProduct.UnitPrice = decimal.Parse(cell.ToString());
                                        }
                                        break;
                                    case 4:
                                        if (cell.ToString().Length == 0)
                                        {
                                            message.Append("UnitInStock do not empty\n");
                                        }
                                        else
                                        {
                                            newProduct.UnitsInStock = short.Parse(cell.ToString());
                                        }
                                        break;
                                    case 5:
                                        if (cell.ToString().Length == 0)
                                        {

                                        }
                                        else
                                        {
                                            newProduct.UnitsOnOrder = short.Parse(cell.ToString());
                                        }
                                        break;
                                    case 6:
                                        if (cell.ToString().Length == 0)
                                        {

                                        }
                                        else
                                        {
                                            newProduct.ReorderLevel = short.Parse(cell.ToString());
                                        }
                                        break;
                                    case 7:
                                        if (cell.ToString().Length == 0)
                                        {
                                            message.Append("Discontinued do not empty\n");
                                        }
                                        else
                                        {
                                            newProduct.Discontinued = cell.ToString().Equals("TRUE");
                                        }
                                        break;
                                }
                                sb.Append(row.GetCell(j).ToString() + " | ");
                            }
                        }
                        sb.Append("\n");
                        await dBContext.Products.AddAsync(newProduct);
                    }
                    await dBContext.SaveChangesAsync();
                    await signalR.Clients.All.SendAsync("LoadProductOnChange");
                    Console.WriteLine(sb);
                }
            }
            return Page();
        }
    }
}
