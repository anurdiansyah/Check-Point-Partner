using Newtonsoft.Json;
using CheckPointPartner.WebUI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckPointPartner.WebUI.Model
{
    public partial class IUsersCollection : ICollection<IUsers>
    {

        List<IUsers> m_Users = new List<IUsers>();

        private readonly UsersContext _context;

        public IUsersCollection(UsersContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_Users.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(IUsers item)
        {
            m_Users.Add(item);
        }

        public void Clear()
        {
            m_Users.Clear();
        }

        public bool Contains(IUsers item)
        {
            return m_Users.Contains(item);
        }

        public void CopyTo(IUsers[] array, int arrayIndex)
        {
            m_Users.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IUsers> GetEnumerator()
        {
            return m_Users.GetEnumerator();
        }

        public bool Remove(IUsers item)
        {
            return m_Users.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Users.GetEnumerator();
        }

        public bool List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            m_Users = _context.iUsers.Where(o => o.UserName.Contains(p_sKeyword)).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQUsers = _context.iUsers.Where(o => o.UserName.Contains(p_sKeyword) && o.EmailConfirmed == true);
            totalRecord = iQUsers.Count();
            m_Users = iQUsers.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
