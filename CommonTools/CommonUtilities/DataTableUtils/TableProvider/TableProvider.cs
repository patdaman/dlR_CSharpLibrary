using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace CommonUtils.DataTableUtils
{

    public class TableProviderArguments
    {
        public Object Data { get; set; }
    }

    public class TableProviderIdentifier
    {
        public String Tag { get; set; }
        public String DisplayName { get; set; }

        public TableProviderIdentifier( string tag, string displayName)
        {
            Tag = tag;
            DisplayName = displayName;
        }

        public TableProviderIdentifier(string tag)
            : this(tag, tag)
        {

        }
    }

    /// <summary>
    /// A Table provider is a data structure that is used to generate multiple views of an underlying input datatable.
    /// </summary>
    public class TableProvider
    {
        public delegate DataTable ProviderDelegate(TableProviderArguments args, DataTable dt);
        
        #region Accessors
        private DataTable _InputDataTable;
        public DataTable InputDataTable
        {
            get { return _InputDataTable; }
            set
            {
                if (value != _InputDataTable)
                {
                    _InputDataTable = value;                    
                }
            }
        }
        #endregion 

        #region Events        
        #endregion 

        private Dictionary<String, ProviderDelegate> _tagDelegateDict = new Dictionary<string, ProviderDelegate>();
        private Dictionary<String, DataTable> _tagDataTableDict = new Dictionary<string, DataTable>(); 

        public TableProvider()
        {
            
        }
        
        public TableProvider( DataTable dt ) : this()
        {
            Initialize(dt);
        }

        public void Initialize(DataTable dt)
        {
            InputDataTable = dt;
            _tagDelegateDict.Clear();
            _tagDataTableDict.Clear(); 
        }


        public void AddProviderDeletage( String tag, ProviderDelegate del )
        {
            _tagDelegateDict.Add(tag, del);
        }

        public DataTable GetDataTable(String tag, TableProviderArguments arg = null)
        {
            Debug.Assert(tag != null);
            ProviderDelegate delegateFunc = null;
            bool exists = _tagDelegateDict.TryGetValue(tag, out delegateFunc);
            if( exists == false)
            {
                throw new KeyNotFoundException(String.Format("Trying to grab delegate and could not find the key {0}.", tag));
            }
            Debug.Assert(delegateFunc != null);

            DataTable dt = null;
            exists = _tagDataTableDict.TryGetValue(tag, out dt);
            if(exists == false)
            {
                dt = delegateFunc(arg, InputDataTable);
                _tagDataTableDict.Add(tag, dt);
            }
            Debug.Assert(dt != null);
            return dt; 
        }

    }
}
