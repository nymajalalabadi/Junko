using Junko.Domain.Entities.Contacts;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.ContactUs
{
    public class FilterTicketDTO : Paging<Ticket>
    {
        #region properties

        public string? Title { get; set; }

        public long? UserId { get; set; }

        public FilterTicketState? FilterTicketState { get; set; }

        public FilterTicketOrder? OrderBy { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        #endregion
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }

    public enum FilterTicketOrder
    {
        CreateDate_DES,
        CreateDate_ASC,
    }

}
