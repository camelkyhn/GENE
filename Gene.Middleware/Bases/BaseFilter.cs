using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Attributes;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Bases
{
    public class BaseFilter
    {
        private int _pageSize;

        [Display(Name = Labels.PageSize)]
        public int PageSize
        {
            get => _pageSize <= 0 ? 5 : _pageSize;
            set => _pageSize = value <= 0 ? 5 : value;
        }

        private int _pageNumber;

        [Display(Name = Labels.PageNumber)]
        public int PageNumber
        {
            get => _pageNumber <= 0 ? 1 : _pageNumber;
            set => _pageNumber = value <= 0 ? 1 : value;
        }

        public int TotalCount { get; set; }

        [Display(Name    = Labels.IsAllData)]
        [Tooltip(Message = Tooltips.IsAllData, Position = Positions.Bottom)]
        public bool IsAllData { get; set; }

        public bool IsRecentItems { get; set; }

        [Display(Name = Labels.DateBefore)]
        public DateTimeOffset? DateBefore { get; set; }

        [Display(Name = Labels.DateAfter)]
        public DateTimeOffset? DateAfter { get; set; }

        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.CreatedUserEmail)]
        public string CreatedUserEmail { get; set; }

        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.UpdatedUserEmail)]
        public string UpdatedUserEmail { get; set; }

        [Display(Name = Labels.Status)]
        public Status? Status { get; set; }
    }
}
