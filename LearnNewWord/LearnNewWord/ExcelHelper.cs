using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace LearnNewWord
{
    static class ExcelHelper
    {
        public static List<Vocab> LoadAllVocabs(string filePath)
        {
            var listVocab = new List<Vocab>();
            if (File.Exists(filePath))
            {
                //TODO
            }
            return listVocab;
        }

        public static List<Vocab> LoadAllVocabs(string filePath, bool[] selectedParts)
        {
            var listVocab = new List<Vocab>();
            if (File.Exists(filePath))
            {
                //TODO
                Excel.Application app = new Excel.Application();
                Excel.Workbook workbook = app.Workbooks.Open(filePath);
                for (int i = 0; i < selectedParts.Length; i++)
                {
                    if (selectedParts[i])
                    {
                        Excel.Worksheet sheet = workbook.Worksheets[i+1];
                        Excel.Range range = sheet.UsedRange;
                        for (int row = 1; row <= range.Rows.Count; row++)
                        {
                            var cell1 = range.Cells[row, 1] as Excel.Range;
                            var cell2 = range.Cells[row, 2] as Excel.Range;
                            var cell3 = range.Cells[row, 3] as Excel.Range;
                            if (cell1 != null&& cell1.Value2!=null)
                            {
                                var vocab = new Vocab()
                                {
                                    Word = cell1.Value2.ToString(),
                                    Kana = cell2 == null || cell2.Value2==null?"":cell2.Value2.ToString(),
                                    Meaning = cell3 == null||cell3.Value2==null?"":cell3.Value2.ToString()
                                };
                                listVocab.Add(vocab);
                            }
                        }
                    }
                    
                }
                //close and release
                workbook.Close();
                Marshal.ReleaseComObject(workbook);

                //quit and release
                app.Quit();
                Marshal.ReleaseComObject(app);
            }
            return listVocab;
        }

        public static List<string> LoadAllPart(string filePath)
        {
            var listPart = new List<string>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(filePath);
            foreach (Excel.Worksheet worksheet in workbook.Worksheets)
            {
                listPart.Add(worksheet.Name);
            }
            //close and release
            workbook.Close();
            Marshal.ReleaseComObject(workbook);

            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);
            return listPart;
        }
    }
}
