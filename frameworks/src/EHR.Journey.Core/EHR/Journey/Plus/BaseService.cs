using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Aspose.Email.Clients.Smtp;
using Aspose.Email;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Reporting;
using Aspose.Pdf.Text;
using Microsoft.AspNetCore.Http;
using Aspose.Words.Drawing;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using EHR.Journey.Core;
using Microsoft.AspNetCore.Authorization;

namespace EHR.Journey.Core
{
    //[Authorize]
    [ServiceFilter(typeof(BaseFilter))]
    [WrapResult]
    public class BaseService<T, D, E> : ApplicationService, IApplicationService
         where T : BaseEntity<T>
     where D : DbContext
     where E : BaseInput
    {
        public readonly IRepository<T, Guid> _repository;
        public readonly ISqlExecuter<D> _db;
        //private readonly IDistributedEventBus _distributedEventBus;
        protected IFreeSql FreeSql => LazyServiceProvider.LazyGetRequiredService<IFreeSql>();
        public BaseService(IRepository<T, Guid> repository, ISqlExecuter<D> db)
        {
            _repository = repository;
            _db = db;
            //_distributedEventBus = distributedEventBus;
        }

       

        public string Test()
        {
            var dt = _db.ExecuteTable("SELECT * FROM AbpUsers");
            var x = JsonConvert.SerializeObject(dt);
            return x;
        }
        public async Task<List<UserOutput>> FreeTest()
        {
            var sql = "select id from AbpUsers";
            var result = await FreeSql.Select<UserOutput>()
            .WithSql(sql)
            .ToListAsync();
            return result;
        }

        public virtual Task<T> InsertAsync(E e)
        {
            var t = ObjectMapper.Map<E, T>(e);
            return _repository.InsertAsync(t);
        }

        public virtual Task InsertManyAsync(List<E> e)
        {
            var t = ObjectMapper.Map<List<E>, List<T>>(e);
            return _repository.InsertManyAsync(t);
        }


        public virtual Task<T> GetAsync(Guid id)
        {
            return _repository.GetAsync(id);
        }

        public virtual Task<List<T>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

 
        public virtual Task<T> UpdateAsync(T t)
        {
            return _repository.UpdateAsync(t);
        }

        public virtual Task UpdateManyAsync(List<T> t)
        {
            return _repository.UpdateManyAsync(t);
        }

        public virtual Task DeleteAsync(T t)
        {
            return _repository.DeleteAsync(t);
        }

        public virtual Task DeleteByIdAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }

