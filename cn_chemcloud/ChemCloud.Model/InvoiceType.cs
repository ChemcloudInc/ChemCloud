using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public enum InvoiceType
    {
        [Description("不需要发票")]
        None,
        [Description("平台发票")]
        VATInvoice,
        [Description("普通发票")]
        OrdinaryInvoices,
        [Description("专票")]
        SpecialTicket
    }
}