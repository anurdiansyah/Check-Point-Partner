using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CheckPointPartner.Api_RD.Model
{
    public class DoCheckInModel
    {
        #region Variable

        private Guid m_gGlobalId;
        private Int32 m_iCrewTypeId;
        private Int32 m_iRumahMakanId;
        private Int32 m_iUangJalanId;
        private Int32 m_iUangJalanCheckPointId;
        private string m_sBase64Photos;
        private string m_sResponseMessage;

        #endregion

        #region Properties

        public Guid GlobalId { get => m_gGlobalId; set => m_gGlobalId = value; }
        public int CrewTypeId { get => m_iCrewTypeId; set => m_iCrewTypeId = value; }
        public int RumahMakanId { get => m_iRumahMakanId; set => m_iRumahMakanId = value; }
        public int UangJalanId { get => m_iUangJalanId; set => m_iUangJalanId = value; }
        public int UangJalanCheckPointId { get => m_iUangJalanCheckPointId; set => m_iUangJalanCheckPointId = value; }
        public string Base64Photos { get => m_sBase64Photos; set => m_sBase64Photos = value; }
        public string ResponseMessage { get => m_sResponseMessage; set => m_sResponseMessage = value; }

        #endregion
    }
}
