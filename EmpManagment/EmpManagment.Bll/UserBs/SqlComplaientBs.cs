using EmpManagment.Bol.ConstantClass;
using EmpManagment.Bol.Entities;
using EmpManagment.Bol.ViewModels.Security;
using EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels;
using EmpManagment.Dal.DbContextClass;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpManagment.Bll.UserBs
{
    public class SqlComplaientBs
    {
        private EmployeeDbContext DbContext;
        //public SqlComplaientBs(EmployeeDbContext employeeDbContext)
        //{
        //    DbContext = employeeDbContext;
        //}

        public SqlComplaientBs()
        {
            this.DbContext = new EmployeeDbContext();
        }

        public ComplaientCategory AddCategory(ComplaientCategory category)
        {
            DbContext.ComplaientCategories.Add(category);
            DbContext.SaveChanges();
            return category;
        }
        public bool CategoryExisits(string description)
        {
            bool isDescriptionRxisits = false;
            var checkIfExisits = DbContext.ComplaientCategories.Where(x => x.Description == description).FirstOrDefault();
            if (checkIfExisits != null)
            {
                isDescriptionRxisits = true;
            }
            return isDescriptionRxisits;
        }
        public ComplaientCategory Delete(int complaientCategoryId)
        {
            ComplaientCategory result = DbContext.ComplaientCategories.Find(complaientCategoryId);
            if (result != null)
            {
                DbContext.ComplaientCategories.Remove(result);
                DbContext.SaveChanges();
            }
            return result;
        }
        public List<ComplaientCategoryViewModel> GetAllComplaientCategory()
        {
            //Query with where condition
            //var getAllCategory = DbContext.ComplaientCategories.Where(x => x.Status == true).Select(
            //   a => new ComplaientCategoryViewModel
            //   {
            //       ComplaientCategoryId = a.ComplaientCategoryId,
            //       Description = a.Description,
            //       Status = ((a.Status == true) ? "Active" : "No Active")
            //   });
            var getAllCategory = (from o in DbContext.ComplaientCategories select o).ToList();
            if (getAllCategory.Count > 0)
            {
                List<ComplaientCategoryViewModel> listComplaientCategoryViewModel = new List<ComplaientCategoryViewModel>();
                foreach (var item in getAllCategory)
                {
                    ComplaientCategoryViewModel complaientCategoryViewModel = new ComplaientCategoryViewModel();
                    complaientCategoryViewModel.ComplaientCategoryId = item.ComplaientCategoryId;
                    complaientCategoryViewModel.Description = item.Description;
                    complaientCategoryViewModel.Status = item.Status;
                    complaientCategoryViewModel.UserStatus = item.Status == true ? "Active" : "Not-Active";
                    listComplaientCategoryViewModel.Add(complaientCategoryViewModel);
                }
                return listComplaientCategoryViewModel;
            }
            return null;
        }
        public ComplaientCategory GetCategoryById(int complaientCategoryId)
        {
            var result = DbContext.ComplaientCategories.Where(x => x.ComplaientCategoryId == complaientCategoryId).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public bool MultipleCategoryDelete(List<int> ids)
        {
            bool deletedSuccessfully = false;
            if (ids.Count > 0)
            {
                foreach (var item in ids)
                {
                    ComplaientCategory result = DbContext.ComplaientCategories.Find(item);
                    if (result != null)
                    {
                        DbContext.ComplaientCategories.Remove(result);
                        DbContext.SaveChanges();
                    }
                }
                deletedSuccessfully = true;
            }
            return deletedSuccessfully;
        }
        public ComplaientCategory UpdateCategoryById(ComplaientCategory category)
        {
            var updateCategory = DbContext.ComplaientCategories.Attach(category);
            DbContext.Entry(updateCategory).State = EntityState.Modified;
            DbContext.SaveChanges();
            return category;
        }
        public IEnumerable<Country> CounteryList()
        {
            var allCountries = DbContext.Countries.Where(x => x.Status == true).ToList();
            if (allCountries.Count > 0)
            {
                return allCountries;
            }
            return null;
        }
        public IEnumerable<State> StateList(int countryId)
        {
            var getStateByCountry = DbContext.States.Where(x => x.CountryId == countryId && x.Status == true).ToList();
            if (getStateByCountry.Count > 0)
            {
                return getStateByCountry;
            }
            return null;
        }
        public IEnumerable<City> CityList(int stateId)
        {
            var getCityByState = DbContext.Cities.Where(x => x.StateId == stateId && x.Status == true).ToList();
            if (getCityByState.Count > 0)
            {
                return getCityByState;
            }
            return null;
        }
        public IEnumerable<ComplaientCategory> ComplaientCategoryList()
        {
            var allComplaientCategory = DbContext.ComplaientCategories.Where(x => x.Status == true).ToList();
            if (allComplaientCategory.Count > 0)
            {
                return allComplaientCategory;
            }
            return null;
        }
        public IEnumerable<Gender> Genders()
        {
            var gender = DbContext.Genders.Where(x => x.Status == true).ToList();
            if (gender.Count > 0)
            {
                return gender;
            }
            return null;
        }
        public IEnumerable<BikeCategory> BikeCategory()
        {
            var result = DbContext.BikeCategories.ToList();
            if (result.Count > 0)
            {
                return result;
            }
            return null;
        }
        public bool AddComplaient(ComplainantViewModel model)
        {
            bool isSavedSucess = false;
            if (model.TermsAndConditions == true)
            {
                int iComplaientId = 0;
                int iComplaientDetailsId = 0;

                //Save Complainet Table values
                Complaients complaients = new Complaients();
                complaients.ComplainantName = model.ComplainantName;
                complaients.ComplainantEmail = model.ComplainantEmail;
                complaients.ComplaientStatus = true;
                complaients.CompaientDate = model.CompaientDate;
                DbContext.Complaients.Add(complaients);
                DbContext.SaveChanges();
                iComplaientId = complaients.ComplaientId;

                //Save ComplaientDetails Table values
                ComplaientDetails complaientDetails = new ComplaientDetails();
                complaientDetails.ComplaientId = iComplaientId;
                complaientDetails.ComplaientCategoryId = model.ComplaientCategoryId;
                complaientDetails.GenderId = model.GenderId;
                complaientDetails.CountryId = model.CountryId;
                complaientDetails.StateId = model.StateId;
                complaientDetails.CityId = model.CityId;
                complaientDetails.Description = model.Description;
                complaientDetails.ComplaientDate = model.ComplaientDate;
                DbContext.ComplaientDetails.Add(complaientDetails);
                DbContext.SaveChanges();
                iComplaientDetailsId = complaientDetails.ComplaientDetailsId;

                //Save ComplaientPermamentAddress Table values
                ComplaientPermamentAddress complaientPermamentAddress = new ComplaientPermamentAddress();
                complaientPermamentAddress.ComplaientDetailsId = iComplaientDetailsId;
                complaientPermamentAddress.Address = model.PermamentAddress;
                complaientPermamentAddress.PostalCode = Convert.ToInt32(model.PermamentAddressPostalCode);
                DbContext.ComplaientPermamentAddresses.Add(complaientPermamentAddress);
                DbContext.SaveChanges();

                //Save ComplaientTempAddress Table values
                ComplaientTempAddress complaientTempAddress = new ComplaientTempAddress();
                complaientTempAddress.ComplaientDetailsId = iComplaientDetailsId;
                complaientTempAddress.Address = model.TempAddress;
                complaientTempAddress.PostalCode = Convert.ToInt32(model.TempAddressPostalCode);
                DbContext.ComplaientTempAddresses.Add(complaientTempAddress);
                DbContext.SaveChanges();

                //Save BikeCategories Table values
                if (model.BikeCategories.Count > 0)
                {
                    foreach (var item in model.BikeCategories)
                    {
                        if (item.Status == true)
                        {
                            BikeCollection bikeCollection = new BikeCollection();
                            bikeCollection.ComplaientDetailsId = iComplaientDetailsId;
                            bikeCollection.BikeCategoryId = item.BikeCategoryId;
                            bikeCollection.Status = item.Status;
                            bikeCollection.CreatedDate = DateTime.Now;
                            DbContext.BikeCollections.Add(bikeCollection);
                            DbContext.SaveChanges();
                        }
                    }
                }

                //Save SingleImageSaveToFolder
                if (model.SingleImageSaveToFolderViewModel != null)
                {
                    Files singleImageSaveToFolder = new Files();
                    singleImageSaveToFolder.ComplaientDetailsId = iComplaientDetailsId;
                    singleImageSaveToFolder.Name = model.SingleImageSaveToFolderViewModel.Name;
                    singleImageSaveToFolder.ContentType = model.SingleImageSaveToFolderViewModel.ContentType;
                    singleImageSaveToFolder.FileEncodingTypes = model.SingleImageSaveToFolderViewModel.FileEncodingTypes;
                    singleImageSaveToFolder.FileStoreMode = model.SingleImageSaveToFolderViewModel.FileStoreMode;
                    singleImageSaveToFolder.Path = model.SingleImageSaveToFolderViewModel.Path;
                    singleImageSaveToFolder.Data = model.SingleImageSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToFolder);
                    DbContext.SaveChanges();
                }

                //Save MultipleImageSaveToFolder
                if (model.MultipleImageSaveToFolderViewModel!=null)
                {
                    foreach (var item in model.MultipleImageSaveToFolderViewModel)
                    {
                        Files listMultipleImageSaveToFolder = new Files();
                        listMultipleImageSaveToFolder.ComplaientDetailsId = iComplaientDetailsId;
                        listMultipleImageSaveToFolder.Name = item.Name;
                        listMultipleImageSaveToFolder.ContentType = item.ContentType;
                        listMultipleImageSaveToFolder.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleImageSaveToFolder.FileStoreMode = item.FileStoreMode;
                        listMultipleImageSaveToFolder.Path = item.Path;
                        listMultipleImageSaveToFolder.Data = item.Data;
                        DbContext.Files.Add(listMultipleImageSaveToFolder);
                        DbContext.SaveChanges();
                    }
                }

                //Save SingleImageSaveToDatabase
                if (model.SingleImageSaveToDatabaseViewModel != null)
                {
                    Files singleImageSaveToDatabaseViewModel = new Files();
                    singleImageSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleImageSaveToDatabaseViewModel.Name = model.SingleImageSaveToDatabaseViewModel.Name;
                    singleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabaseViewModel.ContentType;
                    singleImageSaveToDatabaseViewModel.FileEncodingTypes = model.SingleImageSaveToDatabaseViewModel.FileEncodingTypes;
                    singleImageSaveToDatabaseViewModel.FileStoreMode = model.SingleImageSaveToDatabaseViewModel.FileStoreMode;
                    singleImageSaveToDatabaseViewModel.Path = model.SingleImageSaveToDatabaseViewModel.Path;
                    singleImageSaveToDatabaseViewModel.Data = model.SingleImageSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleImageSaveToDatabase
                if (model.MultipleImageSaveToDatabaseViewModel != null)
                {
                    foreach (var item in model.MultipleImageSaveToDatabaseViewModel)
                    {
                        Files multipleImageSaveToDatabaseViewModel = new Files();
                        multipleImageSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        multipleImageSaveToDatabaseViewModel.Name = item.Name;
                        multipleImageSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleImageSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleImageSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleImageSaveToDatabaseViewModel.Path = item.Path;
                        multipleImageSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleImageSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save SingleFileSaveToFolder
                if (model.SingleFileSaveToFolderViewModel != null)
                {
                    Files singleFileSaveToFolderViewModel = new Files();
                    singleFileSaveToFolderViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleFileSaveToFolderViewModel.Name = model.SingleFileSaveToFolderViewModel.Name;
                    singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolderViewModel.ContentType;
                    singleFileSaveToFolderViewModel.FileEncodingTypes = model.SingleFileSaveToFolderViewModel.FileEncodingTypes;
                    singleFileSaveToFolderViewModel.FileStoreMode = model.SingleFileSaveToFolderViewModel.FileStoreMode;
                    singleFileSaveToFolderViewModel.Path = model.SingleFileSaveToFolderViewModel.Path;
                    singleFileSaveToFolderViewModel.Data = model.SingleFileSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToFolderViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleFileSaveToFolder
                if (model.MultipleFileSaveToFolderViewModel != null)
                {
                    foreach (var item in model.MultipleFileSaveToFolderViewModel)
                    {
                        Files listMultipleFileSaveToFolderViewModel = new Files();
                        listMultipleFileSaveToFolderViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        listMultipleFileSaveToFolderViewModel.Name = item.Name;
                        listMultipleFileSaveToFolderViewModel.ContentType = item.ContentType;
                        listMultipleFileSaveToFolderViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleFileSaveToFolderViewModel.FileStoreMode = item.FileStoreMode;
                        listMultipleFileSaveToFolderViewModel.Path = item.Path;
                        listMultipleFileSaveToFolderViewModel.Data = item.Data;
                        DbContext.Files.Add(listMultipleFileSaveToFolderViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save SingleFileSaveToDatabase
                if (model.SingleFileSaveToDatabaseViewModel != null)
                {
                    Files singleFileSaveToDatabaseViewModel = new Files();
                    singleFileSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                    singleFileSaveToDatabaseViewModel.Name = model.SingleFileSaveToDatabaseViewModel.Name;
                    singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabaseViewModel.ContentType;
                    singleFileSaveToDatabaseViewModel.FileEncodingTypes = model.SingleFileSaveToDatabaseViewModel.FileEncodingTypes;
                    singleFileSaveToDatabaseViewModel.FileStoreMode = model.SingleFileSaveToDatabaseViewModel.FileStoreMode;
                    singleFileSaveToDatabaseViewModel.Path = model.SingleFileSaveToDatabaseViewModel.Path;
                    singleFileSaveToDatabaseViewModel.Data = model.SingleFileSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }

                //Save MultipleFileSaveToDatabase
                if (model.MultipleFileSaveToDatabaseViewModel != null)
                {
                    foreach (var item in model.MultipleFileSaveToDatabaseViewModel)
                    {
                        Files multipleFileSaveToDatabaseViewModel = new Files();
                        multipleFileSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        multipleFileSaveToDatabaseViewModel.Name = item.Name;
                        multipleFileSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleFileSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleFileSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleFileSaveToDatabaseViewModel.Path = item.Path;
                        multipleFileSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleFileSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }

                //Save Bulk and BulkData Table

                if (model.ListOfExcelFileDataSaveToDatabaseViewModel != null)
                {
                    Bulk bulk = new Bulk();
                    bulk.ComplaientDetailsId = iComplaientDetailsId;
                    bulk.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                    bulk.CreatedDate = DateTime.Now;
                    DbContext.Bulk.Add(bulk);
                    DbContext.SaveChanges();
                    if (bulk.BulkId > 0)
                    {
                        foreach (var item in model.ListOfExcelFileDataSaveToDatabaseViewModel)
                        {
                            BulkDatas BulkData = new BulkDatas();
                            BulkData.BulkId = bulk.BulkId;
                            BulkData.Name = item.Name;
                            BulkData.Des = item.Des;
                            BulkData.Email = item.Email;
                            BulkData.MobileNumber = item.MobileNumber;
                            DbContext.BulkDatas.Add(BulkData);
                            DbContext.SaveChanges();
                        }
                    }
                    if (model.ExcelFileDataSaveToDatabaseViewModel != null)
                    {
                        Files excelFileDataSaveToDatabaseViewModel = new Files();
                        excelFileDataSaveToDatabaseViewModel.ComplaientDetailsId = iComplaientDetailsId;
                        excelFileDataSaveToDatabaseViewModel.Name = model.ExcelFileDataSaveToDatabaseViewModel.Name;
                        excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabaseViewModel.ContentType;
                        excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = model.ExcelFileDataSaveToDatabaseViewModel.FileEncodingTypes;
                        excelFileDataSaveToDatabaseViewModel.FileStoreMode = model.ExcelFileDataSaveToDatabaseViewModel.FileStoreMode;
                        excelFileDataSaveToDatabaseViewModel.Path = model.ExcelFileDataSaveToDatabaseViewModel.Path;
                        excelFileDataSaveToDatabaseViewModel.Data = model.ExcelFileDataSaveToDatabaseViewModel.Data;
                        DbContext.Files.Add(excelFileDataSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
                isSavedSucess = true;
            }
            return isSavedSucess;
        }
        public List<ComplainantAndDetailsViewModel> GetAllComplaient(IDataProtector protector)
        {
            var categorizedProducts = (from complaients in DbContext.Complaients
                                       join complaientDetails in DbContext.ComplaientDetails on complaients.ComplaientId equals complaientDetails.ComplaientId
                                       join complaientCategories in DbContext.ComplaientCategories on complaientDetails.ComplaientCategoryId equals complaientCategories.ComplaientCategoryId
                                       join country in DbContext.Countries on complaientDetails.CountryId equals country.CountryId
                                       join state in DbContext.States on complaientDetails.StateId equals state.StateId
                                       join city in DbContext.Cities on complaientDetails.CityId equals city.CityId
                                       select new
                                       {
                                           complaients.ComplaientId,
                                           complaients.ComplainantName,
                                           complaients.ComplainantEmail,
                                           complaients.CompaientDate,
                                           complaientCategories.Description,
                                           country.CountryName,
                                           state.StateName,
                                           city.CityName,
                                           ComplaientDescription = complaientDetails.Description,
                                           complaientDetails.ComplaientDate
                                       }).OrderByDescending(x => x.ComplaientId).ToList();
            if (categorizedProducts.Count >= 0)
            {
                List<ComplainantAndDetailsViewModel> complainantListViewModel = new List<ComplainantAndDetailsViewModel>();
                foreach (var item in categorizedProducts)
                {
                    ComplainantAndDetailsViewModel complainantListView = new ComplainantAndDetailsViewModel();
                    complainantListView.ComplaientId = item.ComplaientId;
                    // Encrypt the ComplaientId value and store in ComplaientEncryptedId property
                    //complainantListView.ComplaientEncryptedId = Convert.ToString(protector.Protect(System.Text.Encoding.Unicode.GetBytes(sComplaientId))); ;
                    complainantListView.ComplainantName = item.ComplainantName;
                    complainantListView.ComplainantEmail = item.ComplainantEmail;

                    complainantListView.CompaientDate = item.CompaientDate;
                    complainantListView.sCompaientDate = Convert.ToDateTime(item.CompaientDate).Date.ToString("dd/MMMM/yyyy");
                    complainantListView.ComplaientDate = item.ComplaientDate;
                    complainantListView.sComplaientDate = Convert.ToDateTime(item.ComplaientDate).Date.ToString("dd/MMMM/yyyy");

                    complainantListView.ComplaientCategoriesDescription = item.Description;
                    complainantListView.CountryName = item.CountryName;
                    complainantListView.StateName = item.StateName;
                    complainantListView.CityName = item.CityName;
                    complainantListView.ComplaientDescription = item.ComplaientDescription;
                    complainantListViewModel.Add(complainantListView);
                }
                return complainantListViewModel;
            }
            return null;
        }
        public List<BikeCategoryViewModel> GetAllBikeCategory()
        {
            var result = (from obj in DbContext.BikeCategories select obj).ToList();
            if (result.Count > 0)
            {
                List<BikeCategoryViewModel> bikeCategoryViewModel = new List<BikeCategoryViewModel>();
                foreach (var item in result)
                {
                    BikeCategoryViewModel bikeCategoryViewModel1 = new BikeCategoryViewModel();
                    bikeCategoryViewModel1.BikeCategoryId = item.BikeCategoryId;
                    bikeCategoryViewModel1.Name = item.Name;
                    bikeCategoryViewModel1.Description = item.Description;
                    bikeCategoryViewModel1.Status = item.Status;
                    bikeCategoryViewModel1.CreatedDate = item.CreatedDate;
                    bikeCategoryViewModel1.sCreatedDate = Convert.ToDateTime(item.CreatedDate).Date.ToString("dd/MMMM/yyyy");
                    bikeCategoryViewModel.Add(bikeCategoryViewModel1);
                }
                return bikeCategoryViewModel;
            }
            return null;
        }

        public ComplainantViewModel GetComplainantBiId(int id)
        {
            ComplainantViewModel complainantViewModel = new ComplainantViewModel();
            var complaientById = (from obj in DbContext.Complaients
                                  join cd in DbContext.ComplaientDetails on obj.ComplaientId equals cd.ComplaientId
                                  join pa in DbContext.ComplaientPermamentAddresses on cd.ComplaientDetailsId equals pa.ComplaientDetailsId
                                  join ta in DbContext.ComplaientTempAddresses on cd.ComplaientDetailsId equals ta.ComplaientDetailsId
                                  where obj.ComplaientId == id
                                  select new
                                  {
                                      obj.ComplaientId,
                                      obj.ComplainantName,
                                      obj.ComplainantEmail,
                                      obj.CompaientDate,
                                      cd.ComplaientDetailsId,
                                      cd.ComplaientCategoryId,
                                      cd.CountryId,
                                      cd.StateId,
                                      cd.CityId,
                                      cd.Description,
                                      cd.ComplaientDate,
                                      cd.GenderId,
                                      pa.ComplaientPermamentAddressId,
                                      PermamentAddress = pa.Address,
                                      PermamentAddressPostalCode = pa.PostalCode,
                                      ta.ComplaientTempAddressId,
                                      TempAddress = ta.Address,
                                      TempAddressPostalCode = ta.PostalCode
                                  }).FirstOrDefault();
            //Get Saved bike category list
            complainantViewModel.ComplaientId = complaientById.ComplaientId;
            complainantViewModel.ComplainantName = complaientById.ComplainantName;
            complainantViewModel.ComplainantEmail = complaientById.ComplainantEmail;

            complainantViewModel.CompaientDate = Convert.ToDateTime(complaientById.CompaientDate).Date;
            complainantViewModel.ComplaientDate = Convert.ToDateTime(complaientById.ComplaientDate).Date;

            complainantViewModel.ComplaientDetailsId = complaientById.ComplaientDetailsId;
            complainantViewModel.ComplaientCategoryId = complaientById.ComplaientCategoryId;
            complainantViewModel.CountryId = complaientById.CountryId;
            complainantViewModel.StateId = complaientById.StateId;
            complainantViewModel.CityId = complaientById.CityId;
            complainantViewModel.Description = complaientById.Description;
            complainantViewModel.ComplaientPermamentAddressId = complaientById.ComplaientPermamentAddressId;
            complainantViewModel.PermamentAddress = complaientById.PermamentAddress;
            complainantViewModel.PermamentAddressPostalCode = Convert.ToString(complaientById.PermamentAddressPostalCode);
            complainantViewModel.ComplaientTempAddressId = complaientById.ComplaientTempAddressId;
            complainantViewModel.TempAddress = complaientById.TempAddress;
            complainantViewModel.TempAddressPostalCode = Convert.ToString(complaientById.TempAddressPostalCode);
            complainantViewModel.ComplaientId = complaientById.ComplaientId;
            complainantViewModel.GenderId = complaientById.GenderId;
            var updateBikeCategories = (from obj in DbContext.BikeCollections where obj.ComplaientDetailsId == complaientById.ComplaientDetailsId select obj).ToList();
            complainantViewModel.BikeCategoriesSelectListId = updateBikeCategories.Select(x => x.BikeCategoryId).ToList();
            //Bind country
            var countries = CounteryList().ToList();
            if (countries.Count > 0)
            {
                foreach (var item in countries)
                {
                    complainantViewModel.Countries.Add(new SelectListItem { Text = item.CountryName, Value = item.CountryId.ToString(), Selected = true });
                }
            }
            //Bind state by country id
            var states = StateList(complaientById.CountryId);
            if (states != null)
            {
                foreach (var item in states)
                {
                    complainantViewModel.States.Add(new SelectListItem
                    {
                        Text = item.StateName,
                        Value = Convert.ToString(item.StateId),
                        Selected = true
                    });
                }
            }
            //Bind city by state id
            var cities = CityList(complaientById.StateId);
            if (cities != null)
            {
                foreach (var item in cities)
                {
                    complainantViewModel.Cities.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId),
                        Selected = true
                    });
                }
            }
            //Bind complaient category
            var complaientCategory = ComplaientCategoryList().ToList();
            if (complaientCategory.Count > 0)
            {
                foreach (var item in complaientCategory)
                {
                    complainantViewModel.ComplaientCategory.Add(new SelectListItem { Text = item.Description, Value = item.ComplaientCategoryId.ToString(), Selected = true });
                }
            }
            //Bind gender
            var gender = Genders().ToList();
            if (gender.Count > 0)
            {
                foreach (var item in gender)
                {
                    complainantViewModel.Gender.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString(), Selected = true });
                }
            }
            //Bind Bike Category for check boxes
            var bikeCategory = BikeCategory().ToList();
            if (bikeCategory.Count > 0)
            {
                complainantViewModel.BikeCategories = bikeCategory.Select(m => new BikeCategoryViewModel()
                {
                    BikeCategoryId = m.BikeCategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Status = updateBikeCategories.Any(x => x.BikeCategoryId == m.BikeCategoryId) ? true : false
                }).ToList();

            }
            //Bind Bike Category for multiple dropdown select list
            var bikeCategoriesSelectList = BikeCategory().ToList();
            if (bikeCategoriesSelectList.Count > 0)
            {
                List<CategoriesSelectListViewModel> list = new List<CategoriesSelectListViewModel>();
                foreach (var item in bikeCategoriesSelectList)
                {
                    CategoriesSelectListViewModel categoriesSelectListViewModel = new CategoriesSelectListViewModel();
                    categoriesSelectListViewModel.BikeCategoryId = item.BikeCategoryId;
                    categoriesSelectListViewModel.Name = item.Name;
                    if (updateBikeCategories.Where(x => x.BikeCategoryId == item.BikeCategoryId).Select(x => x.BikeCategoryId).FirstOrDefault()!=0)
                    {
                        categoriesSelectListViewModel.SelectedId = updateBikeCategories.Where(x => x.BikeCategoryId == item.BikeCategoryId).Select(x => x.BikeCategoryId).FirstOrDefault();
                    }

                    list.Add(categoriesSelectListViewModel);
                }
                complainantViewModel.BikeCategoriesSelectListForSelected = list;
            }

            //Get SingleImageSaveToFolder
            string sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
            var resultSingleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
            if (resultSingleImageSaveToFolder != null)
            {
                FileViewModel singleImageSaveToFolder = new FileViewModel();
                singleImageSaveToFolder.Id = resultSingleImageSaveToFolder.Id;
                singleImageSaveToFolder.Name = resultSingleImageSaveToFolder.Name;
                singleImageSaveToFolder.Path = resultSingleImageSaveToFolder.Path;
                complainantViewModel.SingleImageSaveToFolderViewModel = singleImageSaveToFolder;
            }
            else
            {
                complainantViewModel.SingleImageSaveToFolderViewModel = null;
            }

            //Get MultipleImageSaveToFolder
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
            var resultMultipleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
            if (resultMultipleImageSaveToFolder.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToFolder = new List<FileViewModel>();
                foreach (var item in resultMultipleImageSaveToFolder)
                {
                    FileViewModel fileMultipleImageSaveToFolder = new FileViewModel();
                    fileMultipleImageSaveToFolder.Id = item.Id;
                    fileMultipleImageSaveToFolder.Name = item.Name;
                    fileMultipleImageSaveToFolder.Path = item.Path;
                    listMultipleImageSaveToFolder.Add(fileMultipleImageSaveToFolder);
                }
                complainantViewModel.MultipleImageSaveToFolderViewModel = listMultipleImageSaveToFolder;
            }
            else
            {
                complainantViewModel.MultipleImageSaveToFolderViewModel = null;
            }

            //Get SingleImageSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
            var resultSingleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
            if (resultSingleImageSaveToDatabase != null)
            {
                FileViewModel fileSingleImageSaveToDatabase = new FileViewModel();
                fileSingleImageSaveToDatabase.Id = resultSingleImageSaveToDatabase.Id;
                string[] name = resultSingleImageSaveToDatabase.Name.Split('_');
                fileSingleImageSaveToDatabase.Name = name[1];
                fileSingleImageSaveToDatabase.ContentType = resultSingleImageSaveToDatabase.ContentType;
                fileSingleImageSaveToDatabase.Data = resultSingleImageSaveToDatabase.Data;
                complainantViewModel.SingleImageSaveToDatabaseViewModel = fileSingleImageSaveToDatabase;
            }
            else
            {
                complainantViewModel.SingleImageSaveToDatabaseViewModel = null;
            }

            //Get MultipleImageSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
            var resultMultipleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
            if (resultMultipleImageSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleImageSaveToDatabase = new List<FileViewModel>();
                foreach (var item in resultMultipleImageSaveToDatabase)
                {
                    FileViewModel fileMultipleImageSaveToDatabase = new FileViewModel();
                    fileMultipleImageSaveToDatabase.Id = item.Id;
                    string[] name = item.Name.Split('_');
                    fileMultipleImageSaveToDatabase.Name = name[1];
                    fileMultipleImageSaveToDatabase.ContentType = item.ContentType;
                    fileMultipleImageSaveToDatabase.Data = item.Data;
                    listMultipleImageSaveToDatabase.Add(fileMultipleImageSaveToDatabase);
                }
                complainantViewModel.MultipleImageSaveToDatabaseViewModel = listMultipleImageSaveToDatabase;
            }
            else
            {
                complainantViewModel.MultipleImageSaveToDatabaseViewModel = null;
            }

            //Get SingleFileSaveToFolder
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
            var resultSingleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
            if (resultSingleFileSaveToFolder != null)
            {
                FileViewModel singleFileSaveToFolder = new FileViewModel();
                singleFileSaveToFolder.Id = resultSingleFileSaveToFolder.Id;
                string[] splitSingleFileToFolderName = resultSingleFileSaveToFolder.Name.Split('_');
                singleFileSaveToFolder.Name = splitSingleFileToFolderName[1];
                complainantViewModel.SingleFileSaveToFolderViewModel = singleFileSaveToFolder;
            }
            else
            {
                complainantViewModel.SingleFileSaveToFolderViewModel = null;
            }

            //Get MultipleFileSaveToFolder
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
            var resultMultipleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
            if (resultMultipleFileSaveToFolder.Count > 0)
            {
                List<FileViewModel> listMultipleFileSaveToFolder = new List<FileViewModel>();
                foreach (var item in resultMultipleFileSaveToFolder)
                {
                    FileViewModel multipleFileSaveToFolder = new FileViewModel();
                    multipleFileSaveToFolder.Id = item.Id;
                    string[] multipleFileSaveToFolderName = item.Name.Split('_');
                    multipleFileSaveToFolder.Name = multipleFileSaveToFolderName[1];
                    listMultipleFileSaveToFolder.Add(multipleFileSaveToFolder);
                }
                complainantViewModel.MultipleFileSaveToFolderViewModel = listMultipleFileSaveToFolder;
            }
            else
            {
                complainantViewModel.MultipleFileSaveToFolderViewModel = null;
            }

            //Get SingleFileSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
            var resultSingleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
            if (resultSingleFileSaveToDatabase != null)
            {
                FileViewModel singleFileSaveToDatabase = new FileViewModel();
                singleFileSaveToDatabase.Id = resultSingleFileSaveToDatabase.Id;
                string[] name = resultSingleFileSaveToDatabase.Name.Split('_');
                singleFileSaveToDatabase.Name = name[1];
                complainantViewModel.SingleFileSaveToDatabaseViewModel = singleFileSaveToDatabase;
            }
            else
            {
                complainantViewModel.SingleFileSaveToDatabaseViewModel = null;
            }

            //Get MultipleFileSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
            var resultMultipleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
            if (resultMultipleFileSaveToDatabase.Count > 0)
            {
                List<FileViewModel> listMultipleFileSaveToDatabase = new List<FileViewModel>();
                foreach (var item in resultMultipleFileSaveToDatabase)
                {
                    FileViewModel multipleFileSaveToDatabase = new FileViewModel();
                    multipleFileSaveToDatabase.Id = item.Id;
                    string[] name = item.Name.Split('_');
                    multipleFileSaveToDatabase.Name = name[1];
                    listMultipleFileSaveToDatabase.Add(multipleFileSaveToDatabase);
                }
                complainantViewModel.MultipleFileSaveToDatabaseViewModel = listMultipleFileSaveToDatabase;
            }
            else
            {
                complainantViewModel.MultipleFileSaveToDatabaseViewModel = null;
            }

            //Get Bulk and BulkData table 
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
            var resultExcelFileDataSaveToDatabase = (from o in DbContext.Bulk where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
            if (resultExcelFileDataSaveToDatabase != null)
            {
                var resultExcelFileDataSaveToDatabaseViewModel = (from o in DbContext.Files where o.ComplaientDetailsId == complaientById.ComplaientDetailsId && o.FileStoreMode == resultExcelFileDataSaveToDatabase.FileStoreMode select o).FirstOrDefault();
                if (resultExcelFileDataSaveToDatabaseViewModel != null)
                {
                    FileViewModel excelFileDataSaveToDatabaseViewModel = new FileViewModel();
                    excelFileDataSaveToDatabaseViewModel.Id = resultExcelFileDataSaveToDatabaseViewModel.Id;
                    string[] name = resultExcelFileDataSaveToDatabaseViewModel.Name.Split('_');
                    excelFileDataSaveToDatabaseViewModel.Name = name[1];
                    complainantViewModel.ExcelFileDataSaveToDatabaseViewModel = excelFileDataSaveToDatabaseViewModel;
                }
            }
            else
            {
                complainantViewModel.ExcelFileDataSaveToDatabaseViewModel = null;
            }
            return complainantViewModel;
        }
        public FileViewModel DownloadFile(int id)
        {
            FileViewModel fileViewModel = new FileViewModel();
            var files = (from o in DbContext.Files
                         where o.Id == id
                         select o).FirstOrDefault();
            if (files != null)
            {
                fileViewModel.Id = files.Id;
                fileViewModel.Name = files.Name;
                fileViewModel.ContentType = files.ContentType;
                fileViewModel.FileEncodingTypes = files.FileEncodingTypes;
                fileViewModel.FileStoreMode = files.FileStoreMode;
                fileViewModel.Path = files.Path;
                fileViewModel.Data = files.Data;
            }
            else
            {
                fileViewModel = null;
            }
            return fileViewModel;
        }
        public List<BulkDatas> GetExcelData(string fileStoreMode)
        {
            var getBulkId = (from o in DbContext.Bulk where o.FileStoreMode == fileStoreMode select o).FirstOrDefault();
            if (getBulkId != null)
            {
                var getBulkData = (from o in DbContext.BulkDatas where o.BulkId == getBulkId.BulkId orderby o.Id descending select o).ToList();
                if (getBulkData.Count > 0)
                {
                    List<BulkDatas> bulkDatas = new List<BulkDatas>();
                    foreach (var item in getBulkData)
                    {
                        BulkDatas bulk = new BulkDatas();
                        bulk.Id = item.Id;
                        bulk.BulkId = item.BulkId;
                        bulk.Name = item.Name;
                        bulk.Email = item.Email;
                        bulk.MobileNumber = item.MobileNumber;
                        bulk.Des = item.Des;
                        bulkDatas.Add(bulk);
                    }
                    return bulkDatas;
                }
            }
            return null;
        }
        public bool UpdateComplaient(ComplainantViewModel model)
        {
            bool isUpdateSucess = false;
            //Update Complaient
            var updateComplaient = (from o in DbContext.Complaients where o.ComplaientId == model.ComplaientId select o).FirstOrDefault();
            if (updateComplaient != null)
            {
                updateComplaient.ComplainantName = model.ComplainantName;
                updateComplaient.ComplainantEmail = model.ComplainantEmail;
                updateComplaient.ComplaientStatus = true;
                updateComplaient.CompaientDate = model.CompaientDate;
                DbContext.SaveChanges();
            }

            //Update ComplaientDatail
            var updateComplaientDetail = (from o in DbContext.ComplaientDetails where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updateComplaientDetail != null)
            {
                updateComplaientDetail.ComplaientId = model.ComplaientId;
                updateComplaientDetail.ComplaientCategoryId = model.ComplaientCategoryId;
                updateComplaientDetail.GenderId = model.GenderId;
                updateComplaientDetail.CountryId = model.CountryId;
                updateComplaientDetail.StateId = model.StateId;
                updateComplaientDetail.CityId = model.CityId;
                updateComplaientDetail.Description = model.Description;
                updateComplaientDetail.ComplaientDate = model.ComplaientDate;
                DbContext.SaveChanges();
            }

            //Update ComplaientPermamentAddress Table values
            var updatePermanetAddress = (from o in DbContext.ComplaientPermamentAddresses where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updatePermanetAddress != null)
            {
                updatePermanetAddress.ComplaientDetailsId = model.ComplaientDetailsId;
                updatePermanetAddress.Address = model.PermamentAddress;
                updatePermanetAddress.PostalCode = Convert.ToInt32(model.PermamentAddressPostalCode);
                DbContext.SaveChanges();
            }

            //Update ComplaientTempAddress Table values
            var updateTempAddress = (from o in DbContext.ComplaientTempAddresses where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
            if (updateTempAddress != null)
            {
                updateTempAddress.ComplaientDetailsId = model.ComplaientDetailsId;
                updateTempAddress.Address = model.TempAddress;
                updateTempAddress.PostalCode = Convert.ToInt32(model.TempAddressPostalCode);
                DbContext.SaveChanges();
            }

            //Delete Bike Collections Data then re-insert
            var deleteBikeCategories = (from obj in DbContext.BikeCollections where obj.ComplaientDetailsId == model.ComplaientDetailsId select obj).ToList();
            if (deleteBikeCategories.Count > 0)
            {
                foreach (var item in deleteBikeCategories)
                {
                    DbContext.BikeCollections.Remove(item);
                    DbContext.SaveChanges();
                }
            }

            //Save BikeCategories Table values
            if (model.BikeCategories.Count > 0)
            {
                foreach (var item in model.BikeCategories)
                {
                    if (item.Status == true)
                    {
                        BikeCollection bikeCollection = new BikeCollection();
                        bikeCollection.ComplaientDetailsId = model.ComplaientDetailsId;
                        bikeCollection.BikeCategoryId = item.BikeCategoryId;
                        bikeCollection.Status = item.Status;
                        bikeCollection.CreatedDate = DateTime.Now;
                        DbContext.BikeCollections.Add(bikeCollection);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleImageSaveToFolder values
            string sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleImageSaveToFolder);
            if (model.SingleImageSaveToFolderViewModel != null)
            {
                var updateSingleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
                if (updateSingleImageSaveToFolder != null)
                {
                    DbContext.Files.Remove(updateSingleImageSaveToFolder);
                    DbContext.SaveChanges();

                    Files singleImageSaveToFolder = new Files();
                    singleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToFolder.Name = model.SingleImageSaveToFolderViewModel.Name;
                    singleImageSaveToFolder.ContentType = model.SingleImageSaveToFolderViewModel.ContentType;
                    singleImageSaveToFolder.FileEncodingTypes = model.SingleImageSaveToFolderViewModel.FileEncodingTypes;
                    singleImageSaveToFolder.FileStoreMode = model.SingleImageSaveToFolderViewModel.FileStoreMode;
                    singleImageSaveToFolder.Path = model.SingleImageSaveToFolderViewModel.Path;
                    singleImageSaveToFolder.Data = model.SingleImageSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToFolder);
                    DbContext.SaveChanges();
                }
                else
                {
                    Files singleImageSaveToFolder = new Files();
                    singleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToFolder.Name = model.SingleImageSaveToFolderViewModel.Name;
                    singleImageSaveToFolder.ContentType = model.SingleImageSaveToFolderViewModel.ContentType;
                    singleImageSaveToFolder.FileEncodingTypes = model.SingleImageSaveToFolderViewModel.FileEncodingTypes;
                    singleImageSaveToFolder.FileStoreMode = model.SingleImageSaveToFolderViewModel.FileStoreMode;
                    singleImageSaveToFolder.Path = model.SingleImageSaveToFolderViewModel.Path;
                    singleImageSaveToFolder.Data = model.SingleImageSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToFolder);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleImageSaveToFolder values
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToFolder);
            if (model.MultipleImageSaveToFolderViewModel != null)
            {
                var updateMultipleImageSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
                if (updateMultipleImageSaveToFolder.Count > 0)
                {
                    foreach (var item in updateMultipleImageSaveToFolder)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleImageSaveToFolderViewModel)
                    {
                        Files listMultipleImageSaveToFolder = new Files();
                        listMultipleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleImageSaveToFolder.Name = item.Name;
                        listMultipleImageSaveToFolder.ContentType = item.ContentType;
                        listMultipleImageSaveToFolder.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleImageSaveToFolder.FileStoreMode = item.FileStoreMode;
                        listMultipleImageSaveToFolder.Path = item.Path;
                        listMultipleImageSaveToFolder.Data = item.Data;
                        DbContext.Files.Add(listMultipleImageSaveToFolder);
                        DbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach (var item in model.MultipleImageSaveToFolderViewModel)
                    {
                        Files listMultipleImageSaveToFolder = new Files();
                        listMultipleImageSaveToFolder.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleImageSaveToFolder.Name = item.Name;
                        listMultipleImageSaveToFolder.ContentType = item.ContentType;
                        listMultipleImageSaveToFolder.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleImageSaveToFolder.FileStoreMode = item.FileStoreMode;
                        listMultipleImageSaveToFolder.Path = item.Path;
                        listMultipleImageSaveToFolder.Data = item.Data;
                        DbContext.Files.Add(listMultipleImageSaveToFolder);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleImageSaveToDatabaseViewModel values
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleImageSaveToDatabase);
            if (model.SingleImageSaveToDatabaseViewModel != null)
            {
                var updateSingleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
                if (updateSingleImageSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateSingleImageSaveToDatabase);
                    DbContext.SaveChanges();

                    Files singleImageSaveToDatabaseViewModel = new Files();
                    singleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToDatabaseViewModel.Name = model.SingleImageSaveToDatabaseViewModel.Name;
                    singleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabaseViewModel.ContentType;
                    singleImageSaveToDatabaseViewModel.FileEncodingTypes = model.SingleImageSaveToDatabaseViewModel.FileEncodingTypes;
                    singleImageSaveToDatabaseViewModel.FileStoreMode = model.SingleImageSaveToDatabaseViewModel.FileStoreMode;
                    singleImageSaveToDatabaseViewModel.Path = model.SingleImageSaveToDatabaseViewModel.Path;
                    singleImageSaveToDatabaseViewModel.Data = model.SingleImageSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
                else
                {
                    Files singleImageSaveToDatabaseViewModel = new Files();
                    singleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleImageSaveToDatabaseViewModel.Name = model.SingleImageSaveToDatabaseViewModel.Name;
                    singleImageSaveToDatabaseViewModel.ContentType = model.SingleImageSaveToDatabaseViewModel.ContentType;
                    singleImageSaveToDatabaseViewModel.FileEncodingTypes = model.SingleImageSaveToDatabaseViewModel.FileEncodingTypes;
                    singleImageSaveToDatabaseViewModel.FileStoreMode = model.SingleImageSaveToDatabaseViewModel.FileStoreMode;
                    singleImageSaveToDatabaseViewModel.Path = model.SingleImageSaveToDatabaseViewModel.Path;
                    singleImageSaveToDatabaseViewModel.Data = model.SingleImageSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleImageSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleImageSaveToDatabase values
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleImageSaveToDatabase);
            if (model.MultipleImageSaveToDatabaseViewModel != null)
            {
                var updateMultipleImageSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
                if (updateMultipleImageSaveToDatabase.Count > 0)
                {
                    foreach (var item in updateMultipleImageSaveToDatabase)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleImageSaveToDatabaseViewModel)
                    {
                        Files multipleImageSaveToDatabaseViewModel = new Files();
                        multipleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleImageSaveToDatabaseViewModel.Name = item.Name;
                        multipleImageSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleImageSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleImageSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleImageSaveToDatabaseViewModel.Path = item.Path;
                        multipleImageSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleImageSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach (var item in model.MultipleImageSaveToDatabaseViewModel)
                    {
                        Files multipleImageSaveToDatabaseViewModel = new Files();
                        multipleImageSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleImageSaveToDatabaseViewModel.Name = item.Name;
                        multipleImageSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleImageSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleImageSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleImageSaveToDatabaseViewModel.Path = item.Path;
                        multipleImageSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleImageSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleFileSaveToFolder
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleFileSaveToFolder);
            if (model.SingleFileSaveToFolderViewModel != null)
            {
                var updateSingleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
                if (updateSingleFileSaveToFolder != null)
                {
                    DbContext.Files.Remove(updateSingleFileSaveToFolder);
                    DbContext.SaveChanges();

                    Files singleFileSaveToFolderViewModel = new Files();
                    singleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToFolderViewModel.Name = model.SingleFileSaveToFolderViewModel.Name;
                    singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolderViewModel.ContentType;
                    singleFileSaveToFolderViewModel.FileEncodingTypes = model.SingleFileSaveToFolderViewModel.FileEncodingTypes;
                    singleFileSaveToFolderViewModel.FileStoreMode = model.SingleFileSaveToFolderViewModel.FileStoreMode;
                    singleFileSaveToFolderViewModel.Path = model.SingleFileSaveToFolderViewModel.Path;
                    singleFileSaveToFolderViewModel.Data = model.SingleFileSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToFolderViewModel);
                    DbContext.SaveChanges();
                }
                else
                {
                    Files singleFileSaveToFolderViewModel = new Files();
                    singleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToFolderViewModel.Name = model.SingleFileSaveToFolderViewModel.Name;
                    singleFileSaveToFolderViewModel.ContentType = model.SingleFileSaveToFolderViewModel.ContentType;
                    singleFileSaveToFolderViewModel.FileEncodingTypes = model.SingleFileSaveToFolderViewModel.FileEncodingTypes;
                    singleFileSaveToFolderViewModel.FileStoreMode = model.SingleFileSaveToFolderViewModel.FileStoreMode;
                    singleFileSaveToFolderViewModel.Path = model.SingleFileSaveToFolderViewModel.Path;
                    singleFileSaveToFolderViewModel.Data = model.SingleFileSaveToFolderViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToFolderViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleFileSaveToFolder
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToFolder);
            if (model.MultipleFileSaveToFolderViewModel != null)
            {
                var updateMultipleFileSaveToFolder = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
                if (updateMultipleFileSaveToFolder.Count > 0)
                {
                    foreach (var item in updateMultipleFileSaveToFolder)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }

                    foreach (var item in model.MultipleFileSaveToFolderViewModel)
                    {
                        Files listMultipleFileSaveToFolderViewModel = new Files();
                        listMultipleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleFileSaveToFolderViewModel.Name = item.Name;
                        listMultipleFileSaveToFolderViewModel.ContentType = item.ContentType;
                        listMultipleFileSaveToFolderViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleFileSaveToFolderViewModel.FileStoreMode = item.FileStoreMode;
                        listMultipleFileSaveToFolderViewModel.Path = item.Path;
                        listMultipleFileSaveToFolderViewModel.Data = item.Data;
                        DbContext.Files.Add(listMultipleFileSaveToFolderViewModel);
                        DbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach (var item in model.MultipleFileSaveToFolderViewModel)
                    {
                        Files listMultipleFileSaveToFolderViewModel = new Files();
                        listMultipleFileSaveToFolderViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        listMultipleFileSaveToFolderViewModel.Name = item.Name;
                        listMultipleFileSaveToFolderViewModel.ContentType = item.ContentType;
                        listMultipleFileSaveToFolderViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        listMultipleFileSaveToFolderViewModel.FileStoreMode = item.FileStoreMode;
                        listMultipleFileSaveToFolderViewModel.Path = item.Path;
                        listMultipleFileSaveToFolderViewModel.Data = item.Data;
                        DbContext.Files.Add(listMultipleFileSaveToFolderViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update SingleFileSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.SingleFileSaveToDatabase);
            if (model.SingleFileSaveToDatabaseViewModel != null)
            {
                var updateSingleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
                if (updateSingleFileSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateSingleFileSaveToDatabase);
                    DbContext.SaveChanges();

                    Files singleFileSaveToDatabaseViewModel = new Files();
                    singleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToDatabaseViewModel.Name = model.SingleFileSaveToDatabaseViewModel.Name;
                    singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabaseViewModel.ContentType;
                    singleFileSaveToDatabaseViewModel.FileEncodingTypes = model.SingleFileSaveToDatabaseViewModel.FileEncodingTypes;
                    singleFileSaveToDatabaseViewModel.FileStoreMode = model.SingleFileSaveToDatabaseViewModel.FileStoreMode;
                    singleFileSaveToDatabaseViewModel.Path = model.SingleFileSaveToDatabaseViewModel.Path;
                    singleFileSaveToDatabaseViewModel.Data = model.SingleFileSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
                else
                {
                    Files singleFileSaveToDatabaseViewModel = new Files();
                    singleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    singleFileSaveToDatabaseViewModel.Name = model.SingleFileSaveToDatabaseViewModel.Name;
                    singleFileSaveToDatabaseViewModel.ContentType = model.SingleFileSaveToDatabaseViewModel.ContentType;
                    singleFileSaveToDatabaseViewModel.FileEncodingTypes = model.SingleFileSaveToDatabaseViewModel.FileEncodingTypes;
                    singleFileSaveToDatabaseViewModel.FileStoreMode = model.SingleFileSaveToDatabaseViewModel.FileStoreMode;
                    singleFileSaveToDatabaseViewModel.Path = model.SingleFileSaveToDatabaseViewModel.Path;
                    singleFileSaveToDatabaseViewModel.Data = model.SingleFileSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(singleFileSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }

            //Update MultipleFileSaveToDatabase
            sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.MultipleFileSaveToDatabase);
            if (model.MultipleFileSaveToDatabaseViewModel != null)
            {
                var updateMultipleFileSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).ToList();
                if (updateMultipleFileSaveToDatabase.Count > 0)
                {
                    foreach (var item in updateMultipleFileSaveToDatabase)
                    {
                        DbContext.Files.Remove(item);
                        DbContext.SaveChanges();
                    }
                    foreach (var item in model.MultipleFileSaveToDatabaseViewModel)
                    {
                        Files multipleFileSaveToDatabaseViewModel = new Files();
                        multipleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleFileSaveToDatabaseViewModel.Name = item.Name;
                        multipleFileSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleFileSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleFileSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleFileSaveToDatabaseViewModel.Path = item.Path;
                        multipleFileSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleFileSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach (var item in model.MultipleFileSaveToDatabaseViewModel)
                    {
                        Files multipleFileSaveToDatabaseViewModel = new Files();
                        multipleFileSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                        multipleFileSaveToDatabaseViewModel.Name = item.Name;
                        multipleFileSaveToDatabaseViewModel.ContentType = item.ContentType;
                        multipleFileSaveToDatabaseViewModel.FileEncodingTypes = item.FileEncodingTypes;
                        multipleFileSaveToDatabaseViewModel.FileStoreMode = item.FileStoreMode;
                        multipleFileSaveToDatabaseViewModel.Path = item.Path;
                        multipleFileSaveToDatabaseViewModel.Data = item.Data;
                        DbContext.Files.Add(multipleFileSaveToDatabaseViewModel);
                        DbContext.SaveChanges();
                    }
                }
            }

            //Update Bulk and BulkData Table
            if (model.ListOfExcelFileDataSaveToDatabaseViewModel != null)
            {
                var updateBulk = (from o in DbContext.Bulk where o.ComplaientDetailsId == model.ComplaientDetailsId select o).FirstOrDefault();
                if (updateBulk != null)
                {
                    updateBulk.ComplaientDetailsId = model.ComplaientDetailsId;
                    updateBulk.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                    updateBulk.ModifyDate = DateTime.Now;
                    DbContext.SaveChanges();

                    if (updateBulk.BulkId > 0)
                    {
                        var updateBulkDatas = (from o in DbContext.BulkDatas where o.BulkId == updateBulk.BulkId select o).ToList();
                        if (updateBulkDatas.Count > 0)
                        {
                            foreach (var item in updateBulkDatas)
                            {
                                DbContext.BulkDatas.Remove(item);
                                DbContext.SaveChanges();
                            }
                            foreach (var item in model.ListOfExcelFileDataSaveToDatabaseViewModel)
                            {
                                BulkDatas BulkData = new BulkDatas();
                                BulkData.BulkId = updateBulk.BulkId;
                                BulkData.Name = item.Name;
                                BulkData.Des = item.Des;
                                BulkData.Email = item.Email;
                                BulkData.MobileNumber = item.MobileNumber;
                                DbContext.BulkDatas.Add(BulkData);
                                DbContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    Bulk bulk = new Bulk();
                    bulk.ComplaientDetailsId = model.ComplaientDetailsId;
                    bulk.FileStoreMode = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                    bulk.CreatedDate = DateTime.Now;
                    DbContext.Bulk.Add(bulk);
                    DbContext.SaveChanges();
                    if (bulk.BulkId > 0)
                    {
                        foreach (var item in model.ListOfExcelFileDataSaveToDatabaseViewModel)
                        {
                            BulkDatas BulkData = new BulkDatas();
                            BulkData.BulkId = bulk.BulkId;
                            BulkData.Name = item.Name;
                            BulkData.Des = item.Des;
                            BulkData.Email = item.Email;
                            BulkData.MobileNumber = item.MobileNumber;
                            DbContext.BulkDatas.Add(BulkData);
                            DbContext.SaveChanges();
                        }
                    }
                }
                //Update ExcelFileDataSaveToDatabaseViewModel
                sFileStoreModeOptions = Convert.ToString(FileStoreModeOptions.ExcelFileDataSaveToDatabase);
                var updateExcelFileDataSaveToDatabase = (from o in DbContext.Files where o.ComplaientDetailsId == model.ComplaientDetailsId && o.FileStoreMode == sFileStoreModeOptions select o).FirstOrDefault();
                if (updateExcelFileDataSaveToDatabase != null)
                {
                    DbContext.Files.Remove(updateExcelFileDataSaveToDatabase);
                    DbContext.SaveChanges();

                    Files excelFileDataSaveToDatabaseViewModel = new Files();
                    excelFileDataSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    excelFileDataSaveToDatabaseViewModel.Name = model.ExcelFileDataSaveToDatabaseViewModel.Name;
                    excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabaseViewModel.ContentType;
                    excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = model.ExcelFileDataSaveToDatabaseViewModel.FileEncodingTypes;
                    excelFileDataSaveToDatabaseViewModel.FileStoreMode = model.ExcelFileDataSaveToDatabaseViewModel.FileStoreMode;
                    excelFileDataSaveToDatabaseViewModel.Path = model.ExcelFileDataSaveToDatabaseViewModel.Path;
                    excelFileDataSaveToDatabaseViewModel.Data = model.ExcelFileDataSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(excelFileDataSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
                else
                {
                    Files excelFileDataSaveToDatabaseViewModel = new Files();
                    excelFileDataSaveToDatabaseViewModel.ComplaientDetailsId = model.ComplaientDetailsId;
                    excelFileDataSaveToDatabaseViewModel.Name = model.ExcelFileDataSaveToDatabaseViewModel.Name;
                    excelFileDataSaveToDatabaseViewModel.ContentType = model.ExcelFileDataSaveToDatabaseViewModel.ContentType;
                    excelFileDataSaveToDatabaseViewModel.FileEncodingTypes = model.ExcelFileDataSaveToDatabaseViewModel.FileEncodingTypes;
                    excelFileDataSaveToDatabaseViewModel.FileStoreMode = model.ExcelFileDataSaveToDatabaseViewModel.FileStoreMode;
                    excelFileDataSaveToDatabaseViewModel.Path = model.ExcelFileDataSaveToDatabaseViewModel.Path;
                    excelFileDataSaveToDatabaseViewModel.Data = model.ExcelFileDataSaveToDatabaseViewModel.Data;
                    DbContext.Files.Add(excelFileDataSaveToDatabaseViewModel);
                    DbContext.SaveChanges();
                }
            }
            isUpdateSucess = true;

            return isUpdateSucess;
        }
        public string GetFileNameById(int id)
        {
            return DbContext.Files.Where(x => x.Id == id).Select(s => s.Name).FirstOrDefault();
        }
        public List<string> GetAllFiles(string fileStoreMode, int complaientDetailsId)
        {
            return DbContext.Files.Where((x => x.ComplaientDetailsId == complaientDetailsId && x.FileStoreMode == fileStoreMode)).Select(s => s.Name).ToList();
        }
    }
}
