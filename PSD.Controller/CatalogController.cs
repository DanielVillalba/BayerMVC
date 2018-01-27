using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;

namespace PSD.Controller
{
    public class CatalogController : _BaseController
    {
        public CatalogController(IAppConfiguration configurations)
            : base("CatalogController.", configurations)
        {

        }

        #region Crops

        public List<Cat_Crop> CropRetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<Cat_Crop> auxCrops;
            try
            {
                auxCrops = (List<Cat_Crop>)Repository.Crops.GetAll();
                ResultManager.IsCorrect = true;
                return auxCrops;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropRetrieveAll.511 Excepción al obtener el listado de cultivos: " + ex.Message);
            }

            return null;
        }
        /*
        public List<Cat_CropCategory> CropCategoryRetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<Cat_CropCategory> cropCategories;
            try
            {
                cropCategories = (List<Cat_CropCategory>)Repository.CropCategories.GetAll();
                ResultManager.IsCorrect = true;
                return cropCategories;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropCategoryRetrieveAll.511 Excepción al obtener el listado de cultivos: " + ex.Message);
            }

            return null;
        }*/
        public Cat_Crop CropRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no crop id was received
                ResultManager.Add(ErrorDefault, Trace + "CropRetrieve.131 No se recibio el id del cultivo");
                return null;
            }

            Cat_Crop auxCrop = null;
            try
            {
                auxCrop = Repository.Crops.Get(id);
                if (auxCrop == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "CropRetrieve.511 No se encontró un cultivo con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropRetrieve.511 Excepción al obtener el cultivo a editar: " + ex.Message);
            }

            return auxCrop;
        }

        public bool CropAdd(Cat_Crop item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if(item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropAdd.111 No se recibio el objeto del cultivo a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre del cultivo no puede estar vacio");
                return false;
            }
            if (item.CropCategoryId == 0 || item.CropCategoryId == -1)
            {
                ResultManager.Add("Se debe seleccionar un cultivo straco");
                return false;
            }
            
            //insert new item
            try
            {
                Cat_Crop auxCrop = new Cat_Crop();
                auxCrop.Name = item.Name;
                auxCrop.CropCategoryId = item.CropCategoryId;
                Repository.Crops.Add(auxCrop);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropAdd.511 Excepción al editar el cultivo con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }
        public bool CropUpdate(Cat_Crop item)
        {
            ResultManager.IsCorrect = false;
            
            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropEdit.111 No se recibio el objeto del cultivo a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no crop id was received
                ResultManager.Add(ErrorDefault, Trace + "CropEdit.131 No se recibio el id del cultivo a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre del cultivo no puede estar vacio");
                return false;
            }
            if (item.CropCategoryId == 0 || item.CropCategoryId == -1)
            {
                ResultManager.Add("Se debe seleccionar un cultivo straco");
                return false;
            }

            //update item
            try
            {
                Cat_Crop auxCrop = Repository.Crops.Get(item.Id);
                if(auxCrop == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "CropEdit.311 Cultivo con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                auxCrop.Name = item.Name;
                auxCrop.CropCategoryId = item.CropCategoryId;
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropEdit.511 Excepción al editar el cultivo con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }
        public bool CropDelete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no crop id was received
                ResultManager.Add(ErrorDefault, Trace + "CropDelete.131 No se recibio el id del cultivo a editar");
                return false;
            }
            
            //delete item
            try
            {
                Repository.Crops.Remove(id);
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "CropDelete.511 Excepción al eliminar el cultivo con id '" + id + "': " + ex.Message);
            }
            return false;
        }

        #endregion

        #region Roles

        public List<Cat_UserRole> RoleRetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<Cat_UserRole> auxCrops;
            try
            {
                auxCrops = (List<Cat_UserRole>)Repository.UserRoles.GetAll();
                ResultManager.IsCorrect = true;
                return auxCrops;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RoleRetrieveAll.511 Excepción al obtener el listado de roles: " + ex.Message);
            }

            return null;
        }

        public Cat_UserRole RoleRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no role id was received
                ResultManager.Add(ErrorDefault, Trace + "RoleRetrieve.131 No se recibio el id del role");
                return null;
            }

            Cat_UserRole auxRole = null;
            try
            {
                auxRole = Repository.UserRoles.Get(id);
                if (auxRole == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "RoleRetrieve.511 No se encontró un role con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RoleRetrieve.511 Excepción al obtener el role a editar: " + ex.Message);
            }

            return auxRole;
        }

        public bool RoleUpdate(Cat_UserRole item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "RoleEdit.111 No se recibio el objeto del role a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no role id was received
                ResultManager.Add(ErrorDefault, Trace + "RoleEdit.131 No se recibio el id del role a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre del role no puede estar vacio");
                return false;
            }

            //update item
            try
            {
                Cat_UserRole auxCrop = Repository.UserRoles.Get(item.Id);
                if (auxCrop == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "RoleEdit.311 Role con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                auxCrop.Name = item.Name;
                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "RoleEdit.511 Excepción al editar el role con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }

        #endregion

        #region Zones

        public List<Cat_Zone> ZoneRetrieveAll()
        {
            ResultManager.IsCorrect = false;
            List<Cat_Zone> auxZones;
            try
            {
                auxZones = (List<Cat_Zone>)Repository.Zones.GetAll();
                ResultManager.IsCorrect = true;
                return auxZones;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneRetrieveAll.511 Excepción al obtener el listado de zonas: " + ex.Message);
            }

            return null;
        }

        public Cat_Zone ZoneRetrieve(int id = -1)
        {
            ResultManager.IsCorrect = false;
            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no zone id was received
                ResultManager.Add(ErrorDefault, Trace + "ZoneRetrieve.131 No se recibio el id de la zona");
                return null;
            }

            Cat_Zone auxZone = null;
            try
            {
                auxZone = Repository.Zones.Get(id);
                if (auxZone == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ZoneRetrieve.511 No se encontró una zona con id '" + id + "'");
                }
                else
                {
                    ResultManager.IsCorrect = true;
                }
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneRetrieve.511 Excepción al obtener la zona a editar: " + ex.Message);
            }

            return auxZone;
        }

        public bool ZoneAdd(Cat_Zone item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneAdd.111 No se recibio el objeto de la zona a agregar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre de la zona no puede estar vacio");
                return false;
            }

            //insert new item
            try
            {
                Cat_Zone auxZone = new Cat_Zone();
                auxZone.Name = item.Name;
                auxZone.RegionName = item.RegionName;
                Repository.Zones.Add(auxZone);
                Repository.Complete();

                // update AddressMunicipality table
                foreach (AddressMunicipality municipality in item.AddressMunicipalities)
                {
                    AddressMunicipality aux = Repository.AddressMunicipalities
                                                        .GetAll()
                                                        .Where(x => x.Id == municipality.Id)
                                                        .FirstOrDefault();
                    aux.Cat_ZoneId = auxZone.Id;
                }
                Repository.Complete();

                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneAdd.511 Excepción al agregar la zona nombre '" + item.Name + "': " + ex.Message);
            }
            return false;
        }

        public bool ZoneUpdate(Cat_Zone item)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (item == null)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneEdit.111 No se recibio el objeto zona a editar");
                return false;
            }
            if (item.Id == -1 || item.Id == 0)
            {//no crop id was received
                ResultManager.Add(ErrorDefault, Trace + "ZoneEdit.131 No se recibio el id de la zona a editar");
                return false;
            }
            //-business validations
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                ResultManager.Add("El nombre de la zona no puede estar vacio");
                return false;
            }

            //update item
            try
            {
                Cat_Zone auxZone = Repository.Zones.Get(item.Id);
                if (auxZone == null)
                {
                    ResultManager.Add(ErrorDefault, Trace + "ZoneEdit.311 Zona con id '" + item.Id + "' no encontrado en bd");
                    return false;
                }
                auxZone.Name = item.Name;
                auxZone.RegionName = item.RegionName;

                // first remove municipalities related to this zone
                List<AddressMunicipality> auxMunicipalities = Repository.AddressMunicipalities
                                                                        .GetAll()
                                                                        .Where(x => x.Cat_ZoneId == item.Id)
                                                                        .ToList();

                foreach(AddressMunicipality municipality in auxMunicipalities)
                {
                    municipality.Cat_ZoneId = null;
                }

                // update AddressMunicipality table
                foreach (AddressMunicipality municipality in item.AddressMunicipalities)
                {
                    AddressMunicipality aux = Repository.AddressMunicipalities
                                                        .GetAll()
                                                        .Where(x => x.Id == municipality.Id)
                                                        .FirstOrDefault();
                    aux.Cat_ZoneId = auxZone.Id;
                }

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneEdit.511 Excepción al editar el cultivo con id '" + item.Id + "': " + ex.Message);
            }
            return false;
        }

        public bool ZoneDelete(int id = -1)
        {
            ResultManager.IsCorrect = false;

            //initial validations
            //-sys validations
            if (id == -1 || id == 0)
            {//no zone id was received
                ResultManager.Add(ErrorDefault, Trace + "ZoneDelete.131 No se recibio el id de la zona a eliminar");
                return false;
            }

            //delete item
            try
            {
                // first remove municipalities related to this zone
                List<AddressMunicipality> auxMunicipalities = Repository.AddressMunicipalities
                                                                        .GetAll()
                                                                        .Where(x => x.Cat_ZoneId == id)
                                                                        .ToList();

                foreach (AddressMunicipality municipality in auxMunicipalities)
                {
                    municipality.Cat_ZoneId = null;
                }
                // then remove zone
                Repository.Zones.Remove(id);

                Repository.Complete();
                ResultManager.IsCorrect = true;
                return true;
            }
            catch (Exception ex)
            {
                ResultManager.Add(ErrorDefault, Trace + "ZoneDelete.511 Excepción al eliminar la zona con id '" + id + "': " + ex.Message);
            }
            return false;
        }

        #endregion

        #region Catalogo n...
        #endregion

    }
}