        public virtual Task<List<T>> GetPagedListAsync(PagedAndSortedResultRequestDto input)
        {
            return _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, false);
        }

        //public  async Task<DataSourceResult> KendoListAsync([DataSourceRequest] DataSourceRequest request)
        //{
        //    return await _repository.GetQueryableAsync().Result.ToDataSourceResultAsync(request);
        //}

        protected virtual SmtpClient GetSmtpClient()
        {
            return new SmtpClient();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        public async virtual Task SendEmailAsync(List<string> to, List<string> cc, List<string> bcc, string title, string body, List<string> atts)
        {
            using (var client = GetSmtpClient())
            {
                MailMessage mailMessage = new MailMessage();
                if (to != null)
                {
                    foreach (var toPerson in to)
                    {
                        mailMessage.To.Add(new MailAddress(toPerson));
                    }
                }
                if (cc != null)
                {
                    foreach (var ccPerson in cc)
                    {
                        mailMessage.CC.Add(new MailAddress(ccPerson));
                    }
                }
                if (bcc != null)
                {
                    foreach (var bccPerson in bcc)
                    {
                        mailMessage.Bcc.Add(new MailAddress(bccPerson));
                    }
                }
                if (atts != null)
                {
                    foreach (var att in atts)
                    {
                        Attachment attachment = new Attachment(att);
                        mailMessage.Attachments.Add(attachment);
                    }
                }
                mailMessage.Subject = title;
                mailMessage.HtmlBody = body;
                await client.SendAsync(mailMessage);

            }

        }
        /// <summary>
        /// 根据模板生成文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tempPath"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        protected virtual string GetFileFormTmeplate(string fileName, string tempPath, DataSet ds, string passWord, string wmText)
        {
            var fileType = GetContentType(tempPath);
            var saveType = GetContentType(fileName);
            if (ds.Tables["DT"] == null)
            {
                ds.Tables[0].TableName = "DT";
            }
            if (fileType == MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet)
            {
                var wb = new Workbook(tempPath);
                WorkbookDesigner designer = new WorkbookDesigner(wb);
                designer.SetDataSource(ds);
                designer.Process();
                if (!string.IsNullOrWhiteSpace(wmText))
                {
                    //excel添加水印
                    Aspose.Cells.Drawing.Shape wordart = designer.Workbook.Worksheets[0].Shapes
                        .AddTextEffect(Aspose.Cells.Drawing.MsoPresetTextEffect.TextEffect1, wmText, "Arial Black", 60, false, true, 1, 8, 1, 1, 130, 500);
                }
                if (!string.IsNullOrWhiteSpace(passWord))
                {
                    wb.SetEncryptionOptions(EncryptionType.StrongCryptographicProvider, 128);
                    wb.Settings.Password = passWord;
                }
                if (saveType == "application/pdf")
                {
                    designer.Workbook.Save(fileName, Aspose.Cells.SaveFormat.Pdf);
                }
                if (saveType == "text/html")
                {
                    Aspose.Cells.HtmlSaveOptions hso = new Aspose.Cells.HtmlSaveOptions();
                    hso.ExportImagesAsBase64 = true;
                    hso.ExportHiddenWorksheet = false;
                    hso.ExportActiveWorksheetOnly = true;
                    hso.ExportPrintAreaOnly = true;
                    designer.Workbook.Save(fileName, hso);
                }
                else
                {
                    designer.Workbook.Save(fileName, Aspose.Cells.SaveFormat.Xlsx);
                }

            }
            else if (fileType == MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument)
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(tempPath);

                if (ds != null)
                {
                    ReportingEngine engine = new ReportingEngine();
                    engine.BuildReport(doc, ds);
                    doc.MailMerge.TrimWhitespaces = true;
                    doc.MailMerge.UseNonMergeFields = true;
                    doc.MailMerge.CleanupOptions = Aspose.Words.MailMerging.MailMergeCleanupOptions.RemoveUnusedRegions;
                    doc.MailMerge.FieldMergingCallback = new HandleMergeFieldInsertHtml();
                    doc.MailMerge.ExecuteWithRegions(ds);
                    Aspose.Words.Saving.OoxmlSaveOptions docSaveOptions = new Aspose.Words.Saving.OoxmlSaveOptions();
                    if (!string.IsNullOrWhiteSpace(wmText))
                    {
                        AddWaterMark(doc, wmText);
                    }
                    if (!string.IsNullOrWhiteSpace(passWord))
                    {
                        docSaveOptions.Password = passWord;
                    }

                    if (saveType == "application/pdf")
                    {
                        docSaveOptions.SaveFormat = Aspose.Words.SaveFormat.Pdf;

                        doc.Save(fileName, docSaveOptions);
                    }
                    if (saveType == "text/html")
                    {
                        Aspose.Words.Saving.HtmlSaveOptions hso = new Aspose.Words.Saving.HtmlSaveOptions();
                        hso.ExportImagesAsBase64 = true;
                        hso.ExportHeadersFootersMode = Aspose.Words.Saving.ExportHeadersFootersMode.None;
                        hso.PrettyFormat = true;
                        hso.MetafileFormat = Aspose.Words.Saving.HtmlMetafileFormat.EmfOrWmf;
                        hso.ExportListLabels = Aspose.Words.Saving.ExportListLabels.AsInlineText;
                        hso.ExportRoundtripInformation = true;
                        hso.SaveFormat = Aspose.Words.SaveFormat.Html;
                        doc.Save(fileName, hso);
                    }
                    else
                    {
                        docSaveOptions.SaveFormat = Aspose.Words.SaveFormat.Docx;
                        doc.Save(fileName, docSaveOptions);
                    }
                }


            }
            else if (fileType == MimeTypeNames.ApplicationPdf)
            {
                if (saveType != MimeTypeNames.ApplicationPdf)
                {
                    fileName = tempPath;
                }
                else
                {
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(tempPath);
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        using (DataTable dt = ds.Tables["DT"])
                        {
                            foreach (DataColumn dataColumn in dt.Columns)
                            {
                                TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber("{{" + dataColumn.ColumnName + "}}");
                                pdfDocument.Pages.Accept(textFragmentAbsorber);
                                TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;
                                foreach (TextFragment textFragment in textFragmentCollection)
                                {
                                    textFragment.Text = dt.Rows[0][dataColumn] == null ? "" : dt.Rows[0][dataColumn].ToString();
                                }
                            }

                            TextFragmentAbsorber textFragmentAbsorberSign = new TextFragmentAbsorber("{{SignDate}}");
                            pdfDocument.Pages.Accept(textFragmentAbsorberSign);
                            TextFragmentCollection textFragmentCollectionSign = textFragmentAbsorberSign.TextFragments;
                            foreach (TextFragment textFragment in textFragmentCollectionSign)
                            {
                                textFragment.Text = DateTime.Now.ToString("yyyy-MM-dd");
                            }
                        }
                        pdfDocument.Save(fileName, Aspose.Pdf.SaveFormat.Pdf);
                    }
                    else
                    {
                        fileName = tempPath;
                    }
                }
            }
            else
            {
                fileName = tempPath;
            }
            return fileName;
        }

        protected virtual string Import<F>(string filePath) where F : class
        {
            var ds = ReadExcelToDataSet(filePath);
            try
            {
                if (ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    var list = DataTableToEntity<T>(dt) as List<T>;
                    //SqlExecuter.BulkInsertOrUpdate<T>(list);
                    return filePath;
                }
                else
                {
                    return filePath;
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }


        protected virtual string UploadFile(IFormFile file, string savePath)
        {
            if (file != null)
            {
                var fileDir = savePath;
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                //文件名称
                string fileName = file.FileName;

                //上传的文件的路径
                string filePath = fileDir + $@"\{fileName}";
                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return "OK";
            }
            else
            {
                return "NO";
            }
        }



        protected virtual List<F> DataTableToEntity<F>(DataTable dt) where F : class
        {

            var json = JsonConvert.SerializeObject(dt);
            var list = JsonConvert.DeserializeObject<List<F>>(json);
            return list;

        }

        public static DataTable ToDataTable(string path)
        {
            FileInfo excelFile = new FileInfo(path);
            if (excelFile.Exists)
            {
                try
                {
                    using (var stream = File.OpenRead(path))
                    {
                        using (var workBook = new Workbook(stream))
                        {

                            var sheet = workBook.Worksheets.First();
                            //DataTable tbl = new DataTable();
                            var data = sheet.Cells.ExportDataTable(0, 0, sheet.Cells.MaxDataRow + 1, sheet.Cells.MaxDataColumn + 1, new ExportTableOptions { ExportColumnName = true, PlotVisibleCells = false, PlotVisibleColumns = false, PlotVisibleRows = false });
                            data.TableName = Guid.NewGuid().ToString();
                            foreach (DataColumn item in data.Columns)
                            {
                                item.ColumnName = item.ColumnName.ToLower();
                            }
                            return data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var message = AsposeExceptionScprits.ChangeErrorMessage(ex.Message);
                    throw new Exception(message);
                }
            }
            else
            {
                throw new FileNotFoundException(path + " 未找到指定的Excel文件");
            }
        }

        public static DataTable TableRowTurnToColumn(DataTable source, DataColumn[] groupColumns, DataColumn[] captionColumns, DataColumn valueColumn)
        {
            if (source == null)
                return null;
            DataTable dt = new DataTable();
            if (groupColumns != null)
                foreach (var item in groupColumns)
                {
                    dt.Columns.Add(item.ColumnName, item.DataType);
                }
            foreach (var item in source.AsEnumerable().Select(p => string.Join("_", captionColumns.Select(q => p[q]))).Distinct())
            {
                if (!dt.Columns.Contains(item.ToString()))
                    dt.Columns.Add(item.ToString());
            }
            var groupdata = source.AsEnumerable().GroupBy(p => string.Join("", groupColumns.Select(q => p[q]))).ToList();
            var groupColNames = groupColumns.Select(p => p.ColumnName);
            groupdata.ForEach(x =>
            {
                DataRow newRow = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    if (groupColNames.Contains(col.ColumnName))
                    {
                        newRow[col.ColumnName] = x.FirstOrDefault()[col.ColumnName];
                    }
                    else
                    {
                        var data = x.Where(p => string.Join("_", captionColumns.Select(q => p[q])) == col.ColumnName);
                        if (data.Count() > 0)
                        {
                            newRow[col.ColumnName] = data.Sum(p => Convert.ToDecimal(p[valueColumn]));
                        }
                    }
                }
                dt.Rows.Add(newRow);
            });
            return dt;
        }

        internal static class AsposeExceptionScprits
        {
            private static Dictionary<string, List<string>> _dictPatterns;
            private static Dictionary<string, string> _dictConverts;

            private static Dictionary<string, string> _dictTypeChanges;
            static AsposeExceptionScprits()
            {
                _dictTypeChanges = new Dictionary<string, string>();
                _dictConverts = new Dictionary<string, string>();
                _dictPatterns = new Dictionary<string, List<string>>();
                _dictTypeChanges.Add("string", "文本");
                _dictTypeChanges.Add("int", "数字");
                _dictTypeChanges.Add("long", "数字");
                _dictTypeChanges.Add("dateTime", "日期");
                _dictTypeChanges.Add("date", "日期");
                _dictTypeChanges.Add("boolean", "真/假");

                _dictPatterns.Add(@"The value of the cell [\w]+ should not be a [\w]+ value", new List<string> {
                    @"(?<=The value of the cell )[\w]+(?= should not be a)",
                    @"(?<=should not be a )[\w]+(?= value)"
                });
                _dictConverts.Add(@"The value of the cell [\w]+ should not be a [\w]+ value", @"Excel表格中的单元格[{0}]不可以是{1}内容");
            }
            /// <summary>
            /// 转换Aspose的异常信息
            /// </summary>
            /// <param name="sourceErrorMessage">原始错误信息</param>
            /// <returns></returns>
            internal static string ChangeErrorMessage(string sourceErrorMessage)
            {
                foreach (var item in _dictPatterns)
                {
                    if (Regex.IsMatch(sourceErrorMessage, item.Key))
                    {
                        var values = new List<string>();
                        foreach (var p in item.Value)
                        {
                            var match = Regex.Match(sourceErrorMessage, p);
                            if (match != null)
                            {
                                var val = match.Groups[0].Value;
                                if (_dictTypeChanges.ContainsKey(val.Trim().ToLower()))
                                {
                                    val = _dictTypeChanges[val];
                                }
                                values.Add(val);
                            }
                        }

                        var targetFmt = _dictConverts[item.Key];
                        return targetFmt.AsFormat(values.ToArray());
                    }
                }
                return sourceErrorMessage;
            }
        }

        /// <summary>  
        /// 读取Excel数据到DataSet  
        /// </summary>  
        /// <param name="filePath">The file path.</param>  
        /// <returns></returns>  
        protected virtual DataSet ReadExcelToDataSet(string filePath)
        {
            DataSet ds = new DataSet("ds");
            var xlsx = new Aspose.Cells.Workbook(filePath);

            for (int i = 0; i < xlsx.Worksheets.Count; i++)
            {
                var cells = xlsx.Worksheets[i].Cells;
                var dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxDataColumn);
                ds.Tables.Add(dt);
            }

            return ds;

        }



        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument},
                {".docx",  MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument},
                {".xls",  MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet},
                {".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".html", "text/html"}
            };
        }

        /// <summary>
        /// Word文档插入多个水印
        /// </summary>
        /// <param name="mdoc">Document</param>
        /// <param name="wmText">水印文字名</param>
        public static void AddWaterMark(Aspose.Words.Document mdoc, string wmText)
        {
            Paragraph watermarkPara = new Paragraph(mdoc);
            for (int j = 80; j < 400; j = j + 200)
            {
                for (int i = 80; i < 700; i = i + 200)
                {
                    Shape waterShape = ShapeMore(mdoc, wmText, j, i);
                    watermarkPara.AppendChild(waterShape);
                }
            }

            // 在每个部分中，最多可以有三个不同的标题，因为我们想要出现在所有页面上的水印，插入到所有标题中。  
            foreach (Aspose.Words.Section sect in mdoc.Sections)
            {
                // 每个区段可能有多达三个不同的标题，因为我们希望所有页面上都有水印，将所有的头插入。
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderPrimary);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderFirst);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderEven);
            }
        }

        /// <summary>
        /// 插入多个水印
        /// </summary>
        /// <param name="mdoc">Document</param>
        /// <param name="wmText">水印文字名</param>
        /// <param name="left">左边距多少</param>
        /// <param name="top">上边距多少</param>
        /// <returns></returns>
        private static Shape ShapeMore(Aspose.Words.Document mdoc, string wmText, double left, double top)
        {
            Shape waterShape = new Shape(mdoc, ShapeType.TextPlainText);
            //设置该文本的水印
            waterShape.TextPath.Text = wmText;
            waterShape.TextPath.FontFamily = "宋体";
            waterShape.Width = 80;
            waterShape.Height = 20;
            //文本将从左下角到右上角。
            waterShape.Rotation = -45;
            //绘制水印颜色
            waterShape.Fill.Color = Color.LightGray;//浅灰色水印
            waterShape.StrokeColor = Color.LightGray;
            //将水印放置在页面中心
            waterShape.Left = left;
            waterShape.Top = top;
            waterShape.WrapType = WrapType.None;
            return waterShape;
        }

        private static void InsertWatermarkIntoHeader(Paragraph watermarkPara, Aspose.Words.Section sect, HeaderFooterType headerType)
        {
            HeaderFooter header = sect.HeadersFooters[headerType];

            if (header == null)
            {
                // There is no header of the specified type in the current section, create it.
                header = new HeaderFooter(sect.Document, headerType);
                sect.HeadersFooters.Add(header);
            }

            // Insert a clone of the watermark into the header.
            header.AppendChild(watermarkPara.Clone(true));
        }

        public Dictionary<string, dynamic> Eval(string jObj)
        {
            return new CalcEngine().Eval(jObj);
        }

    }

    public class UserOutput
    {
        public Guid Id { get; set; }
    }
}
