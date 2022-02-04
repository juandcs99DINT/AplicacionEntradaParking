using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AplicacionEntradaParking.servicios
{
    class AzureComputerVisionService
    {
        private readonly Properties.Settings endPointVariables = Properties.Settings.Default;
        private readonly DialogosService dialogosService = new DialogosService();
        public string GetMatricula(string url, string tipo)
        {
            string matricula = "";
            try
            {
                var clientPost = new RestClient(endPointVariables.EndpointComputerVision);
                var requestPost = new RestRequest("vision/v3.2/read/analyze", Method.POST);
                JObject requestBody = new JObject
                {
                    new JProperty("url", url)
                };
                requestPost.AddHeader("Ocp-Apim-Subscription-Key", endPointVariables.computerVisionKey);
                requestPost.AddHeader("Content-Type", "application/json");
                requestPost.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);
                var responsePost = clientPost.Execute(requestPost);
                string urlResultadosPeticion = responsePost.Headers[0].Value.ToString();

                Thread.Sleep(1000);
                bool peticionGetTerminada = false;
                RootComputer root = null;
                while (!peticionGetTerminada)
                {
                    var clientGet = new RestClient(urlResultadosPeticion);
                    var requestGet = new RestRequest("", Method.GET);
                    requestGet.AddHeader("Ocp-Apim-Subscription-Key", endPointVariables.computerVisionKey);
                    var responseGet = clientGet.Execute(requestGet);
                    root = JsonConvert.DeserializeObject<RootComputer>(responseGet.Content);
                    if (root.status == "succeeded")
                    {
                        peticionGetTerminada = true;
                    }
                    Thread.Sleep(1000);
                }
                if (tipo == "Coche")
                {
                    matricula = root.analyzeResult.readResults[0].lines[0].text;
                }
                else if (tipo == "Moto")
                {
                    matricula = root.analyzeResult.readResults[0].lines[0].text + " " +
                      root.analyzeResult.readResults[0].lines[1].text;
                }
            }
            catch (Exception)
            {
                dialogosService.DialogoError("Se ha producido un error al reconocer la matricula del vehículo.");
            }
            return matricula;
        }
    }
    // CLASES PARA DESERIALIZAR SERVICIO COMPUTER VISION
    public class Style
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class Appearance
    {
        public Style style { get; set; }
    }

    public class Word
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
    }

    public class Line
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public Appearance appearance { get; set; }
        public List<Word> words { get; set; }
    }

    public class ReadResult
    {
        public int page { get; set; }
        public double angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
        public List<Line> lines { get; set; }
    }

    public class AnalyzeResult
    {
        public string version { get; set; }
        public string modelVersion { get; set; }
        public List<ReadResult> readResults { get; set; }
    }

    public class RootComputer
    {
        public string status { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastUpdatedDateTime { get; set; }
        public AnalyzeResult analyzeResult { get; set; }
    }
}
