using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lala.API
{
    class PlaylistCollection : IList<Playlist>
    {

        #region IList<Playlist> Members

        public int IndexOf(Playlist item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Playlist item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Playlist this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<Playlist> Members

        public void Add(Playlist item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Playlist item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Playlist[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Playlist item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<Playlist> Members

        public IEnumerator<Playlist> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
