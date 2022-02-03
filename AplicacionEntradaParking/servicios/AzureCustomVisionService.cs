﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.servicios
{
    class AzureCustomVisionService
    {
        private readonly Properties.Settings endPointVariables = Properties.Settings.Default;
        private readonly DialogosService dialogosService = new DialogosService();
        public string GetTipoVehiculo(string url)
        {
            string tipoVehiculo = "";
            try
            {
                var client = new RestClient(endPointVariables.EndpointCustomVision);
                var request = new RestRequest("customvision/v3.0/Prediction/" + endPointVariables.IdProyectoCustomVision +
                    "/classify/iterations/" + endPointVariables.NombrePublicadoCustomVision + "/url", Method.POST);
                JObject requestBody = new JObject
                {
                    new JProperty("url", url)
                };
                request.AddHeader("Prediction-Key", Properties.Settings.Default.customVisionKey);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
                var response = client.Execute(request);
                RootCustom root = JsonConvert.DeserializeObject<RootCustom>(response.Content);
                tipoVehiculo = root.predictions[0].tagName;
            }
            catch (Exception)
            {
                dialogosService.DialogoError("Se ha producido un error al reconocer el tipo de vehículo.");
            }
            return tipoVehiculo;
        }
    }

    // CLASES PARA DESERIALIZAR SERVICIO CUSTOM VISION
    public class Prediction
    {
        public double probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }

    public class RootCustom
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public DateTime created { get; set; }
        public List<Prediction> predictions { get; set; }
    }
}
