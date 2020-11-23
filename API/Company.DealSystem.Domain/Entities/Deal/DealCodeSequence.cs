using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.DealSystem.Domain.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.DealSystem.Domain.Entities
{
    public class DealCodeSequence : BaseEntity
    {
        /// <summary>
        /// Counterparty code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Next sequence for this counterparty code
        /// </summary>
        public int NextSequence { get; set; }
    }
}
