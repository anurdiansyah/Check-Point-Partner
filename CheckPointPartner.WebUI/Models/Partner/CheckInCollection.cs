using Newtonsoft.Json;
using CheckPointPartner.WebUI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckPointPartner.WebUI.Model
{
    public partial class CheckInCollection : ICollection<CheckIn>
    {

        List<CheckIn> m_CheckIn = new List<CheckIn>();

        private readonly UsersContext _context;

        public CheckInCollection(UsersContext context)
        {
            _context = context;
        }

        public int totalRecord { get; set; }

        public int Count => m_CheckIn.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(CheckIn item)
        {
            m_CheckIn.Add(item);
        }

        public void Clear()
        {
            m_CheckIn.Clear();
        }

        public bool Contains(CheckIn item)
        {
            return m_CheckIn.Contains(item);
        }

        public void CopyTo(CheckIn[] array, int arrayIndex)
        {
            m_CheckIn.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CheckIn> GetEnumerator()
        {
            return m_CheckIn.GetEnumerator();
        }

        public bool Remove(CheckIn item)
        {
            return m_CheckIn.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_CheckIn.GetEnumerator();
        }

        public bool List(int? p_iIdPartner, string p_sKeyword)
        {
            bool bIsSuccess = false;
            m_CheckIn = _context.CheckIns.Where(o => o.IdRumahMakan == p_iIdPartner && o.KodeUangJalan.Contains(p_sKeyword)).ToList();

            return bIsSuccess;
        }

        public bool List(int? p_iIdPartner, string p_sKeyword, int p_iSkip, int p_iLength)
        {
            bool bIsSuccess = false;

            var iQUsers = _context.CheckIns.Where(o => o.IdRumahMakan == p_iIdPartner 
                                               && (o.KodeUangJalan.Contains(p_sKeyword) 
                                               || o.CrewName.Contains(p_sKeyword) 
                                               || o.CrewTagNo.Contains(p_sKeyword)));
            totalRecord = iQUsers.Count();
            m_CheckIn = iQUsers.Skip(p_iSkip).Take(p_iLength).OrderBy(o=>o.RowNumber).ToList();

            return bIsSuccess;
        }
    }
}
