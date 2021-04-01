using System.Collections.Generic;

namespace Gene.Middleware.Bases
{
    public class BaseViewModel
    {
        public string NotifyMessage { get; set; }

        public IEnumerable<TableRowActionLink> TableRowActionLinks { get; set; }

        public IEnumerable<TableColumn> TableColumns { get; set; }

        public IEnumerable<SelectOption> PageSizeSource { get; set; } = new[]
        {
            new SelectOption {Text = "Page Size : 5", Value = 5},
            new SelectOption {Text = "Page Size : 10", Value = 10},
            new SelectOption {Text = "Page Size : 20", Value = 20},
            new SelectOption {Text = "Page Size : 50", Value = 50},
            new SelectOption {Text = "Page Size : 100", Value = 100},
        };
    }

    public class TableColumn
    {
        public string Header { get; set; }

        public string DataPath { get; set; }

        public string Class { get; set; }

        public TableCellDataType DataType { get; set; }
    }

    public class TableRowActionLink
    {
        public string Text { get; set; }

        public string Url { get; set; }

        public string ObjectPropertyPath { get; set; }
    }

    public enum TableCellDataType
    {
        Text = 1,
        Boolean = 2,
        Enum = 3,
        Date = 4,
        Image = 5
    }

    public class SelectOption
    {
        public string Text { get; set; }

        public object Value { get; set; }
    }
}