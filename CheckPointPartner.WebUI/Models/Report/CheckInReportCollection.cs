using Newtonsoft.Json;
using CheckPointPartner.WebUI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckPointPartner.WebUI.Model
{
    public partial class CheckInReportCollection : ICollection<CheckInReport>
    {

        List<CheckInReport> m_Users = new List<CheckInReport>();

        private readonly UsersContext _context;

        public CheckInReportCollection(UsersContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_Users.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(CheckInReport item)
        {
            m_Users.Add(item);
        }

        public void Clear()
        {
            m_Users.Clear();
        }

        public bool Contains(CheckInReport item)
        {
            return m_Users.Contains(item);
        }

        public void CopyTo(CheckInReport[] array, int arrayIndex)
        {
            m_Users.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CheckInReport> GetEnumerator()
        {
            return m_Users.GetEnumerator();
        }

        public bool Remove(CheckInReport item)
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
            m_Users = _context.CheckInReports.Where(o => o.KodeUangJalan.Contains(p_sKeyword)).ToList();

            return bIsSuccess;
        }

        public bool List(string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQUsers = _context.CheckInReports.Where(o => o.KodeUangJalan.Contains(p_sKeyword) || o.PartnerName.Contains(p_sKeyword));
            totalRecord = iQUsers.Count();
            m_Users = iQUsers.Skip(p_iSkip).Take(p_iLength).ToList();

            return bIsSuccess;
        }
    }
}
