using EmpManagment.Bol.ConstantClass;
using EmpManagment.Bol.Entities;
using EmpManagment.Bol.ViewModels.Security;
using EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels;
using ExcelDataReader;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.DataProtection;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Web.Areas.User.Controllers
{
    public class ComplaientController : BaseController
    {
        private readonly IDataProtector protector;
        public ComplaientController(IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.protector = dataProtectionProvider.Create(dataProtectionPurposeStrings.ComplaientIdRouteValue, dataProtectionPurposeStrings.ComplaientDetailIdRouteValue, dataProtectionPurposeStrings.ComplaientCategoryIdRouteValue);
        }
        public ComplaientController()
        {

        }
        //Get All Category
        [HttpGet]
        public ActionResult ListCategory()
        {
            return View();
        }
        //Get All Category Ajax Request
        [HttpGet]
        public JsonResult GetAllCategoryList()
        {
            var getAllComplaientCategory = sqlComplaientBs.GetAllComplaientCategory();
            return Json(new { data = getAllComplaientCategory }, JsonRequestBehavior.AllowGet);
        }
        //Add Category Ajax Request
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }
        //Add Category Ajax Post Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(ComplaientCategory category)
        {
            if (ModelState.IsValid)
            {
                if (!sqlComplaientBs.CategoryExisits(category.Description))
                {
                    ComplaientCategory complaientCategory = sqlComplaientBs.AddCategory(category);
                    ViewBag.message = "Added";
                    //Clear the Model.
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Category already exisits.");
                }
            }
            return View();
        }
        //Edit Category Ajax Request
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            ComplaientCategory complaientCategory = sqlComplaientBs.GetCategoryById(id);
            ComplaientCategoryViewModel complaientCategoryViewModel = new ComplaientCategoryViewModel
            {
                ComplaientCategoryId = complaientCategory.ComplaientCategoryId,
                Description = complaientCategory.Description,
                Status = complaientCategory.Status,
                UserStatus = complaientCategory.Status == true ? "Active" : "Not Active"
            };
            return View(complaientCategoryViewModel);

        }
        //Edit Category Ajax Post Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(ComplaientCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                ComplaientCategory complaientCategory = sqlComplaientBs.GetCategoryById(category.ComplaientCategoryId);
                if (complaientCategory != null)
                {
                    complaientCategory.Description = category.Description;
                    complaientCategory.Status = category.Status;
                }
                ComplaientCategory updateCategory = sqlComplaientBs.UpdateCategoryById(complaientCategory);
                ViewBag.message = "Update";
            }
            return View(category);
        }
        //Delete Category Ajax Post Request
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = sqlComplaientBs.Delete(id);
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        //Delete Multiple Category Using Ajax Post Request
        [HttpPost]
        public JsonResult MultipleCategoryDelete(List<int> ids)
        {
            bool result = sqlComplaientBs.MultipleCategoryDelete(ids);
            if (result == true)
            {
                return Json(new { success = true, message = "Selected category's deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Selected category's can not delete !" });
            }
        }
        //Custom Validation for check duplicate Description Using remote validation 
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public JsonResult IsDescriptionInUse(string description)
        {
            bool user = sqlComplaientBs.CategoryExisits(description);
            if (user != true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json($"{description} is already in use.", JsonRequestBehavior.AllowGet);
            }
        }
        //Delete category function not implimented but id value getting form view to controller using ajax request 
        [HttpGet]
        public ActionResult DeleteCategorie(int id)
        {
            bool result = true;
            if (result)
            {
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            else
            {
                return Json(new { success = true, message = "Can not delete.Something went wrong" });
            }
        }
        //Delete multiple complaints function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        public ActionResult MultipleCategoriesDelete(List<int> ids)
        {
            bool result = true;
            if (result)
            {
                return Json(new { success = true, message = "Selected categories's deleted successfully." });
            }
            else
            {
                return Json(new { success = true, message = "Selected categories's can not delete !" });
            }
        }
        //Edit Bike Categories Using ajax request function not implimented
        [HttpGet]
        public ActionResult EditBikeCategories(int id)
        {
            BikeCategoryMainViewModel bikeCategoryMainViewModel = new BikeCategoryMainViewModel();
            bikeCategoryMainViewModel.BikeCategoriesViewModel = new BikeCategoriesViewModel()
            {
                BikeCategoryId = 1,
                Name = "Ravi",
                Description = "Ravi Shanker Pandey",
                Status = true
            };
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Active",Value="true"},
                new SelectListItem{ Text="Not-Active",Value="false"}
            };
            bikeCategoryMainViewModel.Genders = gender;
            return PartialView("CreateBikeCategories", bikeCategoryMainViewModel);
        }
        //Create Bike Categories Get Request
        [HttpGet]
        public PartialViewResult CreateBikeCategories()
        {
            BikeCategoryMainViewModel bikeCategoryMainViewModel = new BikeCategoryMainViewModel();
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{ Text="Active",Value="True"},
                new SelectListItem{ Text="Not-Active",Value="False"}
            };
            bikeCategoryMainViewModel.Genders = gender;
            return PartialView("CreateBikeCategories", bikeCategoryMainViewModel);
        }
        //Create Bike Categories Post Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBikeCategories(BikeCategoryMainViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.BikeCategoriesViewModel.BikeCategoryId != 0)
                {
                    //Edit Code Here
                    ViewBag.message = "success";
                    ModelState.Clear();
                    return Json(new { success = true, message = "Bike category create successfully." });
                }
                else
                {
                    //Create Code here
                    ViewBag.message = "success";
                    ModelState.Clear();
                    return Json(new { success = true, message = "Bike category create successfully." });
                }

            }
            List<ErrorDetails> errorsList = new List<ErrorDetails>();
            var errorsResult = (from o in ModelState
                                select new
                                {
                                    Id = o.Key,
                                    Message = o.Value.Errors
                                }).ToList();
            if (errorsResult.Count > 0)
            {
                foreach (var item in errorsResult)
                {
                    ErrorDetails error = new ErrorDetails();
                    error.Key = item.Id.ToString();
                    error.ErrorMessage = item.Message[0].ErrorMessage;
                    errorsList.Add(error);
                }
                ViewBag.message = "fail";
            }
            model.ErrorDetails = errorsList;
            return Json(new { success = false, message = "Please fill the form !", errorsList });
        }

        //...
        //...
        //...
        //...
        //...
        //...
        //...
        //...

        //Get All Complaintes
        [HttpGet]
        public ActionResult complaients()
        {
            return View();
        }
        //Get All Complaintes and bike category using ajax request
        [HttpGet]
        public JsonResult GetAllComplaient()
        {
            ComplainantListViewModel complainantListViewModels = new ComplainantListViewModel();
            List<ComplainantAndDetailsViewModel> getAllComplaient = sqlComplaientBs.GetAllComplaient(protector);
            complainantListViewModels.complainantAndDetailsViewModels = getAllComplaient;
            List<BikeCategoryViewModel> bikeCategory = sqlComplaientBs.GetAllBikeCategory();
            complainantListViewModels.BikeCategory = bikeCategory;
            return Json(new { data = complainantListViewModels }, JsonRequestBehavior.AllowGet);
        }
        //Bind All Complaintes dropdown using ajax request
        public ComplainantViewModel BindMasterData()
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var countries = sqlComplaientBs.CounteryList().ToList();
            var complaientCategory = sqlComplaientBs.ComplaientCategoryList().ToList();
            var gender = sqlComplaientBs.Genders().ToList();
            var bikeCategory = sqlComplaientBs.BikeCategory().ToList();
            var bikeCategoriesSelectList = sqlComplaientBs.BikeCategory().ToList();

            if (countries.Count > 0)
            {
                foreach (var item in countries)
                {
                    complainantViewModel.Countries.Add(new SelectListItem { Text = item.CountryName, Value = item.CountryId.ToString(), Selected = true });
                }
            }
            if (complaientCategory.Count > 0)
            {
                foreach (var item in complaientCategory)
                {
                    complainantViewModel.ComplaientCategory.Add(new SelectListItem { Text = item.Description, Value = item.ComplaientCategoryId.ToString(), Selected = true });
                }
            }
            if (gender.Count > 0)
            {
                foreach (var item in gender)
                {
                    complainantViewModel.Gender.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString(), Selected = true });
                }
            }
            if (bikeCategory.Count > 0)
            {
                complainantViewModel.BikeCategories = bikeCategory.Select(m => new BikeCategoryViewModel()
                {
                    BikeCategoryId = m.BikeCategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Status = m.Status
                }).ToList();
            }
            if (bikeCategoriesSelectList.Count > 0)
            {
                foreach (var item in bikeCategoriesSelectList)
                {
                    complainantViewModel.BikeCategoriesSelectList.Add(new SelectListItem { Text = item.Name, Value = item.BikeCategoryId.ToString(), Selected = true });
                }
            }
            return complainantViewModel;
        }
        //Create Complaintes Get method
        [HttpGet]
        public ActionResult createcomplaient()
        {
            ComplainantViewModel complainantViewModel = BindMasterData();
            return View(complainantViewModel);
        }
        //Create Complaintes Post method
        [HttpPost]
        public ActionResult createcomplaient(ComplainantViewModel model)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            complainantViewModel = BindMasterData();
            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("", "Please check Terms & Conditions check box!");
                complainantViewModel.Error = "Please check Terms & Conditions check box!";
                return View(complainantViewModel);
            }
            if (ModelState.IsValid)
            {
                FileOpertaion(model);
                bool isSaved = sqlComplaientBs.AddComplaient(model);
                if (isSaved == true)
                {
                    complainantViewModel.Message = "Added";
                    ModelState.Clear();
                }
            }
            List<ErrorDetails> errorsList = new List<ErrorDetails>();
            var errorsResult = (from o in ModelState
                                select new
                                {
                                    Id = o.Key,
                                    Message = o.Value.Errors
                                }).ToList();
            if (errorsResult.Count > 0)
            {
                foreach (var item in errorsResult)
                {
                    ErrorDetails error = new ErrorDetails();
                    error.Key = item.Id.ToString();
                    error.ErrorMessage = item.Message[0].ErrorMessage;
                    errorsList.Add(error);
                }
                complainantViewModel.ErrorDetails = errorsList;
            }
            return View(complainantViewModel);
        }
        //To save image folder 
        public ComplainantViewModel FileOpertaion(ComplainantViewModel model)
        {
            string sPath = Server.MapPath("~/Content/Uploads/");
            string sImageUploadFolder = Path.Combine(sPath, ConfigurationManager.AppSettings["uploadImageFolder"]);
            string sFileUploadFolder = Path.Combine(sPath, ConfigurationManager.AppSettings["uploadFileFolder"]);

            if (model.SingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolderViewModel = new FileViewModel();
                if (!Directory.Exists(sImageUploadFolder))
                {
                    Directory.CreateDirectory(sImageUploadFolder);
                }
                singleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToFolder.FileName;
                singleImageSaveToFolderViewModel.Path = Path.Combine(sImageUploadFolder, singleImageSaveToFolderViewModel.Name);
                singleImageSaveToFolderViewModel.ContentType = model.SingleImageSaveToFolder.ContentType;
                singleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
                singleImageSaveToFolderViewModel.FileEncodingTypes = null;
                singleImageSaveToFolderViewModel.Data = null;
                model.SingleImageSaveToFolder.SaveAs(singleImageSaveToFolderViewModel.Path);
                model.SingleImageSaveToFolderViewModel = singleImageSaveToFolderViewModel;
            }
            var vMultipleImageSaveToFolder = model.MultipleImageSaveToFolder.ToList();
            if (vMultipleImageSaveToFolder[0] != null)
            {
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleImageSaveToFolder)
                {
                    FileViewModel listMultipleImageSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(sImageUploadFolder))
                    {
                        Directory.CreateDirectory(sImageUploadFolder);
                    }
                    listMultipleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    listMultipleImageSaveToFolderViewModel.Path = Path.Combine(sImageUploadFolder, listMultipleImageSaveToFolderViewModel.Name);
                    listMultipleImageSaveToFolderViewModel.ContentType = photo.ContentType;
                    listMultipleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
                    listMultipleImageSaveToFolderViewModel.FileEncodingTypes = null;
                    listMultipleImageSaveToFolderViewModel.Data = null;
                    photo.SaveAs(listMultipleImageSaveToFolderViewModel.Path);
                    listMultipleImageSaveToFolder.Add(listMultipleImageSaveToFolderViewModel);
                }
                model.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }

            if (model.SingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabaseViewModel = new FileViewModel();
                fileSingleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToDatabase.FileName;
                fileSingleImageSaveToDatabaseViewModel.Path = null;
                fileSingleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
                fileSingleImageSaveToDatabaseViewModel.FileEncodingTypes = null;
                if (model.SingleImageSaveToDatabase.InputStream.Length >= 0)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(model.SingleImageSaveToDatabase.InputStream))
                    {
                        bytes = br.ReadBytes(model.SingleImageSaveToDatabase.ContentLength);
                    }
                    fileSingleImageSaveToDatabaseViewModel.Data = bytes;
                }
                else
                {
                    fileSingleImageSaveToDatabaseViewModel.Data = null;
                }
                model.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabaseViewModel;
            }

            var vMultipleImageSaveToDatabase = model.MultipleImageSaveToDatabase.ToList();
            if (vMultipleImageSaveToDatabase[0] != null)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleImageSaveToDatabase)
                {
                    FileViewModel multipleImageSaveToDatabaseViewModel = new FileViewModel();
                    multipleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleImageSaveToDatabaseViewModel.Path = null;
                    multipleImageSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
                    multipleImageSaveToDatabaseViewModel.FileEncodingTypes = null;
                    if (photo.InputStream.Length >= 0)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(photo.InputStream))
                        {
                            bytes = br.ReadBytes(photo.ContentLength);
                        }
                        multipleImageSaveToDatabaseViewModel.Data = bytes;
                    }
                    else
                    {
                        multipleImageSaveToDatabaseViewModel.Data = null;
                    }
                    listMultipleImageSaveToDatabase.Add(multipleImageSaveToDatabaseViewModel);
                }
                model.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }

            if (model.SingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolderViewModel = new FileViewModel();
                if (!Directory.Exists(sFileUploadFolder))
                {
                    Directory.CreateDirectory(sFileUploadFolder);
                }
                singleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToFolder.FileName;
                singleFileSaveToFolderViewModel.Path = Path.Combine(sFileUploadFolder, singleFileSaveToFolderViewModel.Name);
                singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolder.ContentType;
                singleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
                singleFileSaveToFolderViewModel.FileEncodingTypes = null;
                singleFileSaveToFolderViewModel.Data = null;
                model.SingleImageSaveToFolder.SaveAs(singleFileSaveToFolderViewModel.Path);
                model.SingleFileSaveToFolderViewModel = singleFileSaveToFolderViewModel;
            }

            var vMultipleFileSaveToFolder = model.MultipleFileSaveToFolder.ToList();
            if (vMultipleFileSaveToFolder[0] != null)
            {
                List<FileViewModel> listMultipleFileSaveToFolderViewModel = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(sFileUploadFolder))
                    {
                        Directory.CreateDirectory(sFileUploadFolder);
                    }
                    multipleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToFolderViewModel.Path = Path.Combine(sFileUploadFolder, multipleFileSaveToFolderViewModel.Name);
                    multipleFileSaveToFolderViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
                    multipleFileSaveToFolderViewModel.Data = null;
                    multipleFileSaveToFolderViewModel.FileEncodingTypes = null;
                    photo.SaveAs(multipleFileSaveToFolderViewModel.Path);
                    listMultipleFileSaveToFolderViewModel.Add(multipleFileSaveToFolderViewModel);
                }
                model.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolderViewModel;
            }

            if (model.SingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabaseViewModel = new FileViewModel();
                singleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToDatabase.FileName;
                singleFileSaveToDatabaseViewModel.Path = null;
                singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabase.ContentType;
                singleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
                singleFileSaveToDatabaseViewModel.FileEncodingTypes = null;
                if (model.SingleFileSaveToDatabase.InputStream.Length >= 0)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(model.SingleFileSaveToDatabase.InputStream))
                    {
                        bytes = br.ReadBytes(model.SingleFileSaveToDatabase.ContentLength);
                    }
                    singleFileSaveToDatabaseViewModel.Data = bytes;
                }
                else
                {
                    singleFileSaveToDatabaseViewModel.Data = null;
                }
                model.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabaseViewModel;
            }

            var vMultipleFileSaveToDatabase = model.MultipleFileSaveToDatabase.ToList();
            if (vMultipleFileSaveToDatabase[0] != null)
            {
                List<FileViewModel> listMultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabaseViewModel = new FileViewModel();
                    multipleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToDatabaseViewModel.Path = null;
                    multipleFileSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
                    multipleFileSaveToDatabaseViewModel.FileEncodingTypes = null;
                    if (photo.InputStream.Length >= 0)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(photo.InputStream))
                        {
                            bytes = br.ReadBytes(photo.ContentLength);
                        }
                        multipleFileSaveToDatabaseViewModel.Data = bytes;
                    }
                    else
                    {
                        multipleFileSaveToDatabaseViewModel.Data = null;
                    }
                    listMultipleFileSaveToDatabaseViewModel.Add(multipleFileSaveToDatabaseViewModel);
                }
                model.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabaseViewModel;
            }

            if (model.ExcelFileDataSaveToDatabase != null)
            {
                FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                excelFileDataSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.ExcelFileDataSaveToDatabase.FileName;
                excelFileDataSaveToDatabaseViewModel.Path = null;
                excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabase.ContentType;
                excelFileDataSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = null;
                excelFileDataSaveToDatabaseViewModel.Data = null;
                model.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;

                string path = string.Empty;
                if (!Directory.Exists(sFileUploadFolder))
                {
                    Directory.CreateDirectory(sFileUploadFolder);
                }
                path = Path.Combine(sFileUploadFolder, excelFileDataSaveToDatabaseViewModel.Name);
                model.ExcelFileDataSaveToDatabase.SaveAs(path);
                var result = Getdata(path);
                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        if (string.IsNullOrEmpty(item.Email) || string.IsNullOrWhiteSpace(item.Email) || !CommanFunction.ValidateEmail(item.Email))
                //        {
                //            ModelState.AddModelError("", "" + item.Email + " Id not valid!");
                //            complainantViewModel.Error = $"{item.Email} Id not valid!";
                //            return View(complainantViewModel);
                //        }
                //    }
                //}
                model.ListOfExcelFileDataSaveToDatabaseViewModel = result;
            }
            return model;
        }
        //To Get all excel data 
        private List<BulkDatas> Getdata(string name)
        {
            List<BulkDatas> bulk = new List<BulkDatas>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(name, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        BulkDatas bulkData = new BulkDatas();
                        bulkData.Name = reader.GetValue(0).ToString();
                        bulkData.Des = reader.GetValue(1).ToString();
                        bulkData.Email = reader.GetValue(2).ToString();
                        bulkData.MobileNumber = reader.GetValue(3).ToString();
                        bulk.Add(bulkData);
                        //bulk.Add(new BulkData()
                        //{
                        //    Name = reader.GetValue(0).ToString(),
                        //    Des = reader.GetValue(1).ToString()
                        //});
                    }
                }
            }
            return bulk;
        }
        //Edit Get request 
        [HttpGet]
        public ActionResult EditComplaient(string id)
        {
            // Decrypt the employee id using Unprotect method
            /*string decryptedComplaientId = Convert.ToString(protector.Unprotect(System.Text.Encoding.Unicode.GetBytes(id)))*/
            ;
            ComplainantViewModel complainantViewModel = sqlComplaientBs.GetComplainantBiId(Convert.ToInt32(id));
            // Encrypt the ComplaientId value and store in ComplaientEncryptedId property
            //complainantViewModel.ComplaientEncryptedId = Convert.ToString(protector.Protect(System.Text.Encoding.Unicode.GetBytes(id)));
            //complainantViewModel.ComplaientDetailsEncryptedId = Convert.ToString(protector.Protect(System.Text.Encoding.Unicode.GetBytes(complainantViewModel.ComplaientDetailsId.ToString())));
            //complainantViewModel.ComplaientPermamentAddressEncryptedId = Convert.ToString(protector.Protect(System.Text.Encoding.Unicode.GetBytes(complainantViewModel.ComplaientPermamentAddressId.ToString())));
            //complainantViewModel.ComplaientTempAddressEncryptedId = Convert.ToString(protector.Protect(System.Text.Encoding.Unicode.GetBytes(complainantViewModel.ComplaientTempAddressId.ToString())));

            MakeUrl(complainantViewModel);
            return View(complainantViewModel);
        }
        //Edit Get request to make image and file src
        private static void MakeUrl(ComplainantViewModel complainantViewModel)
        {
            if (complainantViewModel.SingleImageSaveToFolderViewModel != null)
            {
                //For Single Image from folder
                //Convert path Uri
                Uri uriSingleImageToFolder = new Uri(complainantViewModel.SingleImageSaveToFolderViewModel.Path);
                //Convert to Uri AbsolutePath
                string singleImageToFolderAbsolutePath = uriSingleImageToFolder.AbsolutePath;
                //Split from root floder
                List<string> snewSingleImageToFolderPath = new List<string>(singleImageToFolderAbsolutePath.Split(new string[] { "/Content/" }, StringSplitOptions.None));
                //get SolutionProjectName from app setting and make new url
                string singleImageToFolderPath = "/" + ConfigurationManager.AppSettings["SolutionProjectName"] + "/Content/" + snewSingleImageToFolderPath[1];
                //Re-Assinged path to model
                complainantViewModel.SingleImageSaveToFolderViewModel.Path = singleImageToFolderPath;// singleImageToFolderPath;

                //Remove guid number from name and get orignal name 
                string[] splitSingleImageToFolderName = complainantViewModel.SingleImageSaveToFolderViewModel.Name.Split('_');
                //Re-Assinged name
                complainantViewModel.SingleImageSaveToFolderViewModel.Name = splitSingleImageToFolderName[1];
            }

            //For Multiple Image from folder
            //List of image 
            if (complainantViewModel.MultipleImageSaveToFolderViewModel != null)
            {
                List<FileViewModel> listMultipleImageSaveToFolderViewModel = new List<FileViewModel>();
                foreach (var item in complainantViewModel.MultipleImageSaveToFolderViewModel)
                {
                    FileViewModel fileViewModel = new FileViewModel();
                    fileViewModel.Id = item.Id;
                    //Convert in Uri
                    Uri uriMultipleImageToFolder = new Uri(item.Path);
                    //Convert to Uri AbsolutePath
                    string multipleImageToFolderAbsolutePath = uriMultipleImageToFolder.AbsolutePath;
                    ////Split from root floder
                    List<string> newMultipleImageToFolderPath = new List<string>(multipleImageToFolderAbsolutePath.Split(new string[] { "/Content/" }, StringSplitOptions.None));
                    //get SolutionProjectName from app setting and make new url
                    string multipleImageToFolderPath = "/" + ConfigurationManager.AppSettings["SolutionProjectName"] + "/Content/" + newMultipleImageToFolderPath[1];
                    //Re-Assinged path to model
                    fileViewModel.Path = multipleImageToFolderPath;
                    //Remove guid number from name and get orignal name 
                    string[] splitMultipleImageToFolderName = item.Name.Split('_');
                    //Re-Assinged name
                    fileViewModel.Name = splitMultipleImageToFolderName[1];
                    listMultipleImageSaveToFolderViewModel.Add(fileViewModel);
                }
                complainantViewModel.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolderViewModel;
            }
        }
        //Edit Post request 
        [HttpPost]
        public ActionResult EditComplaient(ComplainantViewModel model)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            complainantViewModel = model;
            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("", "Please check Terms & Conditions check box!");
                complainantViewModel.Error = "Please check Terms & Conditions check box!";
                return View(complainantViewModel);
            }
            if (ModelState.IsValid)
            {
                //model.ComplaientId = Convert.ToInt32(protector.Unprotect(System.Text.Encoding.Unicode.GetBytes(model.ComplaientEncryptedId)));
                //model.ComplaientDetailsId = Convert.ToInt32(protector.Unprotect(System.Text.Encoding.Unicode.GetBytes(model.ComplaientDetailsEncryptedId)));
                //model.ComplaientPermamentAddressId = Convert.ToInt32(protector.Unprotect(System.Text.Encoding.Unicode.GetBytes(model.ComplaientPermamentAddressEncryptedId)));
                //model.ComplaientTempAddressId = Convert.ToInt32(protector.Unprotect(System.Text.Encoding.Unicode.GetBytes(model.ComplaientTempAddressEncryptedId)));
                FileOpertaionEditMode(model);
                bool updateComplaient = sqlComplaientBs.UpdateComplaient(model);
                if (updateComplaient)
                {
                    return RedirectToAction("complaients");
                }
            }
            return View(complainantViewModel);
        }
        //Edit Post request to make image and file src 
        public ComplainantViewModel FileOpertaionEditMode(ComplainantViewModel model)
        {
            string sPath = Server.MapPath("~/Content/Uploads/");
            string sImageUploadFolder = Path.Combine(sPath, ConfigurationManager.AppSettings["uploadImageFolder"]);
            string sFileUploadFolder = Path.Combine(sPath, ConfigurationManager.AppSettings["uploadFileFolder"]);
            if (model.SingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolderViewModel = new FileViewModel();
                if (model.SingleImageSaveToFolderViewModel != null)
                {
                    string sName = sqlComplaientBs.GetFileNameById(model.SingleImageSaveToFolderViewModel.Id);
                    string fullPath = Path.Combine(sImageUploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!Directory.Exists(sImageUploadFolder))
                {
                    Directory.CreateDirectory(sImageUploadFolder);
                }
                singleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToFolder.FileName;
                singleImageSaveToFolderViewModel.Path = Path.Combine(sImageUploadFolder, singleImageSaveToFolderViewModel.Name);
                singleImageSaveToFolderViewModel.ContentType = model.SingleImageSaveToFolder.ContentType;
                singleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
                singleImageSaveToFolderViewModel.FileEncodingTypes = null;
                singleImageSaveToFolderViewModel.Data = null;
                model.SingleImageSaveToFolder.SaveAs(singleImageSaveToFolderViewModel.Path);
                model.SingleImageSaveToFolderViewModel = singleImageSaveToFolderViewModel;
            }
            else
            {
                model.SingleImageSaveToFolderViewModel = null;
            }
            var vMultipleImageSaveToFolder = model.MultipleImageSaveToFolder.ToList();
            if (vMultipleImageSaveToFolder[0] != null)
            {
                //Delete previous image from folder
                var getFilesId = sqlComplaientBs.GetAllFiles(Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder), model.ComplaientDetailsId);
                if (getFilesId.Count > 0)
                {
                    foreach (var item in getFilesId)
                    {
                        if (item != null)
                        {
                            string fullPath = Path.Combine(sImageUploadFolder, item);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }
                }
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleImageSaveToFolder)
                {
                    FileViewModel listMultipleImageSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(sImageUploadFolder))
                    {
                        Directory.CreateDirectory(sImageUploadFolder);
                    }
                    listMultipleImageSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    listMultipleImageSaveToFolderViewModel.Path = Path.Combine(sImageUploadFolder, listMultipleImageSaveToFolderViewModel.Name);
                    listMultipleImageSaveToFolderViewModel.ContentType = photo.ContentType;
                    listMultipleImageSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
                    listMultipleImageSaveToFolderViewModel.FileEncodingTypes = null;
                    listMultipleImageSaveToFolderViewModel.Data = null;
                    photo.SaveAs(listMultipleImageSaveToFolderViewModel.Path);
                    listMultipleImageSaveToFolder.Add(listMultipleImageSaveToFolderViewModel);
                }
                model.MultipleImageSaveToFolderViewModel = null;
                model.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }
            else
            {
                model.MultipleImageSaveToFolderViewModel = null;
            }

            if (model.SingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabaseViewModel = new FileViewModel();
                fileSingleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleImageSaveToDatabase.FileName;
                fileSingleImageSaveToDatabaseViewModel.Path = null;
                fileSingleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
                fileSingleImageSaveToDatabaseViewModel.FileEncodingTypes = null;
                if (model.SingleImageSaveToDatabase.InputStream.Length >= 0)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(model.SingleImageSaveToDatabase.InputStream))
                    {
                        bytes = br.ReadBytes(model.SingleImageSaveToDatabase.ContentLength);
                    }
                    fileSingleImageSaveToDatabaseViewModel.Data = bytes;
                }
                else
                {
                    fileSingleImageSaveToDatabaseViewModel.Data = null;
                }
                model.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabaseViewModel;
            }
            else
            {
                model.SingleImageSaveToDatabaseViewModel = null;
            }

            var vMultipleImageSaveToDatabase = model.MultipleImageSaveToDatabase.ToList();
            if (vMultipleImageSaveToDatabase[0] != null)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleImageSaveToDatabase)
                {
                    FileViewModel multipleImageSaveToDatabaseViewModel = new FileViewModel();
                    multipleImageSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleImageSaveToDatabaseViewModel.Path = null;
                    multipleImageSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleImageSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
                    multipleImageSaveToDatabaseViewModel.FileEncodingTypes = null;
                    if (photo.InputStream.Length >= 0)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(photo.InputStream))
                        {
                            bytes = br.ReadBytes(photo.ContentLength);
                        }
                        multipleImageSaveToDatabaseViewModel.Data = bytes;
                    }
                    else
                    {
                        multipleImageSaveToDatabaseViewModel.Data = null;
                    }
                    listMultipleImageSaveToDatabase.Add(multipleImageSaveToDatabaseViewModel);
                }
                model.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }
            else
            {
                model.MultipleImageSaveToDatabaseViewModel = null;
            }

            if (model.SingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolderViewModel = new FileViewModel();
                if (model.SingleFileSaveToFolderViewModel != null)
                {
                    string sName = sqlComplaientBs.GetFileNameById(model.SingleFileSaveToFolderViewModel.Id);
                    string fullPath = Path.Combine(sFileUploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!Directory.Exists(sFileUploadFolder))
                {
                    Directory.CreateDirectory(sFileUploadFolder);
                }
                singleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToFolder.FileName;
                singleFileSaveToFolderViewModel.Path = Path.Combine(sFileUploadFolder, singleFileSaveToFolderViewModel.Name);
                singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolder.ContentType;
                singleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
                singleFileSaveToFolderViewModel.FileEncodingTypes = null;
                singleFileSaveToFolderViewModel.Data = null;
                model.SingleFileSaveToFolder.SaveAs(singleFileSaveToFolderViewModel.Path);
                model.SingleFileSaveToFolderViewModel = singleFileSaveToFolderViewModel;
            }
            else
            {
                model.SingleFileSaveToFolderViewModel = null;
            }
            var vMultipleFileSaveToFolder = model.MultipleFileSaveToFolder.ToList();
            if (vMultipleFileSaveToFolder[0] != null)
            {
                //Delete previous file from folder
                var getFilesName = sqlComplaientBs.GetAllFiles(Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder), model.ComplaientDetailsId);
                if (getFilesName.Count > 0)
                {
                    foreach (var item in getFilesName)
                    {
                        if (item != null)
                        {
                            string fullPath = Path.Combine(sFileUploadFolder, item);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }
                }
                List<FileViewModel> listMultipleFileSaveToFolderViewModel = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolderViewModel = new FileViewModel();
                    if (!Directory.Exists(sFileUploadFolder))
                    {
                        Directory.CreateDirectory(sFileUploadFolder);
                    }
                    multipleFileSaveToFolderViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToFolderViewModel.Path = Path.Combine(sFileUploadFolder, multipleFileSaveToFolderViewModel.Name);
                    multipleFileSaveToFolderViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToFolderViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
                    multipleFileSaveToFolderViewModel.Data = null;
                    multipleFileSaveToFolderViewModel.FileEncodingTypes = null;
                    photo.SaveAs(multipleFileSaveToFolderViewModel.Path);
                    listMultipleFileSaveToFolderViewModel.Add(multipleFileSaveToFolderViewModel);
                }
                model.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolderViewModel;
            }
            else
            {
                model.MultipleFileSaveToFolderViewModel = null;
            }

            if (model.SingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabaseViewModel = new FileViewModel();
                singleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.SingleFileSaveToDatabase.FileName;
                singleFileSaveToDatabaseViewModel.Path = null;
                singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabase.ContentType;
                singleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
                singleFileSaveToDatabaseViewModel.FileEncodingTypes = null;

                if (model.SingleFileSaveToDatabase.InputStream.Length >= 0)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(model.SingleFileSaveToDatabase.InputStream))
                    {
                        bytes = br.ReadBytes(model.SingleFileSaveToDatabase.ContentLength);
                    }
                    singleFileSaveToDatabaseViewModel.Data = bytes;
                }
                else
                {
                    singleFileSaveToDatabaseViewModel.Data = null;
                }
                model.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabaseViewModel;
            }
            else
            {
                model.SingleFileSaveToDatabaseViewModel = null;
            }
            var vMultipleFileSaveToDatabase = model.MultipleFileSaveToDatabase.ToList();
            if (vMultipleFileSaveToDatabase[0] != null)
            {
                List<FileViewModel> listMultipleFileSaveToDatabaseViewModel = new List<FileViewModel>();
                foreach (HttpPostedFileBase photo in model.MultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabaseViewModel = new FileViewModel();
                    multipleFileSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    multipleFileSaveToDatabaseViewModel.Path = null;
                    multipleFileSaveToDatabaseViewModel.ContentType = photo.ContentType;
                    multipleFileSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
                    multipleFileSaveToDatabaseViewModel.FileEncodingTypes = null;
                    if (photo.InputStream.Length >= 0)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(photo.InputStream))
                        {
                            bytes = br.ReadBytes(photo.ContentLength);
                        }
                        multipleFileSaveToDatabaseViewModel.Data = bytes;
                    }
                    else
                    {
                        multipleFileSaveToDatabaseViewModel.Data = null;
                    }
                    listMultipleFileSaveToDatabaseViewModel.Add(multipleFileSaveToDatabaseViewModel);
                }
                model.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabaseViewModel;
            }
            else
            {
                model.MultipleFileSaveToDatabaseViewModel = null;
            }

            if (model.ExcelFileDataSaveToDatabase != null)
            {
                FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                if (model.ExcelFileDataSaveToDatabaseViewModel != null)
                {
                    string sName = sqlComplaientBs.GetFileNameById(model.ExcelFileDataSaveToDatabaseViewModel.Id);
                    string fullPath = Path.Combine(sFileUploadFolder, sName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                excelFileDataSaveToDatabaseViewModel.Name = Guid.NewGuid().ToString() + "_" + model.ExcelFileDataSaveToDatabase.FileName;
                excelFileDataSaveToDatabaseViewModel.Path = null;
                excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabase.ContentType;
                excelFileDataSaveToDatabaseViewModel.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = null;
                excelFileDataSaveToDatabaseViewModel.Data = null;
                model.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;

                string path = string.Empty;
                if (!Directory.Exists(sFileUploadFolder))
                {
                    Directory.CreateDirectory(sFileUploadFolder);
                }
                path = Path.Combine(sFileUploadFolder, excelFileDataSaveToDatabaseViewModel.Name);
                model.ExcelFileDataSaveToDatabase.SaveAs(path);

                var result = Getdata(path);
                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        if (string.IsNullOrEmpty(item.Email) || string.IsNullOrWhiteSpace(item.Email) || !CommanFunction.ValidateEmail(item.Email))
                //        {
                //            ModelState.AddModelError("", "" + item.Email + " Id not valid!");
                //            complainantViewModel.Error = $"{item.Email} Id not valid!";
                //            return View(complainantViewModel);
                //        }
                //    }
                //}
                model.ListOfExcelFileDataSaveToDatabaseViewModel = result;
            }
            else
            {
                model.ListOfExcelFileDataSaveToDatabaseViewModel = null;
            }
            return model;
        }
        //Dowonload image using id 
        public FileResult DownloadFileFromFolder(int fileId)
        {
            var getFile = sqlComplaientBs.DownloadFile(fileId);
            byte[] fileBytes = null;
            if (getFile.Data != null)
            {
                Uri uri = new Uri(getFile.Path);
                string uriAbsolutePath = uri.AbsolutePath;
                fileBytes = GetFile(uriAbsolutePath);
                string[] name = getFile.Name.Split('_');
                return File(fileBytes, getFile.ContentType, name[1]);
            }
            else
            {
                Uri uri = new Uri(getFile.Path);
                string uriAbsolutePath = uri.AbsolutePath;
                fileBytes = GetFile(uriAbsolutePath);
                string[] name = getFile.Name.Split('_');
                return File(fileBytes, getFile.ContentType, name[1]);
            }
        }
        //Get file in byte arrary 
        public byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        //Download file from database
        public FileResult DownloadFileFromDb(int fileId)
        {
            //Fetch the File data from Database.
            var getFile = sqlComplaientBs.DownloadFile(fileId);
            if (getFile.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase))
            {

            }
            string[] name = getFile.Name.Split('_');
            return File(getFile.Data, getFile.ContentType, name[1]);
        }
        //Download Excel file from database
        public FileResult DownloadExcelFileFromDb(int fileId)
        {
            //Fetch the File data from Database.
            var getFile = sqlComplaientBs.DownloadFile(fileId);
            string[] name = getFile.Name.Split('_');
            if (getFile.FileStoreMode == Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase))
            {
                var bulkDatas = sqlComplaientBs.GetExcelData(getFile.FileStoreMode);
                if (bulkDatas.Count > 0)
                {
                    var comlumHeadrs = new string[]
                                       {
                                            "Bulk Data Id",
                                            "Bulk Id",
                                            "Name",
                                            "Email",
                                            "Mobile Number",
                                            "Des"
                                       };
                    byte[] result;
                    // If you are a commercial business and have
                    // purchased commercial licenses use the static property
                    // LicenseContext of the ExcelPackage class:
                    //ExcelPackage.LicenseContext = LicenseContext.Commercial;

                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        // add a new worksheet to the empty workbook

                        var worksheet = package.Workbook.Worksheets.Add(name[1]); //Worksheet name
                        using (var cells = worksheet.Cells[1, 1, 1, 6]) //(1,1) (1,5)
                        {
                            cells.Style.Font.Bold = true;
                        }

                        //First add the headers
                        for (var i = 0; i < comlumHeadrs.Count(); i++)
                        {
                            worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                        }
                        //Add values
                        var j = bulkDatas.Count + 1;
                        foreach (var employee in bulkDatas)
                        {
                            if (j > 0)
                            {
                                worksheet.Cells["A" + j].Value = employee.Id;
                                worksheet.Cells["B" + j].Value = employee.BulkId;
                                worksheet.Cells["C" + j].Value = employee.Name;
                                worksheet.Cells["D" + j].Value = employee.Email;
                                worksheet.Cells["E" + j].Value = employee.MobileNumber;
                                worksheet.Cells["F" + j].Value = employee.Des;
                                j = j - 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        result = package.GetAsByteArray();
                    }
                    string[] splitName = name[1].Split('.');
                    string fileName = $"{splitName[0]}-{DateTime.Now.ToShortDateString()}.{splitName[1]}";
                    return File(result, getFile.ContentType, $"{fileName}");
                }
            }
            return null;
        }
        //Delete complaint function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        public ActionResult DeleteComplaient(int id)
        {
            bool result = true;
            if (result)
            {
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            else
            {
                return Json(new { success = true, message = "Selected complaints's can not delete !" });
            }
        }
        //Delete Multiple complaints function not implimented but id value getting form view to controller using ajax request 
        [HttpPost]
        public ActionResult MultipleComplaientDelete(List<int> ids)
        {
            bool result = true;
            if (result)
            {
                return Json(new { success = true, message = "Selected complaints's deleted successfully." });
            }
            else
            {
                return Json(new { success = true, message = "Selected complaints's can not delete !" });
            }
        }
        //Bind State using ajax request 
        [HttpGet]
        public JsonResult GetStatelist(int countryId)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var states = sqlComplaientBs.StateList(countryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    complainantViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId)
                    });
                }
                return Json(complainantViewModel);
            }
            return Json(null);
        }
        //Bind City using ajax request 
        [HttpGet]
        public JsonResult GetCitylist(int stateId)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var cities = sqlComplaientBs.CityList(stateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    complainantViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
                return Json(complainantViewModel);
            }
            return Json(null);
        }
    }
}