using System;
using System.Collections.Generic;
using System.Text;

namespace Ituran.Domain.Enums;

public enum ProcessingStatus
{
    Received = 1,
    Validated = 2,
    PendingERP = 3,
    ProcessingERP = 4,
    ERPCompleted = 5,
    PendingCRM = 6,
    ProcessingCRM = 7,
    CRMCompleted = 8,
    Completed = 9,
    Retrying = 10,
    Failed = 11,
    DeadLetter = 12
}