using Argiculture.Common;
using Argiculture.IRepository;
using Argiculture.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Argiculture.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        public ICrop _Icrop;
        public CropController(ICrop Icrop)
        {
            _Icrop = Icrop;
        }

        [HttpGet]
        public ActionResult GetAllCrops()
        {
            try
            {
                var result = _Icrop.GetAllCrops();
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = CustomMessages.RecordNotExists });
                }
            }
            catch (Exception ex)
            {

                return Ok(new AGResult { IsSuccess = false, Result = ex.Message, Message = CustomMessages.FailedToRetriveData });
            }
        }

        [HttpGet("{ID}")]
        public ActionResult GetCropById(string ID)
        {
            try
            {
                var result = _Icrop.GetCropByID(ID);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = CustomMessages.RecordNotExists });
                }
            }
            catch (Exception ex)
            {

                return Ok(new AGResult { IsSuccess = false, Result = ex.Message, Message = CustomMessages.FailedToRetriveData });
            }
        }

        [HttpPost]
        public ActionResult AddCrop(CropModel cropModel)
        {
            try
            {
                if (string.IsNullOrEmpty(cropModel.Name))
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = Constants.Name });
                }
                var result = _Icrop.AddCrop(cropModel);
                if (result !="0" && result!="-1")
                {
                    return Ok(new AGResult { IsSuccess = true, Result = result, Message = CustomMessages.SavedSuccessfully });
                }
                else if(result=="0")
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = CustomMessages.RecordExists });
                }
                else
                {
                    return Ok(new AGResult { IsSuccess = false, Result = string.Empty, Message = CustomMessages.SomethingWentWrong });
                }
            }
            catch (Exception ex)
            {

                return Ok(new AGResult { IsSuccess = false, Result = ex.Message, Message = CustomMessages.FailedToSaveData });
            }
        }
        [HttpPut]
        public ActionResult UpdateCrop(CropModel cropModel)
        {
            try
            {
                if(string.IsNullOrEmpty(cropModel.ID))
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = Constants.ID });
                }
                if (string.IsNullOrEmpty(cropModel.Name))
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = Constants.Name });
                }
                var result = _Icrop.UpdateCrop(cropModel);
                if (result != "0" && result != "-1")
                {
                    return Ok(new AGResult { IsSuccess = true, Result = result, Message = CustomMessages.SavedSuccessfully });
                }
                else if (result == "0")
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = CustomMessages.RecordExists });
                }
                else
                {
                    return Ok(new AGResult { IsSuccess = false, Result = string.Empty, Message = CustomMessages.SomethingWentWrong });
                }
            }
            catch (Exception ex)
            {

                return Ok(new AGResult { IsSuccess = false, Result = ex.Message, Message = CustomMessages.FailedToSaveData });
            }
        }

        [HttpDelete("{ID}")]
        public ActionResult DeleteCrop(string ID)
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = Constants.ID });
                }
                var result = _Icrop.DeleteCrop(ID);
                if (result != null && !string.IsNullOrEmpty(result))
                {
                    return Ok(new AGResult { IsSuccess = true, Result = result, Message = CustomMessages.DeleteSuccessfully });
                }
                else
                {
                    return Ok(new AGResult { IsSuccess = true, Result = string.Empty, Message = CustomMessages.RecordNotExists });
                }
            }
            catch (Exception ex)
            {

                return Ok(new AGResult { IsSuccess = false, Result = ex.Message, Message = CustomMessages.FailedToSaveData });
            }
        }
    }
}
