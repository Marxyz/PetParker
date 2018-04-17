using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetParker.Models
{
    public class TicketRepository : BaseViewModel
    {
        public double Sum => WorkingTicketSum + FinishedTicketSum;

        public double WorkingTicketSum
        {
            get { return WorkingTickets.Sum(t => t.Cost); }
            
        }

        public double WorkingTicketRealSum
        {
            get { return WorkingTickets.Sum(t => t.RealCost); }

        }

        public double FinishedTicketRealSum
        {
            get { return FinishedTickets.Sum(t => t.RealCost); }
        }

        public double FinishedTicketSum
        {
            get { return FinishedTickets.Sum(t=> t.Cost); }
        }

        private List<Ticket> _bufforList;
        public ObservableCollection<Ticket> FinishedTickets { get; set; }
        public ObservableCollection<Ticket> WorkingTickets { get; set; }

        

        public TicketRepository()
        {
            _bufforList = new List<Ticket>();
            this.WorkingTickets = new ObservableCollection<Ticket>();
            FinishedTickets = new ObservableCollection<Ticket>();
        }

        private void DeleteTicketLeftId(ref Ticket ticket )
        {
            if (WorkingTickets.Contains(ticket))
            {
                WorkingTickets.Remove(ticket);
            }
            else
            {
                FinishedTickets.Remove(ticket);
            }
        }

        public void DeleteTicketChangeId(ref Ticket ticket)
        {
            DeleteTicketLeftId(ref ticket);
            int maxId = GetMaxId();
            int currentId = ticket.ID;
            for (int i = maxId; i > currentId; i--)
            {
                GetTicket(i).ID = GetTicket(i).ID - 1;
            }
            Refresh();
            
        }

        private Ticket GetTicket(int id)
        {
            Ticket first = WorkingTickets.FirstOrDefault(t => t.ID == id);
            return first != null ? WorkingTickets.FirstOrDefault(t => t.ID == id) : FinishedTickets.FirstOrDefault(t => t.ID == id);
        }

        private int GetMaxCollectionId(ObservableCollection<Ticket> collection )
        {
            return collection.Count == 0 ? 0 : collection.Max(t => t.ID);
        }

        private int GetMaxId()
        {
            if (GetMaxCollectionId(WorkingTickets) == 0 && GetMaxCollectionId(FinishedTickets) == 0)
            {
                return 0;
            }
            return GetMaxCollectionId(GetMaxCollectionId(WorkingTickets) >= GetMaxCollectionId(FinishedTickets) ? WorkingTickets : FinishedTickets);
        }

        private void RefreshCollection(ObservableCollection<Ticket> collection)
        {
            _bufforList = collection.ToList();
            collection.Clear();
            foreach (var ticket in _bufforList)
            {
                collection.Add(ticket);
            }
            _bufforList.Clear();

        }

        private void Refresh()
        {
            RefreshCollection(WorkingTickets);
            RefreshCollection(FinishedTickets);
        }

        public void AddTicket( Ticket ticket)
        {
            ticket.ID = GetMaxId() + 1;
            WorkingTickets.Add(ticket);
        }

        public void FinishTicket(ref Ticket ticket)
        {
            ticket.ExitDateTime = DateTime.Now;
            OnPropertyChanged();
            ticket.IsCompleted = true;
            ticket.StopCostTimer();
            WorkingTickets.Remove(ticket);
            FinishedTickets.Add(ticket);
        }
    }
}
