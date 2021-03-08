using EmpManagment.Bol.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class ComplainantViewModel
    {
        public ComplainantViewModel()
        {
            this.ListOfExcelFileDataSaveToDatabaseViewModel = new List<BulkDatas>();
            this.MultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
            this.MultipleFileSaveToDatabase = new List<HttpPostedFileBase>();
            this.MultipleFileSaveToFolderViewModel = new List<FileViewModel>();
            this.MultipleFileSaveToFolder = new List<HttpPostedFileBase>();
            this.MultipleImageSaveToDatabaseViewModel = new List<FileViewModel>();
            this.MultipleImageSaveToDatabase = new List<HttpPostedFileBase>();
            this.MultipleImageSaveToFolderViewModel = new List<FileViewModel>();
            this.MultipleImageSaveToFolder = new List<HttpPostedFileBase>();
            this.BikeCategoriesSelectList = new List<SelectListItem>();
            this.BikeCategories = new List<BikeCategoryViewModel>();
            this.ComplaientCategory = new List<SelectListItem>();
            this.Gender = new List<SelectListItem>();
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
            this.ErrorDetails = new List<ErrorDetails>();
            this.BikeCategoriesSelectListForSelected = new List<CategoriesSelectListViewModel>();
        }
        /// <summary>
        /// Complaient tbl
        /// </summary>
        public int ComplaientId { get; set; }
        public string ComplaientEncryptedId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill name!")]
        [MaxLength(100, ErrorMessage = "Max length 100!")]
        public string ComplainantName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill email!")]
        [MaxLength(100, ErrorMessage = "Max email id length is 100!")]
        [EmailAddress(ErrorMessage = "Please enter valid email!")]
        public string ComplainantEmail { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select complainant date!")]
        [DataType(DataType.Date)]
        [DisplayFormat( DataFormatString = "{0:dd/MMMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CompaientDate { get; set; }
        public string sCompaientDate { get; set; }

        /// <summary>
        /// Complaient Detail tbl
        /// </summary>
        public int ComplaientDetailsId { get; set; }
        public string ComplaientDetailsEncryptedId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select complainant category!")]
        public int ComplaientCategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select country!")]
        public int CountryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select state!")]
        public int StateId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select city!")]
        public int CityId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill description!")]
        [MaxLength(100, ErrorMessage = "Max length 100!")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select complainant date!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ComplaientDate { get; set; }
        public string sComplaientDate { get; set; }

        /// <summary>
        /// PermamentAddress Tbl
        /// </summary>
        public int ComplaientPermamentAddressId { get; set; }
        public string ComplaientPermamentAddressEncryptedId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill permament address!")]
        [MaxLength(100, ErrorMessage = "Max length 100!")]
        [DataType(DataType.MultilineText)]
        public string PermamentAddress { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill permament address postal code!")]
        [MaxLength(6, ErrorMessage = "Max length 6!")]
        public string PermamentAddressPostalCode { get; set; }

        /// <summary>
        /// TempAddress Tbl
        /// </summary>
        public int ComplaientTempAddressId { get; set; }
        public string ComplaientTempAddressEncryptedId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill temp address!")]
        [MaxLength(100, ErrorMessage = "Max length 100!")]
        [DataType(DataType.MultilineText)]
        public string TempAddress { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill temp address postal code!")]
        [MaxLength(6, ErrorMessage = "Max length 6!")]
        public string TempAddressPostalCode { get; set; }
        public bool TermsAndConditions { get; set; }
        public List<SelectListItem> ComplaientCategory { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
        //[Required(AllowEmptyStrings =false,ErrorMessage ="Please select categories option")]
        //public CategoriesOptions? CategoriesOptions { get; set; }
        public List<BikeCategoryViewModel> BikeCategories { get; set; }
        public List<SelectListItem> BikeCategoriesSelectList { get; set; }
        public List<CategoriesSelectListViewModel> BikeCategoriesSelectListForSelected { get; set; }
        public List<int> BikeCategoriesSelectListId { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select gender option")]
        //public GenderOptions? GenderOptions { get; set; }
        public List<SelectListItem> Gender { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select gender!")]
        public int GenderId { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }

        public HttpPostedFileBase SingleImageSaveToFolder { get; set; }
        public List<HttpPostedFileBase> MultipleImageSaveToFolder { get; set; }

        public FileViewModel SingleImageSaveToFolderViewModel { get; set; }
        public List<FileViewModel> MultipleImageSaveToFolderViewModel { get; set; }


        public HttpPostedFileBase SingleImageSaveToDatabase { get; set; }
        public List<HttpPostedFileBase> MultipleImageSaveToDatabase { get; set; }

        public FileViewModel SingleImageSaveToDatabaseViewModel { get; set; }
        public List<FileViewModel> MultipleImageSaveToDatabaseViewModel { get; set; }

        public HttpPostedFileBase SingleFileSaveToFolder { get; set; }
        public List<HttpPostedFileBase> MultipleFileSaveToFolder { get; set; }

        public FileViewModel SingleFileSaveToFolderViewModel { get; set; }
        public List<FileViewModel> MultipleFileSaveToFolderViewModel { get; set; }

        public HttpPostedFileBase SingleFileSaveToDatabase { get; set; }
        public List<HttpPostedFileBase> MultipleFileSaveToDatabase { get; set; }

        public FileViewModel SingleFileSaveToDatabaseViewModel { get; set; }
        public List<FileViewModel> MultipleFileSaveToDatabaseViewModel { get; set; }

        public HttpPostedFileBase ExcelFileDataSaveToDatabase { get; set; }
        public FileViewModel ExcelFileDataSaveToDatabaseViewModel { get; set; }
        public List<BulkDatas> ListOfExcelFileDataSaveToDatabaseViewModel { get; set; }

        public List<ErrorDetails> ErrorDetails { get; set; }
    }
}
