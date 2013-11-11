    /*******************************************************************************************
 * Module Name: ExcelAdapter
 * Created By : Tomy Rhymond
 * Date       : 3/20/2006
 * Version    : 1.0
 * Description: Convert an Excel workbook to a Dataset.
 *              The worksheets will be converted to DataTables
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;

namespace Office.Framework.Excel
{
    public class ExcelAdapter
    {
        private bool _firstRowHeader = true;
        private DataSet _dataSource;
        private Application application;
        private object missing = Type.Missing;
        private string _datasetName = null;

        public ExcelAdapter()
        {
            application = new Application();
            application.Visible = false;
            application.ScreenUpdating = false;
            application.DisplayAlerts = false;
        }

        public bool FirstRowHeader
        {
            get { return _firstRowHeader; }
            set { _firstRowHeader = value; }
        }

        public DataSet DataSource
        {
            get { return _dataSource; }
        }

        public string DatasetName   
        {
            get { return _datasetName; }
            set { _datasetName = value; }
        }
	

        /// <summary>
        /// Loads the specified excel file to a Dataset.
        ///    Dataset name will be set to the Excel FileName.
        ///    Table Name will be set to the worksheet name.
        ///    if the FirstRowHeader is set to false, the column name will be assigned a default name like 'Col1', Col2' etc.
        ///    If any row with all blank column found during the scan, the rest of the rows will be ignored.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Load(string source)
        {
            try
            {
                if (_datasetName == null || _datasetName.Trim() == "")
                {
                    _datasetName = source.Substring(source.LastIndexOf("\\") + 1);
                }

                _dataSource = new DataSet(_datasetName);

                Workbook workbook = application.Workbooks.Open(source,               //Source File Name
                                                               0,                    //UpdateLinks
                                                               true,                 //ReadOnly
                                                               5,                    //Format
                                                               "",                   //Password
                                                               "",                   //WriteResPassword
                                                               true,                 //IgnoreReadOnly
                                                               XlPlatform.xlWindows, //Origin
                                                               "\t",                 //Delimiter
                                                               false,                //Editable
                                                               false,                //Notify
                                                               0,                    //Converter
                                                               false,                //AddToMru
                                                               null,                 //Load
                                                               null);                //CorruptLoad

                for (int i = 1; i < workbook.Worksheets.Count + 1; i++)
                {
                    System.Data.DataTable worksheet = loadDataTable((Worksheet)workbook.Worksheets.get_Item(i));
                    _dataSource.Tables.Add(worksheet);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (application != null)
            {
                application.Workbooks.Close();

                //Remove the application from the task manager
                application.Quit();
            }
        }

        #region Helper Methods

        private System.Data.DataTable loadDataTable(Worksheet sheet)
        {
            object[] columnValues;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.TableName = sheet.Name;
            Microsoft.Office.Interop.Excel.Range range;

            try
            {
                range = getValidRange(sheet);

                object[,] values = (object[,])range.Value2;
                columnValues = new object[values.GetLength(1)];

                for (int i = 1; i <= values.GetLength(0); i++)
                {
                    if (i == 1 && _firstRowHeader)
                    {
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            object value = values[i, j];
                            if (value != null) dt.Columns.Add(value.ToString());
                            else dt.Columns.Add("");
                        }
                    }
                    else
                    {
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            object value = values[i, j];
                            if (value != null)
                            {
                                columnValues[j - 1] = value.ToString();
                            }
                            else
                            {
                                columnValues[j - 1] = "";
                            }
                        }
                        dt.Rows.Add(columnValues);
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                range = null;
            }            
        }

        /// <summary>
        /// Gets the valid range of cells to work with.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <returns>Excel Range</returns>
        private Microsoft.Office.Interop.Excel.Range getValidRange(Worksheet sheet)
        {
            string downAddress = "";
            string rightAddress = "";
            long indexOld = 0;
            long index = 0;
            Microsoft.Office.Interop.Excel.Range startRange;
            Microsoft.Office.Interop.Excel.Range rightRange;
            Microsoft.Office.Interop.Excel.Range downRange;

            try
            {
                // get a range to work with
                startRange = sheet.get_Range("A1", Missing.Value);
                // get the end of values to the right (will stop at the first empty cell)
                rightRange = startRange.get_End(XlDirection.xlToRight);

                //get_End method scans the sheet in the direction specified until it finds a empty cell and returns the previous cell.
                //We need to scan all the columns and find the column with highest number of rows (row count). 
                //Then use the Prefix character on the right cell and the row count to determine the address for the valid range.
                //
                while (true)
                {
                    downRange = rightRange.get_End(XlDirection.xlDown);
                    downAddress = downRange.get_Address(false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                    index = getIndex(downAddress);
                    if (index >= 65536) index = 0;
                    if (index > indexOld) indexOld = index;
                    if (rightRange.Column == 1) break;
                    rightRange = rightRange.Previous;
                }

                rightRange = startRange.get_End(XlDirection.xlToRight);
                rightAddress = rightRange.get_Address(false, false, XlReferenceStyle.xlA1, Type.Missing, Type.Missing);

                return sheet.get_Range("A1", getPrefix(rightAddress) + indexOld);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                startRange = null;
                rightRange = null;
                downRange = null;
            }            
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        private long getIndex(string address)
        {
            string outStr = "";
            for (int i = 0; i < address.Length; i++)
            {
                if (!Char.IsLetter(address[i]))
                {
                    outStr += address[i].ToString();
                }
            }

            return Convert.ToInt64(outStr);
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        private string getPrefix(string address)
        {
            string outStr = "";
            for (int i = 0; i < address.Length; i++)
            {
                if (Char.IsLetter(address[i]))
                {
                    outStr += address[i].ToString();
                }
            }

            return outStr;
        }
        #endregion
    }
}
