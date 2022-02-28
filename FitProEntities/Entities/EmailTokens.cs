using FitPro.Common;
using System;

namespace FitPro.Entities
{
    public partial class EmailTokens : IEntity
    {
        public Guid Token { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}
