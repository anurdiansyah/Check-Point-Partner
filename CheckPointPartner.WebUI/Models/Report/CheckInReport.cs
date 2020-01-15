using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckPointPartner.WebUI.Model
{
    [Table("VT_AbsenRumahMakan")]
    public partial class CheckInReport
    {
        public CheckInReport()
        {
        }

        [Key]
        public DateTime? CheckPointDate { get; set; }
        public string PartnerName { get; set; }
        public string PaymentStatusName { get; set; }
        public string CheckInPhotoId { get; set; }
        public decimal? Harga { get; set; }
        public string CheckPointActor { get; set; }
        public Int32? CrewId { get; set; }
        public string CrewName { get; set; }

        public Int32 IdUangJalan { get; set; }
        public string KodeUangJalan { get; set; }
        public DateTime? TranDateUangJalan { get; set; }
        public string StatusUangJalan { get; set; }
        public Int32 CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Int32? SewaId { get; set; }
        public Int32? LokasiMuatId { get; set; }
        public string LokasiMuatName { get; set; }
        public Int32? LokasiBongkarId { get; set; }
        public string LokasiBongkarName { get; set; }
        public string AlamatMuat { get; set; }
        public string AlamatBongkar { get; set; }
        public Int32? KM { get; set; }
        public string PicMuat { get; set; }
        public string PicMuatPhone { get; set; }
        public string PicBongkar { get; set; }
        public string PicBongkarPhone { get; set; }
        public DateTime? TglMuat { get; set; }
        public DateTime? TglBongkar { get; set; }
        public Int32? LeadTimeAsumsi { get; set; }
        public DateTime? TglDatangMuat { get; set; }
        public DateTime? TglMulaiMuat { get; set; }
        public DateTime? TglSelesaiMuat { get; set; }
        public DateTime? TglAktualJalan { get; set; }
        public DateTime? TglSampaiBongkar { get; set; }
        public DateTime? TglMulaiBongkar { get; set; }
        public DateTime? TglSelesaiBongkar { get; set; }
        public Int32? LeadTimeAktual { get; set; }
        public Int32? LeadTimeBongkar { get; set; }
        public bool? IsValidation { get; set; }
        public string JenisMuatan { get; set; }
        public Int32? TotalKm { get; set; }
        public Int32? JenisPenjualan { get; set; }
        public Double? TotalKg { get; set; }
        public decimal? HargaPerKg { get; set; }
        public decimal? TotalMuatan { get; set; }
        public string Pemohon { get; set; }
        public Int32? TipeKendaraanId { get; set; }
        public Int32? MobilId { get; set; }
        public Int32? ModelKendaraanId { get; set; }
        public string MobilName { get; set; }
        public string NoPolisi { get; set; }
        public Int32? PengemudiId { get; set; }
        public string PengemudiName { get; set; }
        public Int32? KenekId { get; set; }
        public string KenekName { get; set; }
        public Int32? RekeningId { get; set; }
        public string RekeningName { get; set; }
        public string RekeningDesc { get; set; }
        public decimal? BiayaOperasional { get; set; }
        public decimal? BiayaOperasionalRound { get; set; }
        public decimal? BiayaLain { get; set; }
        public decimal? TotalBiaya { get; set; }
        public decimal? TotalBiayaRound { get; set; }
        public decimal? HargaPenjualan { get; set; }
        public decimal? HargaSelisih { get; set; }
        public Double? Margin { get; set; }
        public decimal? TambahanBiaya { get; set; }
        public decimal? TotalAktualBiaya { get; set; }
        public decimal? AktualHargaSelisih { get; set; }
        public Double? AktualMargin { get; set; }
        public bool? IsInvoiced { get; set; }
        public string NoSpj { get; set; }
        public DateTime? TglTerimaOps { get; set; }
        public Int32? Poin { get; set; }
        public DateTime? TglTerimaAcc { get; set; }
        public bool? IsProsesKomisi { get; set; }
        public string Notes { get; set; }
        public string NotesMon { get; set; }
        public string NotesAcc { get; set; }
        public string NotesKlaimCompany { get; set; }
        public string NotesKlaimCustomer { get; set; }
        public string JenisPengiriman { get; set; }
        public string NoResi { get; set; }
        public bool? IsEdit { get; set; }
        public Int32? IsProsesKlaim { get; set; }
        public decimal? KlaimCompany { get; set; }
        public decimal? KlaimCustomer { get; set; }
        public string SelisihKlaim { get; set; }
        public Int32? ProsesUpahIdFromProsesKlaim { get; set; }
        public DateTime? IOn { get; set; }
        public string IBy { get; set; }
        public DateTime? UOn { get; set; }
        public string UBy { get; set; }
    }
}
