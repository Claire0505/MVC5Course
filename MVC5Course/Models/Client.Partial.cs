namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Models.InputValidations;

    [MetadataType(typeof(ClientMetaData))]
    public partial class Client : IValidatableObject, IClientBatchUpdate
    {
        //24 練習設定模型預設值的開發技巧 ( 利用 partial method 與修改 tt 檔 )
        partial void Init()
        {
            this.DateOfBirth = DateTime.Now.AddYears(-18);
        }

        //實作模型驗證
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            // 通常此判斷為當資料為 新增 時需要判斷的模型驗證
            if (this.ClientId == 0)
            {
                // 實作模型驗證 通常 P.K. 為 0 代表著正在執行「新增」動作
            }

            if (this.Longitude.HasValue != this.Latitude.HasValue)
            {
                yield return new ValidationResult("經緯度欄位必須一起設定", new string[] { "Longitude", "Latitude" });
            }

            // 模型驗證可以放入多組 下方為示範
            // 模型驗證為後端驗證，因此送出後才會顯示錯誤訊息內容
            //if (this.DateOfBirth.Value.Year > 1980 && this.City == "Taipei")
            //{
            //    yield return new ValidationResult("條件錯誤", new string[] { "DateOfBirth", "City" });
            //}
        }
    }

    public partial class ClientMetaData 
    {
        [Required]
        public int ClientId { get; set; }
        
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string FirstName { get; set; }
               
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage="欄位長度不得大於 40 個字元")]
        public string LastName { get; set; }
        
        [StringLength(1, ErrorMessage="欄位長度不得大於 1 個字元")]
        public string Gender { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required]
        public Nullable<double> CreditRating { get; set; }
        
        [StringLength(7, ErrorMessage="欄位長度不得大於 7 個字元")]
        public string XCode { get; set; }
        public Nullable<int> OccupationId { get; set; }
        
        [StringLength(20, ErrorMessage="欄位長度不得大於 20 個字元")]
        public string TelephoneNumber { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string Street1 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string Street2 { get; set; }
        
        [StringLength(100, ErrorMessage="欄位長度不得大於 100 個字元")]
        public string City { get; set; }
        
        [StringLength(15, ErrorMessage="欄位長度不得大於 15 個字元")]
        public string ZipCode { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public string Notes { get; set; }

        [IdentificationId]
        public string IdNumber { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
