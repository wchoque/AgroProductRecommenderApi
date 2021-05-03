using System.Collections;
using System.Collections.Generic;

namespace AppCentroIdiomas.Models
{
    public class AvailableUsers : IEnumerable<AvailableUser>
    {
        public List<AvailableUser> AvailableUsersList = new List<AvailableUser>();
        public AvailableUser this[int index]
        {
            get { return AvailableUsersList[index]; }
            set { AvailableUsersList.Insert(index, value); }
        }

        public IEnumerator<AvailableUser> GetEnumerator()
        {
            return AvailableUsersList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
