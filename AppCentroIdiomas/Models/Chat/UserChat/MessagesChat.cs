using System.Collections;
using System.Collections.Generic;

namespace AppCentroIdiomas.Models
{
    public class MessagesChat : IEnumerable<MessageChat>
    {
        public List<MessageChat> AvailableUsersList = new List<MessageChat>();
        public MessageChat this[int index]
        {
            get { return AvailableUsersList[index]; }
            set { AvailableUsersList.Insert(index, value); }
        }

        public IEnumerator<MessageChat> GetEnumerator()
        {
            return AvailableUsersList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
