using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Core
{
    [Table(nameof(EntityHistoryRecord))]
    public class EntityHistoryRecord : Identified<long>
    {
        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityId { get; set; }

        [Required]
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        public string EntityName { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string JsonValue { get; set; }
    }
}